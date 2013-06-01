IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentUser_Delete];
END
GO

CREATE PROCEDURE [dbo].[cms_ContentUser_Delete]
(
    @ContentId        INT
  , @ReceivingUserId  INT
)
AS
BEGIN

  DELETE FROM [dbo].[cms_ContentUser] 
  WHERE     [ContentId]       = @ContentId
        AND [ReceivingUserId] = @ReceivingUserId
	
  RETURN @@ROWCOUNT;
  
END

GO