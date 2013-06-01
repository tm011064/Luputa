IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Groups](
	[GroupId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[GroupType] [tinyint] NOT NULL,
 CONSTRAINT [PK_cms_Groups] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO