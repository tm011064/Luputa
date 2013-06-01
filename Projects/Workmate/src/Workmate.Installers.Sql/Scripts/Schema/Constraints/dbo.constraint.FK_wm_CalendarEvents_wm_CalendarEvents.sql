IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_CalendarEvents]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents]  WITH CHECK ADD  CONSTRAINT [FK_wm_CalendarEvents_wm_CalendarEvents] FOREIGN KEY([ParentCalendarEventId])
REFERENCES [wm_CalendarEvents] ([CalendarEventId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_CalendarEvents]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents] CHECK CONSTRAINT [FK_wm_CalendarEvents_wm_CalendarEvents]
GO