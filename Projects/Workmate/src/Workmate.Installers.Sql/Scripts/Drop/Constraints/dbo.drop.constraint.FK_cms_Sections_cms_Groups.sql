IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_Sections_cms_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Sections]'))
BEGIN
  ALTER TABLE [dbo].[cms_Sections] DROP CONSTRAINT FK_cms_Sections_cms_Groups
END
GO