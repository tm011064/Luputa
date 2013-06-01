IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Emails_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Emails]'))
ALTER TABLE [dbo].[wm_Emails]  WITH CHECK ADD  CONSTRAINT [FK_wm_Emails_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Emails_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Emails]'))
ALTER TABLE [dbo].[wm_Emails] CHECK CONSTRAINT [FK_wm_Emails_wm_Applications]
GO