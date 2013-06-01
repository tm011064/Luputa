IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
ALTER TABLE [dbo].[wm_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_wm_UserRole_wm_Roles] FOREIGN KEY([RoleId])
REFERENCES [wm_Roles] ([RoleId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
ALTER TABLE [dbo].[wm_UserRole] CHECK CONSTRAINT [FK_wm_UserRole_wm_Roles]
GO