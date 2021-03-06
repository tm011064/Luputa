IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags_GetByFileId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Tags_GetByFileId];
END
GO

CREATE PROCEDURE [dbo].[cms_Tags_GetByFileId]
(
  @FileId  INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT  t.Tag
  FROM    cms_Tags t
    JOIN  cms_FileTag ft ON t.TagId = ft.TagId
  WHERE   ft.FileId = @FileId

END
GO