IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_wm_Users_Gender]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[wm_Users] ADD  CONSTRAINT [DF_wm_Users_Gender]  DEFAULT ((1)) FOR [Gender]
END

GO