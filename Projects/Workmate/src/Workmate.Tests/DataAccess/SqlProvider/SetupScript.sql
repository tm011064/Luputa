DECLARE @ApplicationId            INT;
DECLARE @ApplicationName          NVARCHAR(256);
DECLARE @ApplicationDescription   NVARCHAR(512);

DECLARE @RoleNames                NVARCHAR(MAX);
DECLARE @CreateAdministrator      BIT;


SET     @ApplicationId = 99;
SET     @ApplicationName = 'Test Application';
SET     @ApplicationDescription = 'test app';

SET     @RoleNames = 'Registered,SystemAdministrator,UserContentAdministrator,CMSAdministrator,UserAdministrator';


/***** Applications ******/
IF ( NOT EXISTS ( SELECT * FROM wm_Applications WHERE ApplicationName = LOWER(@ApplicationName) ) )
BEGIN
  INSERT INTO wm_Applications ( ApplicationId, ApplicationName, LoweredApplicationName, Description)
  VALUES ( @ApplicationId, @ApplicationName, LOWER(@ApplicationName), @ApplicationDescription );
END

/***** Roles ******/
DECLARE @RoleName NVARCHAR(MAX);
WHILE (LEN(@RoleNames) > 0)
BEGIN
  SET @RoleName = LEFT(@RoleNames, CHARINDEX(',', @RoleNames +',') -1);

  IF ( NOT EXISTS ( SELECT * FROM wm_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId ) )
  BEGIN
    INSERT INTO wm_Roles ( ApplicationId, RoleName, LoweredRoleName, [Description])
    VALUES               ( @ApplicationId, @RoleName, LOWER(@RoleName), NULL );
  END
   
  SET @RoleNames = STUFF(@RoleNames, 1, CHARINDEX(',', @RoleNames+','), '');
END