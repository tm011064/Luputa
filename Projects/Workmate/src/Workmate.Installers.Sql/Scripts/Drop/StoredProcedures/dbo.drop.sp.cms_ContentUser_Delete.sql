IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentUser_Delete];
END
GO