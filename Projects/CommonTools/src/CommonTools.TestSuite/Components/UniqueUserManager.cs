using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using CommonTools.Components.BusinessTier;
using System.Data.SqlClient;
using CommonTools.Components.Logging;
using System.Data.Linq;
using CommonTools.TestSuite.DummyDatabase.SqlProvider;

namespace CommonTools.TestSuite.Components
{
    [BusinessObjectCache("UniqueUser")]
    public static class UniqueUserManager
    {
        public static UniqueUser GetUser(Guid userId)
        {
            UsersDataContext dc = Configuration.GetUsersDataContext();

            var record = dc.UniqueUsers.SingleOrDefault(c => c.UserID == userId);
            if (record != null)
            {
                return new UniqueUser(record);
            }

            return null;
        }


        public static BusinessObjectActionReport<UniqueUserActionStatus> Create(UniqueUser user)
        {
            BusinessObjectActionReport<UniqueUserActionStatus> actionReport = new BusinessObjectActionReport<UniqueUserActionStatus>(UniqueUserActionStatus.Success);
            actionReport.ValidationResult = BusinessObjectManager.Validate(user);
            if (actionReport.ValidationResult.IsValid)
            {
                try
                {
                    UsersDataContext dc = Configuration.GetUsersDataContext();

                    int affectedRows = dc.InsertUniqueUser(user.UserID, user.AccountStatus, user.Timezone, user.Firstname
                        , user.Lastname, user.DateOfBirth, user.City, user.IsNewletterSubscriber);

                    if (affectedRows == 0)
                    {
                        actionReport.Status = UniqueUserActionStatus.NoRecordRowAffected;
                        LogManager.LogEvent(ApplicationLocation.DataAccess, EventType.Error, "User " + user.UserID.ToString() + " was not inserted at the database.");
                    }
                }
                catch (SqlException ex)
                {
                    actionReport.Status = UniqueUserActionStatus.SqlException;
                    LogManager.LogException(ApplicationLocation.DataAccess, ex);
                }
                catch (Exception ex)
                {
                    actionReport.Status = UniqueUserActionStatus.UnknownError;
                    LogManager.LogException(ApplicationLocation.DataAccess, ex);
                }
                finally
                {

                }
            }
            else
            {
                actionReport.Status = UniqueUserActionStatus.ValidationFailed;
                LogManager.LogEvent(ApplicationLocation.DataAccess, EventType.Warning, "User " + user.UserID.ToString() + " was not inserted at the database because the validation failed. Report: "
                    + actionReport.ValidationResult.ToString(CommonTools.TextFormat.ASCII));
            }

            return actionReport;
        }


        public static BusinessObjectActionReport<UniqueUserActionStatus> Update(UniqueUser user)
        {
            BusinessObjectActionReport<UniqueUserActionStatus> actionReport = new BusinessObjectActionReport<UniqueUserActionStatus>(UniqueUserActionStatus.Success);
            actionReport.ValidationResult = BusinessObjectManager.Validate(user);
            if (actionReport.ValidationResult.IsValid)
            {
                try
                {
                    UsersDataContext dc = Configuration.GetUsersDataContext();

                    int affectedRows = dc.UpdateUniqueUser(user.UserID, user.AccountStatus, user.Timezone, user.Firstname, user.Lastname
                        , user.DateOfBirth, user.City, user.IsNewletterSubscriber);

                    if (affectedRows == 0)
                    {
                        actionReport.Status = UniqueUserActionStatus.NoRecordRowAffected;
                        LogManager.LogEvent(ApplicationLocation.DataAccess, EventType.Error
                            , "User " + user.UserID.ToString() + " was not updated at the database.");
                    }
                }
                catch (SqlException ex)
                {
                    actionReport.Status = UniqueUserActionStatus.SqlException;
                    LogManager.LogException(ApplicationLocation.DataAccess, ex);
                }
                catch (Exception ex)
                {
                    actionReport.Status = UniqueUserActionStatus.UnknownError;
                    LogManager.LogException(ApplicationLocation.DataAccess, ex);
                }
                finally
                {

                }
            }
            else
            {
                actionReport.Status = UniqueUserActionStatus.ValidationFailed;
                LogManager.LogEvent(ApplicationLocation.DataAccess, EventType.Warning, 
                    "User " + user.UserID.ToString() + " was not updated at the database because the validation failed. Report: "
                    + actionReport.ValidationResult.ToString(CommonTools.TextFormat.ASCII));
            }

            return actionReport;
        }


        public static BusinessObjectActionReport<UniqueUserActionStatus> Delete(UniqueUser user)
        {
            BusinessObjectActionReport<UniqueUserActionStatus> actionReport = new BusinessObjectActionReport<UniqueUserActionStatus>(UniqueUserActionStatus.Success);
            actionReport.ValidationResult = BusinessObjectManager.Validate(user);
            if (actionReport.ValidationResult.IsValid)
            {
                try
                {
                    UsersDataContext dc = Configuration.GetUsersDataContext();

                    int affectedRows = dc.DeleteUniqueUser(user.UserID);
                    if (affectedRows == 0)
                    {
                        actionReport.Status = UniqueUserActionStatus.NoRecordRowAffected;
                        LogManager.LogEvent(ApplicationLocation.DataAccess, EventType.Error
                            , "User " + user.UserID.ToString() + " was not deleted at the database.");
                    }
                }
                catch (SqlException ex)
                {
                    actionReport.Status = UniqueUserActionStatus.SqlException;
                    LogManager.LogException(ApplicationLocation.DataAccess, ex);
                }
                catch (Exception ex)
                {
                    actionReport.Status = UniqueUserActionStatus.UnknownError;
                    LogManager.LogException(ApplicationLocation.DataAccess, ex);
                }
                finally
                {

                }
            }
            else
            {
                actionReport.Status = UniqueUserActionStatus.ValidationFailed;
                LogManager.LogEvent(ApplicationLocation.DataAccess, EventType.Warning, 
                    "User " + user.UserID.ToString() + " was not deleted at the database because the validation failed. Report: "
                    + actionReport.ValidationResult.ToString(CommonTools.TextFormat.ASCII));
            }

            return actionReport;
        }


        public static List<UniqueUser> GetUserPage(int pageIndex, int pageSize)
        {
            UsersDataContext dc = Configuration.GetUsersDataContext();
            List<UniqueUser> users = new List<UniqueUser>();

            var records = dc.GetUniqueUserPageOrderedByDateOfBirth(pageIndex, pageSize);

            foreach (CommonTools.TestSuite.DummyDatabase.SqlProvider.UniqueUser user in records)
                users.Add(new UniqueUser(user));

            return users;
        }

        public static int GetUniqueUserCount()
        {
            UsersDataContext dc = Configuration.GetUsersDataContext();
            return (from c in dc.UniqueUsers
                    select c).Count();
        }
    }
}
