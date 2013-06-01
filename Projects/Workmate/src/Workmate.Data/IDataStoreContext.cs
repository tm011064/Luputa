using System;
using System.Collections.Generic;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.Membership;
using Workmate.Data.Entities;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Entities.CMS;
using System.Xml.Linq;
using Workmate.Components.Entities.CMS.MessageBoards;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Contracts;
using Workmate.Components.Entities.CMS.Membership;
using Workmate.Components.Contracts.Emails;
using Workmate.Components.Contracts.Company;

namespace Workmate.Data
{
  public interface IDataStoreContext : IDisposable
  {
    #region membership
    wm_User_GetPassword_QueryResult wm_Users_GetPassword(int applicationId, string userName, string email);
    int wm_Users_SetPassword(int userId, string newPassword, string passwordSalt, byte passwordFormat);
    int wm_Users_Insert(int applicationId, string userName, string email, string password, string passwordSalt
     , int passwordFormat, AccountStatus status, List<string> roleNames, int profileImageId, Guid uniqueId
     , UserNameDisplayMode userNameDisplayMode
     , string timeZoneInfoId, string firstName, string lastName, Gender gender, out int userId, out DateTime dateCreatedUtc);

    int wm_UserRole_Insert(IEnumerable<int> userIds, IEnumerable<string> roleNames);
    int wm_UserRole_Delete(IEnumerable<int> userIds, IEnumerable<string> roleNames);
    IEnumerable<string> wm_Roles_GetByUserId(int userId);
    int wm_Roles_InsertIfNotExists(int applicationId, string roleName, string description);
    IEnumerable<int> wm_UserRole_GetByRoleName(int applicationId, string roleName);

    IUserBasic wm_Users_GetUserBasicByUniqueId(Guid uniqueId, bool updateLastActivity);
    IUserBasic wm_Users_GetUserBasicByEmail(string email, int applicationId, bool updateLastActivity);
    IUserBasic wm_Users_GetUserBasicByUserName(string userName, int applicationId, bool updateLastActivity);
    IUserBasic wm_Users_GetUserBasicByUserId(int userId, bool updateLastActivity);

    IUserModel wm_Users_GetUserModel(int userId);

    Dictionary<int, IUserBasic> wm_Users_GetAllUserBasics(int applicationId);
    int wm_Users_GetNumberOfUsersOnline(int applicationId, int minutesSinceLastInActive);

    ChangeCredentialsStatus wm_Users_UpdateEmail(int userId, string email);
    ChangeCredentialsStatus wm_Users_UpdateUserName(int userId, string newUserName);

    int wm_Users_UpdateUserInfo(int userId, bool isPasswordCorrect, bool updateLastLoginActivityDate, int maxInvalidPasswordAttempts
      , out DateTime? lastActivityDateUtc, out DateTime? lastLoginDateUtc, out AccountStatus? status, out int? failedPasswordAttemptCount, out DateTime? lastLockoutDateUtc);
    int wm_Users_UnlockUser(int userId);

    List<IBaseUserModel> wm_Users_GetBaseUserModels(int applicationId, string searchTerm, ref int pageIndex, int pageSize, out int rowCount);
    #endregion

    #region cms

    #region groups
    int cms_Groups_InsertOrUpdate(int groupId, string name, string description, CMSGroupType groupType);
    int cms_Groups_Delete(int groupId);
    CMSGroup cms_Groups_Get(int groupId);
    IEnumerable<CMSGroup> cms_Groups_Get(CMSGroupType groupType);
    IEnumerable<CMSGroup> cms_Groups_GetAll();
    #endregion

    #region sections
    int cms_Sections_Insert(int applicationId, int? parentSectionId, int? groupId, string name, string description, CMSSectionType sectionType, bool isActive, bool isModerated);
    int cms_Sections_Update(int sectionId, int? parentSectionId, int? groupId, string name, string description, CMSSectionType sectionType, bool isActive, bool isModerated);
    int cms_Sections_Delete(int sectionId, bool deleteLinkedThreads);
    CMSSection cms_Sections_Get(int applicationId, CMSSectionType sectionType, string name);
    CMSSection cms_Sections_Get(CMSSectionType sectionType, int sectionId);
    IEnumerable<CMSSection> cms_Sections_Get(int applicationId);
    IEnumerable<CMSSection> cms_Sections_Get(int applicationId, CMSSectionType sectionType);
    IEnumerable<CMSSection> cms_Sections_GetAll();
    #endregion

    #region threads
    CMSThread cms_Threads_Get(CMSSectionType sectionType, int threadId);
    CMSThread cms_Threads_Get(CMSSectionType sectionType, string name);
    IEnumerable<CMSThread> cms_Threads_Get(CMSSectionType sectionType);
    IEnumerable<CMSThread> cms_Threads_Get(int applicationId);
    IEnumerable<CMSThread> cms_Threads_Get(int applicationId, CMSSectionType sectionType);
    int cms_Threads_Insert(int sectionId, string name, DateTime? stickyDateUtc, bool isLocked, bool isSticky, bool isApproved, int threadStatus);
    int cms_Threads_Update(int threadId, int sectionId, string name, DateTime lastViewedDateUtc, DateTime? stickyDateUtc, bool isLocked, bool isSticky, bool isApproved, int threadStatus);
    int cms_Threads_Delete(int threadId, bool deleteLinkedThreads);
    int cms_Threads_IncreaseTotalViews(int threadId, int numberOfViewsToAdd);
    #endregion

    #region contents
    CMSContent cms_Contents_Get(int contentId);
    CMSContent cms_Contents_Get(string urlFriendlyName, string threadName, string sectionName, string groupName);
    IEnumerable<CMSContent> cms_Contents_Get(CMSSectionType sectionType);
    IEnumerable<CMSContent> cms_Contents_Get(CMSGroupType groupType);
    IEnumerable<CMSContent> cms_Contents_Get(int applicationId, CMSSectionType sectionType);
    IEnumerable<CMSContent> cms_Contents_Get(int applicationId, CMSGroupType groupType);

    List<BaseArticleInfo> cms_Contents_GetBaseArticleInfoPage(int? sectionId, int? threadId, List<string> tags, ref int pageIndex, int pageSize, out int rowCount);
    List<MessageInfo> cms_Contents_GetMessageInfoPageFromThreadIndex(int messageBoardThreadId, ref int pageIndex, int pageSize, out int rowCount);
    Dictionary<string, string> cms_Contents_GetAllWithSectionName(int applicationId, CMSSectionType sectionType, byte contentStatus);
    int cms_Sections_UpdateContentBlock(int applicationId, string name, string formattedBody, int authorUserId);

    IArticleModel cms_Contents_GetArticleModel(int articleId);
    IArticleModel cms_Contents_GetArticleModel(string urlFriendlyName, string threadName, string sectionName);

    BaseRatingInfo cms_Contents_GetBaseRatingInfo(int contentId);
    int cms_Contents_IncreaseTotalViews(int contentId, int numberOfViewsToAdd);

    int cms_Contents_Insert(int threadId, int? parentContentId, int authorUserId, short contentLevel, string subject
      , string formattedBody, bool isApproved, bool isLocked, byte contentType, byte contentStatus, XElement extraInfo
      , string urlFriendlyName, IEnumerable<string> tags, IEnumerable<string> contentLevelNodes
      , bool? createLinkedThread, int? linkedThreadSectionId, bool? isLinkedThreadEnabled, LinkedThreadRelationshipType? linkedThreadRelationshipType);
    int cms_Contents_Update(int contentId, int threadId, int? parentContentId, int authorUserId, short contentLevel, string subject, string formattedBody, bool isApproved, bool isLocked, byte contentType, byte contentStatus, XElement extraInfo, int? baseContentId, string urlFriendlyName, IEnumerable<string> tags);
    int cms_Contents_Delete(int contentId, bool deleteLinkedThreads);
    #endregion

    #region files
    CMSFile cms_Files_Get(int fileId);
    CMSFile cms_Files_Get(int fileId, FileType fileTypeFilter);
    IEnumerable<CMSFile> cms_Files_GetMultiple(int applicationId, FileType fileTypeFilter);

    ProfileImage cms_Files_GetProfileImage(int userId);

    int cms_Files_MoveTempFile(int tempFileId, int? contentId, bool useExistingRecordValues, string fileName, string friendlyFileName, System.Collections.Generic.IEnumerable<string> tags);
    int cms_Files_AssignTemporaryProfileImageToUser(int tempFileId);

    IArticleAttachmentModel cms_Files_GetArticleAttachmentModel(int articleAttachmentId);
    IEnumerable<IArticleAttachmentModel> cms_Files_GetArticleAttachmentModels(int articleId);

    int cms_Files_Insert(int applicationId, int? userId, Workmate.Components.Contracts.CMS.FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width, int? contentId, IEnumerable<string> tags);
    int cms_Files_InsertUniqueSystemProfileImage(int applicationId, int? userId, Workmate.Components.Contracts.CMS.FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width, int? contentId, IEnumerable<string> tags);
    int cms_Files_Update(int fileId, int? userId, Workmate.Components.Contracts.CMS.FileType fileType, string fileName, byte[] content, string contentType, int? contentSize, string friendlyFileName, int? height, int? width, int? contentId, IEnumerable<string> tags);
    int cms_Files_Delete(int fileId);
    #endregion

