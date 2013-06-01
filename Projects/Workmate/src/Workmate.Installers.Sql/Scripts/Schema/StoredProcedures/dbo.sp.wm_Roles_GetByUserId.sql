IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Roles_GetByUserId]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Roles_GetByUserId];
END
GO

CREATE PROCEDURE [dbo].[wm_Roles_GetByUserId]
(
  @UserId   INT  
)
AS
BEGIN

  SELECT  r.RoleName
  FROM    wm_Roles r
    JOIN  wm_UserRole ur ON r.RoleId = ur.RoleId
  WHERE   ur.UserId = @UserId

END

GO