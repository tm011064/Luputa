IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Offices_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Offices]'))
BEGIN
  ALTER TABLE [dbo].[wm_Offices] DROP CONSTRAINT FK_wm_Offices_wm_Applications
END
GO