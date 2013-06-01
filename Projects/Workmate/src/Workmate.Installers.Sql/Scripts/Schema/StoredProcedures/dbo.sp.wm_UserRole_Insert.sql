IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_Insert];
END
GO

CREATE PROCEDURE [dbo].[wm_UserRole_Insert]
(
  @UserIds      NVARCHAR(MAX)
  , @RoleNames  NVARCHAR(MAX)
)
AS
BEGIN

  DECLARE @Command NVARCHAR(MAX);
  SET     @Command = 
'
  INSERT INTO wm_UserRole (UserId, RoleId)
  SELECT  x.UserId, y.RoleId
  FROM    
  (
    SELECT  u.UserId
    FROM    wm_Users u
    WHERE   u.UserId IN (' + @UserIds + ')
  ) AS x,
  (
    SELECT  r.RoleId
    FROM    wm_Roles r
    WHERE   r.LoweredRoleName IN (' + LOWER(@RoleNames) + ')
  ) AS y
  WHERE NOT EXISTS ( SELECT ur.* FROM wm_UserRole ur WHERE ur.UserId = x.UserId AND ur.RoleId = y.RoleId )
';

  EXEC (@Command);

END

GO