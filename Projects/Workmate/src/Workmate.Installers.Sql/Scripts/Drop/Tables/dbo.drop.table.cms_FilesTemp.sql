IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_FilesTemp];
END
GO