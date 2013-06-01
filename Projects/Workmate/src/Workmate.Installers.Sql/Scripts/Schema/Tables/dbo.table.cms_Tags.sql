IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Tags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cms_Tags](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](32) NOT NULL,
	[LoweredTag] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_cms_Tags] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO