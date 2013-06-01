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