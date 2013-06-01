IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Roles_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Roles]'))
BEGIN
  ALTER TABLE [dbo].[wm_Roles] DROP CONSTRAINT FK_wm_Roles_wm_Applications
END
GO