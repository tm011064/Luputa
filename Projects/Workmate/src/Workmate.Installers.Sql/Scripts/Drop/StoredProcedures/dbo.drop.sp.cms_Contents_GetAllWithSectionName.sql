IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetAllWithSectionName]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetAllWithSectionName];
END
GO