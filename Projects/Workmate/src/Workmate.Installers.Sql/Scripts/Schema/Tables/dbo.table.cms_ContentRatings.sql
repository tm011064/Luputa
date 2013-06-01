IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ContentRatings](
	[UserId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
	[Rating] [smallint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ContentRatings] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO