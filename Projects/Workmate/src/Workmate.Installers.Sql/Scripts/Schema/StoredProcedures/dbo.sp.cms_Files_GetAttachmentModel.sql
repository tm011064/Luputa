IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_GetAttachmentModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_GetAttachmentModel];
END
GO

CREATE PROCEDURE [dbo].[cms_Files_GetAttachmentModel]
(  
  @ApplicationId  INT = NULL
  , @FileId       INT = NULL
  , @ContentId  INT = NULL
  , @FileType   TINYINT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [FileId]
          , [UserId]
          , [DateCreatedUtc]
          , [FileName]
          , [ContentType]
          , [ContentSize]
          , [FriendlyFileName]
          , [ContentId]
  FROM    [dbo].[cms_Files]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
    AND   (@FileId IS NULL OR [FileId] = @FileId)
    AND   (@FileType IS NULL OR FileType = @FileType)
    AND   (@ContentId IS NULL OR ContentId = @ContentId)
  
END

GO