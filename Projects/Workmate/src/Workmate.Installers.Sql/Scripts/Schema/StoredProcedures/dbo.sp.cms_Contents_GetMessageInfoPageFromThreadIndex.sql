IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetMessageInfoPageFromThreadIndex]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetMessageInfoPageFromThreadIndex];
END
GO

CREATE PROCEDURE [dbo].[cms_Contents_GetMessageInfoPageFromThreadIndex]
(
  @ThreadId     INT
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

  SELECT  @RowCount = COUNT(*)
  FROM    cms_Contents
  WHERE   ThreadId = @ThreadId;
    
  IF (@RowCount / @PageSize < @PageIndex)
  BEGIN
    SET @PageIndex = @RowCount / @PageSize;
  END;
    
  WITH PagedView AS 
  ( 
	  SELECT  ContentId, ROW_NUMBER() OVER (ORDER BY ContentId) AS RowNumber 
	  FROM    cms_Contents
	  WHERE   ThreadId = @ThreadId
  )
  SELECT	c.ContentId
          , c.AuthorUserId
          , c.Subject
          , c.FormattedBody
          , c.DateCreatedUtc
          , c.ContentStatus
          , c.TotalRatings
          , c.RatingSum
          , c.ParentContentId
          , c.ContentLevel
  FROM	  PagedView     pv
    JOIN  cms_Contents  c   ON pv.ContentId = c.ContentId  
  WHERE	pv.RowNumber > (@PageSize * @PageIndex) 
	  AND pv.RowNumber <= (@PageSize * @PageIndex + @PageSize)
  
END

GO