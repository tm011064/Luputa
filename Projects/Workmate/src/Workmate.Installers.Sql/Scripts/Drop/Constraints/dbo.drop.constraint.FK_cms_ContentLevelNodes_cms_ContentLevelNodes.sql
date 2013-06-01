IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentLevelNodes_cms_ContentLevelNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentLevelNodes] DROP CONSTRAINT FK_cms_ContentLevelNodes_cms_ContentLevelNodes
END
GO