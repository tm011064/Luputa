IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FilesTemp_Delete];
END
GO

CREATE PROCEDURE [dbo].[cms_FilesTemp_Delete]
(
  @UserId     INT
  , @FileId   INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_FilesTemp] 
  WHERE       UserId  = @UserId
          AND FileId  = @FileId
	
  RETURN @@ROWCOUNT;
  
END

GO