IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents]  WITH CHECK ADD  CONSTRAINT [FK_cms_Contents_cms_Contents] FOREIGN KEY([ParentContentId])
REFERENCES [cms_Contents] ([ContentId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents] CHECK CONSTRAINT [FK_cms_Contents_cms_Contents]
GO