IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Delete];
END
GO


CREATE PROCEDURE [dbo].[wm_Emails_Delete]
(
    @EmailId                   INT
    , @Status                  TINYINT
    , @OlderThanDateCreatedUtc DATETIME
)
AS
BEGIN

  DELETE FROM [dbo].[wm_Emails] 
  WHERE      (@EmailId IS NULL OR [EmailId] = @EmailId)
        AND  (@Status IS NULL OR [Status] = @Status)
        AND  (@OlderThanDateCreatedUtc IS NULL OR [DateCreatedUtc] < @OlderThanDateCreatedUtc)
	
  RETURN @@ROWCOUNT;
  
END

GO