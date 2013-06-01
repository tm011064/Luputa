IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Threads_cms_Sections]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Threads]'))
BEGIN
  ALTER TABLE [dbo].[cms_Threads] DROP CONSTRAINT FK_cms_Threads_cms_Sections
END
GO