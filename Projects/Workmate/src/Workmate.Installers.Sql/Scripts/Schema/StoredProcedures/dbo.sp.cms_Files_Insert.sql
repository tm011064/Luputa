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