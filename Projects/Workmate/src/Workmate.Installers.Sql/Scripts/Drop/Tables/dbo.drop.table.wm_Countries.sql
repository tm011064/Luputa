IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Countries]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Countries];
END
GO