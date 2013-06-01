
IF EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_wm_Emails_TotalSendAttempts]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Emails]'))
BEGIN
  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_wm_Emails_TotalSendAttempts]') AND type = 'D')
  BEGIN
    ALTER TABLE [dbo].[wm_Emails] DROP CONSTRAINT [DF_wm_Emails_TotalSendAttempts]    
  END
END
GO