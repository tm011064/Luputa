IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentLevelNodes_cms_ContentLevelNodes] FOREIGN KEY([ParentContentLevelNodeId])
REFERENCES [cms_ContentLevelNodes] ([ContentLevelNodeId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes] CHECK CONSTRAINT [FK_cms_ContentLevelNodes_cms_ContentLevelNodes]
GO