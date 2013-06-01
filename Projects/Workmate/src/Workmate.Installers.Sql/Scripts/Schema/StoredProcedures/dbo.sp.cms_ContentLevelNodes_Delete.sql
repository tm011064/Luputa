IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Delete]
(
  @ContentLevelNodeId INT
)
AS
BEGIN
    
  ;WITH recursion AS
  (
    SELECT  cln.ContentLevelNodeId, cln.ParentContentLevelNodeId
    FROM    cms_ContentLevelNodes cln
    WHERE   cln.ContentLevelNodeId = @ContentLevelNodeId
    UNION ALL
    SELECT  p.ContentLevelNodeId, p.ParentContentLevelNodeId
    FROM    cms_ContentLevelNodes p
      JOIN  recursion             rec ON p.ParentContentLevelNodeId = rec.ContentLevelNodeId
  )
  DELETE  d
  FROM    cms_ContentLevelNodes d
    JOIN  recursion             rec ON d.ContentLevelNodeId = rec.ContentLevelNodeId

  RETURN @@ROWCOUNT;

END

GO