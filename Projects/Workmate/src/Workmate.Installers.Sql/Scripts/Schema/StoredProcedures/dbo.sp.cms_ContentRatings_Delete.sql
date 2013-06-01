IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentRatings_Delete]
(
    @UserId     INT
  , @ContentId  INT
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
      
    SET @ErrorCode = -401;
    DELETE FROM [dbo].[cms_ContentRatings] 
    WHERE   [UserId]    = @UserId
        AND [ContentId] = @ContentId
                
    SET @ErrorCode = -402;
    UPDATE  cms_Contents
    SET     RatingSum =       ISNULL(( SELECT SUM(Rating) FROM cms_ContentRatings WHERE ContentId = @ContentId ), 0 )
            , TotalRatings =  ISNULL(( SELECT COUNT (*) FROM cms_ContentRatings WHERE ContentId = @ContentId ), 0 )
    WHERE   ContentId = @ContentId
          
  END TRY
  BEGIN CATCH
	PRINT ERROR_MESSAGE();
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