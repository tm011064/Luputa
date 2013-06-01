IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Delete];
END
GO