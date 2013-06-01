
IF EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_wm_Users_Gender]') AND parent_object_id = OBJECT_ID(N'[dbo].[wm_Users]'))
BEGIN
  IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_wm_Users_Gender]') AND type = 'D')
  BEGIN
    ALTER TABLE [dbo].[wm_Users] DROP CONSTRAINT [DF_wm_Users_Gender]    
  END
END
GO