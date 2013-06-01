IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents]  WITH CHECK ADD  CONSTRAINT [FK_cms_Contents_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [cms_Threads] ([ThreadId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
ALTER TABLE [dbo].[cms_Contents] CHECK CONSTRAINT [FK_cms_Contents_cms_Threads]
GO