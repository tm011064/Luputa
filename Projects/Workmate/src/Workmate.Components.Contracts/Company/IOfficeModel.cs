using System;
using System.Xml.Linq;
namespace Workmate.Components.Contracts.Company
{
  public interface IOfficeModel : IBaseModel
  {
    string AddressLine1 { get; set; }
    string AddressLine2 { get; set; }
    string AddressLine3 { get; set; }
    string AddressLine4 { get; set; }
    int ApplicationId { get; }
    string ContactNumber { get; set; }
    string Country { get; set; }
    string Description { get; set; }
    string Email { get; set; }
    string Fax { get; set; }
    XElement GetExtraInfoClone();
    string Location { get; set; }
    string Name { get; set; }
    int OfficeId { get; set; }
    string PostCode { get; set; }
  }
}
