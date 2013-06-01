IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_SetToSent]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_SetToSent];
END
GO


CREATE PROCEDURE [dbo].[wm_Emails_SetToSent]
(
  @EmailId          INT
  , @Status         TINYINT
  , @Priority       TINYINT -- if the email failed, we might want to reset the priority for the next attempt
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE  [dbo].[wm_Emails] 
  SET     [SentUtc]   = GETUTCDATE()
          , [Status]  = @Status
          , [TotalSendAttempts] = [TotalSendAttempts] + 1
          , [Priority] = @Priority
  WHERE   [EmailId] = @EmailId
	
  RETURN @@ROWCOUNT;
  
END
GO