IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_LinkedThreads]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_LinkedThreads](
	[ThreadId] [int] NOT NULL,
	[ContentId] [int] NOT NULL,
	[RelationshipType] [tinyint] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_cms_LinkedThreads] PRIMARY KEY CLUSTERED 
(
	[ThreadId] ASC,
	[ContentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO