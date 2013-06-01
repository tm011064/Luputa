IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentLevelNodes_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [cms_Threads] ([ThreadId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
ALTER TABLE [dbo].[cms_ContentLevelNodes] CHECK CONSTRAINT [FK_cms_ContentLevelNodes_cms_Threads]
GO