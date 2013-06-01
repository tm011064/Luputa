IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Emails_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Emails_Insert];
END
GO


CREATE PROCEDURE [dbo].[wm_Emails_Insert]
(
    @ApplicationId    INT
  , @Subject          NVARCHAR(256)
  , @Body             NVARCHAR(max)
  , @Recipients       NVARCHAR(max)
  , @Sender           NVARCHAR(256)
  , @CreatedByUserId  INT
  , @Status           TINYINT
  , @Priority         TINYINT
  , @EmailType        TINYINT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[wm_Emails] 
  (
      [ApplicationId]
    , [Subject]
    , [Body]
    , [Recipients]
    , [Sender]
    , [CreatedByUserId]
    , [DateCreatedUtc]
    , [Status]	
    , [Priority]
    , [EmailType]
  ) 
  VALUES
  (
      @ApplicationId
    , @Subject
    , @Body
    , @Recipients
    , @Sender
    , @CreatedByUserId
    , GETUTCDATE()
    , @Status
    , @Priority
    , @EmailType
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO