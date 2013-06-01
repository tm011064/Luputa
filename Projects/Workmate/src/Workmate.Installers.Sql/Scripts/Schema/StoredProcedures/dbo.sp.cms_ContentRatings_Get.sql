IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_ContentRatings_Get]
(
  @UserId     INT
  , @ContentId  INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [UserId]
          , [ContentId]
          , [Rating]
          , [DateCreatedUtc]
  FROM    [dbo].[cms_ContentRatings]
  WHERE   (@UserId      IS NULL OR [UserId] = @UserId)
      AND (@ContentId   IS NULL OR [ContentId] = @ContentId)
  
END

GO