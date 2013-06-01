IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
ALTER TABLE [dbo].[wm_UserRole]  WITH CHECK ADD  CONSTRAINT [FK_wm_UserRole_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_wm_UserRole_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_UserRole]'))
ALTER TABLE [dbo].[wm_UserRole] CHECK CONSTRAINT [FK_wm_UserRole_wm_Users]
GO