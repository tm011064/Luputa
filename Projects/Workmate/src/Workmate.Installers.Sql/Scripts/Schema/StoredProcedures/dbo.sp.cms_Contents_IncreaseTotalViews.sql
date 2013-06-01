IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_IncreaseTotalViews]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_IncreaseTotalViews];
END
GO

CREATE PROCEDURE [dbo].[cms_Contents_IncreaseTotalViews]
(
    @ContentId               INT
    , @NumberOfViewsToAdd   INT
)
AS
BEGIN

  UPDATE  [dbo].[cms_Contents]
  SET     TotalViews = TotalViews + @NumberOfViewsToAdd
  WHERE   ContentId = @ContentId
	
  RETURN @@ROWCOUNT;
  
END

GO