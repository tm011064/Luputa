using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CommonTools.Extensions;
using Workmate.Components.Contracts.Company;

namespace Workmate.Components.Entities.Company
{
  public class OfficeModel : BaseModel, IOfficeModel
  {
    #region members
    private XElement _ExtraInfo;
    #endregion

    #region properties

    public int ApplicationId { get; private set; }

    public int OfficeId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    #region extrainfo
    public virtual string AddressLine1
    {
      get { return this._ExtraInfo.ParseXElementNode("AddressLine1", string.Empty); }
      set { this._ExtraInfo.SetElementValue("AddressLine1", value); }
    }
    public virtual string AddressLine2
    {
      get { return this._ExtraInfo.ParseXElementNode("AddressLine2", string.Empty); }
      set { this._ExtraInfo.SetElementValue("AddressLine2", value); }
    }
    public virtual string AddressLine3
    {
      get { return this._ExtraInfo.ParseXElementNode("AddressLine3", string.Empty); }
      set { this._ExtraInfo.SetElementValue("AddressLine3", value); }
    }
    public virtual string AddressLine4
    {
      get { return this._ExtraInfo.ParseXElementNode("AddressLine4", string.Empty); }
      set { this._ExtraInfo.SetElementValue("AddressLine4", value); }
    }
    public virtual string PostCode
    {
      get { return this._ExtraInfo.ParseXElementNode("PostCode", string.Empty); }
      set { this._ExtraInfo.SetElementValue("PostCode", value); }
    }
    public virtual string Location
    {
      get { return this._ExtraInfo.ParseXElementNode("Location", string.Empty); }
      set { this._ExtraInfo.SetElementValue("Location", value); }
    }
    public virtual string Country
    {
      get { return this._ExtraInfo.ParseXElementNode("Country", string.Empty); }
      set { this._ExtraInfo.SetElementValue("Country", value); }
    }
    public virtual string ContactNumber
    {
      get { return this._ExtraInfo.ParseXElementNode("ContactNumber", string.Empty); }
      set { this._ExtraInfo.SetElementValue("ContactNumber", value); }
    }
    public virtual string Fax
    {
      get { return this._ExtraInfo.ParseXElementNode("Fax", string.Empty); }
      set { this._ExtraInfo.SetElementValue("Fax", value); }
    }
    public virtual string Email
    {
      get { return this._ExtraInfo.ParseXElementNode("Email", string.Empty); }
      set { this._ExtraInfo.SetElementValue("Email", value); }
    }
    #endregion

    #endregion

    #region methods
    public XElement GetExtraInfoClone() { return new XElement(_ExtraInfo); }
    #endregion

    #region constructors
    public OfficeModel()
    {
      this._ExtraInfo = new XElement("r");
    }

    public OfficeModel(int applicationId, string name)
    {
      this.ApplicationId = applicationId;
      this.Name = name;

      this._ExtraInfo = new XElement("r");
    }
    public OfficeModel(int applicationId, int officeId, string name, string description, XElement extraInfo)
    {
      this.ApplicationId = applicationId;
      this.OfficeId = officeId;
      this.Name = name;
      this.Description = description;
      this._ExtraInfo = extraInfo ?? new XElement("r");
    }
    #endregion
  }
}
