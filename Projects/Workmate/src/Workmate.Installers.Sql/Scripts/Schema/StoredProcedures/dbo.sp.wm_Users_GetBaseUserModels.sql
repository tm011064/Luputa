IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetBaseUserModels]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetBaseUserModels];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetBaseUserModels]
(
  @ApplicationId  INT
  , @SearchTerm   NVARCHAR(256)
  , @PageIndex    INT OUTPUT
  , @PageSize     INT
  , @RowCount     INT OUTPUT  
)
AS
BEGIN

  IF (@PageSize = 0)
  BEGIN
    RETURN;
  END

  DECLARE @SearchTermFormatted NVARCHAR(258);
  SET     @SearchTermFormatted = '%' + LOWER(REPLACE(@SearchTerm, '''', '')) + '%';

  SELECT  @RowCount = COUNT(u.UserId)
  FROM    wm_Users  u 
	WHERE   u.ApplicationId = @ApplicationId
	    AND u.LoweredKeywords LIKE @SearchTermFormatted;
    
  IF (@RowCount / @PageSize < @PageIndex)
  BEGIN
    SET @PageIndex = @RowCount / @PageSize;
  END;
    
  WITH PagedView AS 
  ( 
	  SELECT  u.UserId, ROW_NUMBER() OVER (ORDER BY u.UserId) AS RowNumber 
	  FROM    wm_Users  u 
	  WHERE   u.ApplicationId = @ApplicationId
	      AND u.LoweredKeywords LIKE @SearchTermFormatted
  )
  SELECT	u.UserId 
        , u.Email
        , u.ProfileImageId
        , u.FirstName
        , u.LastName
  FROM	  PagedView     pv
    JOIN  wm_Users      u    ON pv.UserId = u.UserId
  WHERE	pv.RowNumber > (@PageSize * @PageIndex) 
	  AND pv.RowNumber <= (@PageSize * @PageIndex + @PageSize)
  
END

GO