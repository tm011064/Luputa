IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Threads](
	[ThreadId] [int] IDENTITY(1,1) NOT NULL,
	[SectionId] [int] NOT NULL,
	[Name] [nvarchar](32) NULL,
	[LoweredName] [nvarchar](32) NULL,
	[LastViewedDateUtc] [datetime] NOT NULL,
	[StickyDateUtc] [datetime] NULL,
	[TotalViews] [int] NOT NULL,
	[TotalReplies] [int] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[IsSticky] [bit] NOT NULL,
	[IsApproved] [bit] NOT NULL,
	[RatingSum] [int] NOT NULL,
	[TotalRatings] [int] NOT NULL,
	[ThreadStatus] [int] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[TotalContents] [int] NOT NULL,
 CONSTRAINT [PK_cms_Threads] PRIMARY KEY CLUSTERED 
(
	[ThreadId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO