IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_InsertOrUpdate];
END
GO