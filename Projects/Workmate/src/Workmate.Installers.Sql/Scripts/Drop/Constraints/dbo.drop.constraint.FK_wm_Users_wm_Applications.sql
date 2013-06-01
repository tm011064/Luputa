IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Users_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Users]'))
BEGIN
  ALTER TABLE [dbo].[wm_Users] DROP CONSTRAINT FK_wm_Users_wm_Applications
END
GO