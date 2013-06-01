IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentUser_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentUser_InsertOrUpdate];
END
GO

CREATE PROCEDURE [dbo].[cms_ContentUser_InsertOrUpdate]
(
    @ContentId        INT
  , @ReceivingUserId  INT
)
AS
BEGIN

  DECLARE @DateReceivedUtc  DATETIME;
  SET     @DateReceivedUtc = GETUTCDATE();

  SET NOCOUNT ON;

  BEGIN TRAN

    UPDATE  [cms_ContentUser] WITH (SERIALIZABLE)
    SET     [DateReceivedUtc]  = @DateReceivedUtc
    WHERE   [ContentId] = @ContentId
        AND [ReceivingUserId] = @ReceivingUserId
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      INSERT INTO [dbo].[cms_ContentUser] 
      (
          [ContentId]
        , [ReceivingUserId]	
        , [DateReceivedUtc]
      ) 
      VALUES
      (
          @ContentId
        , @ReceivingUserId
        , @DateReceivedUtc
      );    
    END
    
  COMMIT TRAN
  
  RETURN 0;
  
END

GO