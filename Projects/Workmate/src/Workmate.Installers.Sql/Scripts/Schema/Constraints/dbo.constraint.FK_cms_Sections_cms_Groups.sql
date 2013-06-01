IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
ALTER TABLE [dbo].[cms_Sections]  WITH CHECK ADD  CONSTRAINT [FK_cms_Sections_cms_Groups] FOREIGN KEY([GroupId])
REFERENCES [cms_Groups] ([GroupId])
ON DELETE SET NULL
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
ALTER TABLE [dbo].[cms_Sections] CHECK CONSTRAINT [FK_cms_Sections_cms_Groups]
GO