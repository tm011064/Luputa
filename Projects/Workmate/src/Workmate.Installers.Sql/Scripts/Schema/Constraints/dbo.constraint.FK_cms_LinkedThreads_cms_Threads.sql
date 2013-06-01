IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
ALTER TABLE [dbo].[cms_LinkedThreads]  WITH CHECK ADD  CONSTRAINT [FK_cms_LinkedThreads_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [cms_Threads] ([ThreadId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
ALTER TABLE [dbo].[cms_LinkedThreads] CHECK CONSTRAINT [FK_cms_LinkedThreads_cms_Threads]
GO