    #region filesTemp
    CMSFile cms_FilesTemp_Get(int tempFileId);
    CMSFile cms_FilesTemp_Get(int tempFileId, FileType fileTypeFilter);
    CMSFile cms_FilesTemp_Get(int tempFileId, int userId, FileType fileTypeFilter);
    CMSFile cms_FilesTemp_GetLatest(int userId, FileType fileTypeFilter);

    int cms_FilesTemp_Delete(int userId, int fileId);
    int cms_FilesTemp_DeleteByUserId(int userId, FileType fileType);
    int cms_FilesTemp_Insert(int applicationId, int? userId, FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width);
    #endregion

    #region contentRatings
    IEnumerable<CMSContentRating> cms_ContentRatings_Get(int contentId);
    CMSContentRating cms_ContentRatings_Get(int userId, int contentId);

    int cms_ContentRatings_Delete(int userId, int contentId);
    int cms_ContentRatings_InsertOrUpdate(short rating, int contentId, int userId, bool allowSelfRating);
    #endregion

    #region threadRatings
    IEnumerable<CMSThreadRating> cms_ThreadRatings_Get(int threadId);
    CMSThreadRating cms_ThreadRatings_Get(int userId, int threadId);

    int cms_ThreadRatings_Delete(int userId, int threadId);
    int cms_ThreadRatings_InsertOrUpdate(short rating, int threadId, int userId);
    #endregion

    #region contentUser
    int cms_ContentUser_Delete(int contentId, int receivingUserId);
    int cms_ContentUser_InsertOrUpdate(int contentId, int receivingUserId);
    #endregion

    #region contentTag
    IEnumerable<string> cms_Tags_GetByContentId(int contentId);
    #endregion

    #region fileTag
    IEnumerable<string> cms_Tags_GetByFileId(int fileId);
    #endregion

    #region content level nodes
    int cms_ContentLevelNodes_Update(int contentLevelNodeId, string name);
    IEnumerable<ICMSContentLevelNode> cms_ContentLevelNodes_Get();
    IEnumerable<ICMSContentLevelNode> cms_ContentLevelNodes_Get(int level, int? parentContentLevelNodeId, int? threadId, int? sectionId);
    int cms_ContentLevelNodes_Insert(IEnumerable<string> contentLevelNodeNames, int? threadId, int? sectionId);
    int cms_ContentLevelNodes_Delete(int contentLevelNodeId);
    #endregion

    #endregion

    #region general

    #region applications
    IApplication wm_Applications_Get(string applicationName);
    int wm_Applications_InsertOrUpdate(IApplication application);
    int wm_Applications_Delete(int applicationId);
    #endregion

    #endregion
    
    #region emails
    int wm_Emails_SetToSent(int emailId, EmailStatus status, EmailPriority newPriority);
    int wm_Emails_Insert(int applicationId, string header, string body, string recipients, string sender, int? createdByUserId
      , EmailStatus status, EmailPriority priority, EmailType emailType);
    int wm_Emails_Delete(int emailId);
    int wm_Emails_Delete(EmailStatus status);
    int wm_Emails_Delete(EmailStatus status, DateTime olderThanDateCreatedUtc);
    IEmail wm_Emails_Get(int emailId);
    IEnumerable<IEmail> wm_Emails_Get(EmailStatus status);
    IEnumerable<IEmail> wm_Emails_Get(int applicationId, EmailStatus status);
    List<IEmail> wm_Emails_PutInSendQueue(int queuedEmailsThresholdInSeconds, int failedEmailsThresholdInSeconds, int totalEmailsToEnqueue);
    #endregion

    #region company

    #region offices
    IOfficeModel wm_Offices_Get(int applicationId, int officeId);
    IEnumerable<IOfficeModel> wm_Offices_Get(int applicationId);
    int wm_Offices_Insert(int applicationId, IOfficeModel office);
    int wm_Offices_Update(IOfficeModel office);
    int wm_Offices_Delete(int applicationId, int officeId);
    #endregion

    #region departments
    IDepartmentWithOfficesModel wm_Departments_GetRecursive(int applicationId, int departmentId);
    IEnumerable<IDepartmentModel> wm_Departments_GetDepartmentModels(int applicationId);
    int wm_Departments_Insert(int? applicationId, int? parentDepartmentId, string name, int? officeId);
    int wm_Departments_Update(int? departmentId, int? parentDepartmentId, string name, int? officeId);
    int wm_Departments_Delete(int applicationId, int departmentId);
    #endregion

    #endregion
  }
}
