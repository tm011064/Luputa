IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Calendars]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_Calendars](
	[CalendarId] [int] NOT NULL,
	[CalendarType] [tinyint] NOT NULL,
	[ApplicationId] [int] NOT NULL,
 CONSTRAINT [PK_wm_Calendars] PRIMARY KEY CLUSTERED 
(
	[CalendarId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO