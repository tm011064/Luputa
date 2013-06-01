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