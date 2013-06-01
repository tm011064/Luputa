IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Threads_TotalContents]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[cms_Threads] ADD  CONSTRAINT [DF_cms_Threads_TotalContents]  DEFAULT ((0)) FOR [TotalContents]
END

GO