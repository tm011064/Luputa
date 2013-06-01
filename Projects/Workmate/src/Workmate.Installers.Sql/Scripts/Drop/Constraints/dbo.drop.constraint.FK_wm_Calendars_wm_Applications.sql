IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Calendars_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Calendars]'))
BEGIN
  ALTER TABLE [dbo].[wm_Calendars] DROP CONSTRAINT FK_wm_Calendars_wm_Applications
END
GO