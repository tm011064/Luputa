IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
BEGIN
  ALTER TABLE [dbo].[cms_FilesTemp] DROP CONSTRAINT FK_cms_FilesTemp_wm_Users
END
GO