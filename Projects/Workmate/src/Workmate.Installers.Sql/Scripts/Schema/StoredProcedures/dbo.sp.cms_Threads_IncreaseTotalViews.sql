IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_IncreaseTotalViews]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_IncreaseTotalViews];
END
GO

CREATE PROCEDURE [dbo].[cms_Threads_IncreaseTotalViews]
(
    @ThreadId               INT
    , @NumberOfViewsToAdd   INT
)
AS
BEGIN

  UPDATE  [dbo].[cms_Threads]
  SET     TotalViews = TotalViews + @NumberOfViewsToAdd
  WHERE   ThreadId = @ThreadId
	
  RETURN @@ROWCOUNT;
  
END

GO