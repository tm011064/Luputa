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