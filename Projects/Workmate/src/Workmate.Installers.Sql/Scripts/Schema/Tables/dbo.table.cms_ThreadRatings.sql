IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ThreadRatings](
	[UserId] [int] NOT NULL,
	[ThreadId] [int] NOT NULL,
	[Rating] [smallint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_cms_ThreadRatings] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO