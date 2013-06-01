IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[IX_cms_Contents_LoweredUrlFriendlyName]') AND name = N'cms_Contents')
BEGIN
  ALTER TABLE [dbo].[cms_Contents] DROP CONSTRAINT [IX_cms_Contents_LoweredUrlFriendlyName]
END
GO