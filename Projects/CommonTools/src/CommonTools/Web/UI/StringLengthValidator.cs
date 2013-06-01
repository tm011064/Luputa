using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CommonTools.Web.UI
{
    /// <summary>
    /// This class contains all string length validator related data
    /// </summary>
    public class StringLengthValidator : CustomValidator
    {
        #region properties

        /// <summary>
        /// Gets or sets the length of the max.
        /// </summary>
        /// <value>The length of the max.</value>
        public int MaxLength { get; set; }
        /// <summary>
        /// Gets or sets the length of the min.
        /// </summary>
        /// <value>The length of the min.</value>
        public int MinLength { get; set; }

        #endregion

        #region events
        /// <summary>
        /// Handles the ServerValidate event of the customStringLengthValidator control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.ServerValidateEventArgs"/> instance containing the event data.</param>
        protected virtual void stringLengthValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = true;
            if (e.Value.Length > MaxLength || e.Value.Length < MinLength)
                e.IsValid = false;
        }
        #endregion

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.ServerValidate += new ServerValidateEventHandler(stringLengthValidator_ServerValidate);

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.EnableClientScript)
            {
                string functionName = this.ID + "IsLengthValid";
                this.ClientValidationFunction = functionName;
                Control controlToValidate = FindControl(this.ControlToValidate);

                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), functionName,
@"
function " + functionName + @"(a,b){var c=document.getElementById('" + controlToValidate.ClientID + @"');b.IsValid=true;if(c!=null){if(c.value.length>" + MaxLength + @"||c.value.length<" + MinLength + @")b.IsValid=false}}
"
, true);
            } 
        }
    }
}
