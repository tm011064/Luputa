IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Get];
END
GO

CREATE PROCEDURE [dbo].[wm_Emails_Get]
(
  @ApplicationId  INT
  , @Status       TINYINT
  , @EmailId      INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [EmailId]
          , [Subject]
          , [Body]
          , [Recipients]
          , [Sender]
          , [CreatedByUserId]
          , [DateCreatedUtc]
          , [Status]
          , [Priority]
          , [SentUtc]
          , [EmailType]
          , [TotalSendAttempts]
  FROM    [dbo].[wm_Emails]
  WHERE   (@ApplicationId IS NULL OR [ApplicationId] = @ApplicationId)
    AND   (@Status IS NULL OR [Status] = @Status)
    AND   (@EmailId IS NULL OR [EmailId] = @EmailId)
  ORDER BY [Priority], [DateCreatedUtc] 
  
END

GO