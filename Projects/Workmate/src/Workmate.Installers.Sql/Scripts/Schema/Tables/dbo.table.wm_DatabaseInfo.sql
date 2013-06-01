IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_DatabaseInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_DatabaseInfo](
	[ActionId] [int] IDENTITY(1,1) NOT NULL,
	[ActionType] [nvarchar](32) NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[DatabaseVersion] [decimal](8, 8) NOT NULL,
	[Comment] [nvarchar](1024) NULL,
 CONSTRAINT [PK_wm_DatabaseInfo] PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO