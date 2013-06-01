using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Components.Contracts.Membership;
using Workmate.Components.Entities.Membership;
using Workmate.Components.Entities.CMS.Articles;
using NSubstitute;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Components.Contracts.CMS;
using Workmate.Components.Contracts;
using Workmate.Components.Entities;
using Workmate.Components.Entities.CMS;

namespace Workmate.Data.MockDataProvider
{
  public class MockDataStoreContext : IDataStoreContext
  {
    #region hard coded data
    private const int JIM_USER_ID = 100000;
    private const string JIM_USER_NAME = "jim";

    private Dictionary<string, UserBasic> _Users = new Dictionary<string, UserBasic>()
    {
      { JIM_USER_NAME, new UserBasic(JIM_USER_ID, JIM_USER_NAME, JIM_USER_NAME + "@workmate.test", DateTime.UtcNow, AccountStatus.Valid, DateTime.UtcNow, DateTime.UtcNow,
          1000, TimeZoneInfo.Local.DisplayName, new List<string>() { UserRole.Registered.ToString()})}
    };
    private Dictionary<int, string> _UserIdUserNameLookup = new Dictionary<int, string>()
    {
      {JIM_USER_ID, JIM_USER_NAME}
    };
    #endregion

    #region members
    private static List<CMSThread> _Threads = new List<CMSThread>();
    private static List<IApplication> _Applications = new List<IApplication>();
    private static List<CMSSection> _Sections = new List<CMSSection>();
    #endregion

    #region IDataStoreContext Members

    public Entities.wm_User_GetPassword_QueryResult wm_Users_GetPassword(int applicationId, string userName, string email)
    {
      switch (userName)
      {
        case JIM_USER_NAME: return new Entities.wm_User_GetPassword_QueryResult()
                    {
                      AccountStatus = _Users[JIM_USER_NAME].AccountStatus,
                      DateCreatedUtc = _Users[JIM_USER_NAME].DateCreatedUtc,
                      Email = _Users[JIM_USER_NAME].Email,
                      LastLoginDateUtc = _Users[JIM_USER_NAME].LastLoginDateUtc,
                      Password = "123",
                      PasswordFormat = 0, // clear
                      ProfileImageId = _Users[JIM_USER_NAME].ProfileImageId,
                      TimeZoneInfoId = _Users[JIM_USER_NAME].TimeZoneInfoId,
                      UserId = JIM_USER_ID,
                      UserName = JIM_USER_NAME
                    };
      }

      return null;
    }

    public int wm_Users_SetPassword(int userId, string newPassword, string passwordSalt, byte passwordFormat)
    {
      throw new NotImplementedException();
    }

    public int wm_Users_Insert(int applicationId, string userName, string email, string password, string passwordSalt, int passwordFormat, byte status, List<string> roleNames, int profileImageId, Guid uniqueId, byte userNameDisplayMode, string timeZoneInfoId, string firstName, string lastName, out int userId, out DateTime dateCreatedUtc)
    {
      throw new NotImplementedException();
    }

    public int wm_UserRole_Insert(IEnumerable<int> userIds, IEnumerable<string> roleNames)
    {
      throw new NotImplementedException();
    }

    public int wm_UserRole_Delete(IEnumerable<int> userIds, IEnumerable<string> roleNames)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> wm_Roles_GetByUserId(int userId)
    {
      if (_UserIdUserNameLookup.ContainsKey(userId))
        return _Users[_UserIdUserNameLookup[userId]].UserRoles;

      return new List<string>();
    }

    public IEnumerable<int> wm_UserRole_GetByRoleName(int applicationId, string roleName)
    {
      throw new NotImplementedException();
    }

    public IUserBasic wm_Users_GetUserBasicByUniqueId(Guid uniqueId, bool updateLastActivity)
    {
      throw new NotImplementedException();
    }

    public IUserBasic wm_Users_GetUserBasicByEmail(string email, int applicationId, bool updateLastActivity)
    {
      throw new NotImplementedException();
    }

    public IUserBasic wm_Users_GetUserBasicByUserName(string userName, int applicationId, bool updateLastActivity)
    {
      throw new NotImplementedException();
    }

    public IUserBasic wm_Users_GetUserBasicByUserId(int userId, bool updateLastActivity)
    {
      if (_UserIdUserNameLookup.ContainsKey(userId))
        return _Users[_UserIdUserNameLookup[userId]];

      return null;
    }

