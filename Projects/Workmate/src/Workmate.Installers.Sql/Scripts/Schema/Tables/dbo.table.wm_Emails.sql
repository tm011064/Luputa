IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Emails](
	[ApplicationId] [int] NOT NULL,
	[EmailId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[Recipients] [nvarchar](max) NOT NULL,
	[Sender] [nvarchar](256) NOT NULL,
	[CreatedByUserId] [int] NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[SentUtc] [datetime] NULL,
	[QueuedUtc] [datetime] NULL,
	[Status] [tinyint] NOT NULL,
	[Priority] [tinyint] NOT NULL,
	[EmailType] [tinyint] NOT NULL,
	[TotalSendAttempts] [int] NOT NULL,
 CONSTRAINT [PK_wm_Emails] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO