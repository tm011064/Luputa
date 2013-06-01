IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Get];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Get]
(
  @Level                        INT = NULL
  , @ParentContentLevelNodeId   INT = NULL
  , @ThreadId                   INT = NULL
  , @SectionId                  INT = NULL
)
AS
BEGIN
  
  SET NOCOUNT ON;
  
  SELECT  [ContentLevelNodeId]
          ,[Name]
          ,[Level]
          ,[ParentContentLevelNodeId]
          ,[BreadCrumbs]
          ,[BreadCrumbsSplitIndexes]
          ,ThreadId
          ,SectionId
  FROM    [cms_ContentLevelNodes]
  WHERE   (@Level IS NULL OR [Level] = @Level)
      AND (@ParentContentLevelNodeId IS NULL OR [ParentContentLevelNodeId] = @ParentContentLevelNodeId)
      AND (@ThreadId IS NULL OR ThreadId = @ThreadId)
      AND (@SectionId IS NULL OR SectionId = @SectionId)

END

GO