IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents]  WITH CHECK ADD  CONSTRAINT [FK_wm_CalendarEvents_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_CalendarEvents_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]'))
ALTER TABLE [dbo].[wm_CalendarEvents] CHECK CONSTRAINT [FK_wm_CalendarEvents_wm_Users]
GO