IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_InsertOrUpdate];
END
GO


CREATE PROCEDURE [dbo].[cms_ThreadRatings_InsertOrUpdate]
(
    @Rating           SMALLINT
  , @ThreadId        INT
  , @UserId           INT
)
AS
BEGIN

  DECLARE @DateCreatedUtc DATETIME;
  SET     @DateCreatedUtc = GETUTCDATE();	
  
  DECLARE @ErrorCode     INT
  SET @ErrorCode = 0

  DECLARE @TranStarted   BIT
  SET @TranStarted = 0

  IF( @@TRANCOUNT = 0 )
  BEGIN
    BEGIN TRANSACTION
    SET @TranStarted = 1
  END
  ELSE
  	SET @TranStarted = 0
  	
  BEGIN TRY
      
    SET @ErrorCode = -261;
    UPDATE  [cms_ThreadRatings] WITH (SERIALIZABLE)
    SET     [Rating] = @Rating
          , [DateCreatedUtc]  = @DateCreatedUtc
    WHERE   [UserId] = @UserId
        AND [ThreadId] = @ThreadId
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      SET @ErrorCode = -262;
      INSERT INTO [dbo].[cms_ThreadRatings] 
      (
          [Rating]
        , [DateCreatedUtc]	
        , [UserId]
        , [ThreadId]
      ) 
      VALUES
      (
          @Rating
        , @DateCreatedUtc
        , @UserId
        , @ThreadId
      );    
    END
    
    SET @ErrorCode = -263;
    UPDATE  cms_Threads
    SET     RatingSum =       ( SELECT SUM(Rating) FROM cms_ThreadRatings WHERE ThreadId = @ThreadId )
            , TotalRatings =  ( SELECT COUNT (*) FROM cms_ThreadRatings WHERE ThreadId = @ThreadId )
    WHERE   ThreadId = @ThreadId
          
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0;

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
      SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  RETURN @ErrorCode

END


GO