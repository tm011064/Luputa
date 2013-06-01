IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Contents_cms_Threads]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Contents]'))
BEGIN
  ALTER TABLE [dbo].[cms_Contents] DROP CONSTRAINT FK_cms_Contents_cms_Threads
END
GO