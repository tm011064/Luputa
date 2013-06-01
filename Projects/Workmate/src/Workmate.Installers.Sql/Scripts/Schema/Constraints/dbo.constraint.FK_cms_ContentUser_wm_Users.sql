IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
ALTER TABLE [dbo].[cms_ContentUser]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentUser_wm_Users] FOREIGN KEY([ReceivingUserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
ALTER TABLE [dbo].[cms_ContentUser] CHECK CONSTRAINT [FK_cms_ContentUser_wm_Users]
GO