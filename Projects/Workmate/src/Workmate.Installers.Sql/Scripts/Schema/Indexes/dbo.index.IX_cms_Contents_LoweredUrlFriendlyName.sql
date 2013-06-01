IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents]') AND name = N'IX_cms_Contents_LoweredUrlFriendlyName')
CREATE NONCLUSTERED INDEX [IX_cms_Contents_LoweredUrlFriendlyName] ON [dbo].[cms_Contents] 
(
	[LoweredUrlFriendlyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO