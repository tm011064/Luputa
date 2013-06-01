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