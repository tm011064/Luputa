IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_SqlErrorLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_SqlErrorLog](
	[ErrorId] [int] IDENTITY(1,1) NOT NULL,
	[ErrorNumber] [int] NOT NULL,
	[ErrorSeverity] [int] NOT NULL,
	[ErrorState] [int] NOT NULL,
	[ErrorProcedure] [nvarchar](128) NOT NULL,
	[ErrorLine] [int] NOT NULL,
	[ErrorMessage] [nvarchar](4000) NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[SystemUser] [nvarchar](128) NOT NULL,
	[ReturnCode] [int] NULL,
 CONSTRAINT [PK__wm_SqlErrorLog] PRIMARY KEY CLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO