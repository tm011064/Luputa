IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetBaseUserModels]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetBaseUserModels];
END
GO