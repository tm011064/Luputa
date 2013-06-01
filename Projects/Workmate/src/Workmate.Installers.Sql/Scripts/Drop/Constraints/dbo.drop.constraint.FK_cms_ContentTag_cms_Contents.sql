IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentTag_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]'))
BEGIN
  ALTER TABLE [dbo].[cms_ContentTag] DROP CONSTRAINT FK_cms_ContentTag_cms_Contents
END
GO