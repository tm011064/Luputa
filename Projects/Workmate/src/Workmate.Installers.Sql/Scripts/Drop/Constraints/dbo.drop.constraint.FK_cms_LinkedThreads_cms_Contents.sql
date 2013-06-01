IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_LinkedThreads_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]'))
BEGIN
  ALTER TABLE [dbo].[cms_LinkedThreads] DROP CONSTRAINT FK_cms_LinkedThreads_cms_Contents
END
GO