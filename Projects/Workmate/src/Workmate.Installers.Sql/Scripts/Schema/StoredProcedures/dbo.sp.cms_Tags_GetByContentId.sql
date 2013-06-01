IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags_GetByContentId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Tags_GetByContentId];
END
GO

CREATE PROCEDURE [dbo].[cms_Tags_GetByContentId]
(
  @ContentId  INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT  t.Tag
  FROM    cms_Tags t
    JOIN  cms_ContentTag ct ON t.TagId = ct.TagId
  WHERE   ct.ContentId = @ContentId

END
GO