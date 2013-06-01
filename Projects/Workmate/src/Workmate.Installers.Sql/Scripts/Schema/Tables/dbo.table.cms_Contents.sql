IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Contents](
	[ContentId] [int] IDENTITY(1,1) NOT NULL,
	[ThreadId] [int] NOT NULL,
	[ParentContentId] [int] NULL,
	[AuthorUserId] [int] NOT NULL,
	[ContentLevel] [smallint] NOT NULL,
	[Subject] [nvarchar](256) NULL,
	[FormattedBody] [nvarchar](max) NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[TotalViews] [int] NOT NULL,
	[ContentType] [tinyint] NOT NULL,
	[RatingSum] [int] NOT NULL,
	[TotalRatings] [int] NOT NULL,
	[ContentStatus] [tinyint] NOT NULL,
	[ExtraInfo] [xml] NOT NULL,
	[BaseContentId] [int] NULL,
	[UrlFriendlyName] [nvarchar](128) NULL,
	[LoweredUrlFriendlyName] [nvarchar](128) NULL,
	[ContentLevelNodeId] [int] NULL,
 CONSTRAINT [PK_cms_Contents] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO