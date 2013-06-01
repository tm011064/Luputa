IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_FilesTemp_Get]
(
  @FileId       INT
  , @FileType   TINYINT = NULL
  , @UserId     INT = NULL
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
  FROM    [dbo].[cms_FilesTemp]
  WHERE   [FileId]  = @FileId
    AND   (@FileType IS NULL OR FileType = @FileType)
    AND   (@UserId IS NULL OR UserId = @UserId)
  
END

GO