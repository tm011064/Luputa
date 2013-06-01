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