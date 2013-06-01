IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Roles_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Roles]'))
ALTER TABLE [dbo].[wm_Roles]  WITH CHECK ADD  CONSTRAINT [FK_wm_Roles_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Roles_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Roles]'))
ALTER TABLE [dbo].[wm_Roles] CHECK CONSTRAINT [FK_wm_Roles_wm_Applications]
GO