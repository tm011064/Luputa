using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using CommonTools.Core.Collections;

namespace CommonTools.WPF
{
  public abstract class BaseViewModel : IDataErrorInfo, IValidatable, INotifyPropertyChanged, INotifyPropertyChanging
  {
    #region members
    private static Dictionary<Type, List<string>> _TypePropertyNamesLookup = new Dictionary<Type, List<string>>();
    private static readonly object _TypePropertyNamesLookupLock = new object();
    protected Dictionary<string, string> _ExternalErrors = new Dictionary<string, string>();
    private static char[] NEW_LINE_AS_CHAR_ARRAY = Environment.NewLine.ToCharArray();

    protected Dictionary<string, ValidationInfo> _ValidProperties = new Dictionary<string, ValidationInfo>();
    private List<string> _BusyStateTracker = new List<string>();
    private readonly object _BusyStateTrackerLock = new object();
    #endregion

    #region events
    public event EventHandler DataRefreshed;
    public event PropertyChangingEventHandler PropertyChanging;
    public event PropertyChangedEventHandler PropertyChanged;
    public event EventHandler<ValidationEventArgs> Validated;

    protected void RaisePropertyChanging(string propertyName)
    {
      var handler = this.PropertyChanging;
      if (handler != null)
        handler(this, new PropertyChangingEventArgs(propertyName));
    }
    protected void RaisePropertyChanged(string propertyName)
    {
      var handler = this.PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    #region properties
    private TimeSpan _VeryBusyDelay = TimeSpan.FromMilliseconds(40);
    public TimeSpan VeryBusyDelay
    {
      get { return _VeryBusyDelay; }
      set { _VeryBusyDelay = value; }
    }

    #region background tasks

    private bool _IsVeryBusy;
    public bool IsVeryBusy
    {
      get { return _IsVeryBusy; }
      set
      {
        if (_IsVeryBusy != value)
        {
          this.RaisePropertyChanging("IsVeryBusy");
          this._IsVeryBusy = value;
          this.RaisePropertyChanged("IsVeryBusy");
        }

      }
    }
    private bool _IsBusy;
    public bool IsBusy
    {
      get { return _IsBusy; }
      set
      {
        if (_IsBusy != value)
        {
          this.RaisePropertyChanging("IsBusy");
          this._IsBusy = value;
          this.RaisePropertyChanged("IsBusy");
        }

      }
    }

    private string _BackgroundProcessText;
    public string BackgroundProcessText
    {
      get { return _BackgroundProcessText; }
      set
      {
        if (_BackgroundProcessText != value)
        {
          this.RaisePropertyChanging("BackgroundProcessText");
          this._BackgroundProcessText = value;
          this.RaisePropertyChanged("BackgroundProcessText");
        }

      }
    }

    #endregion

    #region Validation Popup

    public ICommand ToggleValidationErrorsPopupVisibilityCommand { get; private set; }

    private ObservableCollection<string> _ValidationErrorsCollection = new ObservableCollection<string>();
    public ObservableCollection<string> ValidationErrorsCollection { get { return _ValidationErrorsCollection; } }

    private bool _ShowValidationErrorsPopup;
    public bool ShowValidationErrorsPopup
    {
      get { return _ShowValidationErrorsPopup; }
      set
      {
        if (_ShowValidationErrorsPopup != value)
        {
          this.RaisePropertyChanging("ShowValidationErrorsPopup");
          this._ShowValidationErrorsPopup = value;

          if (value == true)
          {
            ValidationInfo validationInfo = this.Validate();

            this._ValidationErrorsCollection.Clear();
            foreach (string error in validationInfo.GetIndividualErrorMessages(ErrorMessageFormatOptions.ShowDistinct))
              this._ValidationErrorsCollection.Add(error);
            this.RaisePropertyChanged("ValidationErrorsCollection");
          }
          this.RaisePropertyChanged("ShowValidationErrorsPopup");
        }
      }
    }

    #endregion

    #region validation

    private string _Error;
    public string Error
    {
      get { return _Error; }
      private set
      {
        if (_Error != value)
        {
          this.RaisePropertyChanging("Error");
          this._Error = value;
          this.RaisePropertyChanged("Error");
        }
      }
    }

    public string this[string propertyName]
    {
      get
      {
        string message = DoPropertyValidation(propertyName);
        UpdateValidationState();
        return message;
      }
    }

    private bool _IsValid;
    public bool IsValid
    {
      get { return _IsValid; }
      set
      {
        if (_IsValid != value)
        {
          this.RaisePropertyChanging("IsValid");
          this._IsValid = value;
          this.RaisePropertyChanged("IsValid");
        }
      }
    }

    public bool IsUnderlyingDataLoaded { get; private set; }
    #endregion

    #endregion

    #region methods

    public virtual void RaisePropertyChangedEventForAllProperties()
    {
      this.RaisePropertyChanged(null);
    }

    protected void RaiseDataRefreshedEvent()
    {
      this.IsUnderlyingDataLoaded = true;

      var handler = this.DataRefreshed;
      if (handler != null)
        handler(this, EventArgs.Empty);
    }

    public virtual void SaveChanges()
    {
      // to be overridden by children
    }

    protected virtual void DoRefresh(bool ignoreChanges)
    {
      // to be overridden by children
    }

    public void Refresh()
    {
      this.DoRefresh(false);
    }
    public void Refresh(bool ignoreChanges)
    {
      this.DoRefresh(ignoreChanges);
    }

    public virtual int GetTotalChanges()
    {
      return 0;
    }

    private DateTime _LastIdleTime = DateTime.MaxValue;
    private Timer _LastIdleTimer;

    public void SetBusyState(string text)
    {
      if (string.IsNullOrEmpty(text))
        throw new ArgumentNullException("You must provide a text to identify this process.");

      lock (_BusyStateTrackerLock)
      {
        if (!this.IsBusy)
        {
          _LastIdleTime = DateTime.UtcNow;
          if (_LastIdleTimer != null)
          {
            _LastIdleTimer.Dispose();
          }

          _LastIdleTimer = new Timer((object state) =>
            {
              lock (_BusyStateTrackerLock)
              {
                Dispatcher.CurrentDispatcher.Invoke(
                  new Action(() =>
                  {
                    if (this.IsBusy)
                      this.IsVeryBusy = true;
                  }), DispatcherPriority.Normal);
              }
            }, null, 40, Timeout.Infinite);
        }

        this.IsBusy = true;
        this.IsVeryBusy = true;
        _BusyStateTracker.Add(text);

        StringBuilder sb = new StringBuilder();
        int count = _BusyStateTracker.Count;
        for (int i = 0; i < count; i++)
          sb.AppendLine(_BusyStateTracker[i]);
        this.BackgroundProcessText = sb.ToString().TrimEnd(NEW_LINE_AS_CHAR_ARRAY);
      }

      this.OnBusyStateChanged(text);
    }

    public void RemoveBusyState(string text)
    {
      if (string.IsNullOrEmpty(text))
        throw new ArgumentNullException("You must provide a text to identify this process.");

      lock (_BusyStateTrackerLock)
      {
        _BusyStateTracker.Remove(text);
        if (_BusyStateTracker.Count == 0)
        {
          this.IsBusy = false;
          this.IsVeryBusy = false;
          this.BackgroundProcessText = null;
        }
        else
        {
          StringBuilder sb = new StringBuilder();
          int count = _BusyStateTracker.Count;
          for (int i = 0; i < count; i++)
            sb.AppendLine(_BusyStateTracker[i]);
          this.BackgroundProcessText = sb.ToString();
        }
      }

      this.OnBusyStateChanged(text);
    }

    protected virtual void OnBusyStateChanged(string text)
    {
      // to be overridden by children
    }

    protected void ExecuteBackgroundTask(string text, Action action)
    {
      ExecuteBackgroundTask(text, action, null);
    }
    protected void ExecuteBackgroundTask(string text, Action action, Action callback)
    {
      SetBusyState(text);

      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.DoWork += (object sender, DoWorkEventArgs e) =>
        {
          action.Invoke();
        };
      backgroundWorker.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
        {
          if (e.Error != null)
          {
            // Log this
          }
          RemoveBusyState(text);

          if (callback != null)
            callback.Invoke();
        };
      backgroundWorker.RunWorkerAsync();
    }
    #endregion

    #region validation
    public virtual string ValidateProperty(string propertyName)
    {
      // to be overridden
      return null;
    }

    private ValidationInfo GetValidationInfo()
    {
      StringBuilder sb = new StringBuilder();
      bool isValid = true;

      List<PropertyError> propertyErrors = new List<PropertyError>();
      foreach (var kvp in _ExternalErrors)
      {
        if (!string.IsNullOrEmpty(kvp.Value))
        {
          isValid = false;
          sb.AppendLine(kvp.Value);
          propertyErrors.Add(new PropertyError(kvp.Key, kvp.Value));
        }
      }
      foreach (var kvp in _ValidProperties)
      {
        if (!kvp.Value.IsValid)
        {
          isValid = false;
          sb.AppendLine(kvp.Value.ErrorMessage);
          propertyErrors.Add(new PropertyError(kvp.Key, kvp.Value.ErrorMessage));
        }
      }

      return new ValidationInfo(isValid ? null : sb.ToString().Trim(NEW_LINE_AS_CHAR_ARRAY), isValid, propertyErrors);
    }

    private string DoPropertyValidation(string propertyName)
    {
      string validationResult = this.ValidateProperty(propertyName);

      if (validationResult != null)
        validationResult = validationResult.Trim();

      ValidationInfo validationInfo = SetValidPropertiesValue(propertyName, validationResult);

      string message = validationInfo.ErrorMessage;
      if (_ExternalErrors.ContainsKey(propertyName)
        && !string.IsNullOrEmpty(_ExternalErrors[propertyName]))
      {
        if (string.IsNullOrEmpty(message))
          message = _ExternalErrors[propertyName];
        else
          message += Environment.NewLine + _ExternalErrors[propertyName];
      }

      return message;
    }

    public ValidationInfo UpdateValidationState()
    {
      ValidationInfo info = GetValidationInfo();

      this.Error = info.ErrorMessage;
      this.IsValid = info.IsValid;

      var handler = this.Validated;
      if (handler != null)
        handler(this, new ValidationEventArgs(info));

      return info;
    }

    public virtual ValidationInfo Validate()
    {
      foreach (string propertyName in this._ValidProperties.Keys)
        DoPropertyValidation(propertyName);

      return UpdateValidationState();
    }

    private ValidationInfo SetValidPropertiesValue(string propertyName, string error)
    {
      ValidationInfo validationInfo;

      if (_ValidProperties.ContainsKey(propertyName))
      {
        validationInfo = _ValidProperties[propertyName];

        validationInfo.ErrorMessage = error;
        validationInfo.IsValid = string.IsNullOrEmpty(error);
      }
      else
      {
        validationInfo = new ValidationInfo(error, string.IsNullOrEmpty(error), new List<string>() { error });
        _ValidProperties.Add(propertyName, validationInfo);
      }

      return validationInfo;
    }

    #region SetFieldDataError

    public void ClearPropertyDataErrors()
    {
      foreach (string key in new List<string>(_ExternalErrors.Keys))
      {
        _ExternalErrors[key] = null;
      }
      foreach (string key in new List<string>(_ValidProperties.Keys))
      {
        _ValidProperties[key] = new ValidationInfo(null, true);
      }

      ValidationInfo info = GetValidationInfo();

      this.Error = info.ErrorMessage;
      this.IsValid = info.IsValid;
    }

    public void SetPropertyDataError(string propertyName, string error, bool raiseNotification)
    {
      if (_ExternalErrors.ContainsKey(propertyName))
      {
        if (_ExternalErrors[propertyName] == error)
          return;
      }
      else
      {
        if (string.IsNullOrEmpty(error))
          return;
      }

      _ExternalErrors[propertyName] = error;

      ValidationInfo info = GetValidationInfo();

      this.Error = info.ErrorMessage;
      this.IsValid = info.IsValid;

      if (raiseNotification)
        this.RaisePropertyChanged(propertyName);
    }

    #endregion

    #endregion

    #region constructors
    private void Initialize()
    {
      Type type = this.GetType();
      if (!_TypePropertyNamesLookup.ContainsKey(type))
      {
        lock (_TypePropertyNamesLookupLock)
        {
          if (!_TypePropertyNamesLookup.ContainsKey(type))
          {
            List<string> values = new List<string>();
            foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
              values.Add(propertyInfo.Name);

            _TypePropertyNamesLookup[type] = values;
          }
        }
      }
      foreach (string propertyName in _TypePropertyNamesLookup[type])
        _ValidProperties.Add(propertyName, new ValidationInfo(null, true));
    }

    public BaseViewModel()
    {
      Initialize();
      
      // TODO (Roman): this needs a RelayCommand or similar
      //this.ToggleValidationErrorsPopupVisibilityCommand = new RelayCommand(delegate{this.ShowValidationErrorsPopup = !this.ShowValidationErrorsPopup;});
    }
    #endregion
  }
}