    public Dictionary<int, IUserBasic> wm_Users_GetAllUserBasics(int applicationId)
    {
      throw new NotImplementedException();
    }

    public int wm_Users_GetNumberOfUsersOnline(int applicationId, int minutesSinceLastInActive)
    {
      throw new NotImplementedException();
    }

    public Components.Contracts.Membership.ChangeCredentialsStatus wm_Users_UpdateEmail(int userId, string email)
    {
      throw new NotImplementedException();
    }

    public Components.Contracts.Membership.ChangeCredentialsStatus wm_Users_UpdateUserName(int userId, string newUserName)
    {
      throw new NotImplementedException();
    }

    public int wm_Users_UpdateUserInfo(int userId, bool isPasswordCorrect, bool updateLastLoginActivityDate
      , int maxInvalidPasswordAttempts, out DateTime? lastActivityDateUtc, out DateTime? lastLoginDateUtc
      , out Components.Contracts.Membership.AccountStatus? status, out int? failedPasswordAttemptCount, out DateTime? lastLockoutDateUtc)
    {
      lastActivityDateUtc = DateTime.UtcNow;
      lastLoginDateUtc = DateTime.UtcNow;
      status = AccountStatus.Valid;
      failedPasswordAttemptCount = 0;
      lastLockoutDateUtc = DateTime.MinValue;

      switch (userId)
      {
        case JIM_USER_ID: return 1;
      }

      return -1; // valid
    }

    public int wm_Users_UnlockUser(int userId)
    {
      throw new NotImplementedException();
    }
    
    public int cms_Groups_InsertOrUpdate(int groupId, string name, string description, Components.Contracts.CMS.CMSGroupType groupType)
    {
      throw new NotImplementedException();
    }

    public int cms_Groups_Delete(int groupId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSGroup cms_Groups_Get(int groupId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSGroup> cms_Groups_Get(Components.Contracts.CMS.CMSGroupType groupType)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSGroup> cms_Groups_GetAll()
    {
      throw new NotImplementedException();
    }

    public int cms_Sections_Insert(int applicationId, int? parentSectionId, int? groupId, string name, string description, Components.Contracts.CMS.CMSSectionType sectionType, bool isActive, bool isModerated)
    {
      CMSSection record = new CMSSection(
        applicationId
        , _Sections.Count == 0 ? 1000 : _Sections.Last().CMSSectionId + 1
        , parentSectionId
        , groupId
        , name
        , description
        , sectionType
        , isActive
        , isModerated
        , 0
        , 0);

      _Sections.Add(record);

      return record.CMSSectionId;
    }

    public int cms_Sections_Update(int sectionId, int? parentSectionId, int? groupId, string name, string description, Components.Contracts.CMS.CMSSectionType sectionType, bool isActive, bool isModerated)
    {
      throw new NotImplementedException();
    }

    public int cms_Sections_Delete(int sectionId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSSection cms_Sections_Get(Components.Contracts.CMS.CMSSectionType sectionType, string name)
    {
      return _Sections.Find(c => c.Name.ToLowerInvariant() == name.ToLowerInvariant());
    }

    public Components.Entities.CMS.CMSSection cms_Sections_Get(Components.Contracts.CMS.CMSSectionType sectionType, int sectionId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSSection> cms_Sections_Get(int applicationId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSSection> cms_Sections_Get(int applicationId, Components.Contracts.CMS.CMSSectionType sectionType)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSSection> cms_Sections_GetAll()
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSThread cms_Threads_Get(Components.Contracts.CMS.CMSSectionType sectionType, int threadId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSThread cms_Threads_Get(Components.Contracts.CMS.CMSSectionType sectionType, string name)
    {
      return _Threads.Find(c => c.CMSName.ToLowerInvariant() == name.ToLowerInvariant());
    }

    public IEnumerable<Components.Entities.CMS.CMSThread> cms_Threads_Get(Components.Contracts.CMS.CMSSectionType sectionType)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSThread> cms_Threads_Get(int applicationId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSThread> cms_Threads_Get(int applicationId, Components.Contracts.CMS.CMSSectionType sectionType)
    {
      throw new NotImplementedException();
    }
    
    public int cms_Threads_Insert(int sectionId, string name, DateTime? stickyDateUtc, bool isLocked, bool isSticky
      , bool isApproved, int threadStatus)
    {
      CMSThread record = new CMSThread(
        _Threads.Count == 0 ? 1000 : _Threads.Last().CMSThreadId + 1
        , sectionId
        , name
        , DateTime.UtcNow 
        , stickyDateUtc
        , 0
        , 0
        , isLocked
        , isSticky
        , isApproved
        , 0
        , 0
        , 0
        , DateTime.UtcNow);

      _Threads.Add(record);

      return record.CMSThreadId;
    }

    public int cms_Threads_Update(int threadId, int sectionId, string name, DateTime lastViewedDateUtc, DateTime? stickyDateUtc, bool isLocked, bool isSticky, bool isApproved, int threadStatus)
    {
      throw new NotImplementedException();
    }

    public int cms_Threads_Delete(int threadId)
    {
      throw new NotImplementedException();
    }

    public int cms_Threads_IncreaseTotalViews(int threadId, int numberOfViewsToAdd)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSContent cms_Contents_Get(int contentId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSContent cms_Contents_Get(string urlFriendlyName, string threadName, string sectionName, string groupName)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSContent> cms_Contents_Get(Components.Contracts.CMS.CMSSectionType sectionType)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSContent> cms_Contents_Get(Components.Contracts.CMS.CMSGroupType groupType)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSContent> cms_Contents_Get(int applicationId, Components.Contracts.CMS.CMSSectionType sectionType)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSContent> cms_Contents_Get(int applicationId, Components.Contracts.CMS.CMSGroupType groupType)
    {
      throw new NotImplementedException();
    }

    public List<Components.Entities.CMS.Articles.BaseArticleInfo> cms_Contents_GetBaseArticleInfoPage(int sectionId, int threadId, ref int pageIndex, int pageSize, out int rowCount)
    {
      throw new NotImplementedException();
    }

    public List<Components.Entities.CMS.MessageBoards.MessageInfo> cms_Contents_GetMessageInfoPageFromThreadIndex(int messageBoardThreadId, ref int pageIndex, int pageSize, out int rowCount)
    {
      throw new NotImplementedException();
    }

    public Dictionary<string, string> cms_Contents_GetAllWithSectionName(int applicationId, Components.Contracts.CMS.CMSSectionType sectionType, byte contentStatus)
    {
      throw new NotImplementedException();
    }

    public int cms_Sections_UpdateContentBlock(int applicationId, string name, string formattedBody, int authorUserId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.BaseRatingInfo cms_Contents_GetBaseRatingInfo(int contentId)
    {
      throw new NotImplementedException();
    }

    public int cms_Contents_IncreaseTotalViews(int contentId, int numberOfViewsToAdd)
    {
      throw new NotImplementedException();
    }

    public int cms_Contents_Insert(int threadId, int? parentContentId, int authorUserId, short contentLevel, string subject, string formattedBody, bool isApproved, bool isLocked, byte contentType, byte contentStatus, System.Xml.Linq.XElement extraInfo, string urlFriendlyName, IEnumerable<string> tags, bool createNewThread, int? newThreadSectionId, bool isNewThreadApproved)
    {
      throw new NotImplementedException();
    }

    public int cms_Contents_Update(int contentId, int threadId, int? parentContentId, int authorUserId, short contentLevel, string subject, string formattedBody, bool isApproved, bool isLocked, byte contentType, byte contentStatus, System.Xml.Linq.XElement extraInfo, int? baseContentId, string urlFriendlyName, IEnumerable<string> tags)
    {
      throw new NotImplementedException();
    }

    public int cms_Contents_Delete(int contentId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSFile cms_Files_Get(int fileId)
    {
      throw new NotImplementedException();
    }

    public int cms_Files_MoveTempFile(int tempFileId, int? contentId, bool isProfileImage, bool useExistingRecordValues, string fileName, string friendlyFileName, IEnumerable<string> tags)
    {
      throw new NotImplementedException();
    }

    public int cms_Files_Insert(int? userId, Components.Contracts.CMS.FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width, int? contentId, IEnumerable<string> tags)
    {
      throw new NotImplementedException();
    }

    public int cms_Files_Update(int fileId, int? userId, Components.Contracts.CMS.FileType fileType, string fileName, byte[] content, string contentType, int? contentSize, string friendlyFileName, int? height, int? width, int? contentId, IEnumerable<string> tags)
    {
      throw new NotImplementedException();
    }

    public int cms_Files_Delete(int fileId)
    {
      throw new NotImplementedException();
    }

    public int cms_FilesTemp_Delete(int userId, int fileId)
    {
      throw new NotImplementedException();
    }

    public int cms_FilesTemp_DeleteByUserId(int userId, Components.Contracts.CMS.FileType fileType)
    {
      throw new NotImplementedException();
    }

    public int cms_FilesTemp_Insert(int? userId, Components.Contracts.CMS.FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSContentRating> cms_ContentRatings_Get(int contentId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSContentRating cms_ContentRatings_Get(int userId, int contentId)
    {
      throw new NotImplementedException();
    }

    public int cms_ContentRatings_Delete(int userId, int contentId)
    {
      throw new NotImplementedException();
    }

    public int cms_ContentRatings_InsertOrUpdate(short rating, int contentId, int userId, bool allowSelfRating)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Entities.CMS.CMSThreadRating> cms_ThreadRatings_Get(int threadId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSThreadRating cms_ThreadRatings_Get(int userId, int threadId)
    {
      throw new NotImplementedException();
    }

    public int cms_ThreadRatings_Delete(int userId, int threadId)
    {
      throw new NotImplementedException();
    }

    public int cms_ThreadRatings_InsertOrUpdate(short rating, int threadId, int userId)
    {
      throw new NotImplementedException();
    }

    public int cms_ContentUser_Delete(int contentId, int receivingUserId)
    {
      throw new NotImplementedException();
    }

    public int cms_ContentUser_InsertOrUpdate(int contentId, int receivingUserId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> cms_Tags_GetByContentId(int contentId)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
      // do nothing
    }

    #endregion

    #region constructors
    public MockDataStoreContext() { }
    #endregion

    #region IDataStoreContext Members


    public List<Components.Entities.CMS.Articles.BaseArticleInfo> cms_Contents_GetBaseArticleInfoPage(int? sectionId, int? threadId, List<string> tags, ref int pageIndex, int pageSize, out int rowCount)
    {
      throw new NotImplementedException();
    }
    
    public Components.Entities.CMS.CMSFile cms_Files_Get(int fileId, Components.Contracts.CMS.FileType fileTypeFilter)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSFile cms_FilesTemp_Get(int tempFileId)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.CMSFile cms_FilesTemp_Get(int tempFileId, Components.Contracts.CMS.FileType fileTypeFilter)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> cms_Tags_GetByFileId(int fileId)
    {
      throw new NotImplementedException();
    }

    public Components.Contracts.CMS.Articles.IArticleModel cms_Contents_GetArticleModel(int articleId)
    {
      var articleModel = Substitute.For<IArticleModel>();

      articleModel.ArticleStatus.Returns(ArticleStatus.Published);
      articleModel.FormattedBody.Returns("Mock Article Body");
      articleModel.Subject.Returns("Mock Article Subject");

      return articleModel;
    }

    public Components.Contracts.CMS.Articles.IArticleModel cms_Contents_GetArticleModel(string urlFriendlyName, string threadName, string sectionName)
    {
      var articleModel = Substitute.For<IArticleModel>();

      articleModel.ArticleStatus.Returns(ArticleStatus.Published);
      articleModel.FormattedBody.Returns(string.Format("Mock Article Body for name: {0}, category {1}, subcategory {2}"
        , urlFriendlyName ?? "NA"
        , sectionName ?? "NA"
        , threadName ?? "NA"));
      articleModel.Subject.Returns("Mock Article Subject");

      return articleModel;
    }

    #endregion

    #region IDataStoreContext Members
    
    public int cms_Sections_Delete(int sectionId, bool deleteLinkedThreads)
    {
      throw new NotImplementedException();
    }

    public int cms_Threads_Delete(int threadId, bool deleteLinkedThreads)
    {
      throw new NotImplementedException();
    }

    public int cms_Contents_Insert(int threadId, int? parentContentId, int authorUserId, short contentLevel, string subject
      , string formattedBody, bool isApproved, bool isLocked, byte contentType, byte contentStatus
      , System.Xml.Linq.XElement extraInfo, string urlFriendlyName, IEnumerable<string> tags, IEnumerable<string> contentLevelNodes
      , bool? createLinkedThread, int? linkedThreadSectionId, bool? isLinkedThreadEnabled, Components.Contracts.CMS.LinkedThreadRelationshipType? linkedThreadRelationshipType)
    {
      throw new NotImplementedException();
    }

    public int cms_Contents_Delete(int contentId, bool deleteLinkedThreads)
    {
      throw new NotImplementedException();
    }

    public int cms_Files_MoveTempFile(int tempFileId, int? contentId, bool useExistingRecordValues, string fileName, string friendlyFileName, IEnumerable<string> tags)
    {
      throw new NotImplementedException();
    }

    public IArticleAttachmentModel cms_Files_GetArticleAttachmentModel(int articleAttachmentId)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<IArticleAttachmentModel> cms_Files_GetArticleAttachmentModels(int articleId)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IDataStoreContext Members


    public int cms_ContentLevelNodes_Update(int contentLevelNodeId, string name)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<Components.Contracts.CMS.ICMSContentLevelNode> cms_ContentLevelNodes_Get()
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IDataStoreContext Members


    public int cms_ContentLevelNodes_Insert(IEnumerable<string> contentLevelNodeNames, int? threadId, int? sectionId)
    {
      throw new NotImplementedException();
    }

    public int cms_ContentLevelNodes_Delete(int contentLevelNodeId)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IDataStoreContext Members

    public IEnumerable<Components.Contracts.CMS.ICMSContentLevelNode> cms_ContentLevelNodes_Get(int level, int? parentContentLevelNodeId, int? threadId, int? sectionId)
    {
      for (int i = 0; i < 10; i++)
      {
        var contentLevelNode = Substitute.For<ICMSContentLevelNode>();

        string threadName = "NA";
        if (threadId.HasValue)
        {
          CMSThread thread = _Threads.Find(c => c.CMSThreadId == threadId.Value);
          if (thread != null)
          {
            threadName = thread.CMSName;
          }
        }

        contentLevelNode.ContentLevelNodeId.Returns(level * 10 + i);
        contentLevelNode.Level.Returns(level);
        contentLevelNode.Name.Returns("Category "
          + threadName + " "
          + (parentContentLevelNodeId.HasValue ? parentContentLevelNodeId.Value.ToString() : "ROOT")
          + "/" + i + ", L" + level);
        contentLevelNode.ParentContentLevelNodeId.Returns(parentContentLevelNodeId);

        yield return contentLevelNode;
      }
    }

    #endregion

    #region IDataStoreContext Members

    public Components.Contracts.IApplication wm_Applications_Get(string applicationName)
    {
      return _Applications.Find(c => c.ApplicationName.ToLowerInvariant() == applicationName.ToLowerInvariant());
    }

    public int wm_Applications_Insert(string applicationName, string description)
    {
      IApplication application = _Applications.Find(c => c.ApplicationName.ToLowerInvariant() == applicationName.ToLowerInvariant());
      if (application == null)
      {
        application = new Application(_Applications.Count == 0 ? 1000 : _Applications.Last().ApplicationId + 1, applicationName, null);
        _Applications.Add(application);
      }
      return application.ApplicationId;
    }

    public int wm_Applications_Delete(int applicationId)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IDataStoreContext Members


    public List<IBaseUserModel> wm_Users_GetBaseUserModels(int applicationId, string searchTerm, ref int pageIndex, int pageSize, out int rowCount)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IDataStoreContext Members


    public IEnumerable<CMSFile> cms_Files_GetMultiple(int applicationId, FileType fileTypeFilter)
    {
      throw new NotImplementedException();
    }

    public Components.Entities.CMS.Membership.ProfileImage cms_Files_GetProfileImage(int userId)
    {
      throw new NotImplementedException();
    }

    public int cms_Files_AssignTemporaryProfileImageToUser(int tempFileId)
    {
      throw new NotImplementedException();
    }

    public int cms_Files_Insert(int applicationId, int? userId, FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width, int? contentId, IEnumerable<string> tags)
    {
      throw new NotImplementedException();
    }

    public CMSFile cms_FilesTemp_Get(int tempFileId, int userId, FileType fileTypeFilter)
    {
      throw new NotImplementedException();
    }

    public CMSFile cms_FilesTemp_GetLatest(int userId, FileType fileTypeFilter)
    {
      throw new NotImplementedException();
    }

    public int cms_FilesTemp_Insert(int applicationId, int? userId, FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IDataStoreContext Members


    public CMSSection cms_Sections_Get(int applicationId, CMSSectionType sectionType, string name)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region IDataStoreContext Members


    public int cms_Files_InsertUniqueSystemProfileImage(int applicationId, int? userId, FileType fileType, string fileName, byte[] content, string contentType, int contentSize, string friendlyFileName, int height, int width, int? contentId, IEnumerable<string> tags)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
