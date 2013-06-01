IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Roles_InsertIfNotExists]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Roles_InsertIfNotExists];
END
GO

CREATE PROCEDURE [dbo].[wm_Roles_InsertIfNotExists]
(
  @ApplicationId  INT
  , @RoleName     NVARCHAR(256)
  , @Description  NVARCHAR(512)
)
AS
BEGIN

  SET NOCOUNT ON;
  
  DECLARE @RoleId INT;

  BEGIN TRAN

    UPDATE    [dbo].[wm_Roles] WITH (SERIALIZABLE)
    SET       [RoleName] = @RoleName
    WHERE     [ApplicationId] = @ApplicationId
          AND [LoweredRoleName] = LOWER(@RoleName);
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      INSERT INTO [dbo].[wm_Roles] 
      (
          [ApplicationId]
        , [RoleName]
        , [LoweredRoleName]
        , [Description]	
      ) 
      VALUES
      (
          @ApplicationId
        , @RoleName
        , LOWER(@RoleName)
        , @Description
      );
      
      SET @RoleId = SCOPE_IDENTITY();
      
    END
    ELSE
    BEGIN
      SELECT    @RoleId = RoleId
      FROM      [dbo].[wm_Roles]
      WHERE     [ApplicationId] = @ApplicationId
            AND [LoweredRoleName] = LOWER(@RoleName);
    END
    
  COMMIT TRAN
  
  RETURN @RoleId;

END

GO