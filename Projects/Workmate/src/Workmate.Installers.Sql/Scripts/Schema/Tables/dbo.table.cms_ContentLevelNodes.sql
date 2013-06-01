IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_ContentLevelNodes](
	[ContentLevelNodeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[LoweredName] [nvarchar](256) NOT NULL,
	[Level] [int] NOT NULL,
	[ParentContentLevelNodeId] [int] NULL,
	[BreadCrumbs] [nvarchar](max) NULL,
	[BreadCrumbsSplitIndexes] [nvarchar](max) NULL,
	[ThreadId] [int] NULL,
	[SectionId] [int] NULL,
 CONSTRAINT [PK_cms_ContentLevelNodes] PRIMARY KEY CLUSTERED 
(
	[ContentLevelNodeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO