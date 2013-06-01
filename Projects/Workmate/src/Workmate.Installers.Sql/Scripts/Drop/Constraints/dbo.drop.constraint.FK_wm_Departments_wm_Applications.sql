IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
BEGIN
  ALTER TABLE [dbo].[wm_Departments] DROP CONSTRAINT FK_wm_Departments_wm_Applications
END
GO