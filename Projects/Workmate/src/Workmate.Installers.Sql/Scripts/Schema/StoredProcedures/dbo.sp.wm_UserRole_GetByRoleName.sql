IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_GetByRoleName]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_GetByRoleName];
END
GO

CREATE PROCEDURE [dbo].[wm_UserRole_GetByRoleName]
(
  @ApplicationId  INT
  , @RoleName NVARCHAR(256)
)
AS
BEGIN

  SELECT  ur.UserId
  FROM    wm_UserRole ur 
    JOIN  wm_Roles    r  ON ur.RoleId = r.RoleId  
  WHERE   r.ApplicationId = @ApplicationId
      AND r.LoweredRoleName = LOWER(@RoleName)

END

GO