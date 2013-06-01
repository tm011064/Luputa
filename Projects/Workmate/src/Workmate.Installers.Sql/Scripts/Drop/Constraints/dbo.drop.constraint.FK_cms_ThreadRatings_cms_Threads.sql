IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ThreadRatings_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]'))
BEGIN
  ALTER TABLE [dbo].[cms_ThreadRatings] DROP CONSTRAINT FK_cms_ThreadRatings_cms_Threads
END
GO