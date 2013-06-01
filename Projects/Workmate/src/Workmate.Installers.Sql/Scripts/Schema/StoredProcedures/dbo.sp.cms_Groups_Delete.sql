IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_Delete];
END
GO

CREATE PROCEDURE [dbo].[cms_Groups_Delete]
(
    @GroupId  INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_Groups] 
  WHERE       [GroupId] = @GroupId
	
  RETURN @@ROWCOUNT;
  
END

GO