IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
ALTER TABLE [dbo].[cms_ThreadRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ThreadRatings_cms_Threads] FOREIGN KEY([ThreadId])
REFERENCES [cms_Threads] ([ThreadId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
ALTER TABLE [dbo].[cms_ThreadRatings] CHECK CONSTRAINT [FK_cms_ThreadRatings_cms_Threads]
GO