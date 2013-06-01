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