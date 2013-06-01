IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users]') AND name = N'IX_wm_Users_LoweredKeywords')
CREATE NONCLUSTERED INDEX [IX_wm_Users_LoweredKeywords] ON [dbo].[wm_Users] 
(
	[LoweredKeywords] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO