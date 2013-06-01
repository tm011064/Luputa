IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_CalendarEvents]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
BEGIN
  ALTER TABLE [dbo].[wm_CalendarEvents] DROP CONSTRAINT FK_wm_CalendarEvents_wm_CalendarEvents
END
GO