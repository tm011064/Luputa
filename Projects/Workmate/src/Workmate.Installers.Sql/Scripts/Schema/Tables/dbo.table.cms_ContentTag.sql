IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentTag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ContentTag](
	[TagId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
 CONSTRAINT [PK_cms_ContentTag] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO