IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Threads_Get]
(
  @ThreadId         INT           = NULL
  , @SectionType    TINYINT       = NULL
  , @Name           NVARCHAR(128) = NULL 
  , @ApplicationId  INT           = NULL  
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    t.[ThreadId]
          , t.[SectionId]
          , t.[Name]
          , t.[LastViewedDateUtc]
          , t.[StickyDateUtc]
          , t.[TotalViews]
          , t.[TotalReplies]
          , t.[IsLocked]
          , t.[IsSticky]
          , t.[IsApproved]
          , t.[RatingSum]
          , t.[TotalRatings]
          , t.[ThreadStatus]
          , t.[DateCreatedUtc]
  FROM    [dbo].[cms_Threads]   t
    JOIN  [dbo].[cms_Sections]  s ON t.SectionId = s.SectionId    
  WHERE   (@ThreadId      IS NULL OR t.ThreadId = @ThreadId)
      AND (@SectionType   IS NULL OR s.SectionType = @SectionType)
      AND (@ApplicationId IS NULL OR s.ApplicationId = @ApplicationId)
      AND (@Name          IS NULL OR t.LoweredName = LOWER(@Name))
  
END

GO