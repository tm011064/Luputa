IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetArticleModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetArticleModel];
END
GO


CREATE PROCEDURE [dbo].[cms_Contents_GetArticleModel]
(
  @ContentId          INT 
  , @UrlFriendlyName  NVARCHAR(128) = NULL
  , @ThreadName       NVARCHAR(32) = NULL
  , @SectionName      NVARCHAR(128) = NULL
  , @LinkedThreadRelationshipType TINYINT = NULL
)
AS
BEGIN

  SELECT	c.ContentId
          , c.ThreadId
          , t.SectionId
          , c.UrlFriendlyName
          , c.DateCreatedUtc
          , et.TotalContents
          , c.ExtraInfo
          , c.AuthorUserId
          , c.ContentStatus
          , c.Subject
          , c.FormattedBody
          , lt.ThreadId     AS 'LinkedThreadId'
          , lt.IsEnabled    AS 'IsLinkedThreadEnabled'
          , c.ContentLevelNodeId
          , cln.BreadCrumbs
          , cln.BreadCrumbsSplitIndexes
  FROM	  cms_Contents        c    
    JOIN  cms_Threads         t   ON c.ThreadId   = t.ThreadId 
    JOIN  cms_Sections        s   ON t.SectionId  = s.SectionId
    LEFT JOIN  cms_LinkedThreads      lt  ON      c.ContentId = lt.ContentId
                                              AND (@LinkedThreadRelationshipType IS NULL OR lt.RelationshipType = @LinkedThreadRelationshipType) 
    LEFT JOIN  cms_Threads            et  ON lt.ThreadId = et.ThreadId
    LEFT JOIN  cms_ContentLevelNodes  cln ON c.ContentLevelNodeId = cln.ContentLevelNodeId
  WHERE (@ContentId         IS NULL OR c.ContentId = @ContentId)
    AND (@UrlFriendlyName   IS NULL OR c.LoweredUrlFriendlyName = LOWER(@UrlFriendlyName))
    AND (@ThreadName        IS NULL OR t.LoweredName = LOWER(@ThreadName))
    AND (@SectionName       IS NULL OR s.LoweredName = LOWER(@SectionName))
  
END
GO