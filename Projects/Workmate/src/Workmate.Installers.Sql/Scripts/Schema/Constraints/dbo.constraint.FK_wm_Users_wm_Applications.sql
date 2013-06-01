IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Users_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Users]'))
ALTER TABLE [dbo].[wm_Users]  WITH CHECK ADD  CONSTRAINT [FK_wm_Users_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Users_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Users]'))
ALTER TABLE [dbo].[wm_Users] CHECK CONSTRAINT [FK_wm_Users_wm_Applications]
GO