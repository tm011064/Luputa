IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Threads_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Threads]'))
ALTER TABLE [dbo].[cms_Threads]  WITH CHECK ADD  CONSTRAINT [FK_cms_Threads_cms_Sections] FOREIGN KEY([SectionId])
REFERENCES [cms_Sections] ([SectionId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Threads_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Threads]'))
ALTER TABLE [dbo].[cms_Threads] CHECK CONSTRAINT [FK_cms_Threads_cms_Sections]
GO