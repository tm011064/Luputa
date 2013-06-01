IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Contents_Get]
(
  @ContentId          INT           = NULL
  , @SectionType      TINYINT       = NULL
  , @GroupType        TINYINT       = NULL
  , @ApplicationId    INT           = NULL  
  , @UrlFriendlyName  NVARCHAR(128) = NULL
  , @ThreadName       NVARCHAR(32)  = NULL
  , @SectionName      NVARCHAR(128) = NULL
  , @GroupName        NVARCHAR(256) = NULL  
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    c.[ContentId]
          , c.[ThreadId]
          , c.[ParentContentId]
          , c.[AuthorUserId]
          , c.[ContentLevel]
          , c.[Subject]
          , c.[FormattedBody]
          , c.[DateCreatedUtc]
          , c.[IsApproved]
          , c.[IsLocked]
          , c.[TotalViews]
          , c.[ContentType]
          , c.[RatingSum]
          , c.[TotalRatings]
          , c.[ContentStatus]
          , c.[ExtraInfo]
          , c.[BaseContentId]
          , c.[UrlFriendlyName]
          , c.[ContentLevelNodeId]
  FROM    [dbo].[cms_Contents]  c
    JOIN  [dbo].[cms_Threads]   t ON c.ThreadId = t.ThreadId
    JOIN  [dbo].[cms_Sections]  s ON t.SectionId = s.SectionId  
    LEFT JOIN  [dbo].[cms_Groups]    g ON s.GroupId = g.GroupId  
  WHERE   (@ContentId     IS NULL OR c.ContentId = @ContentId)
      AND (@SectionType   IS NULL OR s.SectionType = @SectionType)
      AND (@GroupType     IS NULL OR g.GroupType = @GroupType)
      AND (@ApplicationId IS NULL OR s.ApplicationId = @ApplicationId)
      AND (@UrlFriendlyName IS NULL OR c.LoweredUrlFriendlyName = LOWER(@UrlFriendlyName))
      AND (@ThreadName      IS NULL OR t.LoweredName = LOWER(@ThreadName))
      AND (@SectionName     IS NULL OR s.LoweredName = LOWER(@SectionName))
      AND (@GroupName       IS NULL OR g.Name = @GroupName)
  
END

GO