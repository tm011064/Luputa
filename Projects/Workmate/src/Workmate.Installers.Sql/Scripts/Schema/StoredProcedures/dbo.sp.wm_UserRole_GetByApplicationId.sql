IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_GetByApplicationId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_GetByApplicationId];
END
GO

CREATE PROCEDURE [dbo].[wm_UserRole_GetByApplicationId]
(
  @ApplicationId  INT
)
AS
BEGIN

  SELECT  r.RoleName
          , ur.UserId
  FROM    wm_UserRole ur
    JOIN  wm_Roles    r   ON ur.RoleId = r.RoleId
  WHERE   r.ApplicationId = @ApplicationId

END

GO