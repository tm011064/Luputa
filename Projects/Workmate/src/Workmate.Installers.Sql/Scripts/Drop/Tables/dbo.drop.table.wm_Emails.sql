IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Emails];
END
GO