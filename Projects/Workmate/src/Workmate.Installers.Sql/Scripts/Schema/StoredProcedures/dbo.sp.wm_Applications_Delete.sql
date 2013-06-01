IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_Delete];
END
GO


CREATE PROCEDURE [dbo].[wm_Applications_Delete]
(
    @ApplicationId  INT
)
AS
BEGIN

  DELETE FROM [dbo].[wm_Applications] 
  WHERE      [ApplicationId] = @ApplicationId
	
  RETURN @@ROWCOUNT;
  
END

GO