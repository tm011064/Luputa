IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_PutInSendQueue]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_PutInSendQueue];
END
GO

CREATE PROCEDURE [dbo].[wm_Emails_PutInSendQueue]
(
  @ApplicationId          INT
  
  , @InQueueStatus        TINYINT
  , @UnsentStatus         TINYINT
  , @SendFailedStatus     TINYINT
  
  , @QueuedEmailsThresholdInSeconds INT -- we send either unsent emails or queued emails that have been queued for
                                        -- a long time. This probably means that the server went down while sending
                                        -- emails, putting the mail into a queue but not acutally sending it out 
  , @FailedEmailsThresholdInSeconds INT  
  , @TotalEmailsToEnqueue           INT
)
AS
BEGIN

  SET NOCOUNT ON;

  DECLARE @QueuedThreshold  DATETIME;
  DECLARE @FailedThreshold  DATETIME;
  DECLARE @Now        DATETIME;
  
  SET     @Now = GETUTCDATE();
  SET     @QueuedThreshold = DATEADD(ss, ABS(@QueuedEmailsThresholdInSeconds) * -1, @Now); 
  SET     @FailedThreshold = DATEADD(ss, ABS(@FailedEmailsThresholdInSeconds) * -1, @Now); 

  DECLARE @TableEmailIds TABLE ( EmailId INT );

  UPDATE    e
  SET       e.Status      = @InQueueStatus
            , e.QueuedUtc = @Now 
  OUTPUT    INSERTED.EmailId 
  INTO      @TableEmailIds
  FROM      wm_Emails e
    JOIN    ( SELECT TOP (@TotalEmailsToEnqueue) s1.EmailId
              FROM        wm_Emails s1
              WHERE       (     s1.Status = @InQueueStatus 
                            AND s1.QueuedUtc IS NOT NULL 
                            AND s1.QueuedUtc < @QueuedThreshold )     -- get all queued emails that have not been sent
                      OR  (     s1.Status = @SendFailedStatus
                            AND s1.SentUtc IS NOT NULL 
                            AND s1.SentUtc < @FailedThreshold ) -- and failed attemps
                      OR  (     s1.Status = @UnsentStatus )
              ORDER BY s1.Priority, s1.DateCreatedUtc ) s ON e.EmailId = s.EmailId

  SELECT    e.[ApplicationId]
          , e.[EmailId]
          , e.[Subject]
          , e.[Body]
          , e.[Recipients]
          , e.[Sender]
          , e.[CreatedByUserId]
          , e.[DateCreatedUtc]
          , e.[Status]
          , e.[Priority]
          , e.[SentUtc]
          , e.[EmailType]
          , e.[TotalSendAttempts]
  FROM    [dbo].[wm_Emails] e
    JOIN  @TableEmailIds    te ON e.EmailId = te.EmailId  
  ORDER BY e.[Priority], e.[DateCreatedUtc]
  
END

GO