IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FileTag_cms_Files]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FileTag]'))
BEGIN
  ALTER TABLE [dbo].[cms_FileTag] DROP CONSTRAINT FK_cms_FileTag_cms_Files
END
GO