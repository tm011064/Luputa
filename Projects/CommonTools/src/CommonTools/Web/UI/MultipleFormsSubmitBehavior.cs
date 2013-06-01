using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommonTools.Web.UI
{
    /// <summary>
    /// This Control enables the programmer to simulate multiple form submit-button behaviours on a single asp.net form.
    /// Currently, only textboxes are allowed as identifiying controls.
    /// </summary>
    /// <remarks>
    /// In order to make this work, all buttons on the page have to set their UseSubmitBehavior property to false.
    /// </remarks>
    [ToolboxData("<{0}:MultipleFormsSubmitBehaviourControl runat=server> </{0}:MultipleFormsSubmitBehaviourControl>")]
    public class MultipleFormsSubmitBehaviourControl : UserControl
    {
        #region globals
        private Dictionary<IButtonControl, WebControl[]> _Controls = new Dictionary<IButtonControl, WebControl[]>();
        #endregion

        #region static methods
        /// <summary>
        /// Registers the handle key down script.
        /// </summary>
        /// <param name="page">The page.</param>
        public static void RegisterHandleKeyDownScript(Page page)
        {
            RegisterHandleKeyDownScript(page, true);
        }
        /// <summary>
        /// Registers the handle key down script.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="optimized">if set to <c>true</c> [optimized].</param>
        public static void RegisterHandleKeyDownScript(Page page, bool optimized)
        {
            string script = string.Empty;
            if (!optimized)
            {
                script =
@"
function handleKeyDownEvent(e, submitControlID) {
    if( !e ) {
        if( window.event ) {
            e = window.event;
            } else { 
            return;
        }
    }
    
    if (e.keyCode == 13) { // enter pressed
        var actionElement = document.getElementById(submitControlID);
        if (actionElement != null) {                          
            if (actionElement.onclick != null) { // fire click event          
                if (actionElement.click == null) 
                    actionElement.onclick();
                else 
                    actionElement.click();
                return;
            } else if (actionElement.click != null) { // fire click event
                actionElement.click();
                return;
            }
            if (actionElement.href != null) {
                eval(actionElement.href);
                return;
            }            
        }
    }
}
";
            }
            else
            {
                script =
@"
function handleKeyDownEvent(e,c){if(!e){if(window.event){e=window.event;}else{return;}}" +
"if(e.keyCode==13){a=document.getElementById(c);if(a!=null){if (a.onclick != null){if (a.click == null){a.onclick();}else{a.click();}return;}else if(a.click!=null){a.click();return;}if(a.href!=null){eval(a.href);return;}}}}";
            }
            if (!page.ClientScript.IsClientScriptBlockRegistered(ScriptKey))
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), ScriptKey, script, true);
        }

        #endregion

        #region methods
        /// <summary>
        /// Adds an item that simulates a form's submit behaviour.
        /// </summary>
        /// <param name="button">The button which onclick event should fire when the user clicks the enter button</param>
        /// <param name="focusingControls">an array of controls that are associated with this button's onclick event when the 
        /// user presses the enter button</param>
        public void AddItem(IButtonControl button, params WebControl[] focusingControls)
        {
            _Controls.Add(button, focusingControls);
        }
        #endregion

        #region properties
        private bool _Optimized = true;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MultipleFormsSubmitBehaviourControl"/> javascript code
        /// should be rendered space-optimized or not
        /// </summary>
        /// <value><c>true</c> if optimized; otherwise, <c>false</c>.</value>
        public bool Optimized
        {
            get { return _Optimized; }
            set { _Optimized = value; }
        }

        private static string _ScriptKey = "19b795a9-6ce2-48ea-80d8-01c70f027dbd";
        /// <summary>
        /// Gets or sets the Page.ClientScript ScriptBlock registration key.
        /// </summary>
        /// <value>The script key.</value>
        public static string ScriptKey
        {
            get { return _ScriptKey; }
            set { _ScriptKey = value; }
        }
        #endregion

        #region life-cycle
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            foreach (IButtonControl button in _Controls.Keys)
            {
                string id = string.Empty;

                Button b = button as Button;
                if (b != null)
                {
                    // disable the onsubmit behaviour
                    b.UseSubmitBehavior = false;
                    id = b.ClientID;
                }
                else
                {
                    Control c = button as Control;
                    if (c != null)
                    {
                        id = c.ClientID;
                    }
                }

                foreach (WebControl control in _Controls[button])
                {
                    string currentFocus = control.Attributes["onKeyDown"];
                    if (!string.IsNullOrEmpty(currentFocus) && currentFocus.IndexOf("handleKeyDownEvent(event, '" + id + "');") < 0)
                    {
                        control.Attributes["onKeyDown"] += (currentFocus.EndsWith(";") ? "" : ";") + "handleKeyDownEvent(event, '" + id + "');";
                    }
                    else
                    {
                        control.Attributes["onKeyDown"] = "handleKeyDownEvent(event, '" + id + "');";
                    }
                }
            }

            RegisterHandleKeyDownScript(this.Page, this.Optimized);
        }
        #endregion
    }
}
