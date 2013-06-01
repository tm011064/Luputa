/* DROP CONSTARINTS */

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentLevelNodes] DROP CONSTRAINT FK_cms_ContentLevelNodes_cms_ContentLevelNodes
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentLevelNodes] DROP CONSTRAINT FK_cms_ContentLevelNodes_cms_Sections
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentLevelNodes] DROP CONSTRAINT FK_cms_ContentLevelNodes_cms_Threads
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentRatings] DROP CONSTRAINT FK_cms_ContentRatings_cms_Contents
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentRatings] DROP CONSTRAINT FK_cms_ContentRatings_wm_Users
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
BEGIN
  ALTER TABLE [dbo].[cms_Contents] DROP CONSTRAINT FK_cms_Contents_cms_ContentLevelNodes
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
BEGIN
  ALTER TABLE [dbo].[cms_Contents] DROP CONSTRAINT FK_cms_Contents_cms_Contents
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
BEGIN
  ALTER TABLE [dbo].[cms_Contents] DROP CONSTRAINT FK_cms_Contents_cms_Threads
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentTag] DROP CONSTRAINT FK_cms_ContentTag_cms_Contents
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentTag] DROP CONSTRAINT FK_cms_ContentTag_cms_Tags
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentUser] DROP CONSTRAINT FK_cms_ContentUser_cms_Contents
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentUser] DROP CONSTRAINT FK_cms_ContentUser_wm_Users
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
BEGIN
  ALTER TABLE [dbo].[cms_Files] DROP CONSTRAINT FK_cms_Files_cms_Contents
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
BEGIN
  ALTER TABLE [dbo].[cms_Files] DROP CONSTRAINT FK_cms_Files_wm_Applications
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
BEGIN
  ALTER TABLE [dbo].[cms_Files] DROP CONSTRAINT FK_cms_Files_wm_Users
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
BEGIN
  ALTER TABLE [dbo].[cms_FilesTemp] DROP CONSTRAINT FK_cms_FilesTemp_wm_Applications
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
BEGIN
  ALTER TABLE [dbo].[cms_FilesTemp] DROP CONSTRAINT FK_cms_FilesTemp_wm_Users
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Files]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
BEGIN
  ALTER TABLE [dbo].[cms_FileTag] DROP CONSTRAINT FK_cms_FileTag_cms_Files
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
BEGIN
  ALTER TABLE [dbo].[cms_FileTag] DROP CONSTRAINT FK_cms_FileTag_cms_Tags
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
BEGIN
  ALTER TABLE [dbo].[cms_LinkedThreads] DROP CONSTRAINT FK_cms_LinkedThreads_cms_Contents
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
BEGIN
  ALTER TABLE [dbo].[cms_LinkedThreads] DROP CONSTRAINT FK_cms_LinkedThreads_cms_Threads
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
BEGIN
  ALTER TABLE [dbo].[cms_Sections] DROP CONSTRAINT FK_cms_Sections_cms_Groups
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
BEGIN
  ALTER TABLE [dbo].[cms_Sections] DROP CONSTRAINT FK_cms_Sections_cms_Sections
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
BEGIN
  ALTER TABLE [dbo].[cms_Sections] DROP CONSTRAINT FK_cms_Sections_wm_Applications
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
BEGIN
  ALTER TABLE [dbo].[cms_ThreadRatings] DROP CONSTRAINT FK_cms_ThreadRatings_cms_Threads
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
BEGIN
  ALTER TABLE [dbo].[cms_ThreadRatings] DROP CONSTRAINT FK_cms_ThreadRatings_wm_Users
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Threads_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Threads]'))
BEGIN
  ALTER TABLE [dbo].[cms_Threads] DROP CONSTRAINT FK_cms_Threads_cms_Sections
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_CalendarEvents]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
BEGIN
  ALTER TABLE [dbo].[wm_CalendarEvents] DROP CONSTRAINT FK_wm_CalendarEvents_wm_CalendarEvents
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Calendars]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
BEGIN
  ALTER TABLE [dbo].[wm_CalendarEvents] DROP CONSTRAINT FK_wm_CalendarEvents_wm_Calendars
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
BEGIN
  ALTER TABLE [dbo].[wm_CalendarEvents] DROP CONSTRAINT FK_wm_CalendarEvents_wm_Users
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Calendars_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Calendars]'))
BEGIN
  ALTER TABLE [dbo].[wm_Calendars] DROP CONSTRAINT FK_wm_Calendars_wm_Applications
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
BEGIN
  ALTER TABLE [dbo].[wm_Departments] DROP CONSTRAINT FK_wm_Departments_wm_Applications
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
BEGIN
  ALTER TABLE [dbo].[wm_Departments] DROP CONSTRAINT FK_wm_Departments_wm_Departments
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Offices]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
BEGIN
  ALTER TABLE [dbo].[wm_Departments] DROP CONSTRAINT FK_wm_Departments_wm_Offices
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Emails_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Emails]'))
BEGIN
  ALTER TABLE [dbo].[wm_Emails] DROP CONSTRAINT FK_wm_Emails_wm_Applications
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Offices_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Offices]'))
BEGIN
  ALTER TABLE [dbo].[wm_Offices] DROP CONSTRAINT FK_wm_Offices_wm_Applications
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Roles_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Roles]'))
BEGIN
  ALTER TABLE [dbo].[wm_Roles] DROP CONSTRAINT FK_wm_Roles_wm_Applications
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
BEGIN
  ALTER TABLE [dbo].[wm_UserRole] DROP CONSTRAINT FK_wm_UserRole_wm_Roles
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
BEGIN
  ALTER TABLE [dbo].[wm_UserRole] DROP CONSTRAINT FK_wm_UserRole_wm_Users
END
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Users_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Users]'))
BEGIN
  ALTER TABLE [dbo].[wm_Users] DROP CONSTRAINT FK_wm_Users_wm_Applications
END
GO

/* DROP CHECK CONSTARINTS */

/* DROP INDEXES */

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IX_cms_Contents_LoweredUrlFriendlyName]') AND name = N'cms_Contents')
BEGIN
  ALTER TABLE [dbo].[cms_Contents] DROP CONSTRAINT [IX_cms_Contents_LoweredUrlFriendlyName]
END
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IX_wm_Users_LoweredKeywords]') AND name = N'wm_Users')
BEGIN
  ALTER TABLE [dbo].[wm_Users] DROP CONSTRAINT [IX_wm_Users_LoweredKeywords]
END
GO

/* DROP DEFAULTS */


IF EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Threads_TotalContents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Threads]'))
BEGIN
  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Threads_TotalContents]') AND type = 'D')
  BEGIN
    ALTER TABLE [dbo].[cms_Threads] DROP CONSTRAINT [DF_cms_Threads_TotalContents]    
  END
END
GO


IF EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_wm_Emails_TotalSendAttempts]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Emails]'))
BEGIN
  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_wm_Emails_TotalSendAttempts]') AND type = 'D')
  BEGIN
    ALTER TABLE [dbo].[wm_Emails] DROP CONSTRAINT [DF_wm_Emails_TotalSendAttempts]    
  END
END
GO


IF EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_wm_Users_Gender]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Users]'))
BEGIN
  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_wm_Users_Gender]') AND type = 'D')
  BEGIN
    ALTER TABLE [dbo].[wm_Users] DROP CONSTRAINT [DF_wm_Users_Gender]    
  END
END
GO

/* DROP STORED PROCEDURES */

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Cleanup]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Cleanup];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Update];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_InsertOrUpdate];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetAllWithSectionName]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetAllWithSectionName];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetArticleModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetArticleModel];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetBaseArticleInfoPage]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetBaseArticleInfoPage];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetBaseRatingInfo]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetBaseRatingInfo];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetMessageInfoPageFromThreadIndex]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetMessageInfoPageFromThreadIndex];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_IncreaseTotalViews]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_IncreaseTotalViews];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Update];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentTag_InsertUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentTag_InsertUpdateDelete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentUser_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentUser_InsertOrUpdate];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_GetAttachmentModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_GetAttachmentModel];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_GetProfileImage]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_GetProfileImage];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_MoveTempFile]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_MoveTempFile];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Update];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_DeleteByUserId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_DeleteByUserId];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_GetLatest]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_GetLatest];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FileTag_InsertUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FileTag_InsertUpdateDelete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_InsertOrUpdate];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Update];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_UpdateContentBlock]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_UpdateContentBlock];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags_GetByContentId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Tags_GetByContentId];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags_GetByFileId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Tags_GetByFileId];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_InsertOrUpdate];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_IncreaseTotalViews]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_IncreaseTotalViews];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Update];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_InsertOrUpdate];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Update];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_PutInSendQueue]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_PutInSendQueue];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_SetToSent]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_SetToSent];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Get];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Update];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Roles_GetByUserId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Roles_GetByUserId];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Roles_InsertIfNotExists]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Roles_InsertIfNotExists];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SqlErrorLog_LogLastError]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_SqlErrorLog_LogLastError];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_Delete];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_GetByApplicationId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_GetByApplicationId];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_GetByRoleName]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_GetByRoleName];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetBaseUserModels]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetBaseUserModels];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetNumberOfUsersOnline]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetNumberOfUsersOnline];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetPassword]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetPassword];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetUserBasic]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetUserBasic];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetUserModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetUserModel];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_Insert];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_SetPassword]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_SetPassword];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_UnlockUser]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_UnlockUser];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_UpdateCredentials]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_UpdateCredentials];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_UpdateUserInfo]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_UpdateUserInfo];
END
GO

/* DROP FUNCTIONS */

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitIntegers]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[wm_SplitIntegers];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitStrings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[wm_SplitStrings];
END
GO

/* DROP TABLES */

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_ContentLevelNodes];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_ContentRatings];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_Contents];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_ContentTag];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_ContentUser];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_Files];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_FilesTemp];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FileTag]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_FileTag];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_Groups];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_LinkedThreads];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_Sections];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_Tags];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_ThreadRatings];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_Threads];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Applications];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_CalendarEvents];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Calendars]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Calendars];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Countries]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Countries];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_DatabaseInfo]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_DatabaseInfo];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Departments];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Emails];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Offices];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Roles]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Roles];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SqlErrorLog]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_SqlErrorLog];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_UserRole];
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Users];
END
GO

