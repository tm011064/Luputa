USE [master]
GO
/****** Object:  Database [MyMembershipProvider]    Script Date: 06/22/2012 15:57:48 ******/
CREATE DATABASE [MyMembershipProvider] ON  PRIMARY 
( NAME = N'MyMembershipProvider', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MyMembershipProvider.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MyMembershipProvider_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\MyMembershipProvider_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MyMembershipProvider] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyMembershipProvider].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyMembershipProvider] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [MyMembershipProvider] SET ANSI_NULLS OFF
GO
ALTER DATABASE [MyMembershipProvider] SET ANSI_PADDING OFF
GO
ALTER DATABASE [MyMembershipProvider] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [MyMembershipProvider] SET ARITHABORT OFF
GO
ALTER DATABASE [MyMembershipProvider] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [MyMembershipProvider] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [MyMembershipProvider] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [MyMembershipProvider] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [MyMembershipProvider] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [MyMembershipProvider] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [MyMembershipProvider] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [MyMembershipProvider] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [MyMembershipProvider] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [MyMembershipProvider] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [MyMembershipProvider] SET  DISABLE_BROKER
GO
ALTER DATABASE [MyMembershipProvider] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [MyMembershipProvider] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [MyMembershipProvider] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [MyMembershipProvider] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [MyMembershipProvider] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [MyMembershipProvider] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [MyMembershipProvider] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [MyMembershipProvider] SET  READ_WRITE
GO
ALTER DATABASE [MyMembershipProvider] SET RECOVERY SIMPLE
GO
ALTER DATABASE [MyMembershipProvider] SET  MULTI_USER
GO
ALTER DATABASE [MyMembershipProvider] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [MyMembershipProvider] SET DB_CHAINING OFF
GO
USE [MyMembershipProvider]
GO
/****** Object:  Table [dbo].[wm_Applications]    Script Date: 06/22/2012 15:57:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wm_Applications](
	[ApplicationId] [int] NOT NULL,
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](512) NULL,
 CONSTRAINT [PK_wm_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[wm_UserRole_Insert]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[wm_UserRole_Delete]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[cms_Tags]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Tags](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](32) NOT NULL,
	[LoweredTag] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_cms_Tags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Groups]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Groups](
	[GroupId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[GroupType] [tinyint] NOT NULL,
 CONSTRAINT [PK_cms_Groups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Sections]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Sections](
	[ApplicationId] [int] NOT NULL,
	[SectionId] [int] IDENTITY(1,1) NOT NULL,
	[ParentSectionId] [int] NULL,
	[GroupId] [int] NULL,
	[Name] [nvarchar](128) NOT NULL,
	[LoweredName] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[SectionType] [tinyint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsModerated] [bit] NOT NULL,
	[TotalContents] [int] NOT NULL,
	[TotalThreads] [int] NOT NULL,
 CONSTRAINT [PK_cms_Sections] PRIMARY KEY CLUSTERED 
(
	[SectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[cms_Groups_InsertOrUpdate]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Groups_Get]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Groups_Delete]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[wm_Roles]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wm_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](512) NULL,
 CONSTRAINT [PK_wm_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[wm_Users]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
 CONSTRAINT [PK_wm_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Threads]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_Threads](
	[ThreadId] [int] IDENTITY(1,1) NOT NULL,
	[SectionId] [int] NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[LoweredName] [nvarchar](32) NOT NULL,
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
 CONSTRAINT [PK_cms_Threads] PRIMARY KEY CLUSTERED 
(
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[wm_Users_UpdateUserInfo]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

  RETURN @ErrorCode;

END
GO
/****** Object:  StoredProcedure [dbo].[wm_Users_UpdateCredentials]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[wm_Users_UnlockUser]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[wm_Users_SetPassword]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[wm_Users_Insert]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[wm_Users_GetUserInfos]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[wm_Users_GetUserInfos]
(
  @ApplicationId INT
)
AS
BEGIN
  
  SELECT  u.UserName
          , u.FirstName
          , u.LastName
          , u.ProfileImageId
          , u.UserNameDisplayMode
          , u.UserId
  FROM    wm_Users u
  WHERE   u.ApplicationId = @ApplicationId;

END
GO
/****** Object:  StoredProcedure [dbo].[wm_Users_GetUserBasic]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[wm_Users_GetPassword]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[wm_Users_GetNumberOfUsersOnline]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[wm_UserRole]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wm_UserRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_wm_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_FilesTemp]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_FilesTemp](
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[cms_Sections_Update]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Sections_Insert]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Sections_Get]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Sections_Delete]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Sections_Delete]
(
  @SectionId  INT
)
AS
BEGIN
      
  DELETE FROM [dbo].[cms_Sections] 
  WHERE      [SectionId] = @SectionId
	
  RETURN @@ROWCOUNT;

END
GO
/****** Object:  StoredProcedure [dbo].[cms_FilesTemp_Insert]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_FilesTemp_Insert]
(
    @UserId           INT
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
/****** Object:  StoredProcedure [dbo].[cms_FilesTemp_DeleteByUserId]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_FilesTemp_Delete]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[cms_ThreadRatings]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_ThreadRatings](
	[UserId] [int] NOT NULL,
	[ThreadId] [int] NOT NULL,
	[Rating] [smallint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ThreadRatings] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Contents]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
 CONSTRAINT [PK_cms_Contents] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[wm_UserRole_GetByRoleName]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[wm_UserRole_GetByApplicationId]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[wm_Roles_GetByUserId]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Threads_Update]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Threads_Insert]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
      
    SET @ErrorCode = -1;
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
    
    SET @ErrorCode = -2;
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

  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[cms_Threads_IncreaseTotalViews]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Threads_Get]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Threads_Delete]    Script Date: 06/22/2012 15:57:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Threads_Delete]
(
    @ThreadId INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_Threads] 
  WHERE       [ThreadId]  = @ThreadId
	
  RETURN @@ROWCOUNT;
  
END
GO
/****** Object:  StoredProcedure [dbo].[cms_ThreadRatings_InsertOrUpdate]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_ThreadRatings_InsertOrUpdate]
(
    @Rating           SMALLINT
  , @ThreadId        INT
  , @UserId           INT
  , @DateCreatedUtc   DATETIME
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
      
    SET @ErrorCode = -2;
    UPDATE  [cms_ThreadRatings] WITH (SERIALIZABLE)
    SET     [Rating] = @Rating
          , [DateCreatedUtc]  = @DateCreatedUtc
    WHERE   [UserId] = @UserId
        AND [ThreadId] = @ThreadId
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      SET @ErrorCode = -3;
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
    
    SET @ErrorCode = -4;
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

  RETURN @ThreadId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END

  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[cms_ThreadRatings_Delete]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
      
    SET @ErrorCode = -1;
    DELETE FROM [dbo].[cms_ThreadRatings] 
    WHERE   [UserId]    = @UserId
        AND [ThreadId] = @ThreadId
                
    SET @ErrorCode = -2;
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

  RETURN @ThreadId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END

  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[cms_Contents_IncreaseTotalViews]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_Contents_Get]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Contents_Get]
(
  @ContentId        INT           = NULL
  , @SectionType    TINYINT       = NULL
  , @GroupType      TINYINT       = NULL
  , @ApplicationId  INT           = NULL  
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
  FROM    [dbo].[cms_Contents]  c
    JOIN  [dbo].[cms_Threads]   t ON c.ThreadId = t.ThreadId
    JOIN  [dbo].[cms_Sections]  s ON t.SectionId = s.SectionId  
    JOIN  [dbo].[cms_Groups]    g ON s.GroupId = g.GroupId  
  WHERE   (@ContentId     IS NULL OR c.ContentId = @ContentId)
      AND (@SectionType   IS NULL OR s.SectionType = @SectionType)
      AND (@GroupType     IS NULL OR g.GroupType = @GroupType)
      AND (@ApplicationId IS NULL OR s.ApplicationId = @ApplicationId)
  
END
GO
/****** Object:  StoredProcedure [dbo].[cms_Contents_Delete]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Contents_Delete]
(
    @ContentId  INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_Contents] 
  WHERE      [ContentId] = @ContentId
	
  RETURN @@ROWCOUNT;
  
END
GO
/****** Object:  Table [dbo].[cms_ContentRatings]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_ContentRatings](
	[UserId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
	[Rating] [smallint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ContentRatings] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_ContentTag]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_ContentTag](
	[TagId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
 CONSTRAINT [PK_cms_ContentTag] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_ContentUser]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_ContentUser](
	[ContentId] [int] NOT NULL,
	[ReceivingUserId] [int] NOT NULL,
	[DateReceivedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ContentUser] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC,
	[ReceivingUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cms_Files]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cms_Files](
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[cms_ContentUser_InsertOrUpdate]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_ContentUser_InsertOrUpdate]
(
    @ContentId        INT
  , @ReceivingUserId  INT
  , @DateReceivedUtc  DATETIME
)
AS
BEGIN

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
/****** Object:  StoredProcedure [dbo].[cms_ContentUser_Delete]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_ContentTag_InsertUpdateDelete]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_ContentTag_InsertUpdateDelete]
(
  @ContentId  INT
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
    SET @ErrorCode = -100;
    INSERT INTO #TagsFromXml (Tag, LoweredTag)
    SELECT  x.Tag
            , LOWER(x.Tag)
    FROM  
    (
      SELECT t.a.value('@t','NVARCHAR(32)') AS Tag
      FROM @TagXml.nodes('//r') t(a)
    ) x
    
    SET @ErrorCode = -101;    
    INSERT INTO cms_Tags (Tag, LoweredTag)
    SELECT  tfx.Tag, tfx.LoweredTag
    FROM    #TagsFromXml tfx
      WHERE NOT EXISTS ( SELECT LoweredTag FROM cms_Tags WHERE LoweredTag = tfx.LoweredTag)
  
    SET @ErrorCode = -102;
    DELETE FROM cms_ContentTag
    WHERE       ContentId = @ContentId
    
    SET @ErrorCode = -103;
    INSERT INTO cms_ContentTag (ContentId, TagId)
    SELECT      @ContentId, t.TagId
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

  DROP TABLE #TagsFromXml;
  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[cms_Files_Update]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Files_Update]
(
  @FileId           INT
  , @UserId           INT
  , @FileType         TINYINT
  , @DateCreatedUtc   DATETIME
  , @FileName         NVARCHAR(1024)
  , @Content          VARBINARY(MAX)
  , @ContentType      NVARCHAR(64)
  , @ContentSize      INT
  , @FriendlyFileName NVARCHAR(256)
  , @Height           INT
  , @Width            INT
  , @ContentId        INT
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE  [dbo].[cms_Files] 
  SET       [UserId]            = @UserId
          , [FileType]          = @FileType
          , [DateCreatedUtc]    = @DateCreatedUtc
          , [FileName]          = @FileName
          , [Content]           = @Content
          , [ContentType]       = @ContentType
          , [ContentSize]       = @ContentSize
          , [FriendlyFileName]  = @FriendlyFileName
          , [Height]            = @Height
          , [Width]             = @Width
          , [ContentId]         = @ContentId
  WHERE     [FileId]  = @FileId
	
  RETURN @@ROWCOUNT;
  
END
GO
/****** Object:  StoredProcedure [dbo].[cms_ContentRatings_InsertOrUpdate]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_ContentRatings_InsertOrUpdate]
(
    @Rating           SMALLINT
  , @ContentId        INT
  , @UserId           INT
  , @DateCreatedUtc   DATETIME
  , @AllowSelfRating  BIT  
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
  
    IF ( @AllowSelfRating = 0 
         AND ( SELECT AuthorUserId FROM cms_Contents WHERE ContentId = @ContentId ) = @UserId )
    BEGIN
      SET @ErrorCode = -1;
      GOTO Cleanup;
    END
    
    SET @ErrorCode = -2;
    UPDATE  [cms_ContentRatings] WITH (SERIALIZABLE)
    SET     [Rating] = @Rating
          , [DateCreatedUtc]  = @DateCreatedUtc
    WHERE   [UserId] = @UserId
        AND [ContentId] = @ContentId
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      SET @ErrorCode = -3;
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
    
    SET @ErrorCode = -4;
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

  RETURN @ContentId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END

  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[cms_ContentRatings_Delete]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
      
    SET @ErrorCode = -1;
    DELETE FROM [dbo].[cms_ContentRatings] 
    WHERE   [UserId]    = @UserId
        AND [ContentId] = @ContentId
                
    SET @ErrorCode = -2;
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

  RETURN @ContentId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END

  RETURN @ErrorCode

END
GO
/****** Object:  Table [dbo].[cms_FileTag]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cms_FileTag](
	[FileId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_cms_FileTag] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC,
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[cms_Files_Insert]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Files_Insert]
(
    @UserId           INT
  , @FileType         TINYINT
  , @FileName         NVARCHAR(1024)
  , @Content          VARBINARY(MAX)
  , @ContentType      NVARCHAR(64)
  , @ContentSize      INT
  , @FriendlyFileName NVARCHAR(256)
  , @Height           INT
  , @Width            INT
  , @ContentId        INT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[cms_Files] 
  (
      [UserId]
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
    
  RETURN SCOPE_IDENTITY();
  
END
GO
/****** Object:  StoredProcedure [dbo].[cms_Files_Get]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Files_Get]
(
  @FileId INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [FileId]
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
  WHERE   [FileId]  = @FileId
  
END
GO
/****** Object:  StoredProcedure [dbo].[cms_Files_Delete]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[cms_FileTag_InsertUpdateDelete]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
    SET @ErrorCode = -100;
    INSERT INTO #TagsFromXml (Tag, LoweredTag)
    SELECT  x.Tag
            , LOWER(x.Tag)
    FROM  
    (
      SELECT t.a.value('@t','NVARCHAR(32)') AS Tag
      FROM @TagXml.nodes('//r') t(a)
    ) x
    
    SET @ErrorCode = -101;    
    INSERT INTO cms_Tags (Tag, LoweredTag)
    SELECT  tfx.Tag, tfx.LoweredTag
    FROM    #TagsFromXml tfx
      WHERE NOT EXISTS ( SELECT LoweredTag FROM cms_Tags WHERE LoweredTag = tfx.LoweredTag)
  
    SET @ErrorCode = -102;
    DELETE FROM cms_FileTag
    WHERE       FileId = @FileId
    
    SET @ErrorCode = -103;
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

  DROP TABLE #TagsFromXml;
  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[cms_Contents_Update]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
  
    IF (      @UrlFriendlyName IS NOT NULL
         AND  EXISTS (  SELECT  c.* 
                        FROM    cms_Contents  c
                          JOIN  cms_Threads   t ON c.ThreadId = @ThreadId
                        WHERE   LoweredUrlFriendlyName = LOWER(@UrlFriendlyName)
                            AND c.ContentId <> @ContentId ))
    BEGIN
      RETURN -1;
    END
  
    SET @ErrorCode = -1;    
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

  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[cms_Contents_Insert]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Contents_Insert]
(
    @ThreadId         INT = NULL
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
  , @CreateNewThread        BIT = 0
  , @NewThreadSectionId     INT = NULL
  , @IsNewThreadApproved    BIT = 0
)
AS
BEGIN

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
  
    IF ( @ThreadId IS NULL )
    BEGIN
    
      IF( @CreateNewThread = 0
          OR @NewThreadSectionId IS NULL)
      BEGIN
        SET @ErrorCode = -1; -- no thread specified
        GOTO Cleanup;
      END
      ELSE
      BEGIN
        
        SET @ErrorCode = -2;
        EXEC @ThreadId = cms_Threads_Insert 
          @NewThreadSectionId
          , ''
          , NULL 
          , 0
          , 0
          , @IsNewThreadApproved
          , 1 -- ThreadStatus?
      
      END      
    
    END
  
    IF (      @UrlFriendlyName IS NOT NULL
         AND  EXISTS (  SELECT  c.* 
                        FROM    cms_Contents  c
                          JOIN  cms_Threads   t ON c.ThreadId = @ThreadId
                        WHERE   LoweredUrlFriendlyName = LOWER(@UrlFriendlyName)))
    BEGIN
      SET @ErrorCode = -3;
      GOTO Cleanup;
    END

    SET @ErrorCode = -4;
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
    );
    
    SET @ContentId = SCOPE_IDENTITY();
    
    EXEC @ErrorCode = cms_ContentTag_InsertUpdateDelete @ContentId, @TagXml
    IF (@ErrorCode <> 0)
    BEGIN
      GOTO Cleanup;
    END
    
    UPDATE  s
    SET     s.TotalContents = TotalContents + 1
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

  RETURN @ContentId

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END

  RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[cms_Files_MoveTempFile]    Script Date: 06/22/2012 15:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[cms_Files_MoveTempFile]
(
    @TempFileId               INT
  , @ContentId                INT = NULL
  , @IsProfileImage           BIT
  , @UseExistingRecordValues  BIT  
  , @FileName                 NVARCHAR(1024)
  , @FriendlyFileName         NVARCHAR(256)  
  , @TagXml                   XML
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

  RETURN @ErrorCode

END
GO
/****** Object:  ForeignKey [FK_cms_Sections_cms_Groups]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_Sections]  WITH CHECK ADD  CONSTRAINT [FK_cms_Sections_cms_Groups] FOREIGN KEY([GroupId])
REFERENCES [dbo].[cms_Groups] ([GroupId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[cms_Sections] CHECK CONSTRAINT [FK_cms_Sections_cms_Groups]
GO
/****** Object:  ForeignKey [FK_cms_Sections_cms_Sections]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_Sections]  WITH CHECK ADD  CONSTRAINT [FK_cms_Sections_cms_Sections] FOREIGN KEY([ParentSectionId])
REFERENCES [dbo].[cms_Sections] ([SectionId])
GO
ALTER TABLE [dbo].[cms_Sections] CHECK CONSTRAINT [FK_cms_Sections_cms_Sections]
GO
/****** Object:  ForeignKey [FK_cms_Sections_wm_Applications]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_Sections]  WITH CHECK ADD  CONSTRAINT [FK_cms_Sections_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[wm_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[cms_Sections] CHECK CONSTRAINT [FK_cms_Sections_wm_Applications]
GO
/****** Object:  ForeignKey [FK_wm_Roles_wm_Applications]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[wm_Roles]  WITH CHECK ADD  CONSTRAINT [FK_wm_Roles_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[wm_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[wm_Roles] CHECK CONSTRAINT [FK_wm_Roles_wm_Applications]
GO
/****** Object:  ForeignKey [FK_wm_Users_wm_Applications]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[wm_Users]  WITH CHECK ADD  CONSTRAINT [FK_wm_Users_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[wm_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[wm_Users] CHECK CONSTRAINT [FK_wm_Users_wm_Applications]
GO
/****** Object:  ForeignKey [FK_cms_Threads_cms_Sections]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_Threads]  WITH CHECK ADD  CONSTRAINT [FK_cms_Threads_cms_Sections] FOREIGN KEY([SectionId])
REFERENCES [dbo].[cms_Sections] ([SectionId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_Threads] CHECK CONSTRAINT [FK_cms_Threads_cms_Sections]
GO
/****** Object:  ForeignKey [FK_wm_UserRole_wm_Roles]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[wm_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_wm_UserRole_wm_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[wm_Roles] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[wm_UserRole] CHECK CONSTRAINT [FK_wm_UserRole_wm_Roles]
GO
/****** Object:  ForeignKey [FK_wm_UserRole_wm_Users]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[wm_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_wm_UserRole_wm_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[wm_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[wm_UserRole] CHECK CONSTRAINT [FK_wm_UserRole_wm_Users]
GO
/****** Object:  ForeignKey [FK_cms_FilesTemp_wm_Users]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_FilesTemp]  WITH CHECK ADD  CONSTRAINT [FK_cms_FilesTemp_wm_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[wm_Users] ([UserId])
GO
ALTER TABLE [dbo].[cms_FilesTemp] CHECK CONSTRAINT [FK_cms_FilesTemp_wm_Users]
GO
/****** Object:  ForeignKey [FK_cms_ThreadRatings_cms_Threads]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_ThreadRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ThreadRatings_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [dbo].[cms_Threads] ([ThreadId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_ThreadRatings] CHECK CONSTRAINT [FK_cms_ThreadRatings_cms_Threads]
GO
/****** Object:  ForeignKey [FK_cms_ThreadRatings_wm_Users]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_ThreadRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ThreadRatings_wm_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[wm_Users] ([UserId])
GO
ALTER TABLE [dbo].[cms_ThreadRatings] CHECK CONSTRAINT [FK_cms_ThreadRatings_wm_Users]
GO
/****** Object:  ForeignKey [FK_cms_Contents_cms_Contents]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_Contents]  WITH CHECK ADD  CONSTRAINT [FK_cms_Contents_cms_Contents] FOREIGN KEY([ParentContentId])
REFERENCES [dbo].[cms_Contents] ([ContentId])
GO
ALTER TABLE [dbo].[cms_Contents] CHECK CONSTRAINT [FK_cms_Contents_cms_Contents]
GO
/****** Object:  ForeignKey [FK_cms_Contents_cms_Threads]    Script Date: 06/22/2012 15:57:56 ******/
ALTER TABLE [dbo].[cms_Contents]  WITH CHECK ADD  CONSTRAINT [FK_cms_Contents_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [dbo].[cms_Threads] ([ThreadId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_Contents] CHECK CONSTRAINT [FK_cms_Contents_cms_Threads]
GO
/****** Object:  ForeignKey [FK_cms_ContentRatings_cms_Contents]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_ContentRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentRatings_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[cms_Contents] ([ContentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_ContentRatings] CHECK CONSTRAINT [FK_cms_ContentRatings_cms_Contents]
GO
/****** Object:  ForeignKey [FK_cms_ContentRatings_wm_Users]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_ContentRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentRatings_wm_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[wm_Users] ([UserId])
GO
ALTER TABLE [dbo].[cms_ContentRatings] CHECK CONSTRAINT [FK_cms_ContentRatings_wm_Users]
GO
/****** Object:  ForeignKey [FK_cms_ContentTag_cms_Contents]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_ContentTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentTag_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[cms_Contents] ([ContentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_ContentTag] CHECK CONSTRAINT [FK_cms_ContentTag_cms_Contents]
GO
/****** Object:  ForeignKey [FK_cms_ContentTag_cms_Tags]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_ContentTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentTag_cms_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[cms_Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_ContentTag] CHECK CONSTRAINT [FK_cms_ContentTag_cms_Tags]
GO
/****** Object:  ForeignKey [FK_cms_ContentUser_cms_Contents]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_ContentUser]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentUser_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[cms_Contents] ([ContentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_ContentUser] CHECK CONSTRAINT [FK_cms_ContentUser_cms_Contents]
GO
/****** Object:  ForeignKey [FK_cms_ContentUser_wm_Users]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_ContentUser]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentUser_wm_Users] FOREIGN KEY([ReceivingUserId])
REFERENCES [dbo].[wm_Users] ([UserId])
GO
ALTER TABLE [dbo].[cms_ContentUser] CHECK CONSTRAINT [FK_cms_ContentUser_wm_Users]
GO
/****** Object:  ForeignKey [FK_cms_Files_cms_Contents]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_Files]  WITH CHECK ADD  CONSTRAINT [FK_cms_Files_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [dbo].[cms_Contents] ([ContentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_Files] CHECK CONSTRAINT [FK_cms_Files_cms_Contents]
GO
/****** Object:  ForeignKey [FK_cms_Files_wm_Users]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_Files]  WITH CHECK ADD  CONSTRAINT [FK_cms_Files_wm_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[wm_Users] ([UserId])
GO
ALTER TABLE [dbo].[cms_Files] CHECK CONSTRAINT [FK_cms_Files_wm_Users]
GO
/****** Object:  ForeignKey [FK_cms_FileTag_cms_Files]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_FileTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_FileTag_cms_Files] FOREIGN KEY([FileId])
REFERENCES [dbo].[cms_Files] ([FileId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_FileTag] CHECK CONSTRAINT [FK_cms_FileTag_cms_Files]
GO
/****** Object:  ForeignKey [FK_cms_FileTag_cms_Tags]    Script Date: 06/22/2012 15:57:57 ******/
ALTER TABLE [dbo].[cms_FileTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_FileTag_cms_Tags] FOREIGN KEY([TagId])
REFERENCES [dbo].[cms_Tags] ([TagId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cms_FileTag] CHECK CONSTRAINT [FK_cms_FileTag_cms_Tags]
GO
