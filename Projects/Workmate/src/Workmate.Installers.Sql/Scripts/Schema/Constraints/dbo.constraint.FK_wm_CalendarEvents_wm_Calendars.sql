IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Calendars]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents]  WITH CHECK ADD  CONSTRAINT [FK_wm_CalendarEvents_wm_Calendars] FOREIGN KEY([CalendarId])
REFERENCES [wm_Calendars] ([CalendarId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Calendars]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents] CHECK CONSTRAINT [FK_wm_CalendarEvents_wm_Calendars]
GO