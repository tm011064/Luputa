using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CommonTools.WPF
{
    #region eventargs
    /// <summary>
    /// 
    /// </summary>
    public class HotkeyPressedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyPressedEventArgs"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public HotkeyPressedEventArgs(string identifier)
            : base()
        {
            this.Identifier = identifier;
        }
    }
    #endregion

    /// <summary>
    /// This class simplifies hotkey mappings
    /// </summary>
    public class HotkeyMappingManager
    {
        #region members
        private UIElement _Owner;

        private Dictionary<int, RoutedCommand> _RoutedCommands = new Dictionary<int, RoutedCommand>();
        private Dictionary<int, CommandBinding> _CommandBindings = new Dictionary<int, CommandBinding>();
        private Dictionary<int, InputBinding> _InputBindings = new Dictionary<int, InputBinding>();
        #endregion

        #region private methods
        private int GetKeyGestureKey(KeyGesture keyGesture)
        {
            return int.Parse(keyGesture.Key.ToString("d") + "0000" + keyGesture.Modifiers.ToString("d"));
        }

        #region event callbacks
        void cb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.HotkeyPressed != null)
                this.HotkeyPressed(this, new HotkeyPressedEventArgs((string)e.Parameter));
        }

        void _Owner_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl
                || e.Key == Key.RightCtrl
                || e.Key == Key.LeftShift
                || e.Key == Key.RightShift
                || e.Key == Key.LeftAlt
                || e.Key == Key.RightAlt
                || e.Key == Key.None)
                return;

            int key = GetKeyGestureKey(new KeyGesture(e.Key, Keyboard.Modifiers));
            if (_InputBindings.ContainsKey(key))
            {
                _InputBindings[key].Command.Execute(null);
                e.Handled = true;
            }
        }
        #endregion

        #endregion

        #region public methods
        /// <summary>
        /// Registers the command binding for a specified key gesture. This method will override any preexisting commandbindings for this key gesture.
        /// </summary>
        /// <param name="keyGesture">The key gesture.</param>
        /// <param name="identifier">The identifier.</param>
        public void RegisterCommandBinding(KeyGesture keyGesture, string identifier)
        {
            if (identifier == null)
                throw new ArgumentNullException("The identifier must not be null");

            int key = GetKeyGestureKey(keyGesture);

            if (_InputBindings.ContainsKey(key))
            {
                _Owner.InputBindings.Remove(_InputBindings[key]);
                _Owner.CommandBindings.Remove(_CommandBindings[key]);

                _CommandBindings.Remove(key);
                _RoutedCommands.Remove(key);
                _InputBindings.Remove(key);
            }

            RoutedCommand routedCommand = new RoutedCommand();

            InputBinding ib = new InputBinding(routedCommand, keyGesture) { CommandParameter = identifier };
            _Owner.InputBindings.Add(ib);

            CommandBinding cb = new CommandBinding(routedCommand);
            cb.Executed += new ExecutedRoutedEventHandler(cb_Executed);

            _Owner.CommandBindings.Add(cb);

            _RoutedCommands.Add(key, routedCommand);
            _CommandBindings.Add(key, cb);
            _InputBindings.Add(key, ib);
        }


        /// <summary>
        /// Registers the command binding for a specified key gesture. This method will override any preexisting commandbindings for this key gesture.
        /// </summary>
        /// <param name="keyGesture">The key gesture.</param>
        /// <param name="executedRoutedEventHandler">The event handler which is called when the hotkey is pressed</param>
        public void RegisterCommandBinding(KeyGesture keyGesture, ExecutedRoutedEventHandler executedRoutedEventHandler)
        {
            RegisterCommandBinding(keyGesture, null, executedRoutedEventHandler);
        }
        /// <summary>
        /// Registers the command binding for a specified key gesture. This method will override any preexisting commandbindings for this key gesture.
        /// </summary>
        /// <param name="keyGesture">The key gesture.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="executedRoutedEventHandler">The event handler which is called when the hotkey is pressed</param>
        public void RegisterCommandBinding(KeyGesture keyGesture, object parameter, ExecutedRoutedEventHandler executedRoutedEventHandler)
        {
            int key = GetKeyGestureKey(keyGesture);

            if (_InputBindings.ContainsKey(key))
            {
                _Owner.InputBindings.Remove(_InputBindings[key]);
                _Owner.CommandBindings.Remove(_CommandBindings[key]);

                _CommandBindings.Remove(key);
                _RoutedCommands.Remove(key);
            }

            RoutedCommand routedCommand = new RoutedCommand();

            InputBinding ib = new InputBinding(routedCommand, keyGesture) { CommandParameter = parameter };
            _Owner.InputBindings.Add(ib);

            CommandBinding cb = new CommandBinding(routedCommand);
            cb.Executed += executedRoutedEventHandler;

            _Owner.CommandBindings.Add(cb);

            _RoutedCommands.Add(key, routedCommand);
            _CommandBindings.Add(key, cb);
            _InputBindings.Add(key, ib);
        }
        #endregion

        #region events
        /// <summary>
        /// Occurs when a registered hotkey is pressed.
        /// </summary>
        public event EventHandler<HotkeyPressedEventArgs> HotkeyPressed;
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyMappingManager"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public HotkeyMappingManager(UIElement owner) : this(owner, false) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyMappingManager"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="overrideSystemHotkeys">if set to <c>true</c> the keyboard mapping manager will override system control hotkeys when focused on a control
        /// , e.g. CTRL+C when focus is on a textbox. Use this feature with care.</param>
        public HotkeyMappingManager(UIElement owner, bool overrideSystemHotkeys)
        {
            this._Owner = owner;

            if (overrideSystemHotkeys)
                this._Owner.PreviewKeyDown += new KeyEventHandler(_Owner_PreviewKeyDown);
        }
        #endregion
    }
}
