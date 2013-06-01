IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Calendars]') AND type in (N'U'))
BEGIN
  DROP TABLE [dbo].[wm_Calendars];
END
GO