IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Sections](
	[ApplicationId] [int] NOT NULL,
	[SectionId] [int] IDENTITY(1,1) NOT NULL,
	[ParentSectionId] [int] NULL,
	[GroupId] [int] NULL,
	[Name] [nvarchar](128) NOT NULL,
	[LoweredName] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[SectionType] [tinyint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsModerated] [bit] NOT NULL,
	[TotalContents] [int] NOT NULL,
	[TotalThreads] [int] NOT NULL,
 CONSTRAINT [PK_cms_Sections] PRIMARY KEY CLUSTERED 
(
	[SectionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO