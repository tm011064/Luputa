IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Applications](
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](512) NULL,
	[ExtraInfo] [xml] NOT NULL,
 CONSTRAINT [PK_wm_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO