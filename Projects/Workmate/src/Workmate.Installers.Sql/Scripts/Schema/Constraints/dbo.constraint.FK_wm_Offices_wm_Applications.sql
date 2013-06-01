IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Offices_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Offices]'))
ALTER TABLE [dbo].[wm_Offices]  WITH CHECK ADD  CONSTRAINT [FK_wm_Offices_wm_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [wm_Applications] ([ApplicationId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Offices_wm_Applications]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Offices]'))
ALTER TABLE [dbo].[wm_Offices] CHECK CONSTRAINT [FK_wm_Offices_wm_Applications]
GO