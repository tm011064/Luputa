IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Offices](
	[ApplicationId] [int] NOT NULL,
	[OfficeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](512) NULL,
	[ExtraInfo] [xml] NOT NULL,
 CONSTRAINT [PK_wm_Offices] PRIMARY KEY CLUSTERED 
(
	[OfficeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO