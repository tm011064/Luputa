IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetBaseArticleInfoPage]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetBaseArticleInfoPage];
END
GO



CREATE PROCEDURE [dbo].[cms_Contents_GetBaseArticleInfoPage]
(
  @SectionId    INT
  , @ThreadId   INT
  , @Tags       NVARCHAR(MAX)
  , @PageIndex  INT OUTPUT
  , @PageSize   INT
  , @RowCount   INT OUTPUT  
)
AS
BEGIN

  IF (@PageSize = 0)
  BEGIN
    RETURN;
  END

  SELECT  @RowCount = COUNT(c.ContentId)
  FROM    cms_Contents  c
	       JOIN cms_Threads   t ON c.ThreadId = t.ThreadId
    LEFT JOIN cms_ContentTag  ct    ON c.ContentId = ct.ContentId
    LEFT JOIN cms_Tags        ta    ON ct.TagId = ta.TagId
    LEFT JOIN wm_SplitStrings(@Tags) ss ON ta.LoweredTag = ss.Value 
	WHERE   (@ThreadId  IS NULL OR c.ThreadId = @ThreadId)
	    AND (@SectionId IS NULL OR t.SectionId = @SectionId)
	    AND (@Tags IS NULL OR ss.Value IS NOT NULL)
    
  IF (@RowCount / @PageSize < @PageIndex)
  BEGIN
    SET @PageIndex = @RowCount / @PageSize;
  END;
    
  WITH PagedView AS 
  ( 
	  SELECT  c.ContentId, ROW_NUMBER() OVER (ORDER BY c.ContentId) AS RowNumber 
	  FROM        cms_Contents    c
           JOIN cms_Threads     t     ON c.ThreadId = t.ThreadId
      LEFT JOIN cms_ContentTag  ct    ON c.ContentId = ct.ContentId
	    LEFT JOIN cms_Tags        ta    ON ct.TagId = ta.TagId
	    LEFT JOIN wm_SplitStrings(@Tags) ss ON ta.LoweredTag = ss.Value 	  
	  WHERE   (@ThreadId  IS NULL OR c.ThreadId = @ThreadId)
	      AND (@SectionId IS NULL OR t.SectionId = @SectionId)
	      AND (@Tags IS NULL OR ss.Value IS NOT NULL)
  )
  SELECT	c.ContentId
          , c.ThreadId
          , t.SectionId
          , c.UrlFriendlyName
          , c.DateCreatedUtc
          , et.TotalContents
          , c.ExtraInfo
  FROM	  PagedView     pv
    JOIN  cms_Contents        c   ON pv.ContentId = c.ContentId 
    JOIN  cms_Threads         t   ON c.ThreadId   = t.ThreadId 
    LEFT JOIN  cms_Threads    et  ON t.ExternalThreadId = et.ThreadId
  WHERE	pv.RowNumber > (@PageSize * @PageIndex) 
	  AND pv.RowNumber <= (@PageSize * @PageIndex + @PageSize)
  
END

GO