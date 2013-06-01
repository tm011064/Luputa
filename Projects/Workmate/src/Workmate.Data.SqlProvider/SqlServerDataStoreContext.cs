using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Workmate.Data;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Contracts.Membership;
using Workmate.Data.Entities;
using CommonTools;
using System.Xml.Linq;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Entities.CMS;
using CommonTools.Extensions;
using log4net;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Components.Contracts.CMS.Content;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Contracts.CMS.Articles;
using System.Web;
using Workmate.Components.Contracts;
using Workmate.Components.Entities;
using Workmate.Components.Entities.CMS.Membership;
using Workmate.Components.Contracts.Emails;
using Workmate.Components.Entities.Emails;
using Workmate.Components.Contracts.Company;
using Workmate.Components.Entities.Company;

namespace Workmate.Data.SqlProvider
{
  public class SqlServerDataStoreContext : IDataStoreContext
  {
    #region members
    private SqlConnection _Connection;
    private int _DefaultCommandTimeout;
    private ILog _Log = LogManager.GetLogger("SqlServerDataStoreContext");
    #endregion

    #region interface methods

    #region membership
    public int wm_Users_UnlockUser(int userId)
    {
      return _wm_Users_UnlockUser(userId);
    }
    private int _wm_Users_UnlockUser(int? userId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_UnlockUser";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int wm_Users_UpdateUserInfo(int userId, bool isPasswordCorrect, bool updateLastLoginActivityDate, int maxInvalidPasswordAttempts
      , out DateTime? lastActivityDateUtc, out DateTime? lastLoginDateUtc, out AccountStatus? status, out int? failedPasswordAttemptCount, out DateTime? lastLockoutDateUtc)
    {
      return _wm_Users_UpdateUserInfo(userId, isPasswordCorrect, updateLastLoginActivityDate, maxInvalidPasswordAttempts
      , out lastActivityDateUtc, out lastLoginDateUtc, out status, out failedPasswordAttemptCount, out lastLockoutDateUtc);
    }
    private int _wm_Users_UpdateUserInfo(int? userId, bool? isPasswordCorrect, bool? updateLastLoginActivityDate, int? maxInvalidPasswordAttempts
      , out DateTime? lastActivityDateUtc, out DateTime? lastLoginDateUtc, out AccountStatus? status, out int? failedPasswordAttemptCount, out DateTime? lastLockoutDateUtc)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_UpdateUserInfo";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@IsPasswordCorrect", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isPasswordCorrect == null ? (object)DBNull.Value : isPasswordCorrect));
        cmd.Parameters.Add(new SqlParameter("@UpdateLastLoginActivityDate", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, updateLastLoginActivityDate == null ? (object)DBNull.Value : updateLastLoginActivityDate));
        cmd.Parameters.Add(new SqlParameter("@MaxInvalidPasswordAttempts", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, maxInvalidPasswordAttempts == null ? (object)DBNull.Value : maxInvalidPasswordAttempts));

        cmd.Parameters.Add(new SqlParameter("@LastActivityDateUtc", SqlDbType.DateTime, 8, ParameterDirection.Output, false, ((byte)(23)), ((byte)(3)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@LastLoginDateUtc", SqlDbType.DateTime, 8, ParameterDirection.Output, false, ((byte)(23)), ((byte)(3)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.TinyInt, 1, ParameterDirection.Output, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@FailedPasswordAttemptCount", SqlDbType.Int, 4, ParameterDirection.Output, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@LastLockoutDateUtc", SqlDbType.DateTime, 8, ParameterDirection.Output, false, ((byte)(23)), ((byte)(3)), string.Empty, DataRowVersion.Default, null));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);

        if (returnValue == 0)
        {
          lastActivityDateUtc = (DateTime?)cmd.Parameters["@LastActivityDateUtc"].Value;
          lastLoginDateUtc = (DateTime?)cmd.Parameters["@LastLoginDateUtc"].Value;
          status = (AccountStatus?)(byte?)cmd.Parameters["@Status"].Value;
          failedPasswordAttemptCount = (int?)cmd.Parameters["@FailedPasswordAttemptCount"].Value;
          lastLockoutDateUtc = (DateTime?)cmd.Parameters["@LastLockoutDateUtc"].Value;
        }
        else
        {
          lastActivityDateUtc = null;
          lastLoginDateUtc = null;
          status = null;
          failedPasswordAttemptCount = null;
          lastLockoutDateUtc = null;
        }
      }

      return returnValue;
    }

    public wm_User_GetPassword_QueryResult wm_Users_GetPassword(int applicationId, string userName, string email)
    {
      return _wm_User_GetPassword(applicationId, userName, email);
    }
    private wm_User_GetPassword_QueryResult _wm_User_GetPassword(int? applicationId, string userName, string email)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_GetPassword";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, userName == null ? (object)DBNull.Value : userName));
        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, email == null ? (object)DBNull.Value : email));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        wm_User_GetPassword_QueryResult record = new wm_User_GetPassword_QueryResult();
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record.Password = dr["Password"] == DBNull.Value ? null : (string)dr["Password"];
            record.AccountStatus = (AccountStatus)(byte)dr["Status"];
            record.UserId = (int)dr["UserId"];
            record.Email = dr["Email"] == DBNull.Value ? null : (string)dr["Email"];
            record.DateCreatedUtc = new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc);
            record.LastLoginDateUtc = new DateTime(((DateTime)dr["LastLoginDateUtc"]).Ticks, DateTimeKind.Utc);
            record.PasswordFormat = (byte)dr["PasswordFormat"];
            record.PasswordSalt = dr["PasswordSalt"] == DBNull.Value ? null : (string)dr["PasswordSalt"];
            record.ProfileImageId = (int)dr["ProfileImageId"];
            record.UserName = dr["UserName"] == DBNull.Value ? null : (string)dr["UserName"];
            record.TimeZoneInfoId = dr["TimeZoneInfoId"] == DBNull.Value ? null : (string)dr["TimeZoneInfoId"];

            return record;
          }
        }
      }

      return null;
    }

    public int wm_Users_SetPassword(int userId, string newPassword, string passwordSalt, byte passwordFormat)
    {
      return _wm_User_SetPassword(userId, newPassword, passwordSalt, passwordFormat);
    }
    public int _wm_User_SetPassword(int? userId, string newPassword, string passwordSalt, byte? passwordFormat)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_SetPassword";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@NewPassword", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, newPassword == null ? (object)DBNull.Value : newPassword));
        cmd.Parameters.Add(new SqlParameter("@PasswordSalt", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, passwordSalt == null ? (object)DBNull.Value : passwordSalt));
        cmd.Parameters.Add(new SqlParameter("@PasswordFormat", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, passwordFormat == null ? (object)DBNull.Value : passwordFormat));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int wm_Users_Insert(int applicationId, string userName, string email, string password, string passwordSalt
      , int passwordFormat, AccountStatus status, List<string> roleNames, int profileImageId, Guid uniqueId, UserNameDisplayMode userNameDisplayMode
      , string timeZoneInfoId, string firstName, string lastName, Gender gender, out int userId, out DateTime dateCreatedUtc)
    {
      int? uid;
      DateTime? dcu;

      int returnValue = _wm_User_Insert(applicationId, userName, email, password, passwordSalt, passwordFormat, (byte)status
        , (roleNames != null && roleNames.Count > 0 ? "'" + ConversionHelper.GetCollectionString(roleNames, "','") + "'" : null)
        , profileImageId, uniqueId, (byte)userNameDisplayMode, timeZoneInfoId, firstName, lastName, (byte)gender, out uid, out dcu);

      userId = uid.HasValue ? uid.Value : int.MinValue;
      dateCreatedUtc = dcu.HasValue ? dcu.Value : DateTime.MinValue;

      return returnValue;
    }
    private int _wm_User_Insert(int? applicationId, string userName, string email, string password, string passwordSalt
      , int? passwordFormat, byte? status, string roleNames, int? profileImageId, Guid? uniqueId, byte? userNameDisplayMode
      , string timeZoneInfoId, string firstName, string lastName, byte? gender, out int? userId, out DateTime? dateCreatedUtc)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_Insert";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, userName == null ? (object)DBNull.Value : userName));
        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, email == null ? (object)DBNull.Value : email));
        cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, password == null ? (object)DBNull.Value : password));
        cmd.Parameters.Add(new SqlParameter("@PasswordSalt", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, passwordSalt == null ? (object)DBNull.Value : passwordSalt));
        cmd.Parameters.Add(new SqlParameter("@PasswordFormat", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, passwordFormat == null ? (object)DBNull.Value : passwordFormat));
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, status == null ? (object)DBNull.Value : status));
        cmd.Parameters.Add(new SqlParameter("@RoleNames", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, roleNames == null ? (object)DBNull.Value : roleNames));
        cmd.Parameters.Add(new SqlParameter("@ProfileImageId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, profileImageId == null ? (object)DBNull.Value : profileImageId));
        cmd.Parameters.Add(new SqlParameter("@UniqueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, uniqueId == null ? (object)DBNull.Value : uniqueId));
        cmd.Parameters.Add(new SqlParameter("@UserNameDisplayMode", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, userNameDisplayMode == null ? (object)DBNull.Value : userNameDisplayMode));
        cmd.Parameters.Add(new SqlParameter("@Gender", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, gender == null ? (object)DBNull.Value : gender));
        cmd.Parameters.Add(new SqlParameter("@TimeZoneInfoId", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, timeZoneInfoId == null ? (object)DBNull.Value : timeZoneInfoId));
        cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, firstName == null ? (object)DBNull.Value : firstName));
        cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, lastName == null ? (object)DBNull.Value : lastName));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Output, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@DateCreatedUtc", SqlDbType.DateTime, 8, ParameterDirection.Output, false, ((byte)(23)), ((byte)(3)), string.Empty, DataRowVersion.Default, null));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);

        userId = cmd.Parameters["@UserId"].Value == DBNull.Value ? null : (int?)cmd.Parameters["@UserId"].Value;
        dateCreatedUtc = cmd.Parameters["@DateCreatedUtc"].Value == DBNull.Value ? null : (DateTime?)cmd.Parameters["@DateCreatedUtc"].Value;
      }

      return returnValue;
    }

    public int wm_UserRole_Insert(IEnumerable<int> userIds, IEnumerable<string> roleNames)
    {
      return _wm_UserRole_Insert(ConversionHelper.GetCollectionString(userIds, ","), "'" + ConversionHelper.GetCollectionString(roleNames, "','") + "'");
    }
    private int _wm_UserRole_Insert(string userIds, string roleNames)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_UserRole_Insert";

        cmd.Parameters.Add(new SqlParameter("@UserIds", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, userIds == null ? (object)DBNull.Value : userIds));
        cmd.Parameters.Add(new SqlParameter("@RoleNames", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, roleNames == null ? (object)DBNull.Value : roleNames));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int wm_UserRole_Delete(IEnumerable<int> userIds, IEnumerable<string> roleNames)
    {
      return _wm_UserRole_Delete(ConversionHelper.GetCollectionString(userIds, ","), "'" + ConversionHelper.GetCollectionString(roleNames, "','") + "'");
    }
    private int _wm_UserRole_Delete(string userIds, string roleNames)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_UserRole_Delete";

        cmd.Parameters.Add(new SqlParameter("@UserIds", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, userIds == null ? (object)DBNull.Value : userIds));
        cmd.Parameters.Add(new SqlParameter("@RoleNames", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, roleNames == null ? (object)DBNull.Value : roleNames));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int wm_Roles_InsertIfNotExists(int applicationId, string roleName, string description)
    {
      return _wm_Roles_InsertIfNotExists(applicationId, roleName, description);
    }
    private int _wm_Roles_InsertIfNotExists(int? applicationId, string roleName, string description)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Roles_InsertIfNotExists";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, roleName == null ? (object)DBNull.Value : roleName));
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 512, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, description == null ? (object)DBNull.Value : description));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public IEnumerable<string> wm_Roles_GetByUserId(int userId)
    {
      return _wm_Roles_GetByUserId(userId);
    }
    private IEnumerable<string> _wm_Roles_GetByUserId(int? userId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Roles_GetByUserId";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            if (dr["RoleName"] != DBNull.Value)
              yield return (string)dr["RoleName"];
          }
        }
      }
    }

    public IEnumerable<int> wm_UserRole_GetByRoleName(int applicationId, string roleName)
    {
      return _wm_UserRole_GetByRoleName(applicationId, roleName);
    }
    private IEnumerable<int> _wm_UserRole_GetByRoleName(int? applicationId, string roleName)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_UserRole_GetByRoleName";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@RoleName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, roleName == null ? (object)DBNull.Value : roleName));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            if (dr["UserId"] != DBNull.Value)
              yield return (int)dr["UserId"];
          }
        }
      }
    }

    public Dictionary<int, IUserBasic> wm_Users_GetAllUserBasics(int applicationId)
    {
      return _wm_Users_GetUserBasic(applicationId);
    }
    private Dictionary<int, IUserBasic> _wm_Users_GetUserBasic(int applicationId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      Dictionary<int, List<string>> userRoleLookup = new Dictionary<int, List<string>>();
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_UserRole_GetByApplicationId";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            if (!userRoleLookup.ContainsKey((int)dr["UserId"]))
              userRoleLookup[(int)dr["UserId"]] = new List<string>();

            userRoleLookup[(int)dr["UserId"]].Add((string)dr["RoleName"]);
          }
        }
      }

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_GetUserBasic";

        cmd.Parameters.Add(new SqlParameter("@UniqueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId));
        cmd.Parameters.Add(new SqlParameter("@UpdateLastActivity", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, false));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        Dictionary<int, IUserBasic> returnDict = new Dictionary<int, IUserBasic>();
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            returnDict[(int)dr["UserId"]] = new UserBasic(
              (int)dr["UserId"]
              , (string)dr["UserName"]
              , (string)dr["Email"]
              , new DateTime(((DateTime)dr["LastActivityDateUtc"]).Ticks, DateTimeKind.Utc)
              , (AccountStatus)(byte)dr["Status"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , new DateTime(((DateTime)dr["LastLoginDateUtc"]).Ticks, DateTimeKind.Utc)
              , (int)dr["ProfileImageId"]
              , (string)dr["TimeZoneInfoId"]
              , userRoleLookup.ContainsKey((int)dr["UserId"]) ? userRoleLookup[(int)dr["UserId"]] : new List<string>());
          }
        }
        return returnDict;
      }
    }

    public IUserBasic wm_Users_GetUserBasicByUniqueId(Guid uniqueId, bool updateLastActivity)
    {
      return _wm_Users_GetUserBasic(uniqueId, null, null, null, null, updateLastActivity);
    }
    public IUserBasic wm_Users_GetUserBasicByEmail(string email, int applicationId, bool updateLastActivity)
    {
      return _wm_Users_GetUserBasic(null, email, null, null, applicationId, updateLastActivity);
    }
    public IUserBasic wm_Users_GetUserBasicByUserName(string userName, int applicationId, bool updateLastActivity)
    {
      return _wm_Users_GetUserBasic(null, null, userName, null, applicationId, updateLastActivity);
    }
    public IUserBasic wm_Users_GetUserBasicByUserId(int userId, bool updateLastActivity)
    {
      return _wm_Users_GetUserBasic(null, null, null, userId, null, updateLastActivity);
    }
    private IUserBasic _wm_Users_GetUserBasic(Guid? uniqueId, string email, string userName, int? userId, int? applicationId, bool? updateLastActivity)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      UserBasic userBasic = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_GetUserBasic";

        cmd.Parameters.Add(new SqlParameter("@UniqueId", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, uniqueId == null ? (object)DBNull.Value : uniqueId));
        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, email == null ? (object)DBNull.Value : email));
        cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, userName == null ? (object)DBNull.Value : userName));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@UpdateLastActivity", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, updateLastActivity == null ? (object)DBNull.Value : updateLastActivity));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          if (dr.HasRows)
          {
            while (dr.Read())
            {
              userBasic = new UserBasic(
                (int)dr["UserId"]
                , (string)dr["UserName"]
                , (string)dr["Email"]
                , new DateTime(((DateTime)dr["LastActivityDateUtc"]).Ticks, DateTimeKind.Utc)
                , (AccountStatus)(byte)dr["Status"]
                , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
                , new DateTime(((DateTime)dr["LastLoginDateUtc"]).Ticks, DateTimeKind.Utc)
                , (int)dr["ProfileImageId"]
                , (string)dr["TimeZoneInfoId"]
                , null);
            }
          }
        }
      }

      if (userBasic == null)
        return null;

      userBasic.UserRoles = new HashSet<string>(_wm_Roles_GetByUserId(userBasic.UserId));
      return userBasic;
    }

    public IUserModel wm_Users_GetUserModel(int userId)
    {
      return _wm_Users_GetUserModel(userId);
    }
    private IUserModel _wm_Users_GetUserModel(int? userId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      UserModel record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_GetUserModel";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new UserModel(
              (int)dr["UserId"]
              , (string)dr["Email"]
              , (string)dr["UserName"]
              , (AccountStatus)(byte)dr["Status"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , new DateTime(((DateTime)dr["LastLoginDateUtc"]).Ticks, DateTimeKind.Utc)
              , new DateTime(((DateTime)dr["LastPasswordChangeDateUtc"]).Ticks, DateTimeKind.Utc)
              , new DateTime(((DateTime)dr["LastLockoutDateUtc"]).Ticks, DateTimeKind.Utc)
              , new DateTime(((DateTime)dr["LastActivityDateUtc"]).Ticks, DateTimeKind.Utc)
              , (int)dr["FailedPasswordAttemptCount"]
              , XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None)
              , (string)dr["TimeZoneInfoId"]
              , (int)dr["ProfileImageId"]
              , (Guid)dr["UniqueId"]
              , (UserNameDisplayMode)(byte)dr["UserNameDisplayMode"]
              , (string)dr["FirstName"]
              , (string)dr["LastName"]
              , (Gender)(byte)dr["Gender"]);
          }
        }
        if (record != null)
          record.UserRoles = new HashSet<string>(_wm_Roles_GetByUserId(userId));
      }
      return record;
    }

    public int wm_Users_GetNumberOfUsersOnline(int applicationId, int minutesSinceLastInActive)
    {
      return _wm_Users_GetNumberOfUsersOnline(applicationId, minutesSinceLastInActive);
    }
    private int _wm_Users_GetNumberOfUsersOnline(int? applicationId, int? minutesSinceLastInActive)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_GetNumberOfUsersOnline";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@MinutesSinceLastInActive", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, minutesSinceLastInActive == null ? (object)DBNull.Value : minutesSinceLastInActive));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public ChangeCredentialsStatus wm_Users_UpdateEmail(int userId, string email)
    {
      return _wm_Users_UpdateCredentials(userId, null, email);
    }
    public ChangeCredentialsStatus wm_Users_UpdateUserName(int userId, string newUserName)
    {
      return _wm_Users_UpdateCredentials(userId, newUserName, null);
    }
    private ChangeCredentialsStatus _wm_Users_UpdateCredentials(int? userId, string newUserName, string newEmail)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_UpdateCredentials";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@NewUserName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, newUserName == null ? (object)DBNull.Value : newUserName));
        cmd.Parameters.Add(new SqlParameter("@NewEmail", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, newEmail == null ? (object)DBNull.Value : newEmail));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      switch (returnValue)
      {
        case -1: return ChangeCredentialsStatus.UsernameAlreadyExists;
        case -2: return ChangeCredentialsStatus.EmailAlreadyExists;
        case -3: return ChangeCredentialsStatus.RecordNotFound;
        case 0: return ChangeCredentialsStatus.Success;

        default: return ChangeCredentialsStatus.UnknownError;
      }
    }

    public List<IBaseUserModel> wm_Users_GetBaseUserModels(int applicationId, string searchTerm, ref int pageIndex, int pageSize, out int rowCount)
    {
      return _wm_Users_GetBaseUserModels(applicationId, searchTerm, ref  pageIndex, pageSize, out  rowCount);
    }
    private List<IBaseUserModel> _wm_Users_GetBaseUserModels(int? applicationId, string searchTerm, ref int pageIndex, int pageSize, out int rowCount)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      List<IBaseUserModel> records = new List<IBaseUserModel>();
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Users_GetBaseUserModels";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@SearchTerm", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, searchTerm == null ? (object)DBNull.Value : searchTerm));
        cmd.Parameters.Add(new SqlParameter("@PageIndex", SqlDbType.Int, 4, ParameterDirection.InputOutput, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, pageIndex));
        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, pageSize == null ? (object)DBNull.Value : pageSize));
        cmd.Parameters.Add(new SqlParameter("@RowCount", SqlDbType.Int, 4, ParameterDirection.Output, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            records.Add(new BaseUserModel(
              (int)dr["UserId"]
              , dr["Email"] as string
              , (int)dr["ProfileImageId"]
              , dr["FirstName"] as string
              , dr["LastName"] as string));
          }
        }
        pageIndex = (int)cmd.Parameters["@PageIndex"].Value;
        rowCount = (int)cmd.Parameters["@RowCount"].Value;
      }
      return records;
    }
    #endregion

    #region cms

    #region groups
    public int cms_Groups_Delete(int groupId)
    {
      return _cms_Groups_Delete(groupId);
    }
    private int _cms_Groups_Delete(int? groupId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Groups_Delete";

        cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupId == null ? (object)DBNull.Value : groupId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int cms_Groups_InsertOrUpdate(int groupId, string name, string description, CMSGroupType groupType)
    {
      return _cms_Groups_InsertOrUpdate(groupId, name, description, (byte)groupType);
    }
    private int _cms_Groups_InsertOrUpdate(int? groupId, string name, string description, byte? groupType)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Groups_InsertOrUpdate";

        cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupId == null ? (object)DBNull.Value : groupId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, description == null ? (object)DBNull.Value : description));
        cmd.Parameters.Add(new SqlParameter("@GroupType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupType == null ? (object)DBNull.Value : groupType));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public CMSGroup cms_Groups_Get(int groupId)
    {
      return _cms_Groups_Get(groupId);
    }
    private CMSGroup _cms_Groups_Get(int? groupId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Groups_Get";

        cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupId == null ? (object)DBNull.Value : groupId));
        cmd.Parameters.Add(new SqlParameter("@GroupType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, DBNull.Value));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            return new CMSGroup(
              (int)dr["GroupId"]
              , dr["Name"] as string
              , dr["Description"] as string
              , (CMSGroupType)(byte)dr["GroupType"]);
          }
        }
      }
      return null;
    }

    public IEnumerable<CMSGroup> cms_Groups_Get(CMSGroupType groupType)
    {
      return _cms_Groups_Get((byte)groupType);
    }
    public IEnumerable<CMSGroup> cms_Groups_GetAll()
    {
      return _cms_Groups_Get(null);
    }
    private IEnumerable<CMSGroup> _cms_Groups_Get(byte? groupType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Groups_Get";

        cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, DBNull.Value));
        cmd.Parameters.Add(new SqlParameter("@GroupType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupType == null ? (object)DBNull.Value : groupType));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new CMSGroup(
               (int)dr["GroupId"]
               , dr["Name"] as string
               , dr["Description"] as string
               , (CMSGroupType)(byte)dr["GroupType"]);
          }
        }
      }
    }
    #endregion

    #region sections
    public int cms_Sections_Insert(int applicationId, int? parentSectionId, int? groupId, string name, string description, CMSSectionType sectionType, bool isActive, bool isModerated)
    {
      return _cms_Sections_Insert(applicationId, parentSectionId, groupId, name, description, (byte)sectionType, isActive, isModerated);
    }
    private int _cms_Sections_Insert(int? applicationId, int? parentSectionId, int? groupId, string name, string description, byte? sectionType, bool? isActive, bool? isModerated)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Sections_Insert";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@ParentSectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, parentSectionId == null ? (object)DBNull.Value : parentSectionId));
        cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupId == null ? (object)DBNull.Value : groupId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, description == null ? (object)DBNull.Value : description));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isActive == null ? (object)DBNull.Value : isActive));
        cmd.Parameters.Add(new SqlParameter("@IsModerated", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isModerated == null ? (object)DBNull.Value : isModerated));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_Sections_Update(int sectionId, int? parentSectionId, int? groupId, string name, string description, CMSSectionType sectionType, bool isActive, bool isModerated)
    {
      return _cms_Sections_Update(sectionId, parentSectionId, groupId, name, description, (byte)sectionType, isActive, isModerated);
    }
    private int _cms_Sections_Update(int? sectionId, int? parentSectionId, int? groupId, string name, string description, byte? sectionType, bool? isActive, bool? isModerated)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Sections_Update";

        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionId == null ? (object)DBNull.Value : sectionId));
        cmd.Parameters.Add(new SqlParameter("@ParentSectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, parentSectionId == null ? (object)DBNull.Value : parentSectionId));
        cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupId == null ? (object)DBNull.Value : groupId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 1024, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, description == null ? (object)DBNull.Value : description));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isActive == null ? (object)DBNull.Value : isActive));
        cmd.Parameters.Add(new SqlParameter("@IsModerated", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isModerated == null ? (object)DBNull.Value : isModerated));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_Sections_Delete(int sectionId, bool deleteLinkedThreads)
    {
      return _cms_Sections_Delete(sectionId, deleteLinkedThreads);
    }
    private int _cms_Sections_Delete(int? sectionId, bool? deleteLinkedThreads)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Sections_Delete";

        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionId == null ? (object)DBNull.Value : sectionId));
        cmd.Parameters.Add(new SqlParameter("@DeleteLinkedThreads", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, deleteLinkedThreads == null ? (object)DBNull.Value : deleteLinkedThreads));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public CMSSection cms_Sections_Get(int applicationId, CMSSectionType sectionType, string name)
    {
      return _cms_Sections_Get((byte)sectionType, null, name, applicationId);
    }
    public CMSSection cms_Sections_Get(CMSSectionType sectionType, int sectionId)
    {
      return _cms_Sections_Get((byte)sectionType, sectionId, null, null);
    }
    private CMSSection _cms_Sections_Get(byte? sectionType, int? sectionId, string name, int? applicationId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSSection record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Sections_Get";

        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionId == null ? (object)DBNull.Value : sectionId));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSSection(
              (int)dr["ApplicationId"]
              , (int)dr["SectionId"]
              , dr["ParentSectionId"] == DBNull.Value ? null : (int?)dr["ParentSectionId"]
              , dr["GroupId"] == DBNull.Value ? null : (int?)dr["GroupId"]
              , dr["Name"] as string
              , dr["Description"] as string
              , (CMSSectionType)(byte)dr["SectionType"]
              , (bool)dr["IsActive"]
              , (bool)dr["IsModerated"]
              , (int)dr["TotalContents"]
              , (int)dr["TotalThreads"]);
          }
        }
      }

      return record;
    }

    public int cms_Sections_UpdateContentBlock(int applicationId, string name, string formattedBody, int authorUserId)
    {
      return _cms_Sections_UpdateContentBlock(applicationId, name, formattedBody, (byte)CMSSectionType.Content
        , (byte)ContentBlockStatus.Active, (byte)ContentBlockStatus.Inactive, authorUserId, true);
    }
    private int _cms_Sections_UpdateContentBlock(int? applicationId, string name, string formattedBody, byte? sectionType, byte? activeContentBlockStatus, byte? inactiveContentBlockStatus, int? authorUserId, bool? createPlaceholderIfNotExists)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Sections_UpdateContentBlock";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@FormattedBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, formattedBody == null ? (object)DBNull.Value : formattedBody));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@ActiveContentBlockStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, activeContentBlockStatus == null ? (object)DBNull.Value : activeContentBlockStatus));
        cmd.Parameters.Add(new SqlParameter("@InactiveContentBlockStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, inactiveContentBlockStatus == null ? (object)DBNull.Value : inactiveContentBlockStatus));
        cmd.Parameters.Add(new SqlParameter("@AuthorUserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, authorUserId == null ? (object)DBNull.Value : authorUserId));
        cmd.Parameters.Add(new SqlParameter("@CreatePlaceholderIfNotExists", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, createPlaceholderIfNotExists == null ? (object)DBNull.Value : createPlaceholderIfNotExists));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public IEnumerable<CMSSection> cms_Sections_Get(int applicationId)
    {
      return _cms_Sections_Get(applicationId, null);
    }
    public IEnumerable<CMSSection> cms_Sections_Get(int applicationId, CMSSectionType sectionType)
    {
      return _cms_Sections_Get(applicationId, (byte)sectionType);
    }
    public IEnumerable<CMSSection> cms_Sections_GetAll()
    {
      return _cms_Sections_Get(null, null);
    }
    private IEnumerable<CMSSection> _cms_Sections_Get(int? applicationId, byte? sectionType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Sections_Get";

        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, DBNull.Value));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, DBNull.Value));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new CMSSection(
              (int)dr["ApplicationId"]
              , (int)dr["SectionId"]
              , dr["ParentSectionId"] == DBNull.Value ? null : (int?)dr["ParentSectionId"]
              , dr["GroupId"] == DBNull.Value ? null : (int?)dr["GroupId"]
              , dr["Name"] as string
              , dr["Description"] as string
              , (CMSSectionType)(byte)dr["SectionType"]
              , (bool)dr["IsActive"]
              , (bool)dr["IsModerated"]
              , (int)dr["TotalContents"]
              , (int)dr["TotalThreads"]);
          }
        }
      }
    }
    #endregion

    #region threads
    public CMSThread cms_Threads_Get(CMSSectionType sectionType, int threadId)
    {
      return _cms_Threads_Get(threadId, (byte)sectionType, null, null);
    }
    public CMSThread cms_Threads_Get(CMSSectionType sectionType, string name)
    {
      return _cms_Threads_Get(null, (byte)sectionType, name, null);
    }
    private CMSThread _cms_Threads_Get(int? threadId, byte? sectionType, string name, int? applicationId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSThread record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Threads_Get";

        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSThread(
              (int)dr["ThreadId"]
              , (int)dr["SectionId"]
              , dr["Name"] as string
              , new DateTime(((DateTime)dr["LastViewedDateUtc"]).Ticks, DateTimeKind.Utc)
              , dr["StickyDateUtc"] == DBNull.Value ? null : (DateTime?)new DateTime(((DateTime)dr["StickyDateUtc"]).Ticks, DateTimeKind.Utc)
              , (int)dr["TotalViews"]
              , (int)dr["TotalReplies"]
              , (bool)dr["IsLocked"]
              , (bool)dr["IsSticky"]
              , (bool)dr["IsApproved"]
              , (int)dr["RatingSum"]
              , (int)dr["TotalRatings"]
              , (int)dr["ThreadStatus"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc));
          }
        }
      }
      return record;
    }

    public IEnumerable<CMSThread> cms_Threads_Get(CMSSectionType sectionType)
    {
      return _cms_Threads_GetMultiple(null, (byte)sectionType, null, null);
    }
    public IEnumerable<CMSThread> cms_Threads_Get(int applicationId)
    {
      return _cms_Threads_GetMultiple(null, null, null, applicationId);
    }
    public IEnumerable<CMSThread> cms_Threads_Get(int applicationId, CMSSectionType sectionType)
    {
      return _cms_Threads_GetMultiple(null, (byte)sectionType, null, applicationId);
    }
    private IEnumerable<CMSThread> _cms_Threads_GetMultiple(int? threadId, byte? sectionType, string name, int? applicationId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Threads_Get";

        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new CMSThread(
              (int)dr["ThreadId"]
              , (int)dr["SectionId"]
              , dr["Name"] as string
              , new DateTime(((DateTime)dr["LastViewedDateUtc"]).Ticks, DateTimeKind.Utc)
              , dr["StickyDateUtc"] == DBNull.Value ? null : (DateTime?)new DateTime(((DateTime)dr["StickyDateUtc"]).Ticks, DateTimeKind.Utc)
              , (int)dr["TotalViews"]
              , (int)dr["TotalReplies"]
              , (bool)dr["IsLocked"]
              , (bool)dr["IsSticky"]
              , (bool)dr["IsApproved"]
              , (int)dr["RatingSum"]
              , (int)dr["TotalRatings"]
              , (int)dr["ThreadStatus"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc));
          }
        }
      }
    }

    public int cms_Threads_Insert(int sectionId, string name, DateTime? stickyDateUtc, bool isLocked, bool isSticky, bool isApproved, int threadStatus)
    {
      return _cms_Threads_Insert(sectionId, name, stickyDateUtc, isLocked, isSticky, isApproved, threadStatus);
    }
    private int _cms_Threads_Insert(int? sectionId, string name, DateTime? stickyDateUtc, bool? isLocked, bool? isSticky, bool? isApproved, int? threadStatus)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Threads_Insert";

        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionId == null ? (object)DBNull.Value : sectionId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 32, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@StickyDateUtc", SqlDbType.DateTime, 8, ParameterDirection.Input, false, ((byte)(23)), ((byte)(3)), string.Empty, DataRowVersion.Default, stickyDateUtc == null ? (object)DBNull.Value : stickyDateUtc));
        cmd.Parameters.Add(new SqlParameter("@IsLocked", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isLocked == null ? (object)DBNull.Value : isLocked));
        cmd.Parameters.Add(new SqlParameter("@IsSticky", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isSticky == null ? (object)DBNull.Value : isSticky));
        cmd.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isApproved == null ? (object)DBNull.Value : isApproved));
        cmd.Parameters.Add(new SqlParameter("@ThreadStatus", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadStatus == null ? (object)DBNull.Value : threadStatus));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_Threads_Update(int threadId, int sectionId, string name, DateTime lastViewedDateUtc, DateTime? stickyDateUtc, bool isLocked, bool isSticky, bool isApproved, int threadStatus)
    {
      return _cms_Threads_Update(threadId, sectionId, name, lastViewedDateUtc, stickyDateUtc, isLocked, isSticky, isApproved, threadStatus);
    }
    private int _cms_Threads_Update(int? threadId, int? sectionId, string name, DateTime? lastViewedDateUtc, DateTime? stickyDateUtc, bool? isLocked, bool? isSticky, bool? isApproved, int? threadStatus)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Threads_Update";

        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionId == null ? (object)DBNull.Value : sectionId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 32, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@LastViewedDateUtc", SqlDbType.DateTime, 8, ParameterDirection.Input, false, ((byte)(23)), ((byte)(3)), string.Empty, DataRowVersion.Default, lastViewedDateUtc == null ? (object)DBNull.Value : lastViewedDateUtc));
        cmd.Parameters.Add(new SqlParameter("@StickyDateUtc", SqlDbType.DateTime, 8, ParameterDirection.Input, false, ((byte)(23)), ((byte)(3)), string.Empty, DataRowVersion.Default, stickyDateUtc == null ? (object)DBNull.Value : stickyDateUtc));
        cmd.Parameters.Add(new SqlParameter("@IsLocked", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isLocked == null ? (object)DBNull.Value : isLocked));
        cmd.Parameters.Add(new SqlParameter("@IsSticky", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isSticky == null ? (object)DBNull.Value : isSticky));
        cmd.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isApproved == null ? (object)DBNull.Value : isApproved));
        cmd.Parameters.Add(new SqlParameter("@ThreadStatus", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadStatus == null ? (object)DBNull.Value : threadStatus));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_Threads_Delete(int threadId, bool deleteLinkedThreads)
    {
      return _cms_Threads_Delete(threadId, deleteLinkedThreads);
    }
    private int _cms_Threads_Delete(int? threadId, bool? deleteLinkedThreads)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Threads_Delete";

        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@DeleteLinkedThreads", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, deleteLinkedThreads == null ? (object)DBNull.Value : deleteLinkedThreads));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_Threads_IncreaseTotalViews(int threadId, int numberOfViewsToAdd)
    {
      return _cms_Threads_IncreaseTotalViews(threadId, numberOfViewsToAdd);
    }
    private int _cms_Threads_IncreaseTotalViews(int? threadId, int? numberOfViewsToAdd)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Threads_IncreaseTotalViews";

        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@NumberOfViewsToAdd", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, numberOfViewsToAdd == null ? (object)DBNull.Value : numberOfViewsToAdd));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region contents

    public CMSContent cms_Contents_Get(int contentId)
    {
      return _cms_Contents_Get(contentId, null, null, null, null, null, null, null);
    }
    public CMSContent cms_Contents_Get(string urlFriendlyName, string threadName, string sectionName, string groupName)
    {
      return _cms_Contents_Get(null, null, null, null, urlFriendlyName, threadName, sectionName, groupName);
    }
    private CMSContent _cms_Contents_Get(int? contentId, byte? sectionType, byte? groupType, int? applicationId
      , string urlFriendlyName, string threadName, string sectionName, string groupName)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSContent record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_Get";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@GroupType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupType == null ? (object)DBNull.Value : groupType));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@UrlFriendlyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, urlFriendlyName == null ? (object)DBNull.Value : urlFriendlyName));
        cmd.Parameters.Add(new SqlParameter("@ThreadName", SqlDbType.NVarChar, 32, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadName == null ? (object)DBNull.Value : threadName));
        cmd.Parameters.Add(new SqlParameter("@SectionName", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionName == null ? (object)DBNull.Value : sectionName));
        cmd.Parameters.Add(new SqlParameter("@GroupName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupName == null ? (object)DBNull.Value : groupName));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSContent(
              (int)dr["ContentId"]
              , (int)dr["ThreadId"]
              , dr["ParentContentId"] == DBNull.Value ? null : (int?)dr["ParentContentId"]
              , (int)dr["AuthorUserId"]
              , (short)dr["ContentLevel"]
              , dr["Subject"] as string
              , dr["FormattedBody"] as string
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , (bool)dr["IsApproved"]
              , (bool)dr["IsLocked"]
              , (int)dr["TotalViews"]
              , (byte)dr["ContentType"]
              , (int)dr["RatingSum"]
              , (int)dr["TotalRatings"]
              , (byte)dr["ContentStatus"]
              , XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None)
              , dr["BaseContentId"] == DBNull.Value ? null : (int?)dr["BaseContentId"]
              , dr["UrlFriendlyName"] as string
              , dr["ContentLevelNodeId"] == DBNull.Value ? null : (int?)dr["ContentLevelNodeId"]
              );
          }
        }
      }
      return record;
    }

    public IEnumerable<CMSContent> cms_Contents_Get(CMSSectionType sectionType)
    {
      return _cms_Contents_GetMultiple(null, (byte)sectionType, null, null, null, null, null, null);
    }
    public IEnumerable<CMSContent> cms_Contents_Get(CMSGroupType groupType)
    {
      return _cms_Contents_GetMultiple(null, null, (byte)groupType, null, null, null, null, null);
    }
    public IEnumerable<CMSContent> cms_Contents_Get(int applicationId, CMSSectionType sectionType)
    {
      return _cms_Contents_GetMultiple(null, (byte)sectionType, null, applicationId, null, null, null, null);
    }
    public IEnumerable<CMSContent> cms_Contents_Get(int applicationId, CMSGroupType groupType)
    {
      return _cms_Contents_GetMultiple(null, null, (byte)groupType, applicationId, null, null, null, null);
    }
    private IEnumerable<CMSContent> _cms_Contents_GetMultiple(int? contentId, byte? sectionType, byte? groupType, int? applicationId
      , string urlFriendlyName, string threadName, string sectionName, string groupName)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_Get";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@GroupType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupType == null ? (object)DBNull.Value : groupType));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@UrlFriendlyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, urlFriendlyName == null ? (object)DBNull.Value : urlFriendlyName));
        cmd.Parameters.Add(new SqlParameter("@ThreadName", SqlDbType.NVarChar, 32, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadName == null ? (object)DBNull.Value : threadName));
        cmd.Parameters.Add(new SqlParameter("@SectionName", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionName == null ? (object)DBNull.Value : sectionName));
        cmd.Parameters.Add(new SqlParameter("@GroupName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, groupName == null ? (object)DBNull.Value : groupName));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (SqlDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new CMSContent(
               (int)dr["ContentId"]
               , (int)dr["ThreadId"]
               , dr["ParentContentId"] == DBNull.Value ? null : (int?)dr["ParentContentId"]
               , (int)dr["AuthorUserId"]
               , (short)dr["ContentLevel"]
               , dr["Subject"] as string
               , dr["FormattedBody"] as string
               , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
               , (bool)dr["IsApproved"]
               , (bool)dr["IsLocked"]
               , (int)dr["TotalViews"]
               , (byte)dr["ContentType"]
               , (int)dr["RatingSum"]
               , (int)dr["TotalRatings"]
               , (byte)dr["ContentStatus"]
               , XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None)
               , dr["BaseContentId"] == DBNull.Value ? null : (int?)dr["BaseContentId"]
               , dr["UrlFriendlyName"] as string
               , dr["ContentLevelNodeId"] == DBNull.Value ? null : (int?)dr["ContentLevelNodeId"]
               );
          }
        }
      }
    }

    public List<MessageInfo> cms_Contents_GetMessageInfoPageFromThreadIndex(int messageBoardThreadId, ref int pageIndex, int pageSize, out int rowCount)
    {
      return _cms_Contents_GetMessageInfoPageFromThreadIndex(messageBoardThreadId, ref pageIndex, pageSize, out rowCount);
    }
    private List<MessageInfo> _cms_Contents_GetMessageInfoPageFromThreadIndex(int? threadId, ref int pageIndex, int? pageSize, out int rowCount)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      List<MessageInfo> records = new List<MessageInfo>();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_GetMessageInfoPageFromThreadIndex";

        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@PageIndex", SqlDbType.Int, 4, ParameterDirection.InputOutput, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, pageIndex));
        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, pageSize == null ? (object)DBNull.Value : pageSize));
        cmd.Parameters.Add(new SqlParameter("@RowCount", SqlDbType.Int, 4, ParameterDirection.Output, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            records.Add(new MessageInfo(
              (int)dr["AuthorUserId"]
              , (int)dr["ContentId"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , (Components.Contracts.CMS.MessageBoards.MessageStatus)(byte)dr["ContentStatus"]
              , dr["Subject"] as string
              , dr["FormattedBody"] as string
              , (int)dr["TotalRatings"]
              , (int)dr["RatingSum"]
              , dr["ParentContentId"] == DBNull.Value ? null : (int?)dr["ParentContentId"]
              , (short)dr["ContentLevel"]));
          }
        }
        pageIndex = (int)cmd.Parameters["@PageIndex"].Value;
        rowCount = (int)cmd.Parameters["@RowCount"].Value;
      }

      return records;
    }

    public List<BaseArticleInfo> cms_Contents_GetBaseArticleInfoPage(int? sectionId, int? threadId, List<string> tags, ref int pageIndex, int pageSize, out int rowCount)
    {
      string text = null;
      if (tags != null && tags.Count > 0)
      {
        text = ConversionHelper.GetCollectionString(tags, "','").ToLower();
        text = "'" + text + "'";
      }

      return _cms_Contents_GetBaseArticleInfoPage(sectionId, threadId, text, ref  pageIndex, pageSize, out  rowCount);
    }
    private List<BaseArticleInfo> _cms_Contents_GetBaseArticleInfoPage(int? sectionId, int? threadId, string tags, ref int pageIndex, int? pageSize, out int rowCount)
    {
      List<BaseArticleInfo> records = new List<BaseArticleInfo>();

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_GetBaseArticleInfoPage";

        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionId == null ? (object)DBNull.Value : sectionId));
        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@Tags", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@PageIndex", SqlDbType.Int, 4, ParameterDirection.InputOutput, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, pageIndex));
        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, pageSize == null ? (object)DBNull.Value : pageSize));
        cmd.Parameters.Add(new SqlParameter("@RowCount", SqlDbType.Int, 4, ParameterDirection.Output, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            records.Add(new BaseArticleInfo(
              (int)dr["ContentId"]
              , (int)dr["ThreadId"]
              , (int)dr["SectionId"]
              , dr["UrlFriendlyName"] as string
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["TotalContents"] == DBNull.Value ? 0 : (int)dr["TotalContents"]
              , XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None)
              ));
          }
        }
        pageIndex = (int)cmd.Parameters["@PageIndex"].Value;
        rowCount = (int)cmd.Parameters["@RowCount"].Value;
      }
      return records;
    }

    public IArticleModel cms_Contents_GetArticleModel(int articleId)
    {
      return _cms_Contents_GetArticleModel(articleId, null, null, null, (byte)LinkedThreadRelationshipType.ArticleMessageBoard);
    }
    public IArticleModel cms_Contents_GetArticleModel(string urlFriendlyName, string threadName, string sectionName)
    {
      return _cms_Contents_GetArticleModel(null, (urlFriendlyName ?? string.Empty).Trim(), (threadName ?? string.Empty).Trim()
        , (sectionName ?? string.Empty).Trim(), (byte)LinkedThreadRelationshipType.ArticleMessageBoard);
    }
    private IArticleModel _cms_Contents_GetArticleModel(int? contentId, string urlFriendlyName, string threadName
      , string sectionName, byte? linkedThreadRelationshipType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      ArticleModel articleModel = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_GetArticleModel";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@UrlFriendlyName", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, urlFriendlyName == null ? (object)DBNull.Value : urlFriendlyName));
        cmd.Parameters.Add(new SqlParameter("@ThreadName", SqlDbType.NVarChar, 32, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadName == null ? (object)DBNull.Value : threadName));
        cmd.Parameters.Add(new SqlParameter("@SectionName", SqlDbType.NVarChar, 128, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionName == null ? (object)DBNull.Value : sectionName));
        cmd.Parameters.Add(new SqlParameter("@LinkedThreadRelationshipType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, linkedThreadRelationshipType == null ? (object)DBNull.Value : linkedThreadRelationshipType));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            articleModel = new ArticleModel(
              (int)dr["ContentId"]
            , (int)dr["ThreadId"]
            , (int)dr["SectionId"]
            , dr["UrlFriendlyName"] as string
            , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
            , dr["TotalContents"] == DBNull.Value ? 0 : (int)dr["TotalContents"]
            , XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None)
            , (int)dr["AuthorUserId"]
            , (ArticleStatus)(byte)dr["ContentStatus"]
            , dr["Subject"] as string
            , dr["FormattedBody"] as string
            , (int)dr["LinkedThreadId"]
            , (bool)dr["IsLinkedThreadEnabled"]
            , dr["ContentLevelNodeId"] == DBNull.Value ? 0 : (int)dr["ContentLevelNodeId"]
            , dr["BreadCrumbs"] as string
            , dr["BreadCrumbsSplitIndexes"] as string
            );
          }
        }

        if (articleModel != null)
        {
          articleModel.Author = _wm_Users_GetUserBasic(null, null, null, articleModel.AuthorUserId, null, false);
          articleModel.Attachments = _cms_Files_GetAttachmentModelMultiple(null, articleModel.ArticleId, (byte)FileType.ArticleAttachment).ToList();
        }
      }
      return articleModel;
    }

    public Dictionary<string, string> cms_Contents_GetAllWithSectionName(int applicationId, CMSSectionType sectionType, byte contentStatus)
    {
      return _cms_Contents_GetAllWithSectionName(applicationId, (byte)sectionType, contentStatus);
    }
    private Dictionary<string, string> _cms_Contents_GetAllWithSectionName(int? applicationId, byte? sectionType, byte? contentStatus)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      Dictionary<string, string> records = new Dictionary<string, string>();
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_GetAllWithSectionName";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@SectionType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionType == null ? (object)DBNull.Value : sectionType));
        cmd.Parameters.Add(new SqlParameter("@ContentStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentStatus == null ? (object)DBNull.Value : contentStatus));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            records[dr["LoweredName"] as string] = dr["FormattedBody"] as string;
          }
        }
      }
      return records;
    }

    public BaseRatingInfo cms_Contents_GetBaseRatingInfo(int contentId)
    {
      return _cms_Contents_GetBaseRatingInfo(contentId);
    }
    private BaseRatingInfo _cms_Contents_GetBaseRatingInfo(int? contentId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      BaseRatingInfo record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_GetBaseRatingInfo";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new BaseRatingInfo(
              (int)dr["RatingSum"]
              , (int)dr["TotalRatings"]);
          }
        }
      }
      return record;
    }
    public int cms_Contents_IncreaseTotalViews(int contentId, int numberOfViewsToAdd)
    {
      return _cms_Contents_IncreaseTotalViews(contentId, numberOfViewsToAdd);
    }
    private int _cms_Contents_IncreaseTotalViews(int? contentId, int? numberOfViewsToAdd)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_IncreaseTotalViews";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@NumberOfViewsToAdd", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, numberOfViewsToAdd == null ? (object)DBNull.Value : numberOfViewsToAdd));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int cms_Contents_Insert(int threadId, int? parentContentId, int authorUserId, short contentLevel, string subject
      , string formattedBody, bool isApproved, bool isLocked, byte contentType, byte contentStatus, XElement extraInfo
      , string urlFriendlyName, IEnumerable<string> tags, IEnumerable<string> contentLevelNodes
      , bool? createLinkedThread, int? linkedThreadSectionId, bool? isLinkedThreadEnabled, LinkedThreadRelationshipType? linkedThreadRelationshipType)
    {
      return _cms_Contents_Insert(threadId, parentContentId, authorUserId, contentLevel, subject, formattedBody, isApproved
        , isLocked, contentType, contentStatus, extraInfo == null ? string.Empty : extraInfo.ToString(), urlFriendlyName
        , TagXmlFormatter.GetTagsXml(tags), ContenLevelNodeXmlFormatter.GetXml(contentLevelNodes)
        , createLinkedThread, linkedThreadSectionId, isLinkedThreadEnabled, (byte?)linkedThreadRelationshipType);
    }
    private int _cms_Contents_Insert(int? threadId, int? parentContentId, int? authorUserId, short? contentLevel
      , string subject, string formattedBody, bool? isApproved, bool? isLocked, byte? contentType, byte? contentStatus
      , string extraInfo, string urlFriendlyName, string tagXml, string contentLevelNodesXml
      , bool? createLinkedThread, int? linkedThreadSectionId, bool? isLinkedThreadEnabled, byte? linkedThreadRelationshipType)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_Insert";

        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@ParentContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, parentContentId == null ? (object)DBNull.Value : parentContentId));
        cmd.Parameters.Add(new SqlParameter("@AuthorUserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, authorUserId == null ? (object)DBNull.Value : authorUserId));
        cmd.Parameters.Add(new SqlParameter("@ContentLevel", SqlDbType.SmallInt, 2, ParameterDirection.Input, false, ((byte)(5)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentLevel == null ? (object)DBNull.Value : contentLevel));
        cmd.Parameters.Add(new SqlParameter("@Subject", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, subject == null ? (object)DBNull.Value : subject));
        cmd.Parameters.Add(new SqlParameter("@FormattedBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, formattedBody == null ? (object)DBNull.Value : formattedBody));
        cmd.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isApproved == null ? (object)DBNull.Value : isApproved));
        cmd.Parameters.Add(new SqlParameter("@IsLocked", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isLocked == null ? (object)DBNull.Value : isLocked));
        cmd.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentType == null ? (object)DBNull.Value : contentType));
        cmd.Parameters.Add(new SqlParameter("@ContentStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentStatus == null ? (object)DBNull.Value : contentStatus));
        cmd.Parameters.Add(new SqlParameter("@ExtraInfo", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, extraInfo == null ? (object)DBNull.Value : extraInfo));
        cmd.Parameters.Add(new SqlParameter("@UrlFriendlyName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, urlFriendlyName == null ? (object)DBNull.Value : urlFriendlyName));
        cmd.Parameters.Add(new SqlParameter("@TagXml", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, tagXml == null ? (object)DBNull.Value : tagXml));

        cmd.Parameters.Add(new SqlParameter("@CreateLinkedThread", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, createLinkedThread == null ? (object)DBNull.Value : createLinkedThread));
        cmd.Parameters.Add(new SqlParameter("@LinkedThreadSectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, linkedThreadSectionId == null ? (object)DBNull.Value : linkedThreadSectionId));
        cmd.Parameters.Add(new SqlParameter("@IsLinkedThreadEnabled", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isLinkedThreadEnabled == null ? (object)DBNull.Value : isLinkedThreadEnabled));
        cmd.Parameters.Add(new SqlParameter("@LinkedThreadRelationshipType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, linkedThreadRelationshipType == null ? (object)DBNull.Value : linkedThreadRelationshipType));

        cmd.Parameters.Add(new SqlParameter("@ContentLevelNodesXml", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentLevelNodesXml == null ? (object)DBNull.Value : contentLevelNodesXml));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_Contents_Update(int contentId, int threadId, int? parentContentId, int authorUserId, short contentLevel, string subject, string formattedBody, bool isApproved, bool isLocked, byte contentType, byte contentStatus, XElement extraInfo, int? baseContentId, string urlFriendlyName, IEnumerable<string> tags)
    {
      return _cms_Contents_Update(contentId, threadId, parentContentId, authorUserId, contentLevel, subject, formattedBody, isApproved, isLocked, contentType, contentStatus, extraInfo.ToString(), baseContentId, urlFriendlyName, TagXmlFormatter.GetTagsXml(tags));
    }
    private int _cms_Contents_Update(int? contentId, int? threadId, int? parentContentId, int? authorUserId, short? contentLevel, string subject, string formattedBody, bool? isApproved, bool? isLocked, byte? contentType, byte? contentStatus, string extraInfo, int? baseContentId, string urlFriendlyName, string tagXml)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_Update";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@ParentContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, parentContentId == null ? (object)DBNull.Value : parentContentId));
        cmd.Parameters.Add(new SqlParameter("@AuthorUserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, authorUserId == null ? (object)DBNull.Value : authorUserId));
        cmd.Parameters.Add(new SqlParameter("@ContentLevel", SqlDbType.SmallInt, 2, ParameterDirection.Input, false, ((byte)(5)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentLevel == null ? (object)DBNull.Value : contentLevel));
        cmd.Parameters.Add(new SqlParameter("@Subject", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, subject == null ? (object)DBNull.Value : subject));
        cmd.Parameters.Add(new SqlParameter("@FormattedBody", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, formattedBody == null ? (object)DBNull.Value : formattedBody));
        cmd.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isApproved == null ? (object)DBNull.Value : isApproved));
        cmd.Parameters.Add(new SqlParameter("@IsLocked", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isLocked == null ? (object)DBNull.Value : isLocked));
        cmd.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentType == null ? (object)DBNull.Value : contentType));
        cmd.Parameters.Add(new SqlParameter("@ContentStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentStatus == null ? (object)DBNull.Value : contentStatus));
        cmd.Parameters.Add(new SqlParameter("@ExtraInfo", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, extraInfo == null ? (object)DBNull.Value : extraInfo));
        cmd.Parameters.Add(new SqlParameter("@BaseContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, baseContentId == null ? (object)DBNull.Value : baseContentId));
        cmd.Parameters.Add(new SqlParameter("@UrlFriendlyName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, urlFriendlyName == null ? (object)DBNull.Value : urlFriendlyName));
        cmd.Parameters.Add(new SqlParameter("@TagXml", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, tagXml == null ? (object)DBNull.Value : tagXml));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_Contents_Delete(int contentId, bool deleteLinkedThreads)
    {
      return _cms_Contents_Delete(contentId, deleteLinkedThreads);
    }
    private int _cms_Contents_Delete(int? contentId, bool? deleteLinkedThreads)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Contents_Delete";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@DeleteLinkedThreads", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, deleteLinkedThreads == null ? (object)DBNull.Value : deleteLinkedThreads));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region files
    public CMSFile cms_Files_Get(int fileId)
    {
      return _cms_Files_Get(fileId, null);
    }
    public CMSFile cms_Files_Get(int fileId, FileType fileTypeFilter)
    {
      return _cms_Files_Get(fileId, (byte)fileTypeFilter);
    }
    private CMSFile _cms_Files_Get(int? fileId, byte? fileType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSFile record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_Get";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSFile(
              (int)dr["ApplicationId"]
              , (int)dr["FileId"]
              , dr["UserId"] == DBNull.Value ? null : (int?)dr["UserId"]
              , (FileType)(byte)dr["FileType"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["FileName"] as string
              , (byte[])dr["Content"]
              , dr["ContentType"] as string
              , (int)dr["ContentSize"]
              , dr["FriendlyFileName"] as string
              , (int)dr["Height"]
              , (int)dr["Width"]
              , dr["ContentId"] == DBNull.Value ? null : (int?)dr["ContentId"]
              , false);
          }
        }
      }
      return record;
    }
    public IEnumerable<CMSFile> cms_Files_GetMultiple(int applicationId, FileType fileTypeFilter)
    {
      return _cms_Files_GetMultiple(applicationId, (byte)fileTypeFilter);
    }
    private IEnumerable<CMSFile> _cms_Files_GetMultiple(int? applicationId, byte? fileType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_Get";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new CMSFile(
              (int)dr["ApplicationId"]
              , (int)dr["FileId"]
              , dr["UserId"] == DBNull.Value ? null : (int?)dr["UserId"]
              , (FileType)(byte)dr["FileType"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["FileName"] as string
              , (byte[])dr["Content"]
              , dr["ContentType"] as string
              , (int)dr["ContentSize"]
              , dr["FriendlyFileName"] as string
              , (int)dr["Height"]
              , (int)dr["Width"]
              , null
              , false);
          }
        }
      }
    }

    public ProfileImage cms_Files_GetProfileImage(int userId)
    {
      return _cms_Files_GetProfileImage(userId);
    }
    private ProfileImage _cms_Files_GetProfileImage(int? userId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSFile record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_GetProfileImage";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSFile(
              (int)dr["ApplicationId"]
              , (int)dr["FileId"]
              , dr["UserId"] == DBNull.Value ? null : (int?)dr["UserId"]
              , (FileType)(byte)dr["FileType"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["FileName"] as string
              , (byte[])dr["Content"]
              , dr["ContentType"] as string
              , (int)dr["ContentSize"]
              , dr["FriendlyFileName"] as string
              , (int)dr["Height"]
              , (int)dr["Width"]
              , null
              , true);
          }
        }
      }
      if (record != null)
        return new ProfileImage(record);

      return null;
    }

    public int cms_Files_MoveTempFile(int tempFileId, int? contentId, bool useExistingRecordValues, string fileName, string friendlyFileName, IEnumerable<string> tags)
    {
      return _cms_Files_MoveTempFile(tempFileId, contentId, useExistingRecordValues, fileName, friendlyFileName, TagXmlFormatter.GetTagsXml(tags), false, null);
    }
    public int cms_Files_AssignTemporaryProfileImageToUser(int tempFileId)
    {
      return _cms_Files_MoveTempFile(tempFileId, null, false, null, null, TagXmlFormatter.GetTagsXml(null), true, (byte)FileType.ProfileImage);
    }
    private int _cms_Files_MoveTempFile(int? tempFileId, int? contentId, bool? useExistingRecordValues, string fileName, string friendlyFileName
      , string tagXml, bool? assignProfileImageId, byte? profileImageFileType)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_MoveTempFile";

        cmd.Parameters.Add(new SqlParameter("@TempFileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, tempFileId == null ? (object)DBNull.Value : tempFileId));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@UseExistingRecordValues", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, useExistingRecordValues == null ? (object)DBNull.Value : useExistingRecordValues));
        cmd.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 1024, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileName == null ? (object)DBNull.Value : fileName));
        cmd.Parameters.Add(new SqlParameter("@FriendlyFileName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, friendlyFileName == null ? (object)DBNull.Value : friendlyFileName));
        cmd.Parameters.Add(new SqlParameter("@TagXml", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, tagXml == null ? (object)DBNull.Value : tagXml));
        cmd.Parameters.Add(new SqlParameter("@AssignProfileImageId", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, assignProfileImageId == null ? (object)DBNull.Value : assignProfileImageId));
        cmd.Parameters.Add(new SqlParameter("@ProfileImageFileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, profileImageFileType == null ? (object)DBNull.Value : profileImageFileType));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public IArticleAttachmentModel cms_Files_GetArticleAttachmentModel(int articleAttachmentId)
    {
      return _cms_Files_GetAttachmentModel(articleAttachmentId, null, (byte)FileType.ArticleAttachment);
    }
    private IArticleAttachmentModel _cms_Files_GetAttachmentModel(int? fileId, int? contentId, byte? fileType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      ArticleAttachmentModel articleAttachmentModel = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_GetAttachmentModel";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            articleAttachmentModel = new ArticleAttachmentModel(
              (int)dr["FileId"]
              , (int)dr["ContentId"]
              , (int)dr["UserId"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["FileName"] as string
              , dr["FriendlyFileName"] as string
              , dr["ContentType"] as string
              , (int)dr["ContentSize"]
            );
          }
        }
      }
      return articleAttachmentModel;
    }

    public IEnumerable<IArticleAttachmentModel> cms_Files_GetArticleAttachmentModels(int articleId)
    {
      return _cms_Files_GetAttachmentModelMultiple(null, articleId, (byte)FileType.ArticleAttachment);
    }
    private IEnumerable<IArticleAttachmentModel> _cms_Files_GetAttachmentModelMultiple(int? fileId, int? contentId, byte? fileType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_GetAttachmentModel";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, null));
        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new ArticleAttachmentModel(
              (int)dr["FileId"]
              , (int)dr["ContentId"]
              , (int)dr["UserId"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["FileName"] as string
              , dr["FriendlyFileName"] as string
              , dr["ContentType"] as string
              , (int)dr["ContentSize"]
            );
          }
        }
      }
    }

    public int cms_Files_Insert(int applicationId, int? userId, FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width, int? contentId, IEnumerable<string> tags)
    {
      return _cms_Files_Insert(applicationId, userId, (byte)fileType, fileName, content, contentType, contentSize, friendlyFileName, height, width, contentId, TagXmlFormatter.GetTagsXml(tags), false);
    }
    public int cms_Files_InsertUniqueSystemProfileImage(int applicationId, int? userId, FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width, int? contentId, IEnumerable<string> tags)
    {
      return _cms_Files_Insert(applicationId, userId, (byte)fileType, fileName, content, contentType, contentSize, friendlyFileName, height, width, contentId, TagXmlFormatter.GetTagsXml(tags), true);
    }
    private int _cms_Files_Insert(int? applicationId, int? userId, byte? fileType, string fileName, byte[] content, string contentType, int? contentSize, string friendlyFileName, int? height, int? width, int? contentId, string tagXml, bool? isUniqueSystemProfileImage)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_Insert";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));
        cmd.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 1024, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileName == null ? (object)DBNull.Value : fileName));
        cmd.Parameters.Add(new SqlParameter("@Content", SqlDbType.VarBinary, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, content == null ? (object)DBNull.Value : content));
        cmd.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.NVarChar, 64, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentType == null ? (object)DBNull.Value : contentType));
        cmd.Parameters.Add(new SqlParameter("@ContentSize", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentSize == null ? (object)DBNull.Value : contentSize));
        cmd.Parameters.Add(new SqlParameter("@FriendlyFileName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, friendlyFileName == null ? (object)DBNull.Value : friendlyFileName));
        cmd.Parameters.Add(new SqlParameter("@Height", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, height == null ? (object)DBNull.Value : height));
        cmd.Parameters.Add(new SqlParameter("@Width", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, width == null ? (object)DBNull.Value : width));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@TagXml", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, tagXml == null ? (object)DBNull.Value : tagXml));
        cmd.Parameters.Add(new SqlParameter("@IsUniqueSystemProfileImage", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, isUniqueSystemProfileImage == null ? (object)DBNull.Value : isUniqueSystemProfileImage));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int cms_Files_Update(int fileId, int? userId, FileType fileType, string fileName, byte[] content, string contentType, int? contentSize, string friendlyFileName, int? height, int? width, int? contentId, IEnumerable<string> tags)
    {
      return _cms_Files_Update(fileId, userId, (byte)fileType, fileName, content, contentType, contentSize, friendlyFileName, height, width, contentId, TagXmlFormatter.GetTagsXml(tags));
    }
    private int _cms_Files_Update(int? fileId, int? userId, byte? fileType, string fileName, byte[] content, string contentType, int? contentSize, string friendlyFileName, int? height, int? width, int? contentId, string tagXml)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_Update";

        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));
        cmd.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 1024, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileName == null ? (object)DBNull.Value : fileName));
        cmd.Parameters.Add(new SqlParameter("@Content", SqlDbType.VarBinary, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, content == null ? (object)DBNull.Value : content));
        cmd.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.NVarChar, 64, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentType == null ? (object)DBNull.Value : contentType));
        cmd.Parameters.Add(new SqlParameter("@ContentSize", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentSize == null ? (object)DBNull.Value : contentSize));
        cmd.Parameters.Add(new SqlParameter("@FriendlyFileName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, friendlyFileName == null ? (object)DBNull.Value : friendlyFileName));
        cmd.Parameters.Add(new SqlParameter("@Height", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, height == null ? (object)DBNull.Value : height));
        cmd.Parameters.Add(new SqlParameter("@Width", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, width == null ? (object)DBNull.Value : width));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@TagXml", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, tagXml == null ? (object)DBNull.Value : tagXml));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_Files_Delete(int fileId)
    {
      return _cms_Files_Delete(fileId);
    }
    private int _cms_Files_Delete(int? fileId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Files_Delete";

        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region filesTemp
    public CMSFile cms_FilesTemp_Get(int tempFileId)
    {
      return _cms_FilesTemp_Get(tempFileId, null, null);
    }
    public CMSFile cms_FilesTemp_Get(int tempFileId, FileType fileTypeFilter)
    {
      return _cms_FilesTemp_Get(tempFileId, null, (byte)fileTypeFilter);
    }
    public CMSFile cms_FilesTemp_Get(int tempFileId, int userId, FileType fileTypeFilter)
    {
      return _cms_FilesTemp_Get(tempFileId, userId, (byte)fileTypeFilter);
    }
    private CMSFile _cms_FilesTemp_Get(int? fileId, int? userId, byte? fileType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSFile record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_FilesTemp_Get";

        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSFile(
              (int)dr["ApplicationId"]
              , (int)dr["FileId"]
              , dr["UserId"] == DBNull.Value ? null : (int?)dr["UserId"]
              , (FileType)(byte)dr["FileType"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["FileName"] as string
              , (byte[])dr["Content"]
              , dr["ContentType"] as string
              , (int)dr["ContentSize"]
              , dr["FriendlyFileName"] as string
              , (int)dr["Height"]
              , (int)dr["Width"]
              , null
              , true);
          }
        }
      }
      return record;
    }

    public CMSFile cms_FilesTemp_GetLatest(int userId, FileType fileType)
    {
      return _cms_FilesTemp_GetLatest(userId, (byte)fileType);
    }
    private CMSFile _cms_FilesTemp_GetLatest(int? userId, byte? fileType)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSFile record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_FilesTemp_GetLatest";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSFile(
              (int)dr["ApplicationId"]
              , (int)dr["FileId"]
              , dr["UserId"] == DBNull.Value ? null : (int?)dr["UserId"]
              , (FileType)(byte)dr["FileType"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["FileName"] as string
              , (byte[])dr["Content"]
              , dr["ContentType"] as string
              , (int)dr["ContentSize"]
              , dr["FriendlyFileName"] as string
              , (int)dr["Height"]
              , (int)dr["Width"]
              , null
              , true);
          }
        }
      }

      return record;
    }

    public int cms_FilesTemp_Delete(int userId, int fileId)
    {
      return _cms_FilesTemp_Delete(userId, fileId);
    }
    private int _cms_FilesTemp_Delete(int? userId, int? fileId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_FilesTemp_Delete";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_FilesTemp_DeleteByUserId(int userId, FileType fileType)
    {
      return _cms_FilesTemp_DeleteByUserId(userId, (byte)fileType);
    }
    private int _cms_FilesTemp_DeleteByUserId(int? userId, byte? fileType)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_FilesTemp_DeleteByUserId";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_FilesTemp_Insert(int applicationId, int? userId, FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width)
    {
      return _cms_FilesTemp_Insert(applicationId, userId, (byte)fileType, fileName, content, contentType, contentSize, friendlyFileName, height, width);
    }
    private int _cms_FilesTemp_Insert(int? applicationId, int? userId, byte? fileType, string fileName, byte[] content, string contentType, int? contentSize, string friendlyFileName, int? height, int? width)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_FilesTemp_Insert";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileType == null ? (object)DBNull.Value : fileType));
        cmd.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar, 1024, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileName == null ? (object)DBNull.Value : fileName));
        cmd.Parameters.Add(new SqlParameter("@Content", SqlDbType.VarBinary, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, content == null ? (object)DBNull.Value : content));
        cmd.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.NVarChar, 64, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentType == null ? (object)DBNull.Value : contentType));
        cmd.Parameters.Add(new SqlParameter("@ContentSize", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentSize == null ? (object)DBNull.Value : contentSize));
        cmd.Parameters.Add(new SqlParameter("@FriendlyFileName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, friendlyFileName == null ? (object)DBNull.Value : friendlyFileName));
        cmd.Parameters.Add(new SqlParameter("@Height", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, height == null ? (object)DBNull.Value : height));
        cmd.Parameters.Add(new SqlParameter("@Width", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, width == null ? (object)DBNull.Value : width));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region contentRatings
    public CMSContentRating cms_ContentRatings_Get(int userId, int contentId)
    {
      return _cms_ContentRatings_Get(userId, contentId);
    }
    private CMSContentRating _cms_ContentRatings_Get(int? userId, int? contentId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSContentRating record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentRatings_Get";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSContentRating(
              (int)dr["UserId"]
              , (int)dr["ContentId"]
              , (short)dr["Rating"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc));
          }
        }
      }
      return record;
    }

    public IEnumerable<CMSContentRating> cms_ContentRatings_Get(int contentId)
    {
      return _cms_ContentRatings_GetMultiple(null, contentId);
    }
    private IEnumerable<CMSContentRating> _cms_ContentRatings_GetMultiple(int? userId, int? contentId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentRatings_Get";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new CMSContentRating(
              (int)dr["UserId"]
              , (int)dr["ContentId"]
              , (short)dr["Rating"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc));
          }
        }
      }
    }

    public int cms_ContentRatings_Delete(int userId, int contentId)
    {
      return _cms_ContentRatings_Delete(userId, contentId);
    }
    private int _cms_ContentRatings_Delete(int? userId, int? contentId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentRatings_Delete";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_ContentRatings_InsertOrUpdate(short rating, int contentId, int userId, bool allowSelfRating)
    {
      return _cms_ContentRatings_InsertOrUpdate(rating, contentId, userId, allowSelfRating);
    }
    private int _cms_ContentRatings_InsertOrUpdate(short? rating, int? contentId, int? userId, bool? allowSelfRating)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentRatings_InsertOrUpdate";

        cmd.Parameters.Add(new SqlParameter("@Rating", SqlDbType.SmallInt, 2, ParameterDirection.Input, false, ((byte)(5)), ((byte)(0)), string.Empty, DataRowVersion.Default, rating == null ? (object)DBNull.Value : rating));
        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@AllowSelfRating", SqlDbType.Bit, 1, ParameterDirection.Input, false, ((byte)(1)), ((byte)(0)), string.Empty, DataRowVersion.Default, allowSelfRating == null ? (object)DBNull.Value : allowSelfRating));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region threadRatings
    public CMSThreadRating cms_ThreadRatings_Get(int userId, int threadId)
    {
      return _cms_ThreadRatings_Get(userId, threadId);
    }
    private CMSThreadRating _cms_ThreadRatings_Get(int? userId, int? threadId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      CMSThreadRating record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ThreadRatings_Get";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new CMSThreadRating(
              (int)dr["UserId"]
              , (int)dr["ThreadId"]
              , (short)dr["Rating"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc));
          }
        }
      }
      return record;
    }

    public IEnumerable<CMSThreadRating> cms_ThreadRatings_Get(int threadId)
    {
      return _cms_ThreadRatings_GetMultiple(null, threadId);
    }
    private IEnumerable<CMSThreadRating> _cms_ThreadRatings_GetMultiple(int? userId, int? threadId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ThreadRatings_Get";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new CMSThreadRating(
              (int)dr["UserId"]
              , (int)dr["ThreadId"]
              , (short)dr["Rating"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc));
          }
        }
      }
    }

    public int cms_ThreadRatings_Delete(int userId, int threadId)
    {
      return _cms_ThreadRatings_Delete(userId, threadId);
    }
    private int _cms_ThreadRatings_Delete(int? userId, int? threadId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ThreadRatings_Delete";

        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));
        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int cms_ThreadRatings_InsertOrUpdate(short rating, int threadId, int userId)
    {
      return _cms_ThreadRatings_InsertOrUpdate(rating, threadId, userId);
    }
    private int _cms_ThreadRatings_InsertOrUpdate(short? rating, int? threadId, int? userId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ThreadRatings_InsertOrUpdate";

        cmd.Parameters.Add(new SqlParameter("@Rating", SqlDbType.SmallInt, 2, ParameterDirection.Input, false, ((byte)(5)), ((byte)(0)), string.Empty, DataRowVersion.Default, rating == null ? (object)DBNull.Value : rating));
        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, userId == null ? (object)DBNull.Value : userId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region contentUser
    public int cms_ContentUser_Delete(int contentId, int receivingUserId)
    {
      return _cms_ContentUser_Delete(contentId, receivingUserId);
    }
    private int _cms_ContentUser_Delete(int? contentId, int? receivingUserId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentUser_Delete";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@ReceivingUserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, receivingUserId == null ? (object)DBNull.Value : receivingUserId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int cms_ContentUser_InsertOrUpdate(int contentId, int receivingUserId)
    {
      return _cms_ContentUser_InsertOrUpdate(contentId, receivingUserId);
    }
    private int _cms_ContentUser_InsertOrUpdate(int? contentId, int? receivingUserId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentUser_InsertOrUpdate";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));
        cmd.Parameters.Add(new SqlParameter("@ReceivingUserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, receivingUserId == null ? (object)DBNull.Value : receivingUserId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region contentTag
    public IEnumerable<string> cms_Tags_GetByContentId(int contentId)
    {
      return _cms_Tags_GetByContentId(contentId);
    }
    private IEnumerable<string> _cms_Tags_GetByContentId(int? contentId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Tags_GetByContentId";

        cmd.Parameters.Add(new SqlParameter("@ContentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentId == null ? (object)DBNull.Value : contentId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return (string)dr["Tag"];
          }
        }
      }
    }
    #endregion

    #region fileTag
    public IEnumerable<string> cms_Tags_GetByFileId(int fileId)
    {
      return _cms_Tags_GetByFileId(fileId);
    }
    private IEnumerable<string> _cms_Tags_GetByFileId(int? fileId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_Tags_GetByFileId";

        cmd.Parameters.Add(new SqlParameter("@FileId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, fileId == null ? (object)DBNull.Value : fileId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return (string)dr["Tag"];
          }
        }
      }
    }
    #endregion

    #region content level nodes
    public IEnumerable<ICMSContentLevelNode> cms_ContentLevelNodes_Get()
    {
      return _cms_ContentLevelNodes_Get(null, null, null, null);
    }
    public IEnumerable<ICMSContentLevelNode> cms_ContentLevelNodes_Get(int level, int? parentContentLevelNodeId, int? threadId, int? sectionId)
    {
      return _cms_ContentLevelNodes_Get(level, parentContentLevelNodeId, threadId, sectionId);
    }
    private IEnumerable<ICMSContentLevelNode> _cms_ContentLevelNodes_Get(int? level, int? parentContentLevelNodeId, int? threadId, int? sectionId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentLevelNodes_Get";

        cmd.Parameters.Add(new SqlParameter("@Level", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, level == null ? (object)DBNull.Value : level));
        cmd.Parameters.Add(new SqlParameter("@ParentContentLevelNodeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, parentContentLevelNodeId == null ? (object)DBNull.Value : parentContentLevelNodeId));
        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionId == null ? (object)DBNull.Value : sectionId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new CMSContentLevelNode(
              (int)dr["ContentLevelNodeId"]
              , dr["Name"] as string
              , (int)dr["Level"]
              , dr["ParentContentLevelNodeId"] == DBNull.Value ? null : (int?)dr["ParentContentLevelNodeId"]
              , dr["BreadCrumbs"] as string
              , dr["BreadCrumbsSplitIndexes"] as string
              , dr["ThreadId"] == DBNull.Value ? null : (int?)dr["ThreadId"]
              , dr["SectionId"] == DBNull.Value ? null : (int?)dr["SectionId"]
            );
          }
        }
      }
    }

    public int cms_ContentLevelNodes_Update(int contentLevelNodeId, string name)
    {
      return _cms_ContentLevelNodes_Update(contentLevelNodeId, name);
    }
    private int _cms_ContentLevelNodes_Update(int? contentLevelNodeId, string name)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentLevelNodes_Update";

        cmd.Parameters.Add(new SqlParameter("@ContentLevelNodeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentLevelNodeId == null ? (object)DBNull.Value : contentLevelNodeId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int cms_ContentLevelNodes_Insert(IEnumerable<string> contentLevelNodeNames, int? threadId, int? sectionId)
    {
      return _cms_ContentLevelNodes_Insert(ContenLevelNodeXmlFormatter.GetXml(contentLevelNodeNames), threadId, sectionId);
    }
    private int _cms_ContentLevelNodes_Insert(string nodes, int? threadId, int? sectionId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentLevelNodes_Insert";

        cmd.Parameters.Add(new SqlParameter("@Nodes", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, nodes == null ? (object)DBNull.Value : nodes));
        cmd.Parameters.Add(new SqlParameter("@ThreadId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, threadId == null ? (object)DBNull.Value : threadId));
        cmd.Parameters.Add(new SqlParameter("@SectionId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, sectionId == null ? (object)DBNull.Value : sectionId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int cms_ContentLevelNodes_Delete(int contentLevelNodeId)
    {
      return _cms_ContentLevelNodes_Delete(contentLevelNodeId);
    }
    private int _cms_ContentLevelNodes_Delete(int? contentLevelNodeId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "cms_ContentLevelNodes_Delete";

        cmd.Parameters.Add(new SqlParameter("@ContentLevelNodeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, contentLevelNodeId == null ? (object)DBNull.Value : contentLevelNodeId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #endregion

    #region emails

    public IEmail wm_Emails_Get(int emailId)
    {
      return _wm_Emails_Get(emailId);
    }
    private IEmail _wm_Emails_Get(int? emailId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      IEmail record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Emails_Get";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, (object)DBNull.Value));
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, (object)DBNull.Value));
        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, emailId == null ? (object)DBNull.Value : emailId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new Email(
              (int)dr["ApplicationId"]
              , (int)dr["EmailId"]
              , dr["Subject"] as string
              , dr["Body"] as string
              , dr["Recipients"] as string
              , dr["Sender"] as string
              , dr["CreatedByUserId"] == DBNull.Value ? null : (int?)dr["CreatedByUserId"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["SentUtc"] == DBNull.Value ? null : (DateTime?)(new DateTime(((DateTime)dr["SentUtc"]).Ticks, DateTimeKind.Utc))
              , (EmailStatus)(byte)dr["Status"]
              , (EmailPriority)(byte)dr["Priority"]
              , (EmailType)(byte)dr["EmailType"]
              , (int)dr["TotalSendAttempts"]);
          }
        }
      }
      return record;
    }

    public IEnumerable<IEmail> wm_Emails_Get(EmailStatus status)
    {
      return _wm_Emails_GetMultiple(null, (byte)status);
    }
    public IEnumerable<IEmail> wm_Emails_Get(int applicationId, EmailStatus status)
    {
      return _wm_Emails_GetMultiple(applicationId, (byte)status);
    }
    private IEnumerable<IEmail> _wm_Emails_GetMultiple(int? applicationId, byte? status)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Emails_Get";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, status == null ? (object)DBNull.Value : status));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new Email(
              (int)dr["ApplicationId"]
              , (int)dr["EmailId"]
              , dr["Subject"] as string
              , dr["Body"] as string
              , dr["Recipients"] as string
              , dr["Sender"] as string
              , dr["CreatedByUserId"] == DBNull.Value ? null : (int?)dr["CreatedByUserId"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["SentUtc"] == DBNull.Value ? null : (DateTime?)(new DateTime(((DateTime)dr["SentUtc"]).Ticks, DateTimeKind.Utc))
              , (EmailStatus)(byte)dr["Status"]
              , (EmailPriority)(byte)dr["Priority"]
              , (EmailType)(byte)dr["EmailType"]
              , (int)dr["TotalSendAttempts"]);
          }
        }
      }
    }

    public int wm_Emails_Delete(int emailId)
    {
      return _wm_Emails_Delete(emailId, null, null);
    }
    public int wm_Emails_Delete(EmailStatus status)
    {
      return _wm_Emails_Delete(null, (byte)status, null);
    }
    public int wm_Emails_Delete(EmailStatus status, DateTime olderThanDateCreatedUtc)
    {
      return _wm_Emails_Delete(null, (byte)status, olderThanDateCreatedUtc);
    }
    private int _wm_Emails_Delete(int? emailId, byte? status, DateTime? olderThanDateCreatedUtc)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Emails_Delete";

        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, emailId == null ? (object)DBNull.Value : emailId));
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, status == null ? (object)DBNull.Value : status));
        cmd.Parameters.Add(new SqlParameter("@OlderThanDateCreatedUtc", SqlDbType.DateTime, 8, ParameterDirection.Input, false, ((byte)(23)), ((byte)(3)), string.Empty, DataRowVersion.Default, olderThanDateCreatedUtc == null ? (object)DBNull.Value : olderThanDateCreatedUtc));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int wm_Emails_Insert(int applicationId, string subject, string body, string recipients, string sender, int? createdByUserId, EmailStatus status, EmailPriority priority, EmailType emailType)
    {
      return _wm_Emails_Insert(applicationId, subject, body, recipients, sender, createdByUserId, (byte)status, (byte)priority, (byte)emailType);
    }
    private int _wm_Emails_Insert(int? applicationId, string subject, string body, string recipients, string sender, int? createdByUserId, byte? status, byte? priority, byte? emailType)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Emails_Insert";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@Subject", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, subject == null ? (object)DBNull.Value : subject));
        cmd.Parameters.Add(new SqlParameter("@Body", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, body == null ? (object)DBNull.Value : body));
        cmd.Parameters.Add(new SqlParameter("@Recipients", SqlDbType.NVarChar, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, recipients == null ? (object)DBNull.Value : recipients));
        cmd.Parameters.Add(new SqlParameter("@Sender", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, sender == null ? (object)DBNull.Value : sender));
        cmd.Parameters.Add(new SqlParameter("@CreatedByUserId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, createdByUserId == null ? (object)DBNull.Value : createdByUserId));
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, status == null ? (object)DBNull.Value : status));
        cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, priority == null ? (object)DBNull.Value : priority));
        cmd.Parameters.Add(new SqlParameter("@EmailType", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, emailType == null ? (object)DBNull.Value : emailType));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int wm_Emails_SetToSent(int emailId, EmailStatus emailStatus, EmailPriority newPriority)
    {
      return _wm_Emails_SetToSent(emailId, (byte)emailStatus, (byte)newPriority);
    }
    private int _wm_Emails_SetToSent(int? emailId, byte? status, byte? priority)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Emails_SetToSent";

        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, emailId == null ? (object)DBNull.Value : emailId));
        cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, status == null ? (object)DBNull.Value : status));
        cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, priority == null ? (object)DBNull.Value : priority));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public List<IEmail> wm_Emails_PutInSendQueue(int queuedEmailsThresholdInSeconds, int failedEmailsThresholdInSeconds
      , int totalEmailsToEnqueue)
    {
      return _wm_Emails_PutInSendQueue(null, (byte)EmailStatus.Queued, (byte)EmailStatus.Unsent, (byte)EmailStatus.SendFailed, queuedEmailsThresholdInSeconds, failedEmailsThresholdInSeconds, totalEmailsToEnqueue);
    }
    private List<IEmail> _wm_Emails_PutInSendQueue(int? applicationId, byte? inQueueStatus, byte? unsentStatus, byte? sendFailedStatus, int? queuedEmailsThresholdInSeconds, int? failedEmailsThresholdInSeconds, int? totalEmailsToEnqueue)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      List<IEmail> records = new List<IEmail>();
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Emails_PutInSendQueue";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@InQueueStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, inQueueStatus == null ? (object)DBNull.Value : inQueueStatus));
        cmd.Parameters.Add(new SqlParameter("@UnsentStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, unsentStatus == null ? (object)DBNull.Value : unsentStatus));
        cmd.Parameters.Add(new SqlParameter("@SendFailedStatus", SqlDbType.TinyInt, 1, ParameterDirection.Input, false, ((byte)(3)), ((byte)(0)), string.Empty, DataRowVersion.Default, sendFailedStatus == null ? (object)DBNull.Value : sendFailedStatus));
        cmd.Parameters.Add(new SqlParameter("@QueuedEmailsThresholdInSeconds", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, queuedEmailsThresholdInSeconds == null ? (object)DBNull.Value : queuedEmailsThresholdInSeconds));
        cmd.Parameters.Add(new SqlParameter("@FailedEmailsThresholdInSeconds", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, failedEmailsThresholdInSeconds == null ? (object)DBNull.Value : failedEmailsThresholdInSeconds));
        cmd.Parameters.Add(new SqlParameter("@TotalEmailsToEnqueue", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, totalEmailsToEnqueue == null ? (object)DBNull.Value : totalEmailsToEnqueue));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            records.Add(new Email(
              (int)dr["ApplicationId"]
              , (int)dr["EmailId"]
              , dr["Subject"] as string
              , dr["Body"] as string
              , dr["Recipients"] as string
              , dr["Sender"] as string
              , dr["CreatedByUserId"] == DBNull.Value ? null : (int?)dr["CreatedByUserId"]
              , new DateTime(((DateTime)dr["DateCreatedUtc"]).Ticks, DateTimeKind.Utc)
              , dr["SentUtc"] == DBNull.Value ? null : (DateTime?)(new DateTime(((DateTime)dr["SentUtc"]).Ticks, DateTimeKind.Utc))
              , (EmailStatus)(byte)dr["Status"]
              , (EmailPriority)(byte)dr["Priority"]
              , (EmailType)(byte)dr["EmailType"]
              , (int)dr["TotalSendAttempts"]));
          }
        }
      }
      return records;
    }
    #endregion

    #region offices
    public IOfficeModel wm_Offices_Get(int applicationId, int officeId)
    {
      return _wm_Offices_Get(applicationId, officeId);
    }
    private IOfficeModel _wm_Offices_Get(int? applicationId, int? officeId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      IOfficeModel record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Offices_Get";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@OfficeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, officeId == null ? (object)DBNull.Value : officeId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new OfficeModel(
              (int)dr["ApplicationId"]
              , (int)dr["OfficeId"]
              , dr["Name"] as string
              , dr["Description"] as string
              , XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None));
          }
        }
      }
      return record;
    }

    public IEnumerable<IOfficeModel> wm_Offices_Get(int applicationId)
    {
      return _wm_Offices_GetMultiple(applicationId, null);
    }
    private IEnumerable<IOfficeModel> _wm_Offices_GetMultiple(int? applicationId, int? officeId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Offices_Get";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@OfficeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, officeId == null ? (object)DBNull.Value : officeId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            yield return new OfficeModel(
              (int)dr["ApplicationId"]
              , (int)dr["OfficeId"]
              , dr["Name"] as string
              , dr["Description"] as string
              , XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None));
          }
        }
      }
    }

    public int wm_Offices_Insert(int applicationId, IOfficeModel record)
    {
      return _wm_Offices_Insert(applicationId, record.Name, record.Description, record.GetExtraInfoClone().ToString());
    }
    private int _wm_Offices_Insert(int? applicationId, string name, string description, string extraInfo)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Offices_Insert";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 512, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, description == null ? (object)DBNull.Value : description));
        cmd.Parameters.Add(new SqlParameter("@ExtraInfo", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, extraInfo == null ? (object)DBNull.Value : extraInfo));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int wm_Offices_Update(IOfficeModel office)
    {
      return _wm_Offices_Update(office.OfficeId, office.Name, office.Description, office.GetExtraInfoClone().ToString());
    }
    private int _wm_Offices_Update(int? officeId, string name, string description, string extraInfo)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Offices_Update";

        cmd.Parameters.Add(new SqlParameter("@OfficeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, officeId == null ? (object)DBNull.Value : officeId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 512, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, description == null ? (object)DBNull.Value : description));
        cmd.Parameters.Add(new SqlParameter("@ExtraInfo", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, extraInfo == null ? (object)DBNull.Value : extraInfo));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int wm_Offices_Delete(int applicationId, int officeId)
    {
      return _wm_Offices_Delete(applicationId, officeId);
    }
    private int _wm_Offices_Delete(int? applicationId, int? officeId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Offices_Delete";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@OfficeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, officeId == null ? (object)DBNull.Value : officeId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region departments
    public IDepartmentWithOfficesModel wm_Departments_GetRecursive(int applicationId, int departmentId)
    {
      return _wm_Departments_GetRecursive(applicationId, departmentId);
    }
    private IDepartmentWithOfficesModel _wm_Departments_GetRecursive(int applicationId, int departmentId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      Dictionary<int, IOfficeModel> officeModels = new Dictionary<int, IOfficeModel>();
      foreach (IOfficeModel office in wm_Offices_Get(applicationId))
        officeModels[office.OfficeId] = office;

      Dictionary<int, IDepartmentModel> departmentModels = new Dictionary<int, IDepartmentModel>();
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Departments_GetRecursive";

        cmd.Parameters.Add(new SqlParameter("@DepartmentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, departmentId == null ? (object)DBNull.Value : departmentId));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            departmentModels[(int)dr["DepartmentId"]] = new DepartmentModel(
              (int)dr["ApplicationId"]
              , (int)dr["DepartmentId"]
              , dr["ParentDepartmentId"] == DBNull.Value ? null : (int?)dr["ParentDepartmentId"]
              , dr["Name"] as string
              , dr["OfficeId"] == DBNull.Value ? null : (int?)dr["OfficeId"]);
          }
        }
      }

      if (departmentModels.Count == 0)
        return null;

      foreach (IDepartmentModel record in departmentModels.Values)
      {
        if (record.ParentDepartmentId.HasValue && departmentModels.ContainsKey(record.ParentDepartmentId.Value))
          record.ParentDepartment = departmentModels[record.ParentDepartmentId.Value];
        if (record.OfficeId.HasValue && officeModels.ContainsKey(record.OfficeId.Value))
          record.Office = officeModels[record.OfficeId.Value];
      }

      return new DepartmentWithOfficesModel(departmentModels[departmentId], officeModels.Values);
    }
    public IEnumerable<IDepartmentModel> wm_Departments_GetDepartmentModels(int applicationId)
    {
      return _wm_Departments_GetDepartmentModel_Multiple(null, applicationId, null);
    }
    private IEnumerable<IDepartmentModel> _wm_Departments_GetDepartmentModel_Multiple(int? departmentId, int applicationId, int? officeId)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      Dictionary<int, IOfficeModel> officeModels = new Dictionary<int, IOfficeModel>();
      foreach (IOfficeModel office in wm_Offices_Get(applicationId))
        officeModels[office.OfficeId] = office;

      Dictionary<int, IDepartmentModel> departmentModels = new Dictionary<int, IDepartmentModel>();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Departments_Get";

        cmd.Parameters.Add(new SqlParameter("@DepartmentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, departmentId == null ? (object)DBNull.Value : departmentId));
        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId));
        cmd.Parameters.Add(new SqlParameter("@OfficeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, officeId == null ? (object)DBNull.Value : officeId));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            departmentModels[(int)dr["DepartmentId"]] = new DepartmentModel(
              (int)dr["ApplicationId"]
              , (int)dr["DepartmentId"]
              , dr["ParentDepartmentId"] == DBNull.Value ? null : (int?)dr["ParentDepartmentId"]
              , dr["Name"] as string
              , dr["OfficeId"] == DBNull.Value ? null : (int?)dr["OfficeId"]);
          }
        }
      }

      foreach (IDepartmentModel record in departmentModels.Values)
      {
        if (record.ParentDepartmentId.HasValue && departmentModels.ContainsKey(record.ParentDepartmentId.Value))
          record.ParentDepartment = departmentModels[record.ParentDepartmentId.Value];
        if (record.OfficeId.HasValue && officeModels.ContainsKey(record.OfficeId.Value))
          record.Office = officeModels[record.OfficeId.Value];
      }

      foreach (IDepartmentModel record in departmentModels.Values)
        yield return record;
    }
    public int wm_Departments_Insert(int? applicationId, int? parentDepartmentId, string name, int? officeId)
    {
      return _wm_Departments_Insert(applicationId, parentDepartmentId, name, officeId);
    }
    private int _wm_Departments_Insert(int? applicationId, int? parentDepartmentId, string name, int? officeId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Departments_Insert";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@ParentDepartmentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, parentDepartmentId == null ? (object)DBNull.Value : parentDepartmentId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@OfficeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, officeId == null ? (object)DBNull.Value : officeId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int wm_Departments_Update(int? departmentId, int? parentDepartmentId, string name, int? officeId)
    {
      return _wm_Departments_Update(departmentId, parentDepartmentId, name, officeId);
    }
    private int _wm_Departments_Update(int? departmentId, int? parentDepartmentId, string name, int? officeId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Departments_Update";

        cmd.Parameters.Add(new SqlParameter("@DepartmentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, departmentId == null ? (object)DBNull.Value : departmentId));
        cmd.Parameters.Add(new SqlParameter("@ParentDepartmentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, parentDepartmentId == null ? (object)DBNull.Value : parentDepartmentId));
        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, name == null ? (object)DBNull.Value : name));
        cmd.Parameters.Add(new SqlParameter("@OfficeId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, officeId == null ? (object)DBNull.Value : officeId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    public int wm_Departments_Delete(int applicationId, int departmentId)
    {
      return _wm_Departments_Delete(applicationId, departmentId);
    }
    private int _wm_Departments_Delete(int applicationId, int? departmentId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Departments_Delete";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@DepartmentId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, departmentId == null ? (object)DBNull.Value : departmentId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #region applications
    public IApplication wm_Applications_Get(string applicationName)
    {
      return _wm_Applications_Get(null, applicationName);
    }
    private IApplication _wm_Applications_Get(int? applicationId, string applicationName)
    {
      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      IApplication record = null;
      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Applications_Get";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));
        cmd.Parameters.Add(new SqlParameter("@ApplicationName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationName == null ? (object)DBNull.Value : applicationName));

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        using (System.Data.IDataReader dr = cmd.ExecuteReader())
        {
          while (dr.Read())
          {
            record = new Application(
              (int)dr["ApplicationId"]
              , dr["ApplicationName"] as string
              , dr["Description"] as string
              , XElement.Parse(dr["ExtraInfo"] as string, LoadOptions.None));
          }
        }
      }
      return record;
    }

    public int wm_Applications_InsertOrUpdate(IApplication application)
    {
      return _wm_Applications_InsertOrUpdate(application.ApplicationName, application.Description, application.GetExtraInfoClone().ToString());
    }
    private int _wm_Applications_InsertOrUpdate(string applicationName, string description, string extraInfo)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Applications_InsertOrUpdate";

        cmd.Parameters.Add(new SqlParameter("@ApplicationName", SqlDbType.NVarChar, 256, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationName == null ? (object)DBNull.Value : applicationName));
        cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 512, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, description == null ? (object)DBNull.Value : description));
        cmd.Parameters.Add(new SqlParameter("@ExtraInfo", SqlDbType.Xml, -1, ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), string.Empty, DataRowVersion.Default, extraInfo == null ? (object)DBNull.Value : extraInfo));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }

    public int wm_Applications_Delete(int applicationId)
    {
      return _wm_Applications_Delete(applicationId);
    }
    private int _wm_Applications_Delete(int? applicationId)
    {
      int returnValue = int.MinValue;

      if (_Connection.State != ConnectionState.Open)
        _Connection.Open();

      using (SqlCommand cmd = _Connection.CreateCommand())
      {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandTimeout = _DefaultCommandTimeout;
        cmd.CommandText = "wm_Applications_Delete";

        cmd.Parameters.Add(new SqlParameter("@ApplicationId", SqlDbType.Int, 4, ParameterDirection.Input, false, ((byte)(10)), ((byte)(0)), string.Empty, DataRowVersion.Default, applicationId == null ? (object)DBNull.Value : applicationId));

        SqlParameter rv = new SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
        cmd.Parameters.Add(rv);

        if (_Log.IsDebugEnabled)
          _Log.Debug(cmd.ToSqlExecStatement());

        cmd.ExecuteNonQuery();
        if (rv != null && rv.Value != null)
          returnValue = (int)(rv.Value);
      }

      return returnValue;
    }
    #endregion

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
      if (_Connection != null)
      {
        try { _Connection.Dispose(); }
        catch { }
      }
    }

    #endregion

    #region constructors
    public SqlServerDataStoreContext(string connectionString) : this(connectionString, 30) { }
    public SqlServerDataStoreContext(string connectionString, int defaultCommandTimeout)
    {
      _DefaultCommandTimeout = defaultCommandTimeout;
      _Connection = new SqlConnection(connectionString);
    }
    #endregion
  }
}
