IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
ALTER TABLE [dbo].[cms_ContentTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentTag_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [cms_Contents] ([ContentId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
ALTER TABLE [dbo].[cms_ContentTag] CHECK CONSTRAINT [FK_cms_ContentTag_cms_Contents]
GO