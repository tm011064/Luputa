
IF EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_cms_Threads_TotalContents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_Threads]'))
BEGIN
  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_cms_Threads_TotalContents]') AND type = 'D')
  BEGIN
    ALTER TABLE [dbo].[cms_Threads] DROP CONSTRAINT [DF_cms_Threads_TotalContents]    
  END
END
GO