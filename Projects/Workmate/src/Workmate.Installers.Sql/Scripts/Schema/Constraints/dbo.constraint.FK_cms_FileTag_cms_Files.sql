IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Files]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
ALTER TABLE [dbo].[cms_FileTag]  WITH CHECK ADD  CONSTRAINT [FK_cms_FileTag_cms_Files] FOREIGN KEY([FileId])
REFERENCES [cms_Files] ([FileId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Files]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
ALTER TABLE [dbo].[cms_FileTag] CHECK CONSTRAINT [FK_cms_FileTag_cms_Files]
GO