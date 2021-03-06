IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentRatings_InsertOrUpdate];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentRatings_InsertOrUpdate]
(
    @Rating           SMALLINT
  , @ContentId        INT
  , @UserId           INT
  , @AllowSelfRating  BIT  
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
  
    IF ( @AllowSelfRating = 0 
         AND ( SELECT AuthorUserId FROM cms_Contents WHERE ContentId = @ContentId ) = @UserId )
    BEGIN
      SET @ErrorCode = -1;
      GOTO Cleanup;
    END
    
    SET @ErrorCode = -411;
    UPDATE  [cms_ContentRatings] WITH (SERIALIZABLE)
    SET     [Rating] = @Rating
          , [DateCreatedUtc]  = @DateCreatedUtc
    WHERE   [UserId] = @UserId
        AND [ContentId] = @ContentId
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      SET @ErrorCode = -412;
      INSERT INTO [dbo].[cms_ContentRatings] 
      (
          [Rating]
        , [DateCreatedUtc]	
        , [UserId]
        , [ContentId]
      ) 
      VALUES
      (
          @Rating
        , @DateCreatedUtc
        , @UserId
        , @ContentId
      );    
    END
    
    SET @ErrorCode = -413;
    UPDATE  cms_Contents
    SET     RatingSum =       ( SELECT SUM(Rating) FROM cms_ContentRatings WHERE ContentId = @ContentId )
            , TotalRatings =  ( SELECT COUNT (*) FROM cms_ContentRatings WHERE ContentId = @ContentId )
    WHERE   ContentId = @ContentId
          
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