using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace CommonTools.Core.Collections
{
  public class ChangeTrackingItemInfo<T>
  {
    public T Item { get; private set; }
    public CollectionItemState CollectionItemState { get; private set; }

    public ChangeTrackingItemInfo(T item, CollectionItemState collectionItemState)
    {
      this.Item = item;
      this.CollectionItemState = collectionItemState;
    }
  }

  public enum CollectionItemState
  {
    NotChanged,
    Changed,
    NewItem,
    Deleted
  }
  public enum ErrorMessageFormatOptions
  {
    ShowAll,
    ShowDistinct
  }

  public class PropertyError
  {
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }

    public PropertyError(string propertyName, string errorMessage)
    {
      this.PropertyName = propertyName;
      this.ErrorMessage = errorMessage;
    }
  }

  public class ValidationInfo
  {
    #region members
    private List<ValidationInfo> _InvalidValidationInfos;
    private Dictionary<string, PropertyError> _PropertyErrors;
    #endregion

    #region properties
    private string _ErrorMessage;
    public string ErrorMessage
    {
      get { return _ErrorMessage; }
      set
      {
        this.ErrorMessage = value;
        this._PropertyErrors.Clear();
        if (!string.IsNullOrEmpty(value))
        {
          string key = Guid.NewGuid().ToString();
          this._PropertyErrors.Add(key, new PropertyError(key, value));
        }
      }
    }

    public bool IsValid { get; set; }
    #endregion

    #region public methods
    public bool HasPropertyError(string propertyName)
    {
      return this._PropertyErrors.ContainsKey(propertyName);
    }
    public PropertyError GetPropertyError(string propertyName)
    {
      if (this._PropertyErrors.ContainsKey(propertyName))
        return this._PropertyErrors[propertyName];

      return null;
    }

    public IEnumerable<ValidationInfo> GetInvalidValidationInfos()
    {
      foreach (ValidationInfo validationInfo in _InvalidValidationInfos)
        yield return validationInfo;
    }

    public IEnumerable<string> GetIndividualErrorMessages(ErrorMessageFormatOptions errorMessageFormatOptions)
    {
      switch (errorMessageFormatOptions)
      {
        case ErrorMessageFormatOptions.ShowAll:
          foreach (string text in this._PropertyErrors.Values.Select(c => c.ErrorMessage))
            yield return text;
          yield break;

        case ErrorMessageFormatOptions.ShowDistinct:
          foreach (string text in new HashSet<string>(this._PropertyErrors.Values.Select(c => c.ErrorMessage)))
            yield return text;
          yield break;

        default: throw new NotImplementedException();
      }
    }

    public string GenerateErrorMessage(ErrorMessageFormatOptions errorMessageFormatOptions)
    {
      return GenerateErrorMessage(errorMessageFormatOptions, false);
    }
    public string GenerateErrorMessage(ErrorMessageFormatOptions errorMessageFormatOptions, bool useBullets)
    {
      if (this._PropertyErrors.Count == 0)
        return null;

      StringBuilder sb = new StringBuilder();
      string prefix = useBullets ? "\u2022 " : string.Empty;
      switch (errorMessageFormatOptions)
      {
        case ErrorMessageFormatOptions.ShowAll:
          foreach (string text in this._PropertyErrors.Values.Select(c => c.ErrorMessage))
            sb.AppendLine(prefix + text);
          return sb.ToString();

        case ErrorMessageFormatOptions.ShowDistinct:
          foreach (string text in new HashSet<string>(this._PropertyErrors.Values.Select(c => c.ErrorMessage)))
            sb.AppendLine(prefix + text);
          return sb.ToString();

        default: throw new NotImplementedException();
      }
    }
    #endregion

    #region constructors
    public ValidationInfo(string errorMessage, bool isValid)
      : this(errorMessage, isValid, null, null, null) { }

    public ValidationInfo(string errorMessage, bool isValid
      , IEnumerable<string> errorMessages)
      : this(errorMessage, isValid, errorMessages, null, null) { }

    public ValidationInfo(string errorMessage, bool isValid
      , IEnumerable<PropertyError> propertyErrors)
      : this(errorMessage, isValid, null, propertyErrors, null) { }

    public ValidationInfo(string errorMessage, bool isValid
      , IEnumerable<string> errorMessages, IEnumerable<ValidationInfo> invalidValidationInfos)
      : this(errorMessage, isValid, errorMessages, null, invalidValidationInfos) { }

    public ValidationInfo(string errorMessage, bool isValid, IEnumerable<ValidationInfo> invalidValidationInfos)
      : this(errorMessage, isValid, null, null, invalidValidationInfos) { }

    public ValidationInfo(string errorMessage, bool isValid
      , IEnumerable<PropertyError> propertyErrors, IEnumerable<ValidationInfo> invalidValidationInfos)
      : this(errorMessage, isValid, null, propertyErrors, invalidValidationInfos) { }

    private ValidationInfo(string errorMessage, bool isValid, IEnumerable<string> errorMessages
      , IEnumerable<PropertyError> propertyErrors, IEnumerable<ValidationInfo> invalidValidationInfos)
    {
      this.IsValid = IsValid;

      this._ErrorMessage = errorMessage;

      string key;
      List<string> errorMessageList = new List<string>(errorMessages ?? new List<string>());

      this._PropertyErrors = new Dictionary<string, PropertyError>();
      int count = errorMessageList.Count;

      if (count != 0)
      {
        for (int i = 0; i < count; i++)
        {
          key = Guid.NewGuid().ToString();
          this._PropertyErrors.Add(key, new PropertyError(key, errorMessageList[i]));
        }
      }
      else
      {
        foreach (PropertyError propertyError in new List<PropertyError>(propertyErrors ?? new List<PropertyError>()))
          this._PropertyErrors[propertyError.PropertyName] = propertyError;
      }

      this._InvalidValidationInfos = new List<ValidationInfo>(invalidValidationInfos ?? new List<ValidationInfo>());
    }
    #endregion
  }

  public class ValidationEventArgs : EventArgs
  {
    public ValidationInfo ValidationInfo { get; private set; }

    public ValidationEventArgs(ValidationInfo validationInfo)
    {
      this.ValidationInfo = validationInfo;
    }
  }

  public interface IValidatable
  {
    bool IsValid { get; }
    ValidationInfo Validate();
    event EventHandler<ValidationEventArgs> Validated;
  }

  public class ChangeTrackingObservableCollection<T> : ObservableCollection<T>, IChangeTracking, IValidatable, INotifyPropertyChanged, IBindingList
    where T : INotifyPropertyChanged, IValidatable
  {
    #region nested classes
    public class ElementInfo
    {
      public CollectionItemState CollectionItemState { get; set; }

      public ElementInfo(CollectionItemState collectionItemState)
      {
        this.CollectionItemState = collectionItemState;
      }
    }
    #endregion

    #region members
    private Dictionary<T, ElementInfo> _CollectionItemStates = new Dictionary<T, ElementInfo>();

    private long _TotalChangedItems = 0;
    private long _TotalNewItems = 0;
    private long _TotalDeletedItems = 0;

    private bool _IsInitializing = false;
    private bool _IsValidating = false;

    private bool _IsSilentReplace;

    private readonly object _AccessLock = new object();
    private readonly object _InitializationLock = new object();

    private HashSet<string> _ChangeTrackingPropertyNameFilter;
    #endregion

    #region events
    public event EventHandler<ValidationEventArgs> Validated;

    public event PropertyChangedEventHandler ItemPropertyChanged;
    public event PropertyChangingEventHandler PropertyChanging;
    #endregion

    #region properties
    public long TotalChangedItems
    {
      get { lock (_AccessLock) { return _TotalChangedItems; } }
    }
    public long TotalNewItems
    {
      get { lock (_AccessLock) { return _TotalNewItems; } }
    }
    public long TotalDeletedItems
    {
      get { lock (_AccessLock) { return _TotalDeletedItems; } }
    }

    public long TotalChanges
    {
      get { lock (_AccessLock) { return _TotalChangedItems + _TotalNewItems + _TotalDeletedItems; } }
    }

    private bool _IsValid;
    public bool IsValid
    {
      get { return _IsValid; }
      set
      {
        if (_IsValid != value)
        {
          RaisePropertyChanging("IsValid");
          _IsValid = value;
          RaisePropertyChanged("IsValid");
        }
      }
    }
    #endregion

    #region overrides
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Move: return;
        case NotifyCollectionChangedAction.Reset: return;
        case NotifyCollectionChangedAction.Replace:
          if (this._IsSilentReplace)
          {
            base.OnCollectionChanged(e);
            return;
          }
          break;
      }

      lock (_AccessLock)
      {
        #region remove items
        if (e.OldItems != null)
        {
          foreach (T element in e.OldItems)
          {
            switch (_CollectionItemStates[element].CollectionItemState)
            {
              case CollectionItemState.NotChanged:
                _CollectionItemStates[element].CollectionItemState = CollectionItemState.Deleted;
                _TotalDeletedItems++;
                break;

              case CollectionItemState.NewItem:
                _TotalNewItems--;
                _CollectionItemStates.Remove(element);
                break;

              case CollectionItemState.Changed:
                _CollectionItemStates[element].CollectionItemState = CollectionItemState.Deleted;
                _TotalChangedItems--;
                _TotalDeletedItems++;
                break;

              case CollectionItemState.Deleted:
                throw new ApplicationException("This should not happen.");
            }

            element.PropertyChanged -= ContainedElementChanged;
          }
        }
        #endregion

        #region add items
        if (e.NewItems != null)
        {
          if (_IsInitializing)
          {
            foreach (T element in e.NewItems)
            {
              _CollectionItemStates[element] = new ElementInfo(CollectionItemState.NotChanged);
              element.Validated += ContainedElementValidated;
              element.PropertyChanged += ContainedElementChanged;
            }
          }
          else
          {
            foreach (T element in e.NewItems)
            {
              _CollectionItemStates[element] = new ElementInfo(CollectionItemState.NewItem);
              _TotalNewItems++;

              element.Validated += ContainedElementValidated;
              element.PropertyChanged += ContainedElementChanged;
            }
          }
        }
        #endregion

        RaiseItemsChangedProperties();
      }

      base.OnCollectionChanged(e);
    }

    protected override void ClearItems()
    {
      lock (_AccessLock)
      {
        foreach (T element in this)
          element.PropertyChanged -= ContainedElementChanged;

        this._CollectionItemStates.Clear();

        this._TotalChangedItems = 0;
        this._TotalNewItems = 0;
        this._TotalDeletedItems = 0;
      }

      base.ClearItems();
    }
    #endregion

    #region private methods
    private void ContainedElementValidated(object sender, ValidationEventArgs e)
    {
      if (!_IsValidating)
      {
        if (!e.ValidationInfo.IsValid)
        {
          this.IsValid = false;
        }
        else
        {
          bool isValid = true;
          foreach (T element in this)
          {
            if (!element.IsValid)
            {
              isValid = false;
              break;
            }
          }
          this.IsValid = isValid;
        }
      }
    }
    private void ContainedElementChanged(object sender, PropertyChangedEventArgs e)
    {
      T element = (T)sender;

      if (_CollectionItemStates.ContainsKey(element)
        && _CollectionItemStates[element].CollectionItemState != CollectionItemState.NewItem
        && _CollectionItemStates[element].CollectionItemState != CollectionItemState.Changed)
      {
        lock (_AccessLock)
        {
          if (!_ChangeTrackingPropertyNameFilter.Contains(e.PropertyName))
          {
            _CollectionItemStates[element].CollectionItemState = CollectionItemState.Changed;
            _TotalChangedItems++;
          }

          RaiseItemsChangedProperties();
        }
      }

      OnPropertyChanged(e);

      if (this.ItemPropertyChanged != null)
        this.ItemPropertyChanged(this, e);

      var handler = this.ListChanged;
      if (handler != null)
      {
        int index = this.IndexOf(element);
        if (index >= 0)
          handler(this, new ListChangedEventArgs(ListChangedType.ItemChanged, index));
      }
    }

    protected void RaisePropertyChanged(string propertyName)
    {
      this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
    protected void RaisePropertyChanging(string propertyName)
    {
      var handler = this.PropertyChanging;

      if (handler != null)
        handler(this, new PropertyChangingEventArgs(propertyName));
    }

    private void RaiseItemsChangedProperties()
    {
      RaisePropertyChanged("TotalChangedItems");
      RaisePropertyChanged("TotalNewItems");
      RaisePropertyChanged("TotalDeletedItems");
      RaisePropertyChanged("TotalChanges");
      RaisePropertyChanged("IsChanged");
    }
    #endregion

    #region IChangeTracking members
    public void AcceptChanges()
    {
      lock (_AccessLock)
      {
        List<T> deletedElements = new List<T>();

        foreach (T element in _CollectionItemStates.Keys)
        {
          switch (_CollectionItemStates[element].CollectionItemState)
          {
            case CollectionItemState.Deleted:
              deletedElements.Add(element);
              break;

            case CollectionItemState.Changed:
              _CollectionItemStates[element].CollectionItemState = CollectionItemState.NotChanged;
              _TotalChangedItems--;
              break;
            case CollectionItemState.NewItem:
              _CollectionItemStates[element].CollectionItemState = CollectionItemState.NotChanged;
              _TotalNewItems--;
              break;
          }
        }

        foreach (T element in deletedElements)
        {
          _CollectionItemStates.Remove(element);
          _TotalDeletedItems--;
        }
      }

      RaiseItemsChangedProperties();
    }

    public bool IsChanged { get { return this.TotalChanges > 0; } }
    #endregion

    #region IBindingList Members

    public void AddIndex(PropertyDescriptor property)
    {
      throw new NotImplementedException();
    }

    private Func<T> _CreateNewItem;
    public void SetCreateNewItemDelegate(Func<T> func) { this._CreateNewItem = func; }

    public object AddNew()
    {
      if (this._CreateNewItem != null)
      {
        T obj = this._CreateNewItem();
        if (obj != null)
        {
          this.Add(obj, false);
          return obj;
        }
      }

      throw new NotSupportedException();
    }

    public bool AllowEdit { get; set; }
    public bool AllowNew { get; set; }
    public bool AllowRemove { get; set; }

    public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
    {
      throw new NotImplementedException();
    }

    public int Find(PropertyDescriptor property, object key)
    {
      throw new NotImplementedException();
    }

    public bool IsSorted { get; set; }

    public event ListChangedEventHandler ListChanged;

    public void RemoveIndex(PropertyDescriptor property)
    {
      throw new NotImplementedException();
    }

    public void RemoveSort()
    {
      throw new NotImplementedException();
    }

    public ListSortDirection SortDirection { get { return ListSortDirection.Ascending; } }

    public PropertyDescriptor SortProperty
    {
      get { throw new NotImplementedException(); }
    }

    public bool SupportsChangeNotification { get { return false; } }
    public bool SupportsSearching { get { return false; } }
    public bool SupportsSorting { get { return false; } }

    #endregion

    #region public methods
    public bool IsChangeTrackingFiltered(string propertyName)
    {
      return this._ChangeTrackingPropertyNameFilter.Contains(propertyName);
    }

    public void DoSilentReplace(T oldItem, T newItem)
    {
      int index = IndexOf(oldItem);
      if (index >= 0)
      {
        lock (_AccessLock)
        {
          try
          {
            this._IsSilentReplace = true;
            this[index] = newItem;
          }
          finally
          {
            this._IsSilentReplace = false;
          }
        }
      }
    }

    public ValidationInfo Validate()
    {
      return this.Validate(false);
    }

    public ValidationInfo Validate(bool includeUnchanged)
    {
      this._IsValidating = true;
      bool isValid = true;
      ValidationInfo validationInfo;

      List<string> errorMessages = new List<string>();
      List<ValidationInfo> validationInfos = new List<ValidationInfo>();
      StringBuilder sb = new StringBuilder();

      try
      {
        lock (_AccessLock)
        {
          foreach (T element in this)
          {
            validationInfo = element.Validate();
            if (
                (includeUnchanged
                 || _CollectionItemStates[element].CollectionItemState != CollectionItemState.NotChanged
                )
                && !validationInfo.IsValid
              )
            {
              isValid = false;
              foreach (string errorMessage in validationInfo.GetIndividualErrorMessages(ErrorMessageFormatOptions.ShowDistinct))
              {
                if (string.IsNullOrEmpty(errorMessage))
                  continue;

                sb.AppendLine(errorMessage);
                errorMessages.Add(errorMessage);
              }

              validationInfos.Add(validationInfo);
            }
          }
        }

        this.IsValid = isValid;
      }
      finally { _IsValidating = false; }

      validationInfo = new ValidationInfo(sb.Length == 0 ? null : sb.ToString(), isValid, errorMessages, validationInfos);

      var handler = this.Validated;
      if (handler != null)
      {
        handler(this, new ValidationEventArgs(validationInfo));
      }

      return validationInfo;
    }

    public IEnumerable<T> GetChangedItems()
    {
      List<T> items = new List<T>();
      lock (_AccessLock)
      {
        foreach (KeyValuePair<T, ElementInfo> kvp in _CollectionItemStates)
        {
          if (kvp.Value.CollectionItemState == CollectionItemState.Changed)
            items.Add(kvp.Key);
        }
      }
      return items;
    }

    public IEnumerable<T> GetDeletedItems()
    {
      List<T> items = new List<T>();
      lock (_AccessLock)
      {
        foreach (KeyValuePair<T, ElementInfo> kvp in _CollectionItemStates)
        {
          if (kvp.Value.CollectionItemState == CollectionItemState.Deleted)
            items.Add(kvp.Key);
        }
      }
      return items;
    }

    public IEnumerable<T> GetNewItems()
    {
      List<T> items = new List<T>();
      lock (_AccessLock)
      {
        foreach (KeyValuePair<T, ElementInfo> kvp in _CollectionItemStates)
        {
          if (kvp.Value.CollectionItemState == CollectionItemState.NewItem)
            items.Add(kvp.Key);
        }
      }
      return items;
    }

    public List<ChangeTrackingItemInfo<T>> GetAllChanges()
    {
      List<ChangeTrackingItemInfo<T>> items = new List<ChangeTrackingItemInfo<T>>();
      lock (_AccessLock)
      {
        foreach (KeyValuePair<T, ElementInfo> kvp in _CollectionItemStates)
        {
          if (kvp.Value.CollectionItemState != CollectionItemState.NotChanged)
          {
            items.Add(new ChangeTrackingItemInfo<T>(kvp.Key, kvp.Value.CollectionItemState));
          }
        }
      }
      return items;
    }

    public void Add(T item, bool isInitializing)
    {
      if (isInitializing)
      {
        lock (_InitializationLock)
        {
          this._IsInitializing = true;
          try { this.Add(item); }
          finally { this._IsInitializing = false; }
        }
      }
      else
      {
        this.Add(item);
      }
    }
    #endregion

    #region constructors
    public void Initialize(IEnumerable<T> items)
    {
      this.AllowEdit = true;
      this.AllowNew = true;
      this.AllowRemove = true;

      if (items != null)
      {
        lock (_InitializationLock)
        {
          this._IsInitializing = true;
          try
          {
            this.Clear();

            foreach (T item in items)
              this.Add(item);
          }
          finally
          {
            this._IsInitializing = false;
          }
        }
      }
    }

    public ChangeTrackingObservableCollection()
    {
      this._ChangeTrackingPropertyNameFilter = new HashSet<string>();
      Initialize(null);
    }
    public ChangeTrackingObservableCollection(params string[] ignoreChangeForPropertyName)
    {
      this._ChangeTrackingPropertyNameFilter = new HashSet<string>(ignoreChangeForPropertyName ?? new string[0]);
      Initialize(null);
    }
    public ChangeTrackingObservableCollection(IEnumerable<T> items)
    {
      this._ChangeTrackingPropertyNameFilter = new HashSet<string>();
      Initialize(items);
    }
    public ChangeTrackingObservableCollection(IEnumerable<T> items, params string[] ignoreChangeForPropertyName)
    {
      this._ChangeTrackingPropertyNameFilter = new HashSet<string>(ignoreChangeForPropertyName ?? new string[0]);
      Initialize(items);
    }
    #endregion

  }
}
