IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Groups_Get]
(
  @GroupId      INT     = NULL
  , @GroupType  TINYINT = NULL
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [GroupId]
          , [Name]
          , [Description]
          , [GroupType]
  FROM    [dbo].[cms_Groups]
  WHERE   (@GroupId   IS NULL OR GroupId = @GroupId)
      AND (@GroupType IS NULL OR GroupType = @GroupType)
  
END

GO