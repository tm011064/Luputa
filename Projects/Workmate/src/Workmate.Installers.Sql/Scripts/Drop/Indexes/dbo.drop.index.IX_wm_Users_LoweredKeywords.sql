IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IX_wm_Users_LoweredKeywords]') AND name = N'wm_Users')
BEGIN
  ALTER TABLE [dbo].[wm_Users] DROP CONSTRAINT [IX_wm_Users_LoweredKeywords]
END
GO