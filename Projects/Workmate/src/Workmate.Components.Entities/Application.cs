using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts;
using CommonTools.Extensions;
using System.Xml.Linq;

namespace Workmate.Components.Entities
{
  public class Application : IApplication
  {
    #region members
    private XElement _ExtraInfo;
    #endregion

    #region properties

    public int ApplicationId { get; set; }

    public string ApplicationName { get; private set; }

    public string Description { get; private set; }

    #region extrainfo
    public virtual string DefaultAdminSenderEmailAddress
    {
      get { return this._ExtraInfo.ParseXElementNode("defaultAdminSenderEmailAddress", "admin@workmate.com"); }
      set { this._ExtraInfo.SetElementValue("defaultAdminSenderEmailAddress", value); }
    }
    #endregion

    #endregion

    #region methods
    public XElement GetExtraInfoClone() { return new XElement(_ExtraInfo); }
    #endregion

    #region constructors
    public Application(string applicationName, string description)
    {
      this.ApplicationName = applicationName;
      this.Description = description;

      this._ExtraInfo = new XElement("i");
    }
    public Application(int applicationId, string applicationName, string description, XElement extraInfo)
    {
      this.ApplicationId = applicationId;
      this.ApplicationName = applicationName;
      this.Description = description;

      this._ExtraInfo = extraInfo ?? new XElement("i");
    }
    #endregion
  }
}
