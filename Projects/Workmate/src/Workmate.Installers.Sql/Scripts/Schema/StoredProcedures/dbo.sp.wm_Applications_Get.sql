IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_Get];
END
GO

CREATE PROCEDURE [dbo].[wm_Applications_Get]
(
  @ApplicationId      INT
  , @ApplicationName  NVARCHAR(256)
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [ApplicationName]
          , [Description]
          , [ExtraInfo]
  FROM    [dbo].[wm_Applications]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
      AND (@ApplicationName IS NULL OR [LoweredApplicationName] = LOWER(@ApplicationName))
  
END

GO