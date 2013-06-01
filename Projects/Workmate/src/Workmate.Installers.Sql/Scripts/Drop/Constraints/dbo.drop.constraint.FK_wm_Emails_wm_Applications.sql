IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Emails_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Emails]'))
BEGIN
  ALTER TABLE [dbo].[wm_Emails] DROP CONSTRAINT FK_wm_Emails_wm_Applications
END
GO