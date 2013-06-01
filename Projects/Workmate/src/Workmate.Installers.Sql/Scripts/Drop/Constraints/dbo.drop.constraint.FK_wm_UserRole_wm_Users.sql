IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
BEGIN
  ALTER TABLE [dbo].[wm_UserRole] DROP CONSTRAINT FK_wm_UserRole_wm_Users
END
GO