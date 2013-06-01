IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_UserRole_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_UserRole_Delete];
END
GO

CREATE PROCEDURE [dbo].[wm_UserRole_Delete]
(
  @UserIds      NVARCHAR(MAX)
  , @RoleNames  NVARCHAR(MAX)
)
AS
BEGIN

  DECLARE @Command NVARCHAR(MAX);
  SET     @Command = 
'
  DELETE FROM wm_UserRole
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
';

  EXEC (@Command);

END

GO