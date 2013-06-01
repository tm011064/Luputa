IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Get];
END
GO

CREATE PROCEDURE [dbo].[wm_Offices_Get]
(
  @ApplicationId INT
  , @OfficeId INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [OfficeId]
          , [Name]
          , [Description]
          , [ExtraInfo]
  FROM    [dbo].[wm_Offices]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
      AND (@OfficeId IS NULL OR [OfficeId] = @OfficeId)
  
END

GO