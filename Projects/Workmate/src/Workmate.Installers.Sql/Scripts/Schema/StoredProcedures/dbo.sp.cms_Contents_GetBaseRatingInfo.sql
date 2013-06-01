IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetBaseRatingInfo]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetBaseRatingInfo];
END
GO

CREATE PROCEDURE [dbo].[cms_Contents_GetBaseRatingInfo]
(
  @ContentId  INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [RatingSum]
          , [TotalRatings]
  FROM    [dbo].[cms_Contents]
  WHERE   [ContentId] = @ContentId
  
END

GO