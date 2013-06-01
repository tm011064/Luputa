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