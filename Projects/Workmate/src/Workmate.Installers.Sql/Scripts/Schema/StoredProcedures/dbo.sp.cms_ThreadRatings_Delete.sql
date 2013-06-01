IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ThreadRatings_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ThreadRatings_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_ThreadRatings_Delete]
(
    @UserId     INT
  , @ThreadId  INT
)
AS
BEGIN

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
      
    SET @ErrorCode = -251;
    DELETE FROM [dbo].[cms_ThreadRatings] 
    WHERE   [UserId]    = @UserId
        AND [ThreadId] = @ThreadId
                
    SET @ErrorCode = -252;
    UPDATE  cms_Threads
    SET     RatingSum =       ISNULL(( SELECT SUM(Rating) FROM cms_ThreadRatings WHERE ThreadId = @ThreadId ), 0 )
            , TotalRatings =  ISNULL(( SELECT COUNT (*) FROM cms_ThreadRatings WHERE ThreadId = @ThreadId ), 0 )
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