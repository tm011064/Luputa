IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_Get];
END
GO



CREATE PROCEDURE [dbo].[cms_ThreadRatings_Get]
(
  @UserId   INT
  , @ThreadId INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [UserId]
          , [ThreadId]
          , [Rating]
          , [DateCreatedUtc]
  FROM    [dbo].[cms_ThreadRatings]
  WHERE   (@UserId      IS NULL OR [UserId] = @UserId)
      AND (@ThreadId   IS NULL OR [ThreadId] = @ThreadId)
  
END

GO