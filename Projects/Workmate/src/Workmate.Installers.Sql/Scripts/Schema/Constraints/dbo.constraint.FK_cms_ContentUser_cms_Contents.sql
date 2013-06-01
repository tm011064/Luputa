IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
ALTER TABLE [dbo].[cms_ContentUser]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentUser_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [cms_Contents] ([ContentId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentUser_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]'))
ALTER TABLE [dbo].[cms_ContentUser] CHECK CONSTRAINT [FK_cms_ContentUser_cms_Contents]
GO