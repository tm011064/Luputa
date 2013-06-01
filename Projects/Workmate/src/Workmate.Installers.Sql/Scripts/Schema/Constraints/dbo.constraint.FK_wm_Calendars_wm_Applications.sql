IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Calendars_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Calendars]'))
ALTER TABLE [dbo].[wm_Calendars]  WITH CHECK ADD  CONSTRAINT [FK_wm_Calendars_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Calendars_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Calendars]'))
ALTER TABLE [dbo].[wm_Calendars] CHECK CONSTRAINT [FK_wm_Calendars_wm_Applications]
GO