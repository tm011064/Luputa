IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_Sections];
END
GO