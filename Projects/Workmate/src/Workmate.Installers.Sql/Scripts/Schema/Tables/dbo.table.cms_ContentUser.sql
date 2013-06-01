IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ContentUser](
	[ContentId] [int] NOT NULL,
	[ReceivingUserId] [int] NOT NULL,
	[DateReceivedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ContentUser] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC,
	[ReceivingUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO