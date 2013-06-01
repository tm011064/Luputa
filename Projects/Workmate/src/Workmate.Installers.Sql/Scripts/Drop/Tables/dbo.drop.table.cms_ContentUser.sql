IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[cms_ContentUser];
END
GO