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