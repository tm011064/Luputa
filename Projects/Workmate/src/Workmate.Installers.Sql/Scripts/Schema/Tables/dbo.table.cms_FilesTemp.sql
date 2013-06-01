IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_FilesTemp](
	[ApplicationId] [int] NOT NULL,
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[FileType] [tinyint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[FileName] [nvarchar](1024) NULL,
	[Content] [varbinary](max) NOT NULL,
	[ContentType] [nvarchar](64) NULL,
	[ContentSize] [int] NOT NULL,
	[FriendlyFileName] [nvarchar](256) NULL,
	[Height] [int] NOT NULL,
	[Width] [int] NOT NULL,
 CONSTRAINT [PK_cms_FilesTemp] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO