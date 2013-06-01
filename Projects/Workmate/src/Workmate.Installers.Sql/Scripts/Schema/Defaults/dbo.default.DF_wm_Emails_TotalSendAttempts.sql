IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_wm_Emails_TotalSendAttempts]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[wm_Emails] ADD  CONSTRAINT [DF_wm_Emails_TotalSendAttempts]  DEFAULT ((0)) FOR [TotalSendAttempts]
END

GO