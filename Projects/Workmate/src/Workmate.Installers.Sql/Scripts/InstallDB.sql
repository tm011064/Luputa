/* TABLES */

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ContentLevelNodes](
	[ContentLevelNodeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[LoweredName] [nvarchar](256) NOT NULL,
	[Level] [int] NOT NULL,
	[ParentContentLevelNodeId] [int] NULL,
	[BreadCrumbs] [nvarchar](max) NULL,
	[BreadCrumbsSplitIndexes] [nvarchar](max) NULL,
	[ThreadId] [int] NULL,
	[SectionId] [int] NULL,
 CONSTRAINT [PK_cms_ContentLevelNodes] PRIMARY KEY CLUSTERED 
(
	[ContentLevelNodeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ContentRatings](
	[UserId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
	[Rating] [smallint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ContentRatings] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Contents](
	[ContentId] [int] IDENTITY(1,1) NOT NULL,
	[ThreadId] [int] NOT NULL,
	[ParentContentId] [int] NULL,
	[AuthorUserId] [int] NOT NULL,
	[ContentLevel] [smallint] NOT NULL,
	[Subject] [nvarchar](256) NULL,
	[FormattedBody] [nvarchar](max) NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[TotalViews] [int] NOT NULL,
	[ContentType] [tinyint] NOT NULL,
	[RatingSum] [int] NOT NULL,
	[TotalRatings] [int] NOT NULL,
	[ContentStatus] [tinyint] NOT NULL,
	[ExtraInfo] [xml] NOT NULL,
	[BaseContentId] [int] NULL,
	[UrlFriendlyName] [nvarchar](128) NULL,
	[LoweredUrlFriendlyName] [nvarchar](128) NULL,
	[ContentLevelNodeId] [int] NULL,
 CONSTRAINT [PK_cms_Contents] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ContentTag](
	[TagId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
 CONSTRAINT [PK_cms_ContentTag] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ContentUser](
	[ContentId] [int] NOT NULL,
	[ReceivingUserId] [int] NOT NULL,
	[DateReceivedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ContentUser] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC,
	[ReceivingUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Files](
	[ApplicationId] [int] NOT NULL,
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[FileType] [tinyint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[FileName] [nvarchar](1024) NULL,
	[Content] [varbinary](max) NOT NULL,
	[ContentType] [nvarchar](64) NULL,
	[ContentSize] [int] NOT NULL,
	[FriendlyFileName] [nvarchar](256) NULL,
	[Height] [int] NOT NULL,
	[Width] [int] NOT NULL,
	[ContentId] [int] NULL,
 CONSTRAINT [PK_cms_Files] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_FilesTemp](
	[ApplicationId] [int] NOT NULL,
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[FileType] [tinyint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[FileName] [nvarchar](1024) NULL,
	[Content] [varbinary](max) NOT NULL,
	[ContentType] [nvarchar](64) NULL,
	[ContentSize] [int] NOT NULL,
	[FriendlyFileName] [nvarchar](256) NULL,
	[Height] [int] NOT NULL,
	[Width] [int] NOT NULL,
 CONSTRAINT [PK_cms_FilesTemp] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FileTag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_FileTag](
	[FileId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_cms_FileTag] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Groups](
	[GroupId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[GroupType] [tinyint] NOT NULL,
 CONSTRAINT [PK_cms_Groups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_LinkedThreads](
	[ThreadId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
	[RelationshipType] [tinyint] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_cms_LinkedThreads] PRIMARY KEY CLUSTERED 
(
	[ThreadId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Sections](
	[ApplicationId] [int] NOT NULL,
	[SectionId] [int] IDENTITY(1,1) NOT NULL,
	[ParentSectionId] [int] NULL,
	[GroupId] [int] NULL,
	[Name] [nvarchar](128) NOT NULL,
	[LoweredName] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[SectionType] [tinyint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsModerated] [bit] NOT NULL,
	[TotalContents] [int] NOT NULL,
	[TotalThreads] [int] NOT NULL,
 CONSTRAINT [PK_cms_Sections] PRIMARY KEY CLUSTERED 
(
	[SectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Tags](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](32) NOT NULL,
	[LoweredTag] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_cms_Tags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ThreadRatings](
	[UserId] [int] NOT NULL,
	[ThreadId] [int] NOT NULL,
	[Rating] [smallint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ThreadRatings] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Threads](
	[ThreadId] [int] IDENTITY(1,1) NOT NULL,
	[SectionId] [int] NOT NULL,
	[Name] [nvarchar](32) NULL,
	[LoweredName] [nvarchar](32) NULL,
	[LastViewedDateUtc] [datetime] NOT NULL,
	[StickyDateUtc] [datetime] NULL,
	[TotalViews] [int] NOT NULL,
	[TotalReplies] [int] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[IsSticky] [bit] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[RatingSum] [int] NOT NULL,
	[TotalRatings] [int] NOT NULL,
	[ThreadStatus] [int] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[TotalContents] [int] NOT NULL,
 CONSTRAINT [PK_cms_Threads] PRIMARY KEY CLUSTERED 
(
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Applications](
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](512) NULL,
	[ExtraInfo] [xml] NOT NULL,
 CONSTRAINT [PK_wm_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_CalendarEvents](
	[CalendarEventId] [int] IDENTITY(1,1) NOT NULL,
	[CalendarId] [int] NOT NULL,
	[ParentCalendarEventId] [int] NULL,
	[UserId] [int] NOT NULL,
	[DateFromUtc] [datetime] NULL,
	[DateToUtc] [datetime] NULL,
	[CalendarEventType] [tinyint] NOT NULL,
	[CalendarEventStatus] [tinyint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[Comment] [nvarchar](1024) NULL,
 CONSTRAINT [PK_wm_CalendarEvents] PRIMARY KEY CLUSTERED 
(
	[CalendarEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Calendars]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Calendars](
	[CalendarId] [int] NOT NULL,
	[CalendarType] [tinyint] NOT NULL,
	[ApplicationId] [int] NOT NULL,
 CONSTRAINT [PK_wm_Calendars] PRIMARY KEY CLUSTERED 
(
	[CalendarId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Countries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Countries](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[ShortCode] [nvarchar](3) NOT NULL,
 CONSTRAINT [PK_wm_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [UK_wm_Countries_] UNIQUE NONCLUSTERED 
(
	[ShortCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_DatabaseInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_DatabaseInfo](
	[ActionId] [int] IDENTITY(1,1) NOT NULL,
	[ActionType] [nvarchar](32) NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[DatabaseVersion] [decimal](8, 8) NOT NULL,
	[Comment] [nvarchar](1024) NULL,
 CONSTRAINT [PK_wm_DatabaseInfo] PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Departments](
	[ApplicationId] [int] NOT NULL,
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[ParentDepartmentId] [int] NULL,
	[Name] [nvarchar](256) NOT NULL,
	[OfficeId] [int] NULL,
 CONSTRAINT [PK_wm_Departments] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Emails](
	[ApplicationId] [int] NOT NULL,
	[EmailId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[Recipients] [nvarchar](max) NOT NULL,
	[Sender] [nvarchar](256) NOT NULL,
	[CreatedByUserId] [int] NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[SentUtc] [datetime] NULL,
	[QueuedUtc] [datetime] NULL,
	[Status] [tinyint] NOT NULL,
	[Priority] [tinyint] NOT NULL,
	[EmailType] [tinyint] NOT NULL,
	[TotalSendAttempts] [int] NOT NULL,
 CONSTRAINT [PK_wm_Emails] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Offices](
	[ApplicationId] [int] NOT NULL,
	[OfficeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](512) NULL,
	[ExtraInfo] [xml] NOT NULL,
 CONSTRAINT [PK_wm_Offices] PRIMARY KEY CLUSTERED 
(
	[OfficeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](512) NULL,
 CONSTRAINT [PK_wm_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SqlErrorLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_SqlErrorLog](
	[ErrorId] [int] IDENTITY(1,1) NOT NULL,
	[ErrorNumber] [int] NOT NULL,
	[ErrorSeverity] [int] NOT NULL,
	[ErrorState] [int] NOT NULL,
	[ErrorProcedure] [nvarchar](128) NOT NULL,
	[ErrorLine] [int] NOT NULL,
	[ErrorMessage] [nvarchar](4000) NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[SystemUser] [nvarchar](128) NOT NULL,
	[ReturnCode] [int] NULL,
 CONSTRAINT [PK__wm_SqlErrorLog] PRIMARY KEY CLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_UserRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_wm_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [tinyint] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[LoweredEmail] [nvarchar](256) NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[LastLoginDateUtc] [datetime] NOT NULL,
	[LastPasswordChangeDateUtc] [datetime] NOT NULL,
	[LastLockoutDateUtc] [datetime] NOT NULL,
	[LastActivityDateUtc] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[ExtraInfo] [xml] NOT NULL,
	[TimeZoneInfoId] [nvarchar](128) NOT NULL,
	[ProfileImageId] [int] NOT NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[UserNameDisplayMode] [tinyint] NOT NULL,
	[FirstName] [nvarchar](128) NULL,
	[LastName] [nvarchar](128) NULL,
	[LoweredKeywords] [nvarchar](450) NOT NULL,
	[Gender] [tinyint] NOT NULL,
 CONSTRAINT [PK_wm_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

/* DEFAULTS */

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Threads_TotalContents]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Threads] ADD  CONSTRAINT [DF_cms_Threads_TotalContents]  DEFAULT ((0)) FOR [TotalContents]
END

GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_wm_Emails_TotalSendAttempts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[wm_Emails] ADD  CONSTRAINT [DF_wm_Emails_TotalSendAttempts]  DEFAULT ((0)) FOR [TotalSendAttempts]
END

GO

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_wm_Users_Gender]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[wm_Users] ADD  CONSTRAINT [DF_wm_Users_Gender]  DEFAULT ((1)) FOR [Gender]
END

GO

/* CHECK CONSTRAINTS */

/* INDEXES */

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents]') AND name = N'IX_cms_Contents_LoweredUrlFriendlyName')
CREATE NONCLUSTERED INDEX [IX_cms_Contents_LoweredUrlFriendlyName] ON [dbo].[cms_Contents] 
(
	[LoweredUrlFriendlyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users]') AND name = N'IX_wm_Users_LoweredKeywords')
CREATE NONCLUSTERED INDEX [IX_wm_Users_LoweredKeywords] ON [dbo].[wm_Users] 
(
	[LoweredKeywords] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO

/* CONSTRAINTS */

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentLevelNodes_cms_ContentLevelNodes] FOREIGN KEY([ParentContentLevelNodeId])
REFERENCES [cms_ContentLevelNodes] ([ContentLevelNodeId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes] CHECK CONSTRAINT [FK_cms_ContentLevelNodes_cms_ContentLevelNodes]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentLevelNodes_cms_Sections] FOREIGN KEY([SectionId])
REFERENCES [cms_Sections] ([SectionId])
ON DELETE SET NULL
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes] CHECK CONSTRAINT [FK_cms_ContentLevelNodes_cms_Sections]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentLevelNodes_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [cms_Threads] ([ThreadId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes] CHECK CONSTRAINT [FK_cms_ContentLevelNodes_cms_Threads]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
ALTER TABLE [dbo].[cms_ContentRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentRatings_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [cms_Contents] ([ContentId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
ALTER TABLE [dbo].[cms_ContentRatings] CHECK CONSTRAINT [FK_cms_ContentRatings_cms_Contents]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
ALTER TABLE [dbo].[cms_ContentRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentRatings_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
ALTER TABLE [dbo].[cms_ContentRatings] CHECK CONSTRAINT [FK_cms_ContentRatings_wm_Users]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents]  WITH CHECK ADD  CONSTRAINT [FK_cms_Contents_cms_ContentLevelNodes] FOREIGN KEY([ContentLevelNodeId])
REFERENCES [cms_ContentLevelNodes] ([ContentLevelNodeId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents] CHECK CONSTRAINT [FK_cms_Contents_cms_ContentLevelNodes]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents]  WITH CHECK ADD  CONSTRAINT [FK_cms_Contents_cms_Contents] FOREIGN KEY([ParentContentId])
REFERENCES [cms_Contents] ([ContentId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents] CHECK CONSTRAINT [FK_cms_Contents_cms_Contents]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents]  WITH CHECK ADD  CONSTRAINT [FK_cms_Contents_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [cms_Threads] ([ThreadId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents] CHECK CONSTRAINT [FK_cms_Contents_cms_Threads]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
ALTER TABLE [dbo].[cms_ContentTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentTag_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [cms_Contents] ([ContentId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
ALTER TABLE [dbo].[cms_ContentTag] CHECK CONSTRAINT [FK_cms_ContentTag_cms_Contents]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
ALTER TABLE [dbo].[cms_ContentTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentTag_cms_Tags] FOREIGN KEY([TagId])
REFERENCES [cms_Tags] ([TagId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
ALTER TABLE [dbo].[cms_ContentTag] CHECK CONSTRAINT [FK_cms_ContentTag_cms_Tags]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
ALTER TABLE [dbo].[cms_ContentUser]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentUser_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [cms_Contents] ([ContentId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
ALTER TABLE [dbo].[cms_ContentUser] CHECK CONSTRAINT [FK_cms_ContentUser_cms_Contents]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
ALTER TABLE [dbo].[cms_ContentUser]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentUser_wm_Users] FOREIGN KEY([ReceivingUserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
ALTER TABLE [dbo].[cms_ContentUser] CHECK CONSTRAINT [FK_cms_ContentUser_wm_Users]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
ALTER TABLE [dbo].[cms_Files]  WITH CHECK ADD  CONSTRAINT [FK_cms_Files_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [cms_Contents] ([ContentId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
ALTER TABLE [dbo].[cms_Files] CHECK CONSTRAINT [FK_cms_Files_cms_Contents]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
ALTER TABLE [dbo].[cms_Files]  WITH CHECK ADD  CONSTRAINT [FK_cms_Files_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
ALTER TABLE [dbo].[cms_Files] CHECK CONSTRAINT [FK_cms_Files_wm_Applications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
ALTER TABLE [dbo].[cms_Files]  WITH CHECK ADD  CONSTRAINT [FK_cms_Files_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Files_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Files]'))
ALTER TABLE [dbo].[cms_Files] CHECK CONSTRAINT [FK_cms_Files_wm_Users]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
ALTER TABLE [dbo].[cms_FilesTemp]  WITH CHECK ADD  CONSTRAINT [FK_cms_FilesTemp_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
ALTER TABLE [dbo].[cms_FilesTemp] CHECK CONSTRAINT [FK_cms_FilesTemp_wm_Applications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
ALTER TABLE [dbo].[cms_FilesTemp]  WITH CHECK ADD  CONSTRAINT [FK_cms_FilesTemp_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
ALTER TABLE [dbo].[cms_FilesTemp] CHECK CONSTRAINT [FK_cms_FilesTemp_wm_Users]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Files]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
ALTER TABLE [dbo].[cms_FileTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_FileTag_cms_Files] FOREIGN KEY([FileId])
REFERENCES [cms_Files] ([FileId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Files]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
ALTER TABLE [dbo].[cms_FileTag] CHECK CONSTRAINT [FK_cms_FileTag_cms_Files]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
ALTER TABLE [dbo].[cms_FileTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_FileTag_cms_Tags] FOREIGN KEY([TagId])
REFERENCES [cms_Tags] ([TagId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Tags]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
ALTER TABLE [dbo].[cms_FileTag] CHECK CONSTRAINT [FK_cms_FileTag_cms_Tags]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
ALTER TABLE [dbo].[cms_LinkedThreads]  WITH CHECK ADD  CONSTRAINT [FK_cms_LinkedThreads_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [cms_Contents] ([ContentId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
ALTER TABLE [dbo].[cms_LinkedThreads] CHECK CONSTRAINT [FK_cms_LinkedThreads_cms_Contents]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
ALTER TABLE [dbo].[cms_LinkedThreads]  WITH CHECK ADD  CONSTRAINT [FK_cms_LinkedThreads_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [cms_Threads] ([ThreadId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
ALTER TABLE [dbo].[cms_LinkedThreads] CHECK CONSTRAINT [FK_cms_LinkedThreads_cms_Threads]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
ALTER TABLE [dbo].[cms_Sections]  WITH CHECK ADD  CONSTRAINT [FK_cms_Sections_cms_Groups] FOREIGN KEY([GroupId])
REFERENCES [cms_Groups] ([GroupId])
ON DELETE SET NULL
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
ALTER TABLE [dbo].[cms_Sections] CHECK CONSTRAINT [FK_cms_Sections_cms_Groups]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
ALTER TABLE [dbo].[cms_Sections]  WITH CHECK ADD  CONSTRAINT [FK_cms_Sections_cms_Sections] FOREIGN KEY([ParentSectionId])
REFERENCES [cms_Sections] ([SectionId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
ALTER TABLE [dbo].[cms_Sections] CHECK CONSTRAINT [FK_cms_Sections_cms_Sections]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
ALTER TABLE [dbo].[cms_Sections]  WITH CHECK ADD  CONSTRAINT [FK_cms_Sections_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
ALTER TABLE [dbo].[cms_Sections] CHECK CONSTRAINT [FK_cms_Sections_wm_Applications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
ALTER TABLE [dbo].[cms_ThreadRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ThreadRatings_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [cms_Threads] ([ThreadId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
ALTER TABLE [dbo].[cms_ThreadRatings] CHECK CONSTRAINT [FK_cms_ThreadRatings_cms_Threads]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
ALTER TABLE [dbo].[cms_ThreadRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ThreadRatings_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
ALTER TABLE [dbo].[cms_ThreadRatings] CHECK CONSTRAINT [FK_cms_ThreadRatings_wm_Users]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Threads_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Threads]'))
ALTER TABLE [dbo].[cms_Threads]  WITH CHECK ADD  CONSTRAINT [FK_cms_Threads_cms_Sections] FOREIGN KEY([SectionId])
REFERENCES [cms_Sections] ([SectionId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Threads_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Threads]'))
ALTER TABLE [dbo].[cms_Threads] CHECK CONSTRAINT [FK_cms_Threads_cms_Sections]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_CalendarEvents]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents]  WITH CHECK ADD  CONSTRAINT [FK_wm_CalendarEvents_wm_CalendarEvents] FOREIGN KEY([ParentCalendarEventId])
REFERENCES [wm_CalendarEvents] ([CalendarEventId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_CalendarEvents]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents] CHECK CONSTRAINT [FK_wm_CalendarEvents_wm_CalendarEvents]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Calendars]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents]  WITH CHECK ADD  CONSTRAINT [FK_wm_CalendarEvents_wm_Calendars] FOREIGN KEY([CalendarId])
REFERENCES [wm_Calendars] ([CalendarId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Calendars]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents] CHECK CONSTRAINT [FK_wm_CalendarEvents_wm_Calendars]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents]  WITH CHECK ADD  CONSTRAINT [FK_wm_CalendarEvents_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents] CHECK CONSTRAINT [FK_wm_CalendarEvents_wm_Users]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Calendars_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Calendars]'))
ALTER TABLE [dbo].[wm_Calendars]  WITH CHECK ADD  CONSTRAINT [FK_wm_Calendars_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Calendars_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Calendars]'))
ALTER TABLE [dbo].[wm_Calendars] CHECK CONSTRAINT [FK_wm_Calendars_wm_Applications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
ALTER TABLE [dbo].[wm_Departments]  WITH CHECK ADD  CONSTRAINT [FK_wm_Departments_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
ALTER TABLE [dbo].[wm_Departments] CHECK CONSTRAINT [FK_wm_Departments_wm_Applications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
ALTER TABLE [dbo].[wm_Departments]  WITH CHECK ADD  CONSTRAINT [FK_wm_Departments_wm_Departments] FOREIGN KEY([ParentDepartmentId])
REFERENCES [wm_Departments] ([DepartmentId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
ALTER TABLE [dbo].[wm_Departments] CHECK CONSTRAINT [FK_wm_Departments_wm_Departments]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Offices]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
ALTER TABLE [dbo].[wm_Departments]  WITH CHECK ADD  CONSTRAINT [FK_wm_Departments_wm_Offices] FOREIGN KEY([OfficeId])
REFERENCES [wm_Offices] ([OfficeId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Offices]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
ALTER TABLE [dbo].[wm_Departments] CHECK CONSTRAINT [FK_wm_Departments_wm_Offices]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Emails_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Emails]'))
ALTER TABLE [dbo].[wm_Emails]  WITH CHECK ADD  CONSTRAINT [FK_wm_Emails_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Emails_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Emails]'))
ALTER TABLE [dbo].[wm_Emails] CHECK CONSTRAINT [FK_wm_Emails_wm_Applications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Offices_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Offices]'))
ALTER TABLE [dbo].[wm_Offices]  WITH CHECK ADD  CONSTRAINT [FK_wm_Offices_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Offices_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Offices]'))
ALTER TABLE [dbo].[wm_Offices] CHECK CONSTRAINT [FK_wm_Offices_wm_Applications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Roles_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Roles]'))
ALTER TABLE [dbo].[wm_Roles]  WITH CHECK ADD  CONSTRAINT [FK_wm_Roles_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Roles_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Roles]'))
ALTER TABLE [dbo].[wm_Roles] CHECK CONSTRAINT [FK_wm_Roles_wm_Applications]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
ALTER TABLE [dbo].[wm_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_wm_UserRole_wm_Roles] FOREIGN KEY([RoleId])
REFERENCES [wm_Roles] ([RoleId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
ALTER TABLE [dbo].[wm_UserRole] CHECK CONSTRAINT [FK_wm_UserRole_wm_Roles]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
ALTER TABLE [dbo].[wm_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_wm_UserRole_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
ALTER TABLE [dbo].[wm_UserRole] CHECK CONSTRAINT [FK_wm_UserRole_wm_Users]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Users_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Users]'))
ALTER TABLE [dbo].[wm_Users]  WITH CHECK ADD  CONSTRAINT [FK_wm_Users_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Users_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Users]'))
ALTER TABLE [dbo].[wm_Users] CHECK CONSTRAINT [FK_wm_Users_wm_Applications]
GO

/* STORED PROCEDURES */

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Cleanup]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Cleanup];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Cleanup]
(
    @ThreadId     INT = NULL
  , @SectionId    INT = NULL
)
AS
BEGIN

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
    
    -- now, clean up duplicates in case the name already existed. This is done via merging the nodes
    DECLARE @ContentLevelNodesToRemove TABLE 
    (
      ContentLevelNodeId          INT
    );
    DECLARE @Duplicates TABLE 
    (
      ContentLevelNodeId          INT
      , MasterContentLevelNodeId  INT
    );

    DECLARE @MaxLevel INT;
    DECLARE @CurrentLevel INT;

    SET     @CurrentLevel = 0;
    SELECT  @MaxLevel = MAX([Level])
    FROM    cms_ContentLevelNodes

    IF ( @MaxLevel IS NOT NULL )
    BEGIN

      WHILE ( @CurrentLevel <= @MaxLevel )
      BEGIN
       
        -- have a clean table
        DELETE FROM @Duplicates;
       
        INSERT INTO @Duplicates (ContentLevelNodeId, MasterContentLevelNodeId)
        SELECT  nested.ContentLevelNodeId, nested.MasterContentLevelNodeId
        FROM
        (
          SELECT  cln.ContentLevelNodeId
                  , ( SELECT TOP 1 cln2.ContentLevelNodeId 
                      FROM    cms_ContentLevelNodes cln2
                      WHERE   cln2.LoweredName = dup.LoweredName 
                          AND cln2.ParentContentLevelNodeId = dup.ParentContentLevelNodeId ) AS MasterContentLevelNodeId
          FROM
          (
          
            SELECT    LoweredName
                      , ParentContentLevelNodeId
            FROM      cms_ContentLevelNodes
            WHERE     [Level] = @CurrentLevel
                  AND ((@ThreadId IS NULL AND ThreadId IS NULL) OR (ThreadId = @ThreadId))
                  AND ((@SectionId IS NULL AND SectionId IS NULL) OR (SectionId = @SectionId))
            GROUP BY  LoweredName, ParentContentLevelNodeId
            HAVING COUNT(LoweredName) > 1
            
          ) AS dup
            JOIN  cms_ContentLevelNodes cln ON      cln.LoweredName = dup.LoweredName 
                                                AND cln.ParentContentLevelNodeId = dup.ParentContentLevelNodeId
        ) AS nested
        WHERE nested.ContentLevelNodeId <> nested.MasterContentLevelNodeId
          
        SET @ErrorCode = -371;
        -- update the content records which reference the duplicated node       
        UPDATE  c
        SET     c.ContentLevelNodeId = dup.MasterContentLevelNodeId
        FROM    cms_Contents c
          JOIN  @Duplicates  dup  ON c.ContentLevelNodeId = dup.ContentLevelNodeId
          
        SET @ErrorCode = -372;
        -- now, update all children of the duplicated node to reference the new parent node
        UPDATE  cln
        SET     cln.ParentContentLevelNodeId = dup.MasterContentLevelNodeId
        FROM    cms_ContentLevelNodes cln
          JOIN  @Duplicates           dup ON cln.ParentContentLevelNodeId = dup.ContentLevelNodeId

        SET @ErrorCode = -373;
        -- remember which duplicates need to be removed
        INSERT INTO @ContentLevelNodesToRemove
        SELECT      ContentLevelNodeId
        FROM        @Duplicates
        
        SET @CurrentLevel = @CurrentLevel + 1;
        
      END

    END

    SET @ErrorCode = -374;
    -- finally, remove all duplicates
    DELETE cln
    FROM    cms_ContentLevelNodes       cln
      JOIN  @ContentLevelNodesToRemove  dup ON cln.ContentLevelNodeId = dup.ContentLevelNodeId
  
    SET @CurrentLevel = 0;
    WHILE ( @CurrentLevel <= @MaxLevel )
    BEGIN -- update breadcrumbs

      SET @ErrorCode = -375;
      UPDATE  cln
      SET     cln.BreadCrumbs = ISNULL ( p.BreadCrumbs, '' ) + cln.Name
              , cln.BreadCrumbsSplitIndexes = ISNULL ( p.BreadCrumbsSplitIndexes, '' ) 
                                              + CASE 
                                                  WHEN @CurrentLevel = 0 THEN ''
                                                  ELSE ','
                                                END
                                              + CAST(LEN(cln.Name) AS NVARCHAR(32))
      FROM    cms_ContentLevelNodes cln
        LEFT  JOIN  cms_ContentLevelNodes p   ON cln.ParentContentLevelNodeId = p.ContentLevelNodeId
      WHERE   cln.[Level] = @CurrentLevel     
          AND ((@ThreadId IS NULL AND cln.ThreadId IS NULL) OR (cln.ThreadId = @ThreadId))
          AND ((@SectionId IS NULL AND cln.SectionId IS NULL) OR (cln.SectionId = @SectionId))
      
      SET @CurrentLevel = @CurrentLevel + 1;
    END
  
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END




GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Delete]
(
  @ContentLevelNodeId INT
)
AS
BEGIN
    
  ;WITH recursion AS
  (
    SELECT  cln.ContentLevelNodeId, cln.ParentContentLevelNodeId
    FROM    cms_ContentLevelNodes cln
    WHERE   cln.ContentLevelNodeId = @ContentLevelNodeId
    UNION ALL
    SELECT  p.ContentLevelNodeId, p.ParentContentLevelNodeId
    FROM    cms_ContentLevelNodes p
      JOIN  recursion             rec ON p.ParentContentLevelNodeId = rec.ContentLevelNodeId
  )
  DELETE  d
  FROM    cms_ContentLevelNodes d
    JOIN  recursion             rec ON d.ContentLevelNodeId = rec.ContentLevelNodeId

  RETURN @@ROWCOUNT;

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Get];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Get]
(
  @Level                        INT = NULL
  , @ParentContentLevelNodeId   INT = NULL
  , @ThreadId                   INT = NULL
  , @SectionId                  INT = NULL
)
AS
BEGIN
  
  SET NOCOUNT ON;
  
  SELECT  [ContentLevelNodeId]
          ,[Name]
          ,[Level]
          ,[ParentContentLevelNodeId]
          ,[BreadCrumbs]
          ,[BreadCrumbsSplitIndexes]
          ,ThreadId
          ,SectionId
  FROM    [cms_ContentLevelNodes]
  WHERE   (@Level IS NULL OR [Level] = @Level)
      AND (@ParentContentLevelNodeId IS NULL OR [ParentContentLevelNodeId] = @ParentContentLevelNodeId)
      AND (@ThreadId IS NULL OR ThreadId = @ThreadId)
      AND (@SectionId IS NULL OR SectionId = @SectionId)

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Insert];
END
GO



CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Insert]
(
  @Nodes          XML
  , @ThreadId     INT = NULL
  , @SectionId    INT = NULL
)
AS
BEGIN

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
 
    DECLARE @Name                     NVARCHAR(256);
    DECLARE @Level                    INT;
    DECLARE @ContentLevelNodeId       INT;
    DECLARE @ParentContentLevelNodeId INT;
    
    SET @Level = 0;
    
    DECLARE NodesCursor CURSOR LOCAL
    FOR     SELECT  t.a.value('@n','NVARCHAR(256)') AS Name
            FROM    @Nodes.nodes('//i') t(a)
            ORDER BY t.a.value('@l','INT')            
    OPEN    NodesCursor    
    FETCH NEXT FROM NodesCursor INTO @Name
    WHILE (@@FETCH_STATUS <> -1)
    BEGIN
        
      SET @ContentLevelNodeId = NULL;
        
      SET @ErrorCode = -351;
      SELECT  @ContentLevelNodeId         = ContentLevelNodeId
      FROM    cms_ContentLevelNodes
      WHERE       LoweredName = LOWER(@Name)
        AND (     (ParentContentLevelNodeId IS NULL AND @ParentContentLevelNodeId IS NULL)
              OR  (@ParentContentLevelNodeId = ParentContentLevelNodeId) );
                
      IF ( @ContentLevelNodeId IS NULL )
      BEGIN
      
        SET @ErrorCode = -352;        
        IF ( @ParentContentLevelNodeId IS NULL )
        BEGIN
          INSERT INTO cms_ContentLevelNodes ([Name]
                                            ,[LoweredName]
                                            ,[Level]
                                            ,[ParentContentLevelNodeId]
                                            ,[BreadCrumbs]
                                            ,[BreadCrumbsSplitIndexes]
                                            ,[ThreadId]
                                            ,[SectionId])
          VALUES  ( @Name
                    , LOWER(@Name)
                    , @Level
                    , @ParentContentLevelNodeId
                    , @Name
                    , CAST(LEN(@Name) AS NVARCHAR(32))
                    , @ThreadId
                    , @SectionId)
        END
        ELSE
        BEGIN        
          INSERT INTO cms_ContentLevelNodes ([Name]
                                            ,[LoweredName]
                                            ,[Level]
                                            ,[ParentContentLevelNodeId]
                                            ,[BreadCrumbs]
                                            ,[BreadCrumbsSplitIndexes]
                                            ,[ThreadId]
                                            ,[SectionId])
          SELECT  @Name
                  , LOWER(@Name)
                  , @Level
                  , @ParentContentLevelNodeId
                  , cln.BreadCrumbs + @Name
                  , cln.BreadCrumbsSplitIndexes + ',' + CAST(LEN(@Name) AS NVARCHAR(32))
                  , @ThreadId
                  , @SectionId
          FROM    cms_ContentLevelNodes cln
          WHERE   cln.ContentLevelNodeId = @ParentContentLevelNodeId        
        END
        
        SET @ParentContentLevelNodeId = SCOPE_IDENTITY();
        
      END
      ELSE
      BEGIN
        SET @ParentContentLevelNodeId = @ContentLevelNodeId;
      END
    
      SET @Level = @Level + 1;
    
      FETCH NEXT FROM NodesCursor INTO @Name
    END
    CLOSE NodesCursor    
    DEALLOCATE NodesCursor
  
    SET @ErrorCode = @ParentContentLevelNodeId;
  
  END TRY
  BEGIN CATCH
    PRINT ERROR_MESSAGE();
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @ErrorCode

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END



GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Update];
END
GO



CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Update]
(
  @ContentLevelNodeId INT
  , @Name             NVARCHAR(256)
)
AS
BEGIN
  
  IF ( NOT EXISTS ( SELECT * FROM cms_ContentLevelNodes WHERE ContentLevelNodeId = @ContentLevelNodeId ) )
  BEGIN
    RETURN -1003; 
  END
  
  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
 
    SET @ErrorCode = -361;
    -- first, rename
    UPDATE  cms_ContentLevelNodes
    SET     Name = @Name
            , LoweredName = LOWER(@Name)
    WHERE   ContentLevelNodeId = @ContentLevelNodeId
    
    DECLARE @ThreadId     INT
            , @SectionId  INT;
    SELECT  @ThreadId = ThreadId
            , @SectionId = SectionId
    FROM    cms_ContentLevelNodes
    WHERE   ContentLevelNodeId = @ContentLevelNodeId
    
    EXEC @ErrorCode = cms_ContentLevelNodes_Cleanup @ThreadId, @SectionId 
    IF ( @ErrorCode <> 0 )
    BEGIN
      GOTO Cleanup;
    END
    
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END



GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentRatings_Delete]
(
    @UserId     INT
  , @ContentId  INT
)
AS
BEGIN

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
      
    SET @ErrorCode = -401;
    DELETE FROM [dbo].[cms_ContentRatings] 
    WHERE   [UserId]    = @UserId
        AND [ContentId] = @ContentId
                
    SET @ErrorCode = -402;
    UPDATE  cms_Contents
    SET     RatingSum =       ISNULL(( SELECT SUM(Rating) FROM cms_ContentRatings WHERE ContentId = @ContentId ), 0 )
            , TotalRatings =  ISNULL(( SELECT COUNT (*) FROM cms_ContentRatings WHERE ContentId = @ContentId ), 0 )
    WHERE   ContentId = @ContentId
          
  END TRY
  BEGIN CATCH
	PRINT ERROR_MESSAGE();
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_ContentRatings_Get]
(
  @UserId     INT
  , @ContentId  INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [UserId]
          , [ContentId]
          , [Rating]
          , [DateCreatedUtc]
  FROM    [dbo].[cms_ContentRatings]
  WHERE   (@UserId      IS NULL OR [UserId] = @UserId)
      AND (@ContentId   IS NULL OR [ContentId] = @ContentId)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_InsertOrUpdate];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentRatings_InsertOrUpdate]
(
    @Rating           SMALLINT
  , @ContentId        INT
  , @UserId           INT
  , @AllowSelfRating  BIT  
)
AS
BEGIN

  DECLARE @DateCreatedUtc DATETIME;
  SET     @DateCreatedUtc = GETUTCDATE();		

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
  
    IF ( @AllowSelfRating = 0 
         AND ( SELECT AuthorUserId FROM cms_Contents WHERE ContentId = @ContentId ) = @UserId )
    BEGIN
      SET @ErrorCode = -1;
      GOTO Cleanup;
    END
    
    SET @ErrorCode = -411;
    UPDATE  [cms_ContentRatings] WITH (SERIALIZABLE)
    SET     [Rating] = @Rating
          , [DateCreatedUtc]  = @DateCreatedUtc
    WHERE   [UserId] = @UserId
        AND [ContentId] = @ContentId
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      SET @ErrorCode = -412;
      INSERT INTO [dbo].[cms_ContentRatings] 
      (
          [Rating]
        , [DateCreatedUtc]	
        , [UserId]
        , [ContentId]
      ) 
      VALUES
      (
          @Rating
        , @DateCreatedUtc
        , @UserId
        , @ContentId
      );    
    END
    
    SET @ErrorCode = -413;
    UPDATE  cms_Contents
    SET     RatingSum =       ( SELECT SUM(Rating) FROM cms_ContentRatings WHERE ContentId = @ContentId )
            , TotalRatings =  ( SELECT COUNT (*) FROM cms_ContentRatings WHERE ContentId = @ContentId )
    WHERE   ContentId = @ContentId
          
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_Contents_Delete]
(
    @ContentId            INT
    , @DeleteLinkedThreads BIT = 1
)
AS
BEGIN

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
  
    DECLARE @ThreadId INT;
    DECLARE @LinkedThreadId INT;
  
    SET @ErrorCode = -301;    
    SELECT  @ThreadId = ThreadId
    FROM    [cms_Contents]
    WHERE   ContentId = @ContentId
    
    IF ( @ThreadId IS NULL )
    BEGIN
      SET @ErrorCode = -1003;
      GOTO Cleanup;
    END
    
    SET @ErrorCode = -302;   
    SELECT  @LinkedThreadId = ThreadId
    FROM    cms_LinkedThreads
    WHERE   ContentId = @ContentId
    
    IF ( @LinkedThreadId IS NOT NULL )
    BEGIN
    
      SET @ErrorCode = -303;      
      DELETE FROM cms_LinkedThreads
      WHERE       ContentId = @ContentId; 
      
      IF ( @DeleteLinkedThreads = 1 )
      BEGIN
      
        SET @ErrorCode = -304;      
        DELETE FROM cms_Threads
        WHERE       ThreadId = @LinkedThreadId;
      
      END         
    
    END
	           	  
    SET @ErrorCode = -305
    SELECT  @ThreadId = ThreadId
    FROM    [cms_Contents]
    WHERE   ContentId = @ContentId
	           
    SET @ErrorCode = -306
    UPDATE  cms_Threads
    SET     TotalContents = TotalContents - 1
    WHERE   ThreadId = @ThreadId;  
    
    SET @ErrorCode = -307
    UPDATE  s
    SET     s.TotalContents = s.TotalContents - 1
    FROM    cms_Sections  s
      JOIN  cms_Threads   t ON s.SectionId = t.SectionId
    WHERE   t.ThreadId = @ThreadId; 
    
    SET @ErrorCode = -308;      
    DELETE FROM [dbo].[cms_Contents] 
    WHERE       [ContentId] = @ContentId
    
	  SET @ErrorCode = @@ROWCOUNT;
	           
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @ErrorCode;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Contents_Get]
(
  @ContentId          INT           = NULL
  , @SectionType      TINYINT       = NULL
  , @GroupType        TINYINT       = NULL
  , @ApplicationId    INT           = NULL  
  , @UrlFriendlyName  NVARCHAR(128) = NULL
  , @ThreadName       NVARCHAR(32)  = NULL
  , @SectionName      NVARCHAR(128) = NULL
  , @GroupName        NVARCHAR(256) = NULL  
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    c.[ContentId]
          , c.[ThreadId]
          , c.[ParentContentId]
          , c.[AuthorUserId]
          , c.[ContentLevel]
          , c.[Subject]
          , c.[FormattedBody]
          , c.[DateCreatedUtc]
          , c.[IsApproved]
          , c.[IsLocked]
          , c.[TotalViews]
          , c.[ContentType]
          , c.[RatingSum]
          , c.[TotalRatings]
          , c.[ContentStatus]
          , c.[ExtraInfo]
          , c.[BaseContentId]
          , c.[UrlFriendlyName]
          , c.[ContentLevelNodeId]
  FROM    [dbo].[cms_Contents]  c
    JOIN  [dbo].[cms_Threads]   t ON c.ThreadId = t.ThreadId
    JOIN  [dbo].[cms_Sections]  s ON t.SectionId = s.SectionId  
    LEFT JOIN  [dbo].[cms_Groups]    g ON s.GroupId = g.GroupId  
  WHERE   (@ContentId     IS NULL OR c.ContentId = @ContentId)
      AND (@SectionType   IS NULL OR s.SectionType = @SectionType)
      AND (@GroupType     IS NULL OR g.GroupType = @GroupType)
      AND (@ApplicationId IS NULL OR s.ApplicationId = @ApplicationId)
      AND (@UrlFriendlyName IS NULL OR c.LoweredUrlFriendlyName = LOWER(@UrlFriendlyName))
      AND (@ThreadName      IS NULL OR t.LoweredName = LOWER(@ThreadName))
      AND (@SectionName     IS NULL OR s.LoweredName = LOWER(@SectionName))
      AND (@GroupName       IS NULL OR g.Name = @GroupName)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetAllWithSectionName]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetAllWithSectionName];
END
GO

CREATE PROCEDURE cms_Contents_GetAllWithSectionName
(
  @ApplicationId    INT
  , @SectionType    TINYINT  
  , @ContentStatus  TINYINT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT  s.LoweredName
          , c.FormattedBody
  FROM    cms_Contents  c
    JOIN  cms_Threads   t ON c.ThreadId = t.ThreadId
    JOIN  cms_Sections  s ON t.SectionId = s.SectionId
  WHERE   s.ApplicationId = @ApplicationId
      AND s.SectionType = @SectionType
      AND c.ContentStatus = @ContentStatus

END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetArticleModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetArticleModel];
END
GO


CREATE PROCEDURE [dbo].[cms_Contents_GetArticleModel]
(
  @ContentId          INT 
  , @UrlFriendlyName  NVARCHAR(128) = NULL
  , @ThreadName       NVARCHAR(32) = NULL
  , @SectionName      NVARCHAR(128) = NULL
  , @LinkedThreadRelationshipType TINYINT = NULL
)
AS
BEGIN

  SELECT	c.ContentId
          , c.ThreadId
          , t.SectionId
          , c.UrlFriendlyName
          , c.DateCreatedUtc
          , et.TotalContents
          , c.ExtraInfo
          , c.AuthorUserId
          , c.ContentStatus
          , c.Subject
          , c.FormattedBody
          , lt.ThreadId     AS 'LinkedThreadId'
          , lt.IsEnabled    AS 'IsLinkedThreadEnabled'
          , c.ContentLevelNodeId
          , cln.BreadCrumbs
          , cln.BreadCrumbsSplitIndexes
  FROM	  cms_Contents        c    
    JOIN  cms_Threads         t   ON c.ThreadId   = t.ThreadId 
    JOIN  cms_Sections        s   ON t.SectionId  = s.SectionId
    LEFT JOIN  cms_LinkedThreads      lt  ON      c.ContentId = lt.ContentId
                                              AND (@LinkedThreadRelationshipType IS NULL OR lt.RelationshipType = @LinkedThreadRelationshipType) 
    LEFT JOIN  cms_Threads            et  ON lt.ThreadId = et.ThreadId
    LEFT JOIN  cms_ContentLevelNodes  cln ON c.ContentLevelNodeId = cln.ContentLevelNodeId
  WHERE (@ContentId         IS NULL OR c.ContentId = @ContentId)
    AND (@UrlFriendlyName   IS NULL OR c.LoweredUrlFriendlyName = LOWER(@UrlFriendlyName))
    AND (@ThreadName        IS NULL OR t.LoweredName = LOWER(@ThreadName))
    AND (@SectionName       IS NULL OR s.LoweredName = LOWER(@SectionName))
  
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetBaseArticleInfoPage]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetBaseArticleInfoPage];
END
GO



CREATE PROCEDURE [dbo].[cms_Contents_GetBaseArticleInfoPage]
(
  @SectionId    INT
  , @ThreadId   INT
  , @Tags       NVARCHAR(MAX)
  , @PageIndex  INT OUTPUT
  , @PageSize   INT
  , @RowCount   INT OUTPUT  
)
AS
BEGIN

  IF (@PageSize = 0)
  BEGIN
    RETURN;
  END

  SELECT  @RowCount = COUNT(c.ContentId)
  FROM    cms_Contents  c
	       JOIN cms_Threads   t ON c.ThreadId = t.ThreadId
    LEFT JOIN cms_ContentTag  ct    ON c.ContentId = ct.ContentId
    LEFT JOIN cms_Tags        ta    ON ct.TagId = ta.TagId
    LEFT JOIN wm_SplitStrings(@Tags) ss ON ta.LoweredTag = ss.Value 
	WHERE   (@ThreadId  IS NULL OR c.ThreadId = @ThreadId)
	    AND (@SectionId IS NULL OR t.SectionId = @SectionId)
	    AND (@Tags IS NULL OR ss.Value IS NOT NULL)
    
  IF (@RowCount / @PageSize < @PageIndex)
  BEGIN
    SET @PageIndex = @RowCount / @PageSize;
  END;
    
  WITH PagedView AS 
  ( 
	  SELECT  c.ContentId, ROW_NUMBER() OVER (ORDER BY c.ContentId) AS RowNumber 
	  FROM        cms_Contents    c
           JOIN cms_Threads     t     ON c.ThreadId = t.ThreadId
      LEFT JOIN cms_ContentTag  ct    ON c.ContentId = ct.ContentId
	    LEFT JOIN cms_Tags        ta    ON ct.TagId = ta.TagId
	    LEFT JOIN wm_SplitStrings(@Tags) ss ON ta.LoweredTag = ss.Value 	  
	  WHERE   (@ThreadId  IS NULL OR c.ThreadId = @ThreadId)
	      AND (@SectionId IS NULL OR t.SectionId = @SectionId)
	      AND (@Tags IS NULL OR ss.Value IS NOT NULL)
  )
  SELECT	c.ContentId
          , c.ThreadId
          , t.SectionId
          , c.UrlFriendlyName
          , c.DateCreatedUtc
          , et.TotalContents
          , c.ExtraInfo
  FROM	  PagedView     pv
    JOIN  cms_Contents        c   ON pv.ContentId = c.ContentId 
    JOIN  cms_Threads         t   ON c.ThreadId   = t.ThreadId 
    LEFT JOIN  cms_Threads    et  ON t.ExternalThreadId = et.ThreadId
  WHERE	pv.RowNumber > (@PageSize * @PageIndex) 
	  AND pv.RowNumber <= (@PageSize * @PageIndex + @PageSize)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetBaseRatingInfo]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetBaseRatingInfo];
END
GO

CREATE PROCEDURE [dbo].[cms_Contents_GetBaseRatingInfo]
(
  @ContentId  INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [RatingSum]
          , [TotalRatings]
  FROM    [dbo].[cms_Contents]
  WHERE   [ContentId] = @ContentId
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetMessageInfoPageFromThreadIndex]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetMessageInfoPageFromThreadIndex];
END
GO

CREATE PROCEDURE [dbo].[cms_Contents_GetMessageInfoPageFromThreadIndex]
(
  @ThreadId     INT
  , @PageIndex  INT OUTPUT
  , @PageSize   INT
  , @RowCount   INT OUTPUT
)
AS
BEGIN

  IF (@PageSize = 0)
  BEGIN
    RETURN;
  END

  SELECT  @RowCount = COUNT(*)
  FROM    cms_Contents
  WHERE   ThreadId = @ThreadId;
    
  IF (@RowCount / @PageSize < @PageIndex)
  BEGIN
    SET @PageIndex = @RowCount / @PageSize;
  END;
    
  WITH PagedView AS 
  ( 
	  SELECT  ContentId, ROW_NUMBER() OVER (ORDER BY ContentId) AS RowNumber 
	  FROM    cms_Contents
	  WHERE   ThreadId = @ThreadId
  )
  SELECT	c.ContentId
          , c.AuthorUserId
          , c.Subject
          , c.FormattedBody
          , c.DateCreatedUtc
          , c.ContentStatus
          , c.TotalRatings
          , c.RatingSum
          , c.ParentContentId
          , c.ContentLevel
  FROM	  PagedView     pv
    JOIN  cms_Contents  c   ON pv.ContentId = c.ContentId  
  WHERE	pv.RowNumber > (@PageSize * @PageIndex) 
	  AND pv.RowNumber <= (@PageSize * @PageIndex + @PageSize)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_IncreaseTotalViews]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_IncreaseTotalViews];
END
GO

CREATE PROCEDURE [dbo].[cms_Contents_IncreaseTotalViews]
(
    @ContentId               INT
    , @NumberOfViewsToAdd   INT
)
AS
BEGIN

  UPDATE  [dbo].[cms_Contents]
  SET     TotalViews = TotalViews + @NumberOfViewsToAdd
  WHERE   ContentId = @ContentId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Insert];
END
GO


CREATE PROCEDURE [dbo].[cms_Contents_Insert]
(
    @ThreadId         INT
  , @ParentContentId  INT
  , @AuthorUserId     INT
  , @ContentLevel     SMALLINT
  , @Subject          NVARCHAR(256)
  , @FormattedBody    NVARCHAR(max)
  , @IsApproved       BIT
  , @IsLocked         BIT
  , @ContentType      TINYINT
  , @ContentStatus    TINYINT
  , @ExtraInfo        XML
  , @UrlFriendlyName  NVARCHAR(256)
  , @TagXml           XML
  
  , @CreateLinkedThread           BIT = 0
  , @LinkedThreadSectionId        INT = NULL
  , @IsLinkedThreadEnabled        BIT = 0
  , @LinkedThreadRelationshipType TINYINT = NULL  
  
  , @ContentLevelNodesXml   XML = NULL
)
AS
BEGIN
  
  IF (      @UrlFriendlyName IS NOT NULL
       AND  EXISTS (  SELECT  c.* 
                      FROM    cms_Contents  c
                        JOIN  cms_Threads   t ON c.ThreadId = @ThreadId
                      WHERE   LoweredUrlFriendlyName = LOWER(@UrlFriendlyName)))
  BEGIN
    RETURN -1003; -- name not unique
  END
    
  DECLARE @ContentId INT;

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY 

    DECLARE @ContentLevelNodeId INT;
    SET     @ContentLevelNodeId = NULL;
    
    IF ( @ContentLevelNodesXml IS NOT NULL )
    BEGIN
    
      SET @ErrorCode = -316;
      EXEC @ContentLevelNodeId = cms_ContentLevelNodes_Insert @ContentLevelNodesXml
      
      IF ( @ContentLevelNodeId <= 0 )
      BEGIN
        SET @ErrorCode = @ContentLevelNodeId;
        GOTO Cleanup;
      END
          
    END

    SET @ErrorCode = -311;
    INSERT INTO [dbo].[cms_Contents] 
    (
        [ThreadId]
      , [ParentContentId]
      , [AuthorUserId]
      , [ContentLevel]
      , [Subject]
      , [FormattedBody]
      , [DateCreatedUtc]
      , [IsApproved]
      , [IsLocked]
      , [TotalViews]
      , [ContentType]
      , [RatingSum]
      , [TotalRatings]
      , [ContentStatus]
      , [ExtraInfo]
      , [BaseContentId]	
      , [ContentLevelNodeId]
    ) 
    VALUES
    (
        @ThreadId
      , @ParentContentId
      , @AuthorUserId
      , @ContentLevel
      , @Subject
      , @FormattedBody
      , GETUTCDATE()
      , @IsApproved
      , @IsLocked
      , 0
      , @ContentType
      , 0
      , 0
      , @ContentStatus
      , @ExtraInfo
      , NULL
      , @ContentLevelNodeId
    );
    
    SET @ContentId = SCOPE_IDENTITY();
    
    EXEC @ErrorCode = cms_ContentTag_InsertUpdateDelete @ContentId, @TagXml
    IF (@ErrorCode <> 0)
    BEGIN
      GOTO Cleanup;
    END
    
    SET @ErrorCode = -312
    UPDATE  cms_Threads
    SET     TotalContents = TotalContents + 1
    WHERE   ThreadId = @ThreadId;  
    
    SET @ErrorCode = -313
    UPDATE  s
    SET     s.TotalContents = s.TotalContents + 1
    FROM    cms_Sections  s
      JOIN  cms_Threads   t ON s.SectionId = t.SectionId
    WHERE   t.ThreadId = @ThreadId;  
    
        
    IF( @CreateLinkedThread = 1
        AND @LinkedThreadRelationshipType IS NOT NULL
        AND EXISTS (SELECT SectionId FROM cms_Sections WHERE SectionId = @LinkedThreadSectionId))
    BEGIN
            
      DECLARE @LinkedThreadId INT;
      SET @ErrorCode = -314;
      EXEC @LinkedThreadId = cms_Threads_Insert 
        @LinkedThreadSectionId
        , ''
        , NULL 
        , 0
        , 0
        , 1
        , 0 -- ThreadStatus?
    
      IF (@LinkedThreadId <= 0)
      BEGIN
        SET @ErrorCode = @LinkedThreadId;
        GOTO Cleanup;
      END
      
      SET @ErrorCode = -315;
      INSERT INTO [cms_LinkedThreads]
             ([ThreadId]
             ,[ContentId]
             ,[RelationshipType]
             ,[IsEnabled])
       VALUES
             (@LinkedThreadId
             ,@ContentId
             ,@LinkedThreadRelationshipType
             ,@IsLinkedThreadEnabled)
    
    END      
  
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @ContentId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Update];
END
GO


CREATE PROCEDURE [dbo].[cms_Contents_Update]
(
  @ContentId        INT
  , @ThreadId         INT
  , @ParentContentId  INT
  , @AuthorUserId     INT
  , @ContentLevel     SMALLINT
  , @Subject          NVARCHAR(256)
  , @FormattedBody    NVARCHAR(MAX)
  , @IsApproved       BIT
  , @IsLocked         BIT
  , @ContentType      TINYINT
  , @ContentStatus    TINYINT
  , @ExtraInfo        XML
  , @BaseContentId    INT
  , @UrlFriendlyName  NVARCHAR(256)
  , @TagXml           XML
)
AS
BEGIN
  
  IF (      @UrlFriendlyName IS NOT NULL
       AND  EXISTS (  SELECT  c.* 
                      FROM    cms_Contents  c
                        JOIN  cms_Threads   t ON c.ThreadId = @ThreadId
                      WHERE   LoweredUrlFriendlyName = LOWER(@UrlFriendlyName)
                          AND c.ContentId <> @ContentId ))
  BEGIN
    RETURN -1003; -- name not unique
  END

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
  
    SET @ErrorCode = -321;    
    UPDATE  [dbo].[cms_Contents] 
    SET       [ThreadId]        = @ThreadId
            , [ParentContentId] = @ParentContentId
            , [AuthorUserId]    = @AuthorUserId
            , [ContentLevel]    = @ContentLevel
            , [Subject]         = @Subject
            , [FormattedBody]   = @FormattedBody
            , [IsApproved]      = @IsApproved
            , [IsLocked]        = @IsLocked
            , [ContentType]     = @ContentType
            , [ContentStatus]   = @ContentStatus
            , [ExtraInfo]       = @ExtraInfo
            , [BaseContentId]   = @BaseContentId
    WHERE     [ContentId] = @ContentId
      
    EXEC @ErrorCode = cms_ContentTag_InsertUpdateDelete @ContentId, @TagXml
    IF (@ErrorCode <> 0)
    BEGIN
      GOTO Cleanup;
    END
  
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentTag_InsertUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentTag_InsertUpdateDelete];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentTag_InsertUpdateDelete]
(
  @ContentId  INT
  , @TagXml    XML
)
AS
BEGIN
  
  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY

    -- we expect small data, so use xpath over openxml
    SET @ErrorCode = -421;
    DECLARE @TagsFromXml TABLE 
    (
      Tag           NVARCHAR(32)
      , LoweredTag  NVARCHAR(32)
    );
    
    INSERT INTO @TagsFromXml (Tag, LoweredTag)
    SELECT  x.Tag
            , LOWER(x.Tag)
    FROM  
    (
      SELECT t.a.value('@t','NVARCHAR(32)') AS Tag
      FROM @TagXml.nodes('//r') t(a)
    ) x
    
    SET @ErrorCode = -422;    
    INSERT INTO cms_Tags (Tag, LoweredTag)
    SELECT  tfx.Tag, tfx.LoweredTag
    FROM    @TagsFromXml tfx
      WHERE NOT EXISTS ( SELECT LoweredTag FROM cms_Tags WHERE LoweredTag = tfx.LoweredTag)
  
    SET @ErrorCode = -423;
    DELETE FROM cms_ContentTag
    WHERE       ContentId = @ContentId
    
    SET @ErrorCode = -424;
    INSERT INTO cms_ContentTag (ContentId, TagId)
    SELECT      @ContentId, t.TagId
    FROM        cms_Tags      t
          JOIN  @TagsFromXml  tfx ON t.LoweredTag = tfx.LoweredTag
  
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentUser_Delete];
END
GO

CREATE PROCEDURE [dbo].[cms_ContentUser_Delete]
(
    @ContentId        INT
  , @ReceivingUserId  INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_ContentUser] 
  WHERE     [ContentId]       = @ContentId
        AND [ReceivingUserId] = @ReceivingUserId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentUser_InsertOrUpdate];
END
GO

CREATE PROCEDURE [dbo].[cms_ContentUser_InsertOrUpdate]
(
    @ContentId        INT
  , @ReceivingUserId  INT
)
AS
BEGIN

  DECLARE @DateReceivedUtc  DATETIME;
  SET     @DateReceivedUtc = GETUTCDATE();

  SET NOCOUNT ON;

  BEGIN TRAN

    UPDATE  [cms_ContentUser] WITH (SERIALIZABLE)
    SET     [DateReceivedUtc]  = @DateReceivedUtc
    WHERE   [ContentId] = @ContentId
        AND [ReceivingUserId] = @ReceivingUserId
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      INSERT INTO [dbo].[cms_ContentUser] 
      (
          [ContentId]
        , [ReceivingUserId]	
        , [DateReceivedUtc]
      ) 
      VALUES
      (
          @ContentId
        , @ReceivingUserId
        , @DateReceivedUtc
      );    
    END
    
  COMMIT TRAN
  
  RETURN 0;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Delete];
END
GO

CREATE PROCEDURE [dbo].[cms_Files_Delete]
(
    @FileId INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_Files] 
  WHERE       [FileId]  = @FileId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Files_Get]
(
  @ApplicationId  INT = NULL
  , @FileId       INT = NULL
  , @ContentId    INT = NULL
  , @FileType     TINYINT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [FileId]
          , [ApplicationId]
          , [UserId]
          , [FileType]
          , [DateCreatedUtc]
          , [FileName]
          , [Content]
          , [ContentType]
          , [ContentSize]
          , [FriendlyFileName]
          , [Height]
          , [Width]
          , [ContentId]
  FROM    [dbo].[cms_Files]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
    AND   (@FileId IS NULL OR [FileId] = @FileId)
    AND   (@FileType IS NULL OR FileType = @FileType)
    AND   (@ContentId IS NULL OR ContentId = @ContentId)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_GetAttachmentModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_GetAttachmentModel];
END
GO

CREATE PROCEDURE [dbo].[cms_Files_GetAttachmentModel]
(  
  @ApplicationId  INT = NULL
  , @FileId       INT = NULL
  , @ContentId  INT = NULL
  , @FileType   TINYINT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [FileId]
          , [UserId]
          , [DateCreatedUtc]
          , [FileName]
          , [ContentType]
          , [ContentSize]
          , [FriendlyFileName]
          , [ContentId]
  FROM    [dbo].[cms_Files]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
    AND   (@FileId IS NULL OR [FileId] = @FileId)
    AND   (@FileType IS NULL OR FileType = @FileType)
    AND   (@ContentId IS NULL OR ContentId = @ContentId)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_GetProfileImage]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_GetProfileImage];
END
GO



CREATE PROCEDURE [dbo].[cms_Files_GetProfileImage]
(
  @UserId     INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    f.[FileId]
          , f.[UserId]
          , f.[FileType]
          , f.[DateCreatedUtc]
          , f.[FileName]
          , f.[Content]
          , f.[ContentType]
          , f.[ContentSize]
          , f.[FriendlyFileName]
          , f.[Height]
          , f.[Width]
  FROM    [dbo].[cms_Files] f
    JOIN  [dbo].[wm_Users]  u ON f.FileId = u.ProfileImageId
  WHERE   u.UserId = @UserId
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Insert];
END
GO


CREATE PROCEDURE [dbo].[cms_Files_Insert]
(
    @UserId           INT
  , @ApplicationId    INT
  , @FileType         TINYINT
  , @FileName         NVARCHAR(1024)
  , @Content          VARBINARY(MAX)
  , @ContentType      NVARCHAR(64)
  , @ContentSize      INT
  , @FriendlyFileName NVARCHAR(256)
  , @Height           INT
  , @Width            INT
  , @ContentId        INT  
  , @TagXml           XML
  , @IsUniqueSystemProfileImage BIT = 0
)
AS
BEGIN

  DECLARE @FileId INT;

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
  
    IF ( @IsUniqueSystemProfileImage = 1 )
    BEGIN
    
      UPDATE  [cms_Files] WITH (SERIALIZABLE)
      SET       [FriendlyFileName] = @FriendlyFileName
      WHERE     [ApplicationId] = @ApplicationId
            AND [ContentType] = @ContentType
            AND [FriendlyFileName] = @FriendlyFileName;
    
      IF ( @@ROWCOUNT > 0 )
      BEGIN    
        -- this system profile image already exists, so we let the program know via the errorcode
        SET @ErrorCode = -501;
        GOTO Cleanup;
      END
      
    END
  
    SET @ErrorCode = -502;
	  INSERT INTO [dbo].[cms_Files] 
	  (
	    [UserId]
	  , [ApplicationId]
	  , [FileType]
	  , [DateCreatedUtc]
	  , [FileName]
	  , [Content]
	  , [ContentType]
	  , [ContentSize]
	  , [FriendlyFileName]
	  , [Height]
	  , [Width]
	  , [ContentId]	
	  ) 
	  VALUES
	  (
	    @UserId
	  , @ApplicationId
	  , @FileType
	  , GETUTCDATE()
	  , @FileName
	  , @Content
	  , @ContentType
	  , @ContentSize
	  , @FriendlyFileName
	  , @Height
	  , @Width
	  , @ContentId
	  );
    
    SET @FileId = SCOPE_IDENTITY();
    
    EXEC @ErrorCode = cms_FileTag_InsertUpdateDelete @FileId, @TagXml
    IF (@ErrorCode <> 0)
    BEGIN
      GOTO Cleanup;
    END 
  
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @FileId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_MoveTempFile]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_MoveTempFile];
END
GO


CREATE PROCEDURE [dbo].[cms_Files_MoveTempFile]
(
    @TempFileId               INT
  , @ContentId                INT = NULL
  , @UseExistingRecordValues  BIT  
  , @FileName                 NVARCHAR(1024)
  , @FriendlyFileName         NVARCHAR(256)  
  , @TagXml                   XML
  
  , @AssignProfileImageId     BIT = 0  
  , @ProfileImageFileType     TINYINT = NULL
)
AS
BEGIN

  DECLARE @FileId INT;

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
  
    SET @ErrorCode = -1;
    INSERT INTO [dbo].[cms_Files] 
    (
        [UserId]
      , [ApplicationId]
      , [FileType]
      , [DateCreatedUtc]
      , [FileName]
      , [Content]
      , [ContentType]
      , [ContentSize]
      , [FriendlyFileName]
      , [Height]
      , [Width]
      , [ContentId]	
    ) 
    SELECT  fp.UserId
            , fp.ApplicationId
            , fp.FileType
            , GETUTCDATE()
            , CASE WHEN @UseExistingRecordValues = 1 THEN fp.[FileName] ELSE @FileName END
            , fp.Content
            , fp.ContentType
            , fp.ContentSize
            , CASE WHEN @UseExistingRecordValues = 1 THEN fp.[FriendlyFileName] ELSE @FriendlyFileName END
            , fp.Height
            , fp.Width
            , @ContentId
    FROM    cms_FilesTemp fp
    WHERE   fp.FileId = @TempFileId
    
    SET @FileId = SCOPE_IDENTITY();
    
    IF (    @FileId IS NOT NULL
        AND @AssignProfileImageId = 1 
        AND @ProfileImageFileType IS NOT NULL )
    BEGIN
    
      SET @ErrorCode = -2;      
      DECLARE @UserId         INT;
      
      SELECT  @UserId = UserId
      FROM    cms_FilesTemp
      WHERE   FileId = @TempFileId;
      
      SET @ErrorCode = -3;      
      -- first, if the user had a custom profile image before, we delete it
      DECLARE @ProfileImageId INT;
      
      SELECT  @ProfileImageId = f.FileId
      FROM    cms_Files     f
        JOIN  wm_Users      u   ON f.FileId = u.ProfileImageId
      WHERE   u.UserId = @UserId
          AND f.FileType  = @ProfileImageFileType -- check so we don't have a system profile image   
      
      SET @ErrorCode = -4;      
      UPDATE  wm_Users
      SET     ProfileImageId = @FileId
      WHERE   UserId = @UserId
      
      IF ( @ProfileImageId IS NOT NULL )
      BEGIN
      SET @ErrorCode = -5;      
        DELETE FROM cms_Files
        WHERE       FileId = @ProfileImageId;
      END
      
    END
      
    SET @ErrorCode = -6;
    DELETE FROM cms_FilesTemp
    WHERE FileId = @TempFileId;
    
    EXEC @ErrorCode = cms_FileTag_InsertUpdateDelete @FileId, @TagXml
    IF (@ErrorCode <> 0)
    BEGIN
      GOTO Cleanup;
    END
    
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @FileId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Update];
END
GO


CREATE PROCEDURE [dbo].[cms_Files_Update]
(
  @FileId           INT
  , @UserId           INT
  , @FileType         TINYINT
  , @FileName         NVARCHAR(1024)
  , @Content          VARBINARY(MAX)
  , @ContentType      NVARCHAR(64)
  , @ContentSize      INT
  , @FriendlyFileName NVARCHAR(256)
  , @Height           INT
  , @Width            INT
  , @ContentId        INT
  , @TagXml           XML
)
AS
BEGIN


  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
  
    SET @ErrorCode = -521;
	  UPDATE  [dbo].[cms_Files] 
	  SET       [UserId]            = @UserId
		    , [FileType]          = @FileType
		    , [FileName]          = @FileName
		    , [Content]           = @Content
		    , [ContentType]       = @ContentType
		    , [ContentSize]       = @ContentSize
		    , [FriendlyFileName]  = @FriendlyFileName
		    , [Height]            = @Height
		    , [Width]             = @Width
		    , [ContentId]         = @ContentId
	  WHERE     [FileId]  = @FileId
    
    EXEC @ErrorCode = cms_FileTag_InsertUpdateDelete @FileId, @TagXml
    IF (@ErrorCode <> 0)
    BEGIN
      GOTO Cleanup;
    END 
  
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_Delete];
END
GO

CREATE PROCEDURE [dbo].[cms_FilesTemp_Delete]
(
  @UserId     INT
  , @FileId   INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_FilesTemp] 
  WHERE       UserId  = @UserId
          AND FileId  = @FileId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_DeleteByUserId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_DeleteByUserId];
END
GO

CREATE PROCEDURE [dbo].[cms_FilesTemp_DeleteByUserId]
(
  @UserId     INT
  , @FileType TINYINT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_FilesTemp] 
  WHERE       UserId   = @UserId
          AND FileType = @FileType
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_FilesTemp_Get]
(
  @FileId       INT
  , @FileType   TINYINT = NULL
  , @UserId     INT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [FileId]
          , [ApplicationId]
          , [UserId]
          , [FileType]
          , [DateCreatedUtc]
          , [FileName]
          , [Content]
          , [ContentType]
          , [ContentSize]
          , [FriendlyFileName]
          , [Height]
          , [Width]
  FROM    [dbo].[cms_FilesTemp]
  WHERE   [FileId]  = @FileId
    AND   (@FileType IS NULL OR FileType = @FileType)
    AND   (@UserId IS NULL OR UserId = @UserId)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_GetLatest]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_GetLatest];
END
GO


CREATE PROCEDURE [dbo].[cms_FilesTemp_GetLatest]
(
  @UserId     INT 
  , @FileType   TINYINT 
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    TOP 1 
            [FileId]
          , [ApplicationId]
          , [UserId]
          , [FileType]
          , [DateCreatedUtc]
          , [FileName]
          , [Content]
          , [ContentType]
          , [ContentSize]
          , [FriendlyFileName]
          , [Height]
          , [Width]
  FROM    [dbo].[cms_FilesTemp]
  WHERE   UserId  = @UserId
    AND   FileType = @FileType
  ORDER BY DateCreatedUtc DESC
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_Insert];
END
GO

CREATE PROCEDURE [dbo].[cms_FilesTemp_Insert]
(
    @UserId           INT
  , @ApplicationId    INT
  , @FileType         TINYINT
  , @FileName         NVARCHAR(1024)
  , @Content          VARBINARY(MAX)
  , @ContentType      NVARCHAR(64)
  , @ContentSize      INT
  , @FriendlyFileName NVARCHAR(256)
  , @Height           INT
  , @Width            INT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[cms_FilesTemp] 
  (
      [UserId]
    , [ApplicationId]
    , [FileType]
    , [DateCreatedUtc]
    , [FileName]
    , [Content]
    , [ContentType]
    , [ContentSize]
    , [FriendlyFileName]
    , [Height]
    , [Width]	
  ) 
  VALUES
  (
      @UserId
    , @ApplicationId
    , @FileType
    , GETUTCDATE()
    , @FileName
    , @Content
    , @ContentType
    , @ContentSize
    , @FriendlyFileName
    , @Height
    , @Width
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FileTag_InsertUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FileTag_InsertUpdateDelete];
END
GO


CREATE PROCEDURE [dbo].[cms_FileTag_InsertUpdateDelete]
(
  @FileId  INT
  , @TagXml    XML
)
AS
BEGIN
  
  CREATE TABLE #TagsFromXml
  (
    Tag NVARCHAR(32)
    , LoweredTag NVARCHAR(32)
  );
    
  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY

    -- we expect small data, so use xpath over openxml
    SET @ErrorCode = -571;
    INSERT INTO #TagsFromXml (Tag, LoweredTag)
    SELECT  x.Tag
            , LOWER(x.Tag)
    FROM  
    (
      SELECT t.a.value('@t','NVARCHAR(32)') AS Tag
      FROM @TagXml.nodes('//r') t(a)
    ) x
    
    SET @ErrorCode = -572;    
    INSERT INTO cms_Tags (Tag, LoweredTag)
    SELECT  tfx.Tag, tfx.LoweredTag
    FROM    #TagsFromXml tfx
      WHERE NOT EXISTS ( SELECT LoweredTag FROM cms_Tags WHERE LoweredTag = tfx.LoweredTag)
  
    SET @ErrorCode = -573;
    DELETE FROM cms_FileTag
    WHERE       FileId = @FileId
    
    SET @ErrorCode = -574;
    INSERT INTO cms_FileTag (FileId, TagId)
    SELECT      @FileId, t.TagId
    FROM        cms_Tags      t
          JOIN  #TagsFromXml  tfx ON t.LoweredTag = tfx.LoweredTag
  
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  DROP TABLE #TagsFromXml;
  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  DROP TABLE #TagsFromXml;
  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_Delete];
END
GO

CREATE PROCEDURE [dbo].[cms_Groups_Delete]
(
    @GroupId  INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_Groups] 
  WHERE       [GroupId] = @GroupId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Groups_Get]
(
  @GroupId      INT     = NULL
  , @GroupType  TINYINT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [GroupId]
          , [Name]
          , [Description]
          , [GroupType]
  FROM    [dbo].[cms_Groups]
  WHERE   (@GroupId   IS NULL OR GroupId = @GroupId)
      AND (@GroupType IS NULL OR GroupType = @GroupType)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_InsertOrUpdate];
END
GO

CREATE PROCEDURE [dbo].[cms_Groups_InsertOrUpdate]
(
    @GroupId      INT
  , @Name         NVARCHAR(256)
  , @Description  NVARCHAR(1024)
  , @GroupType    TINYINT
)
AS
BEGIN

  SET NOCOUNT ON;

  BEGIN TRAN

    UPDATE  [dbo].[cms_Groups] WITH (SERIALIZABLE)
    SET       [Name]        = @Name
            , [Description] = @Description
            , [GroupType]   = @GroupType
    WHERE     [GroupId] = @GroupId;
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      INSERT INTO [dbo].[cms_Groups] 
      (
          [Name]
        , [Description]
        , [GroupType]	
        , [GroupId]
      ) 
      VALUES
      (
          @Name
        , @Description
        , @GroupType
        , @GroupId
      ); 
    END
    
  COMMIT TRAN
  
  RETURN 0;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_Sections_Delete]
(
  @SectionId  INT
  , @DeleteLinkedThreads BIT = 1
)
AS
BEGIN

  DECLARE @ThreadId INT;

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
  
    DECLARE @t_Threads TABLE ( ThreadId INT );
    
    INSERT INTO @t_Threads ( ThreadId )
    SELECT      lt.ThreadId
    FROM    [dbo].[cms_LinkedThreads] lt
      JOIN  [dbo].[cms_Threads] t ON lt.ThreadId = t.ThreadId  
    WHERE   t.SectionId = @SectionId
    UNION ALL
    SELECT      lt.ThreadId
    FROM    cms_Threads       t 
      JOIN  cms_Contents      c   ON t.ThreadId = c.ThreadId
      JOIN  cms_LinkedThreads lt  ON c.ContentId = lt.ContentId
    WHERE   t.SectionId = @SectionId
    
    SET @ErrorCode = -101;
    DELETE  lt 
    FROM    [cms_LinkedThreads] lt
      JOIN  @t_Threads          t   ON lt.ThreadId = t.ThreadId
    
    IF ( @DeleteLinkedThreads = 1 )
    BEGIN
    
      SET @ErrorCode = -102;
      DELETE  t
      FROM    [cms_Threads] t
        JOIN  @t_Threads    tt  ON t.ThreadId = tt.ThreadId
    
    END
                    
    SET @ErrorCode = -103;
    DELETE FROM [dbo].[cms_Sections] 
    WHERE      [SectionId] = @SectionId
	           
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Sections_Get]
(
  @SectionId        INT           = NULL
  , @SectionType    TINYINT       = NULL
  , @Name           NVARCHAR(128) = NULL 
  , @ApplicationId  INT           = NULL  
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [SectionId]
          , [ParentSectionId]
          , [GroupId]
          , [Name]
          , [Description]
          , [SectionType]
          , [IsActive]
          , [IsModerated]
          , [TotalContents]
          , [TotalThreads]
  FROM    [dbo].[cms_Sections]
  WHERE   (@SectionId     IS NULL OR SectionId = @SectionId)
      AND (@SectionType   IS NULL OR SectionType = @SectionType)
      AND (@ApplicationId IS NULL OR ApplicationId = @ApplicationId)
      AND (@Name          IS NULL OR LoweredName = LOWER(@Name))
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Insert];
END
GO

CREATE PROCEDURE [dbo].[cms_Sections_Insert]
(
    @ApplicationId    INT
  , @ParentSectionId  INT
  , @GroupId          INT
  , @Name             NVARCHAR(128)
  , @Description      NVARCHAR(1024)
  , @SectionType      TINYINT
  , @IsActive         BIT
  , @IsModerated      BIT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[cms_Sections] 
  (
      [ApplicationId]
    , [ParentSectionId]
    , [GroupId]
    , [Name]
    , [LoweredName]
    , [Description]
    , [SectionType]
    , [IsActive]
    , [IsModerated]
    , [TotalContents]
    , [TotalThreads]	
  ) 
  VALUES
  (
      @ApplicationId
    , @ParentSectionId
    , @GroupId
    , @Name
    , LOWER(@Name)
    , @Description
    , @SectionType
    , @IsActive
    , @IsModerated
    , 0
    , 0
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Update];
END
GO

CREATE PROCEDURE [dbo].[cms_Sections_Update]
(
    @SectionId        INT
  , @ParentSectionId  INT
  , @GroupId          INT
  , @Name             NVARCHAR(128)
  , @Description      NVARCHAR(1024)
  , @SectionType      TINYINT
  , @IsActive         BIT
  , @IsModerated      BIT
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE  [dbo].[cms_Sections] 
  SET       [ParentSectionId] = @ParentSectionId
          , [GroupId]         = @GroupId
          , [Name]            = @Name
          , [LoweredName]     = LOWER(@Name)
          , [Description]     = @Description
          , [SectionType]     = @SectionType
          , [IsActive]        = @IsActive
          , [IsModerated]     = @IsModerated
  WHERE     [SectionId] = @SectionId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_UpdateContentBlock]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_UpdateContentBlock];
END
GO


CREATE PROCEDURE [dbo].[cms_Sections_UpdateContentBlock]
(
    @ApplicationId    INT
  , @Name             NVARCHAR(128)
  , @FormattedBody    NVARCHAR(MAX)
  , @SectionType      TINYINT
  , @ActiveContentBlockStatus   TINYINT
  , @InactiveContentBlockStatus TINYINT
  , @AuthorUserId     INT
  , @CreatePlaceholderIfNotExists BIT
)
AS
BEGIN  

  DECLARE @SectionId INT;
  DECLARE @ThreadId INT;
  DECLARE @ContentId INT;

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY

    SELECT  @SectionId = s.SectionId
            , @ThreadId = t.ThreadId
    FROM    cms_Sections  s
      LEFT JOIN  cms_Threads   t ON s.SectionId = t.SectionId
    WHERE   s.ApplicationId = @ApplicationId
        AND s.LoweredName = LOWER(@Name);
         
    IF (@SectionId IS NULL)
    BEGIN
    
      IF (@CreatePlaceholderIfNotExists = 0)
      BEGIN
          SET @ErrorCode = -1;
          GOTO Cleanup;
      END
      ELSE
      BEGIN
      
        EXEC @SectionId = [cms_Sections_Insert] 
           @ApplicationId
          , NULL -- @ParentSectionId
          , NULL -- @GroupId
          , @Name
          , NULL -- @Description
          , @SectionType
          , 1 -- @IsActive
          , 0 -- @IsModerated
                
        IF (@SectionId <= 0)
        BEGIN
          SET @ErrorCode = @SectionId;
          GOTO Cleanup;
        END        
        
      END
                
    END
                
    IF (@ThreadId IS NULL)
    BEGIN
    
      EXEC @ThreadId = [cms_Threads_Insert] 
         @SectionId
        , NULL -- @Name
        , NULL -- @StickyDateUtc
        , 0 -- @IsLocked
        , 0 -- @IsSticky
        , 1 -- @IsApproved
        , 0 -- @ThreadStatus
    
      IF (@ThreadId <= 0)
      BEGIN
        SET @ErrorCode = @ThreadId;
        GOTO Cleanup;
      END
      
    END
    
    SET @ErrorCode = -111;
    UPDATE  cms_Contents
    SET     ContentStatus = @InactiveContentBlockStatus
    WHERE   ThreadId = @ThreadId      

    EXEC @ContentId = [cms_Contents_Insert] 
        @ThreadId -- @ThreadId
      , NULL -- @ParentContentId
      , @AuthorUserId
      , 0 -- @ContentLevel
      , NULL -- @Subject
      , @FormattedBody
      , 1 -- @IsApproved
      , 0 -- @IsLocked
      , 0 -- @ContentType
      , @ActiveContentBlockStatus -- @ContentStatus
      , '<r />' -- @ExtraInfo
      , NULL -- @UrlFriendlyName
      , NULL -- @TagXml
      , 0 -- @CreateNewThread
      , NULL -- @NewThreadSectionId
      , NULL -- @IsNewThreadApproved      
  
    IF (@ContentId <= 0)
    BEGIN
      SET @ErrorCode = @ContentId;
      GOTO Cleanup;
    END

  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @ContentId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags_GetByContentId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Tags_GetByContentId];
END
GO

CREATE PROCEDURE [dbo].[cms_Tags_GetByContentId]
(
  @ContentId  INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT  t.Tag
  FROM    cms_Tags t
    JOIN  cms_ContentTag ct ON t.TagId = ct.TagId
  WHERE   ct.ContentId = @ContentId

END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags_GetByFileId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Tags_GetByFileId];
END
GO

CREATE PROCEDURE [dbo].[cms_Tags_GetByFileId]
(
  @FileId  INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT  t.Tag
  FROM    cms_Tags t
    JOIN  cms_FileTag ft ON t.TagId = ft.TagId
  WHERE   ft.FileId = @FileId

END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_ThreadRatings_Delete]
(
    @UserId     INT
  , @ThreadId  INT
)
AS
BEGIN

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
      
    SET @ErrorCode = -251;
    DELETE FROM [dbo].[cms_ThreadRatings] 
    WHERE   [UserId]    = @UserId
        AND [ThreadId] = @ThreadId
                
    SET @ErrorCode = -252;
    UPDATE  cms_Threads
    SET     RatingSum =       ISNULL(( SELECT SUM(Rating) FROM cms_ThreadRatings WHERE ThreadId = @ThreadId ), 0 )
            , TotalRatings =  ISNULL(( SELECT COUNT (*) FROM cms_ThreadRatings WHERE ThreadId = @ThreadId ), 0 )
    WHERE   ThreadId = @ThreadId
          
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_Get];
END
GO



CREATE PROCEDURE [dbo].[cms_ThreadRatings_Get]
(
  @UserId   INT
  , @ThreadId INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [UserId]
          , [ThreadId]
          , [Rating]
          , [DateCreatedUtc]
  FROM    [dbo].[cms_ThreadRatings]
  WHERE   (@UserId      IS NULL OR [UserId] = @UserId)
      AND (@ThreadId   IS NULL OR [ThreadId] = @ThreadId)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_InsertOrUpdate];
END
GO


CREATE PROCEDURE [dbo].[cms_ThreadRatings_InsertOrUpdate]
(
    @Rating           SMALLINT
  , @ThreadId        INT
  , @UserId           INT
)
AS
BEGIN

  DECLARE @DateCreatedUtc DATETIME;
  SET     @DateCreatedUtc = GETUTCDATE();	
  
  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
      
    SET @ErrorCode = -261;
    UPDATE  [cms_ThreadRatings] WITH (SERIALIZABLE)
    SET     [Rating] = @Rating
          , [DateCreatedUtc]  = @DateCreatedUtc
    WHERE   [UserId] = @UserId
        AND [ThreadId] = @ThreadId
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      SET @ErrorCode = -262;
      INSERT INTO [dbo].[cms_ThreadRatings] 
      (
          [Rating]
        , [DateCreatedUtc]	
        , [UserId]
        , [ThreadId]
      ) 
      VALUES
      (
          @Rating
        , @DateCreatedUtc
        , @UserId
        , @ThreadId
      );    
    END
    
    SET @ErrorCode = -263;
    UPDATE  cms_Threads
    SET     RatingSum =       ( SELECT SUM(Rating) FROM cms_ThreadRatings WHERE ThreadId = @ThreadId )
            , TotalRatings =  ( SELECT COUNT (*) FROM cms_ThreadRatings WHERE ThreadId = @ThreadId )
    WHERE   ThreadId = @ThreadId
          
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_Threads_Delete]
(
    @ThreadId               INT
    , @DeleteLinkedThreads  BIT = 1
)
AS
BEGIN

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
  
    DECLARE @t_Threads TABLE ( ThreadId INT );
    
    INSERT INTO @t_Threads  ( ThreadId )
    SELECT      lt.ThreadId
    FROM    cms_Contents      c   
      JOIN  cms_LinkedThreads lt  ON c.ContentId = lt.ContentId
    WHERE   c.ThreadId = @ThreadId
    
    SET @ErrorCode = -201;
    DELETE  FROM    [cms_LinkedThreads] 
    WHERE   ThreadId = @ThreadId
    
    IF ( @DeleteLinkedThreads = 1 )
    BEGIN
    
      SET @ErrorCode = -202;
      DELETE  lt 
      FROM    [cms_LinkedThreads] lt
        JOIN  @t_Threads          t   ON lt.ThreadId = t.ThreadId
    
      SET @ErrorCode = -203;
      DELETE  t
      FROM    [cms_Threads] t
        JOIN  @t_Threads    tt  ON t.ThreadId = tt.ThreadId
    
    END
    
    SET @ErrorCode = -204;
    DELETE FROM [dbo].[cms_Threads] 
    WHERE       [ThreadId]  = @ThreadId
    
    SET @ErrorCode = @@ROWCOUNT;
	           
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @ErrorCode;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Threads_Get]
(
  @ThreadId         INT           = NULL
  , @SectionType    TINYINT       = NULL
  , @Name           NVARCHAR(128) = NULL 
  , @ApplicationId  INT           = NULL  
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    t.[ThreadId]
          , t.[SectionId]
          , t.[Name]
          , t.[LastViewedDateUtc]
          , t.[StickyDateUtc]
          , t.[TotalViews]
          , t.[TotalReplies]
          , t.[IsLocked]
          , t.[IsSticky]
          , t.[IsApproved]
          , t.[RatingSum]
          , t.[TotalRatings]
          , t.[ThreadStatus]
          , t.[DateCreatedUtc]
  FROM    [dbo].[cms_Threads]   t
    JOIN  [dbo].[cms_Sections]  s ON t.SectionId = s.SectionId    
  WHERE   (@ThreadId      IS NULL OR t.ThreadId = @ThreadId)
      AND (@SectionType   IS NULL OR s.SectionType = @SectionType)
      AND (@ApplicationId IS NULL OR s.ApplicationId = @ApplicationId)
      AND (@Name          IS NULL OR t.LoweredName = LOWER(@Name))
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_IncreaseTotalViews]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_IncreaseTotalViews];
END
GO

CREATE PROCEDURE [dbo].[cms_Threads_IncreaseTotalViews]
(
    @ThreadId               INT
    , @NumberOfViewsToAdd   INT
)
AS
BEGIN

  UPDATE  [dbo].[cms_Threads]
  SET     TotalViews = TotalViews + @NumberOfViewsToAdd
  WHERE   ThreadId = @ThreadId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Insert];
END
GO


CREATE PROCEDURE [dbo].[cms_Threads_Insert]
(
    @SectionId      INT
  , @Name           NVARCHAR(32)
  , @StickyDateUtc  DATETIME
  , @IsLocked       BIT
  , @IsSticky       BIT
  , @IsApproved     BIT
  , @ThreadStatus   INT
)
AS
BEGIN

  DECLARE @ThreadId INT;

  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
      
    SET @ErrorCode = -211;
    INSERT INTO [dbo].[cms_Threads] 
    (
        [SectionId]
      , [Name]
      , [LoweredName]
      , [LastViewedDateUtc]
      , [StickyDateUtc]
      , [TotalViews]
      , [TotalReplies]
      , [IsLocked]
      , [IsSticky]
      , [IsApproved]
      , [RatingSum]
      , [TotalRatings]
      , [ThreadStatus]
      , [DateCreatedUtc]	
    ) 
    VALUES
    (
        @SectionId
      , @Name
      , LOWER(@Name)
      , GETUTCDATE()
      , @StickyDateUtc
      , 0
      , 0
      , @IsLocked
      , @IsSticky
      , @IsApproved
      , 0
      , 0
      , @ThreadStatus
      , GETUTCDATE()
    );
    
    SET @ThreadId = SCOPE_IDENTITY();
    
    SET @ErrorCode = -212;
    UPDATE  s
    SET     s.TotalThreads = TotalThreads + 1
    FROM    cms_Sections  s
      JOIN  cms_Threads   t ON s.SectionId = t.SectionId
    WHERE   t.ThreadId = @ThreadId; 
              
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @ThreadId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Update];
END
GO

CREATE PROCEDURE [dbo].[cms_Threads_Update]
(
  @ThreadId       INT
  , @SectionId      INT
  , @Name           NVARCHAR(32)
  , @LastViewedDateUtc DATETIME
  , @StickyDateUtc  DATETIME
  , @IsLocked       BIT
  , @IsSticky       BIT
  , @IsApproved     BIT
  , @ThreadStatus   INT
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE    [dbo].[cms_Threads] 
  SET       [SectionId]       = @SectionId
          , [Name]            = @Name
          , [LoweredName]     = LOWER(@Name)
          , [LastViewedDateUtc]  = @LastViewedDateUtc
          , [StickyDateUtc]      = @StickyDateUtc
          , [IsLocked]        = @IsLocked
          , [IsSticky]        = @IsSticky
          , [IsApproved]      = @IsApproved
          , [ThreadStatus]    = @ThreadStatus
  WHERE     [ThreadId]  = @ThreadId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_Delete];
END
GO


CREATE PROCEDURE [dbo].[wm_Applications_Delete]
(
    @ApplicationId  INT
)
AS
BEGIN

  DELETE FROM [dbo].[wm_Applications] 
  WHERE      [ApplicationId] = @ApplicationId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_Get];
END
GO

CREATE PROCEDURE [dbo].[wm_Applications_Get]
(
  @ApplicationId      INT
  , @ApplicationName  NVARCHAR(256)
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [ApplicationName]
          , [Description]
          , [ExtraInfo]
  FROM    [dbo].[wm_Applications]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
      AND (@ApplicationName IS NULL OR [LoweredApplicationName] = LOWER(@ApplicationName))
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_InsertOrUpdate];
END
GO

CREATE PROCEDURE [dbo].[wm_Applications_InsertOrUpdate]
(
    @ApplicationName        NVARCHAR(256)
  , @Description            NVARCHAR(512)
  , @ExtraInfo              XML
)
AS
BEGIN

  SET NOCOUNT ON;
  
  DECLARE @ApplicationId INT;

  BEGIN TRAN

    UPDATE    [dbo].[wm_Applications] WITH (SERIALIZABLE)
    SET       [Description] = @Description
              , [ExtraInfo] = @ExtraInfo
    WHERE     [LoweredApplicationName] = LOWER(@ApplicationName);
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      INSERT INTO [dbo].[wm_Applications] 
      (
          [ApplicationName]
        , [LoweredApplicationName]
        , [Description]	
        , [ExtraInfo]	
      ) 
      VALUES
      (
          @ApplicationName
        , LOWER(@ApplicationName)
        , @Description
        , @ExtraInfo
      );
      
      SET @ApplicationId = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
      SELECT    @ApplicationId = ApplicationId
      FROM      [dbo].[wm_Applications]
      WHERE     [LoweredApplicationName] = LOWER(@ApplicationName);
    END
    
  COMMIT TRAN
  
  RETURN @ApplicationId;
    
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Delete];
END
GO


CREATE PROCEDURE [dbo].[wm_Departments_Delete]
(
    @DepartmentId INT
)
AS
BEGIN

  DELETE FROM [dbo].[wm_Departments] 
  WHERE      [DepartmentId]  = @DepartmentId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Get];
END
GO

CREATE PROCEDURE [dbo].[wm_Departments_Get]
(
  @DepartmentId     INT
  , @ApplicationId  INT
  , @OfficeId       INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [DepartmentId]
          , [ParentDepartmentId]
          , [Name]
          , [OfficeId]
  FROM    [dbo].[wm_Departments]
  WHERE   ([DepartmentId] IS NULL OR [DepartmentId] = @DepartmentId)
      AND ([ApplicationId] IS NULL OR [ApplicationId] = @ApplicationId)
      AND ([OfficeId] IS NULL OR [OfficeId] = @OfficeId)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Insert];
END
GO


CREATE PROCEDURE [dbo].[wm_Departments_Insert]
(
    @ApplicationId      INT
  , @ParentDepartmentId INT
  , @Name               NVARCHAR(256)
  , @OfficeId           INT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[wm_Departments] 
  (
      [ApplicationId]
    , [ParentDepartmentId]
    , [Name]
    , [OfficeId]
  ) 
  VALUES
  (
      @ApplicationId
    , @ParentDepartmentId
    , @Name
    , @OfficeId
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Update];
END
GO


CREATE PROCEDURE [dbo].[wm_Departments_Update]
(
  @DepartmentId         INT
  , @ParentDepartmentId INT
  , @Name               NVARCHAR(256)
  , @OfficeId           INT
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE  [dbo].[wm_Departments] 
  SET       [ParentDepartmentId]  = @ParentDepartmentId
          , [Name]                = @Name
          , [OfficeId]            = @OfficeId
  WHERE   [DepartmentId]  = @DepartmentId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Delete];
END
GO


CREATE PROCEDURE [dbo].[wm_Emails_Delete]
(
    @EmailId                   INT
    , @Status                  TINYINT
    , @OlderThanDateCreatedUtc DATETIME
)
AS
BEGIN

  DELETE FROM [dbo].[wm_Emails] 
  WHERE      (@EmailId IS NULL OR [EmailId] = @EmailId)
        AND  (@Status IS NULL OR [Status] = @Status)
        AND  (@OlderThanDateCreatedUtc IS NULL OR [DateCreatedUtc] < @OlderThanDateCreatedUtc)
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Get];
END
GO

CREATE PROCEDURE [dbo].[wm_Emails_Get]
(
  @ApplicationId  INT
  , @Status       TINYINT
  , @EmailId      INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [EmailId]
          , [Subject]
          , [Body]
          , [Recipients]
          , [Sender]
          , [CreatedByUserId]
          , [DateCreatedUtc]
          , [Status]
          , [Priority]
          , [SentUtc]
          , [EmailType]
          , [TotalSendAttempts]
  FROM    [dbo].[wm_Emails]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
    AND   (@Status IS NULL OR [Status] = @Status)
    AND   (@EmailId IS NULL OR [EmailId] = @EmailId)
  ORDER BY [Priority], [DateCreatedUtc] 
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Insert];
END
GO


CREATE PROCEDURE [dbo].[wm_Emails_Insert]
(
    @ApplicationId    INT
  , @Subject          NVARCHAR(256)
  , @Body             NVARCHAR(max)
  , @Recipients       NVARCHAR(max)
  , @Sender           NVARCHAR(256)
  , @CreatedByUserId  INT
  , @Status           TINYINT
  , @Priority         TINYINT
  , @EmailType        TINYINT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[wm_Emails] 
  (
      [ApplicationId]
    , [Subject]
    , [Body]
    , [Recipients]
    , [Sender]
    , [CreatedByUserId]
    , [DateCreatedUtc]
    , [Status]	
    , [Priority]
    , [EmailType]
  ) 
  VALUES
  (
      @ApplicationId
    , @Subject
    , @Body
    , @Recipients
    , @Sender
    , @CreatedByUserId
    , GETUTCDATE()
    , @Status
    , @Priority
    , @EmailType
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_PutInSendQueue]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_PutInSendQueue];
END
GO

CREATE PROCEDURE [dbo].[wm_Emails_PutInSendQueue]
(
  @ApplicationId          INT
  
  , @InQueueStatus        TINYINT
  , @UnsentStatus         TINYINT
  , @SendFailedStatus     TINYINT
  
  , @QueuedEmailsThresholdInSeconds INT -- we send either unsent emails or queued emails that have been queued for
                                        -- a long time. This probably means that the server went down while sending
                                        -- emails, putting the mail into a queue but not acutally sending it out 
  , @FailedEmailsThresholdInSeconds INT  
  , @TotalEmailsToEnqueue           INT
)
AS
BEGIN

  SET NOCOUNT ON;

  DECLARE @QueuedThreshold  DATETIME;
  DECLARE @FailedThreshold  DATETIME;
  DECLARE @Now        DATETIME;
  
  SET     @Now = GETUTCDATE();
  SET     @QueuedThreshold = DATEADD(ss, ABS(@QueuedEmailsThresholdInSeconds) * -1, @Now); 
  SET     @FailedThreshold = DATEADD(ss, ABS(@FailedEmailsThresholdInSeconds) * -1, @Now); 

  DECLARE @TableEmailIds TABLE ( EmailId INT );

  UPDATE    e
  SET       e.Status      = @InQueueStatus
            , e.QueuedUtc = @Now 
  OUTPUT    INSERTED.EmailId 
  INTO      @TableEmailIds
  FROM      wm_Emails e
    JOIN    ( SELECT TOP (@TotalEmailsToEnqueue) s1.EmailId
              FROM        wm_Emails s1
              WHERE       (     s1.Status = @InQueueStatus 
                            AND s1.QueuedUtc IS NOT NULL 
                            AND s1.QueuedUtc < @QueuedThreshold )     -- get all queued emails that have not been sent
                      OR  (     s1.Status = @SendFailedStatus
                            AND s1.SentUtc IS NOT NULL 
                            AND s1.SentUtc < @FailedThreshold ) -- and failed attemps
                      OR  (     s1.Status = @UnsentStatus )
              ORDER BY s1.Priority, s1.DateCreatedUtc ) s ON e.EmailId = s.EmailId

  SELECT    e.[ApplicationId]
          , e.[EmailId]
          , e.[Subject]
          , e.[Body]
          , e.[Recipients]
          , e.[Sender]
          , e.[CreatedByUserId]
          , e.[DateCreatedUtc]
          , e.[Status]
          , e.[Priority]
          , e.[SentUtc]
          , e.[EmailType]
          , e.[TotalSendAttempts]
  FROM    [dbo].[wm_Emails] e
    JOIN  @TableEmailIds    te ON e.EmailId = te.EmailId  
  ORDER BY e.[Priority], e.[DateCreatedUtc]
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_SetToSent]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_SetToSent];
END
GO


CREATE PROCEDURE [dbo].[wm_Emails_SetToSent]
(
  @EmailId          INT
  , @Status         TINYINT
  , @Priority       TINYINT -- if the email failed, we might want to reset the priority for the next attempt
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE  [dbo].[wm_Emails] 
  SET     [SentUtc]   = GETUTCDATE()
          , [Status]  = @Status
          , [TotalSendAttempts] = [TotalSendAttempts] + 1
          , [Priority] = @Priority
  WHERE   [EmailId] = @EmailId
	
  RETURN @@ROWCOUNT;
  
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Delete];
END
GO


CREATE PROCEDURE [dbo].[wm_Offices_Delete]
(
  @ApplicationId  INT
  , @OfficeId     INT
)
AS
BEGIN

  DELETE FROM [dbo].[wm_Offices] 
  WHERE       [OfficeId]  = @OfficeId
        AND   [ApplicationId] = @ApplicationId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Get];
END
GO

CREATE PROCEDURE [dbo].[wm_Offices_Get]
(
  @ApplicationId INT
  , @OfficeId INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [OfficeId]
          , [Name]
          , [Description]
          , [ExtraInfo]
  FROM    [dbo].[wm_Offices]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
      AND (@OfficeId IS NULL OR [OfficeId] = @OfficeId)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Insert];
END
GO


CREATE PROCEDURE [dbo].[wm_Offices_Insert]
(
    @ApplicationId  INT
  , @Name           NVARCHAR(256)
  , @Description    NVARCHAR(512)
  , @ExtraInfo      XML
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[wm_Offices] 
  (
      [ApplicationId]
    , [Name]
    , [Description]
    , [ExtraInfo]	
  ) 
  VALUES
  (
      @ApplicationId
    , @Name
    , @Description
    , @ExtraInfo
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Update];
END
GO


CREATE PROCEDURE [dbo].[wm_Offices_Update]
(
  @OfficeId       INT
  , @Name           NVARCHAR(256)
  , @Description    NVARCHAR(512)
  , @ExtraInfo      XML
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE  [dbo].[wm_Offices] 
  SET       [Name]          = @Name
          , [Description]   = @Description
          , [ExtraInfo]     = @ExtraInfo
  WHERE   [OfficeId]  = @OfficeId
	
  RETURN @@ROWCOUNT;
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Roles_GetByUserId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Roles_GetByUserId];
END
GO

CREATE PROCEDURE [dbo].[wm_Roles_GetByUserId]
(
  @UserId   INT  
)
AS
BEGIN

  SELECT  r.RoleName
  FROM    wm_Roles r
    JOIN  wm_UserRole ur ON r.RoleId = ur.RoleId
  WHERE   ur.UserId = @UserId

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Roles_InsertIfNotExists]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Roles_InsertIfNotExists];
END
GO

CREATE PROCEDURE [dbo].[wm_Roles_InsertIfNotExists]
(
  @ApplicationId  INT
  , @RoleName     NVARCHAR(256)
  , @Description  NVARCHAR(512)
)
AS
BEGIN

  SET NOCOUNT ON;
  
  DECLARE @RoleId INT;

  BEGIN TRAN

    UPDATE    [dbo].[wm_Roles] WITH (SERIALIZABLE)
    SET       [RoleName] = @RoleName
    WHERE     [ApplicationId] = @ApplicationId
          AND [LoweredRoleName] = LOWER(@RoleName);
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      INSERT INTO [dbo].[wm_Roles] 
      (
          [ApplicationId]
        , [RoleName]
        , [LoweredRoleName]
        , [Description]	
      ) 
      VALUES
      (
          @ApplicationId
        , @RoleName
        , LOWER(@RoleName)
        , @Description
      );
      
      SET @RoleId = SCOPE_IDENTITY();
      
    END
    ELSE
    BEGIN
      SELECT    @RoleId = RoleId
      FROM      [dbo].[wm_Roles]
      WHERE     [ApplicationId] = @ApplicationId
            AND [LoweredRoleName] = LOWER(@RoleName);
    END
    
  COMMIT TRAN
  
  RETURN @RoleId;

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SqlErrorLog_LogLastError]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_SqlErrorLog_LogLastError];
END
GO


CREATE PROCEDURE [dbo].[wm_SqlErrorLog_LogLastError]
(
  @ReturnCode INT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;
  
  IF ( ERROR_NUMBER() IS NULL )
    RETURN;
      
  BEGIN TRY  
  
    INSERT INTO [wm_SqlErrorLog] ([ErrorNumber]
                                 ,[ErrorSeverity]
                                 ,[ErrorState]
                                 ,[ErrorProcedure]
                                 ,[ErrorLine]
                                 ,[ErrorMessage]
                                 ,[SystemUser]
                                 ,[DateCreatedUtc]
                                 ,[ReturnCode])
    VALUES(ERROR_NUMBER()
          ,ERROR_SEVERITY()
          ,ERROR_STATE()
          ,ERROR_PROCEDURE()
          ,ERROR_LINE()
          ,ERROR_MESSAGE()
          ,SYSTEM_USER
          ,GETUTCDATE()
          ,@ReturnCode)
          
  END TRY
  BEGIN CATCH
  
  END CATCH
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_Delete];
END
GO

CREATE PROCEDURE [dbo].[wm_UserRole_Delete]
(
  @UserIds      NVARCHAR(MAX)
  , @RoleNames  NVARCHAR(MAX)
)
AS
BEGIN

  DECLARE @Command NVARCHAR(MAX);
  SET     @Command = 
'
  DELETE FROM wm_UserRole
  SELECT  x.UserId, y.RoleId
  FROM    
  (
    SELECT  u.UserId
    FROM    wm_Users u
    WHERE   u.UserId IN (' + @UserIds + ')
  ) AS x,
  (
    SELECT  r.RoleId
    FROM    wm_Roles r
    WHERE   r.LoweredRoleName IN (' + LOWER(@RoleNames) + ')
  ) AS y
';

  EXEC (@Command);

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_GetByApplicationId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_GetByApplicationId];
END
GO

CREATE PROCEDURE [dbo].[wm_UserRole_GetByApplicationId]
(
  @ApplicationId  INT
)
AS
BEGIN

  SELECT  r.RoleName
          , ur.UserId
  FROM    wm_UserRole ur
    JOIN  wm_Roles    r   ON ur.RoleId = r.RoleId
  WHERE   r.ApplicationId = @ApplicationId

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_GetByRoleName]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_GetByRoleName];
END
GO

CREATE PROCEDURE [dbo].[wm_UserRole_GetByRoleName]
(
  @ApplicationId  INT
  , @RoleName NVARCHAR(256)
)
AS
BEGIN

  SELECT  ur.UserId
  FROM    wm_UserRole ur 
    JOIN  wm_Roles    r  ON ur.RoleId = r.RoleId  
  WHERE   r.ApplicationId = @ApplicationId
      AND r.LoweredRoleName = LOWER(@RoleName)

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_Insert];
END
GO

CREATE PROCEDURE [dbo].[wm_UserRole_Insert]
(
  @UserIds      NVARCHAR(MAX)
  , @RoleNames  NVARCHAR(MAX)
)
AS
BEGIN

  DECLARE @Command NVARCHAR(MAX);
  SET     @Command = 
'
  INSERT INTO wm_UserRole (UserId, RoleId)
  SELECT  x.UserId, y.RoleId
  FROM    
  (
    SELECT  u.UserId
    FROM    wm_Users u
    WHERE   u.UserId IN (' + @UserIds + ')
  ) AS x,
  (
    SELECT  r.RoleId
    FROM    wm_Roles r
    WHERE   r.LoweredRoleName IN (' + LOWER(@RoleNames) + ')
  ) AS y
  WHERE NOT EXISTS ( SELECT ur.* FROM wm_UserRole ur WHERE ur.UserId = x.UserId AND ur.RoleId = y.RoleId )
';

  EXEC (@Command);

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetBaseUserModels]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetBaseUserModels];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetBaseUserModels]
(
  @ApplicationId  INT
  , @SearchTerm   NVARCHAR(256)
  , @PageIndex    INT OUTPUT
  , @PageSize     INT
  , @RowCount     INT OUTPUT  
)
AS
BEGIN

  IF (@PageSize = 0)
  BEGIN
    RETURN;
  END

  DECLARE @SearchTermFormatted NVARCHAR(258);
  SET     @SearchTermFormatted = '%' + LOWER(REPLACE(@SearchTerm, '''', '')) + '%';

  SELECT  @RowCount = COUNT(u.UserId)
  FROM    wm_Users  u 
	WHERE   u.ApplicationId = @ApplicationId
	    AND u.LoweredKeywords LIKE @SearchTermFormatted;
    
  IF (@RowCount / @PageSize < @PageIndex)
  BEGIN
    SET @PageIndex = @RowCount / @PageSize;
  END;
    
  WITH PagedView AS 
  ( 
	  SELECT  u.UserId, ROW_NUMBER() OVER (ORDER BY u.UserId) AS RowNumber 
	  FROM    wm_Users  u 
	  WHERE   u.ApplicationId = @ApplicationId
	      AND u.LoweredKeywords LIKE @SearchTermFormatted
  )
  SELECT	u.UserId 
        , u.Email
        , u.ProfileImageId
        , u.FirstName
        , u.LastName
  FROM	  PagedView     pv
    JOIN  wm_Users      u    ON pv.UserId = u.UserId
  WHERE	pv.RowNumber > (@PageSize * @PageIndex) 
	  AND pv.RowNumber <= (@PageSize * @PageIndex + @PageSize)
  
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetNumberOfUsersOnline]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetNumberOfUsersOnline];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetNumberOfUsersOnline]
(
  @ApplicationId              INT
  , @MinutesSinceLastInActive INT
)
AS
BEGIN

    DECLARE @DateActive DATETIME
    SELECT  @DateActive = DATEADD(MINUTE, -(@MinutesSinceLastInActive), GETUTCDATE());

    DECLARE @NumOnline INT;
    
    SELECT  @NumOnline = COUNT(*)
    FROM      wm_Users u        (NOLOCK)
            , wm_Applications a (NOLOCK)
    WHERE     u.ApplicationId = a.ApplicationId                  
          AND u.LastActivityDateUtc > @DateActive
          AND u.ApplicationId = @ApplicationId;
            
    RETURN(@NumOnline)
    
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetPassword]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetPassword];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetPassword]
(
  @ApplicationId                INT
  , @UserName                   NVARCHAR(256)
  , @Email                      NVARCHAR(256)
)
AS
BEGIN

  DECLARE @ErrorCode INT;
  SET     @ErrorCode = 0;
  
  IF (@UserName IS NULL AND @Email IS NULL)
  BEGIN
    SET @ErrorCode = -1;
    RETURN @ErrorCode;  
  END

  SELECT  u.[Password]
          , u.[Status]
          , u.[UserId]
          , u.[Email]
          , u.[DateCreatedUtc]
          , u.[LastLoginDateUtc]
          , u.[PasswordFormat]
          , u.[PasswordSalt]
          , u.[ProfileImageId]
          , u.[UserName]
          , u.[TimeZoneInfoId]
  FROM    wm_Users u
  WHERE   u.ApplicationId = @ApplicationId
      AND (@UserName  IS NULL OR LOWER(@UserName) = u.[LoweredUserName])
      AND (@Email     IS NULL OR LOWER(@Email) = u.[LoweredEmail])
      
  RETURN 0;

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetUserBasic]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetUserBasic];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetUserBasic]
(
  @UniqueId             UNIQUEIDENTIFIER = NULL
  , @Email              NVARCHAR(256) = NULL
  , @UserName           NVARCHAR(256) = NULL
  , @UserId             INT = NULL
  , @ApplicationId      INT
  , @UpdateLastActivity BIT
)
AS
BEGIN

  IF (    @UpdateLastActivity = 1
      AND (    @UniqueId IS NOT NULL  
            OR @UserId IS NOT NULL 
            OR @Email IS NOT NULL 
            OR @UserName IS NOT NULL ))
  BEGIN
  
    UPDATE  wm_Users
    SET     LastActivityDateUtc = GETUTCDATE()
    WHERE   (@UniqueId  IS NULL OR UniqueId = @UniqueId)
        AND (@UserId    IS NULL OR UserId = @UserId)
        AND (@Email     IS NULL OR (ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email)))
        AND (@UserName  IS NULL OR (ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)))
    
  END

  SELECT  u.UserId
          , u.Email
          , u.Status
          , u.DateCreatedUtc
          , u.UserName
          , u.LastActivityDateUtc
          , u.LastLoginDateUtc
          , u.ProfileImageId
          , u.TimeZoneInfoId
  FROM    wm_Users u
  WHERE   (@UniqueId  IS NULL OR u.UniqueId = @UniqueId)
      AND (@UserId    IS NULL OR u.UserId = @UserId)
      AND (@Email     IS NULL OR (u.ApplicationId = @ApplicationId AND u.LoweredEmail = LOWER(@Email)))
      AND (@UserName  IS NULL OR (u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)))
      OR  (     @UniqueId IS NULL  
            AND @UserId IS NULL 
            AND @Email IS NULL 
            AND @UserName IS NULL
            AND u.ApplicationId = @ApplicationId) -- get all per application...

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetUserModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetUserModel];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetUserModel]
(
  @UserId INT
)
AS
BEGIN

  SELECT  u.UserId
          , u.Email
          , u.Status
          , u.DateCreatedUtc
          , u.UserName
          
          , u.LastActivityDateUtc
          , u.LastLoginDateUtc
          , u.LastPasswordChangeDateUtc
          , u.LastLockoutDateUtc
          
          , u.FailedPasswordAttemptCount
          , u.UniqueId
          , u.UserNameDisplayMode
          
          , u.ExtraInfo
          , u.ProfileImageId
          , u.TimeZoneInfoId
          , u.FirstName
          , u.LastName
          
          , u.Gender
  FROM    wm_Users u
  WHERE   u.UserId = @UserId

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_Insert];
END
GO


CREATE PROCEDURE [dbo].[wm_Users_Insert]
(
  @ApplicationId          INT
  , @UserName             NVARCHAR(256)
  , @Email                NVARCHAR(256)
  , @Password             NVARCHAR(256)
  , @PasswordSalt         NVARCHAR(128)
  , @PasswordFormat       INT
  , @Status               TINYINT
  , @RoleNames            NVARCHAR(MAX)
  , @ProfileImageId       INT
  , @UniqueId             UNIQUEIDENTIFIER
  , @UserNameDisplayMode  TINYINT
  , @TimeZoneInfoId       NVARCHAR(128)
  , @FirstName            NVARCHAR(128)
  , @LastName             NVARCHAR(128)
  , @Gender               TINYINT
  , @UserId               INT       OUT
  , @DateCreatedUtc       DATETIME  OUT  
)
AS
BEGIN
  
  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY

    SET @DateCreatedUtc = GETUTCDATE();

    IF ( EXISTS ( SELECT * FROM wm_Users WHERE ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName) ) )
    BEGIN
      SET @ErrorCode = -1;
      GOTO Cleanup;
    END
    IF ( EXISTS ( SELECT * FROM wm_Users WHERE ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email) ) )
    BEGIN
      SET @ErrorCode = -2;
      GOTO Cleanup;
    END
    
    SET @ErrorCode = -3;
    INSERT INTO [dbo].[wm_Users] 
    (
        [ApplicationId]
      , [Password]
      , [PasswordFormat]
      , [PasswordSalt]
      , [Email]
      , [LoweredEmail]
      , [UserName]
      , [LoweredUserName]
      , [Status]
      , [DateCreatedUtc]
      , [LastLoginDateUtc]
      , [LastPasswordChangeDateUtc]
      , [LastLockoutDateUtc]
      , [LastActivityDateUtc]
      , [FailedPasswordAttemptCount]
      , [ExtraInfo]
      , [TimeZoneInfoId]
      , [ProfileImageId]
      , [UniqueId]
      , [UserNameDisplayMode]
      , [FirstName]
      , [LastName]	
      , [Gender]
      , [LoweredKeywords]
    ) 
    VALUES
    (
        @ApplicationId
      , @Password
      , @PasswordFormat
      , @PasswordSalt
      , @Email
      , LOWER(@Email)
      , @UserName
      , LOWER(@UserName)
      , @Status
      , @DateCreatedUtc
      , @DateCreatedUtc
      , @DateCreatedUtc
      , @DateCreatedUtc
      , @DateCreatedUtc
      , 0
      , '<i></i>'
      , @TimeZoneInfoId
      , @ProfileImageId
      , @UniqueId
      , @UserNameDisplayMode
      , @FirstName
      , @LastName
      , @Gender
      , LOWER(@Email)
        + ' ' + LOWER(ISNULL(REPLACE(@FirstName, '''', ''), '')) 
        + ' ' + LOWER(ISNULL(REPLACE(@LastName, '''', ''), '')) 
        + ' ' + LOWER(ISNULL(REPLACE(@FirstName, '''', ''), ''))
    );
    
    SET @UserId = SCOPE_IDENTITY();
    
    IF (@RoleNames IS NOT NULL)
    BEGIN
      SET @ErrorCode = -4;
      DECLARE @UserIds NVARCHAR(MAX);
      SET     @UserIds = CAST(@UserId AS NVARCHAR(MAX));
      
      EXEC wm_UserRole_Insert @UserIds, @RoleNames
    END
    
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;
        
  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_SetPassword]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_SetPassword];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_SetPassword]
(
  @UserId           INT
  , @NewPassword    NVARCHAR(128)
  , @PasswordSalt   NVARCHAR(128)
  , @PasswordFormat TINYINT
)
AS
BEGIN

  UPDATE  wm_Users
  SET       [Password] = @NewPassword
          , [PasswordSalt] = @PasswordSalt
          , [PasswordFormat] = @PasswordFormat  
          , [LastPasswordChangeDateUtc] = GETUTCDATE()          
  WHERE   UserId = @UserId
  
  RETURN @@ROWCOUNT;

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_UnlockUser]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_UnlockUser];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_UnlockUser]
(
  @UserId INT
)
AS
BEGIN
    
  UPDATE  wm_Users
  SET     [Status] = (
                        SELECT  CASE 
                                  WHEN u.[Status] = 6 THEN 1  -- was Locked, so Valid
                                  WHEN u.[Status] = 7 THEN 5  -- was LockedAwaitingEmailVerification, so AwaitingEmailVerification
                                  ELSE 1
                                END
                        FROM    wm_Users u
                        WHERE   u.UserId = @UserId
                      )
          , LastLockoutDateUtc = CONVERT( DATETIME, '17540101', 112 )
          , FailedPasswordAttemptCount = 0
  WHERE   UserId = @UserId

  RETURN @@ROWCOUNT;

END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_UpdateCredentials]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_UpdateCredentials];
END
GO


CREATE PROCEDURE [dbo].[wm_Users_UpdateCredentials]
(
  @UserId         INT
  , @NewUserName  NVARCHAR(256) = NULL
  , @NewEmail     NVARCHAR(256) = NULL
)
AS
BEGIN
  
  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY

    DECLARE @ApplicationId INT;
    SELECT  @ApplicationId = ApplicationId
    FROM    wm_Users
    WHERE   UserId = @UserId;

    IF ( @ApplicationId IS NULL )
    BEGIN
      SET @ErrorCode = -3;
      GOTO Cleanup;
    END
    
    IF ( @NewUserName IS NOT NULL )
    BEGIN
    
      IF ( EXISTS ( SELECT * FROM wm_Users WHERE ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@NewUserName) ) )
      BEGIN
        SET @ErrorCode = -1;
        GOTO Cleanup;
      END
      
      SET @ErrorCode = -101;
      UPDATE  wm_Users
      SET     UserName = @NewUserName
              , LoweredUserName = LOWER(@NewUserName)
      WHERE   UserId = @UserId;
      
    END
    
    IF ( @NewEmail IS NOT NULL )
    BEGIN
    
      IF ( EXISTS ( SELECT * FROM wm_Users WHERE ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@NewEmail) ) )
      BEGIN
        SET @ErrorCode = -2;
        GOTO Cleanup;
      END
      
      SET @ErrorCode = -102;
      UPDATE  wm_Users
      SET     Email = @NewEmail
              , LoweredEmail = LOWER(@NewEmail)
      WHERE   UserId = @UserId;
      
      SET @ErrorCode = -103;
      UPDATE  wm_Users
      SET     LoweredKeywords = LoweredEmail 
                                + ' ' + LOWER(ISNULL(REPLACE(FirstName, '''', ''), '')) 
                                + ' ' + LOWER(ISNULL(REPLACE(LastName, '''', ''), '')) 
                                + ' ' + LOWER(ISNULL(REPLACE(FirstName, '''', ''), ''))
                          
    END    
    
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_UpdateUserInfo]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_UpdateUserInfo];
END
GO


CREATE PROCEDURE [dbo].[wm_Users_UpdateUserInfo]
(
    @UserId                           INT
    , @IsPasswordCorrect              BIT
    , @UpdateLastLoginActivityDate    BIT
    , @MaxInvalidPasswordAttempts     INT
    
    , @LastActivityDateUtc            DATETIME  OUT
    , @LastLoginDateUtc               DATETIME  OUT
    , @Status                         TINYINT   OUT
    , @FailedPasswordAttemptCount     INT       OUT
    , @LastLockoutDateUtc             DATETIME  OUT
)
AS
BEGIN
  
  DECLARE @CurrentTimeUtc DATETIME;
  SET     @CurrentTimeUtc = GETUTCDATE();
        
  DECLARE @ErrorCode     int
  SET @ErrorCode = 0

  DECLARE @TranStarted   bit
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0

  BEGIN TRY

  SET @ErrorCode = -1;
  SELECT  @Status = u.[Status]
          , @LastLockoutDateUtc = u.LastLockoutDateUtc
          , @FailedPasswordAttemptCount = u.FailedPasswordAttemptCount
          , @LastActivityDateUtc = u.LastActivityDateUtc
          , @LastLoginDateUtc = u.LastLoginDateUtc
  FROM    wm_Users u WITH ( UPDLOCK )
  WHERE   u.UserId = @UserId

  IF ( @Status IS NULL )
  BEGIN
    SET @ErrorCode = -2;
    GOTO Cleanup
  END

  IF(     @Status = 6   -- Locked
      OR  @Status = 7 ) -- LockedAwaitingEmailVerification
  BEGIN -- locked
      SET @ErrorCode = @Status;
      GOTO Cleanup
  END

  IF( @IsPasswordCorrect = 0 )
  BEGIN
  
    SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1    
    IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
    BEGIN
      
      IF ( @Status = 5 ) -- AwaitingEmailVerification
      BEGIN
        SET @Status = 7;
      END
      ELSE
      BEGIN
        SET @Status = 6;
      END
    
      SET @LastLockoutDateUtc = @CurrentTimeUtc;
    END
    
  END
  ELSE
  BEGIN
    SET @FailedPasswordAttemptCount = 0
  END

  IF( @UpdateLastLoginActivityDate = 1 )
  BEGIN
  
    SET @LastActivityDateUtc = @CurrentTimeUtc;
    
    IF( @IsPasswordCorrect = 1 )
    BEGIN
      SET @LastLoginDateUtc = @CurrentTimeUtc;
    END
      
  END

  SET @ErrorCode = -3;
  UPDATE  wm_Users
  SET     LastActivityDateUtc = @LastActivityDateUtc
        , LastLoginDateUtc = @LastLoginDateUtc
        , [Status] = @Status
        , FailedPasswordAttemptCount = @FailedPasswordAttemptCount
        , LastLockoutDateUtc = @LastLockoutDateUtc
  WHERE   UserId = @UserId

  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH

  IF( @TranStarted = 1 )
  BEGIN
SET @TranStarted = 0
COMMIT TRANSACTION
  END

  RETURN 0;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode;

END


GO

/* FUNCTIONS */

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitIntegers]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[wm_SplitIntegers];
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitIntegers]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION dbo.wm_SplitIntegers
(
	@List NVARCHAR(MAX)
)
RETURNS @ParsedList TABLE (	Number INT )
AS
BEGIN

	DECLARE @Number NVARCHAR(32)
	        , @Pos INT;

	SET @List = LTRIM(RTRIM(@List))+ '','';
	SET @Pos = CHARINDEX('','', @List, 1);

	IF (REPLACE(@List, '','', '''') <> '''')
	BEGIN
		WHILE (@Pos > 0)
		BEGIN
		
			SET @Number = LTRIM(RTRIM(LEFT(@List, @Pos - 1)))
			IF (@Number <> '''')
			BEGIN
				INSERT INTO @ParsedList (Number) 
				VALUES (CAST(@Number AS int)) --Use Appropriate conversion
			END
			SET @List = RIGHT(@List, LEN(@List) - @Pos)
			SET @Pos = CHARINDEX('','', @List, 1)

		END
	END	
	RETURN
END
' 
END

GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitStrings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
  DROP FUNCTION [dbo].[wm_SplitStrings];
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SplitStrings]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION dbo.wm_SplitStrings
(
	@List NVARCHAR(MAX)
)
RETURNS @ParsedList TABLE (	Value NVARCHAR(MAX) )
AS
BEGIN

  IF (@List IS NULL)
    RETURN;

	DECLARE @String NVARCHAR(MAX)
	        , @Pos INT;

	SET @List = '''''','' + LTRIM(RTRIM(@List)) + '','''''';	
	SET @Pos = CHARINDEX('''''','''''', @List, 1);

	IF (REPLACE(@List, '''''','''''', '''') <> '''')
	BEGIN
		WHILE (@Pos > 0)
		BEGIN
		
			SET @String = LTRIM(RTRIM(LEFT(@List, @Pos - 1)))
			IF (@String <> '''')
			BEGIN
				INSERT INTO @ParsedList (Value) 
				VALUES (CAST(@String AS NVARCHAR(MAX))) --Use Appropriate conversion
			END
			SET @List = RIGHT(@List, LEN(@List) - @Pos - 2)
			SET @Pos = CHARINDEX('''''','''''', @List, 1)

		END
	END	
	RETURN
END
' 
END

GO

