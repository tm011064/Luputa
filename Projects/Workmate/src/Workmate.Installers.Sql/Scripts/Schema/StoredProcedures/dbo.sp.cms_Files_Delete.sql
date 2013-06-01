IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_Delete];
END
GO

CREATE PROCEDURE [dbo].[cms_Files_Delete]
(
    @FileId INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_Files] 
  WHERE       [FileId]  = @FileId
	
  RETURN @@ROWCOUNT;
  
END

GO