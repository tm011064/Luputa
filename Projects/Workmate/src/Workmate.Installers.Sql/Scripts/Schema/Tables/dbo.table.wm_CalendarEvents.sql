IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_CalendarEvents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[wm_CalendarEvents](
	[CalendarEventId] [int] IDENTITY(1,1) NOT NULL,
	[CalendarId] [int] NOT NULL,
	[ParentCalendarEventId] [int] NULL,
	[UserId] [int] NOT NULL,
	[DateFromUtc] [datetime] NULL,
	[DateToUtc] [datetime] NULL,
	[CalendarEventType] [tinyint] NOT NULL,
	[CalendarEventStatus] [tinyint] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	[Comment] [nvarchar](1024) NULL,
 CONSTRAINT [PK_wm_CalendarEvents] PRIMARY KEY CLUSTERED 
(
	[CalendarEventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO