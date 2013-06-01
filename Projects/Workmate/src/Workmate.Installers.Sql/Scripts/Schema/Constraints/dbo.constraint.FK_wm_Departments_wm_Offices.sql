IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Offices]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
ALTER TABLE [dbo].[wm_Departments]  WITH CHECK ADD  CONSTRAINT [FK_wm_Departments_wm_Offices] FOREIGN KEY([OfficeId])
REFERENCES [wm_Offices] ([OfficeId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_Departments_wm_Offices]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Departments]'))
ALTER TABLE [dbo].[wm_Departments] CHECK CONSTRAINT [FK_wm_Departments_wm_Offices]
GO