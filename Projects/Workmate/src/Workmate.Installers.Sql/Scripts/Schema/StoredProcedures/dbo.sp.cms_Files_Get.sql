IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Files_Get]
(
  @ApplicationId  INT = NULL
  , @FileId       INT = NULL
  , @ContentId    INT = NULL
  , @FileType     TINYINT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [FileId]
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
          , [ContentId]
  FROM    [dbo].[cms_Files]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
    AND   (@FileId IS NULL OR [FileId] = @FileId)
    AND   (@FileType IS NULL OR FileType = @FileType)
    AND   (@ContentId IS NULL OR ContentId = @ContentId)
  
END

GO