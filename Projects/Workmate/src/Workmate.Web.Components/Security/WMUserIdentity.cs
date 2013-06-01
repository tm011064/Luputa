using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Security.Principal;
using System.Web.Security;
using System.Xml.Linq;
using Workmate.Components.Contracts.Membership;
using CommonTools.Web.Security;
using System.Runtime.Serialization;
using ProtoBuf;
using Workmate.Components.Entities.Membership;
using System.IO;
using Workmate.Components.Contracts;

namespace Workmate.Web.Components.Security
{
  [ProtoContract(SkipConstructor = true)]
  public class WMUserIdentity : IIdentity, IUserBasic, IProtobufSerializable
  {
    #region members

    #endregion

    #region properties
    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    [ProtoMember(1)]
    public string UserName { get; set; }

    private int _UserId;
    /// <summary>
    /// Gets or sets the user id.
    /// </summary>
    /// <value>The user id.</value>
    [ProtoMember(2)]
    public int UserId
    {
      get { return _UserId; }
      set { _UserId = value; }
    }
    private string _Email;
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [ProtoMember(3)]
    public string Email
    {
      get { return _Email; }
      set { _Email = value; }
    }

    private DateTime _LastActivityDateUtc;
    /// <summary>
    /// Gets or sets the last activity date.
    /// </summary>
    /// <value>The last activity date.</value>
    [ProtoMember(4)]
    public DateTime LastActivityDateUtc
    {
      get { return _LastActivityDateUtc; }
      set { _LastActivityDateUtc = value; }
    }

    private DateTime _LastActivityUpdate;
    /// <summary>
    /// Gets or sets the last activity update.
    /// </summary>
    /// <value>The last activity update.</value>
    [ProtoMember(5)]
    public DateTime LastActivityUpdate
    {
      get { return _LastActivityUpdate; }
      set { _LastActivityUpdate = value; }
    }
    private DateTime _DateCreatedUtc;
    /// <summary>
    /// Gets the creation date.
    /// </summary>
    /// <value>The creation date.</value>
    [ProtoMember(6)]
    public DateTime DateCreatedUtc
    {
      get { return _DateCreatedUtc; }
      set { _DateCreatedUtc = value; }
    }

    private AccountStatus _AccountStatus;
    /// <summary>
    /// Gets the account status.
    /// </summary>
    /// <value>The account status.</value>
    [ProtoMember(7)]
    public AccountStatus AccountStatus
    {
      get { return _AccountStatus; }
      private set { _AccountStatus = value; }
    }


    private int _ProfileImageId;
    /// <summary>
    /// Gets the profile image id.
    /// </summary>
    /// <value>The profile image id.</value>
    [ProtoMember(8)]
    public int ProfileImageId
    {
      get { return _ProfileImageId; }
      private set { _ProfileImageId = value; }
    }

    private string _TimeZoneInfoId;
    /// <summary>
    /// Gets the time zone info id.
    /// </summary>
    /// <value>The time zone info id.</value>
    [ProtoMember(9)]
    public string TimeZoneInfoId
    {
      get { return _TimeZoneInfoId; }
      private set { _TimeZoneInfoId = value; }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is anonymous.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is anonymous; otherwise, <c>false</c>.
    /// </value>
    public bool IsAnonymous { get; private set; }

    private DateTime _LastRecordCheckUtc;
    /// <summary>
    /// Gets or sets the last record check.
    /// </summary>
    /// <value>The last record check.</value>
    [ProtoMember(11)]
    public DateTime LastRecordCheckUtc
    {
      get { return _LastRecordCheckUtc; }
      set { _LastRecordCheckUtc = value; }
    }

    private DateTime _LastLoginDateUtc;
    /// <summary>
    /// Gets or sets the last login date.
    /// </summary>
    /// <value>The last login date.</value>
    [ProtoMember(12)]
    public DateTime LastLoginDateUtc
    {
      get { return _LastLoginDateUtc; }
      set { _LastLoginDateUtc = value; }
    }

    private HashSet<string> _UserRoles;
    /// <summary>
    /// Gets the user roles.
    /// </summary>
    /// <value>The user roles.</value>
    [ProtoMember(13)]
    public HashSet<string> UserRoles
    {
      get { return _UserRoles; }
      set { _UserRoles = value; }
    }

    #endregion

    #region IIdentity Members

    [ProtoMember(14)]
    public string AuthenticationType { get; private set; }

    [ProtoMember(15)]
    public bool IsAuthenticated { get; private set; }

    [ProtoMember(16)]
    public string Name { get; private set; }

    #endregion

    #region ISerializable Members

    public byte[] Serialize()
    {
      byte[] bytes;
      using (MemoryStream ms = new MemoryStream())
      {
        Serializer.Serialize(ms, this);
        bytes = ms.ToArray();
      }
      return bytes;
    }

    #endregion

    #region constructors

    #region static instances
    public static WMUserIdentity Create(IUserBasic userBasic, bool updateLastActivityDateUtc, DateTime lastActivityUpdate, DateTime lastRecordCheckUtc)
    {
      WMUserIdentity identity = new WMUserIdentity();

      identity.UserId = userBasic.UserId;
      identity.Email = userBasic.Email;
      identity.AccountStatus = userBasic.AccountStatus;
      identity.DateCreatedUtc = userBasic.DateCreatedUtc;
      identity.LastActivityDateUtc = updateLastActivityDateUtc ? DateTime.UtcNow
                                                         : userBasic.LastActivityDateUtc;
      identity.LastActivityUpdate = lastActivityUpdate;
      identity.UserRoles = userBasic.UserRoles;
      identity.LastRecordCheckUtc = lastRecordCheckUtc;
      identity.LastLoginDateUtc = userBasic.LastLoginDateUtc;
      identity.ProfileImageId = userBasic.ProfileImageId;
      identity.TimeZoneInfoId = userBasic.TimeZoneInfoId;

      identity.IsAnonymous = userBasic.IsAnonymous;

      return identity;
    }
    #endregion

    public WMUserIdentity(FormsIdentity identity)
    {
      WMUserIdentity cookie = AuthenticationCookieManager.GetEmbeddedDataFromAuthenticationCookie<WMUserIdentity>();

      this.UserId = cookie.UserId;
      this.Email = cookie.Email;
      this.AccountStatus = cookie.AccountStatus;
      this.DateCreatedUtc = cookie.DateCreatedUtc;
      this.LastActivityDateUtc = cookie.LastActivityDateUtc;
      this.LastActivityUpdate = cookie.LastActivityUpdate;
      this.LastLoginDateUtc = cookie.LastLoginDateUtc;
      this.LastRecordCheckUtc = cookie.LastRecordCheckUtc;
      this.ProfileImageId = cookie.ProfileImageId;
      this.TimeZoneInfoId = cookie.TimeZoneInfoId;

      this.UserRoles = cookie.UserRoles;

      this.Name = identity.Name;
      this.IsAuthenticated = identity.IsAuthenticated;
      this.AuthenticationType = identity.AuthenticationType;
      this.UserName = identity.Name;

      this.IsAnonymous = false;
    }
    private WMUserIdentity()
    {
    }
    #endregion
  }
}
