IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Insert];
END
GO


CREATE PROCEDURE [dbo].[cms_Threads_Insert]
(
    @SectionId      INT
  , @Name           NVARCHAR(32)
  , @StickyDateUtc  DATETIME
  , @IsLocked       BIT
  , @IsSticky       BIT
  , @IsApproved     BIT
  , @ThreadStatus   INT
)
AS
BEGIN

  DECLARE @ThreadId INT;

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
      
    SET @ErrorCode = -211;
    INSERT INTO [dbo].[cms_Threads] 
    (
        [SectionId]
      , [Name]
      , [LoweredName]
      , [LastViewedDateUtc]
      , [StickyDateUtc]
      , [TotalViews]
      , [TotalReplies]
      , [IsLocked]
      , [IsSticky]
      , [IsApproved]
      , [RatingSum]
      , [TotalRatings]
      , [ThreadStatus]
      , [DateCreatedUtc]	
    ) 
    VALUES
    (
        @SectionId
      , @Name
      , LOWER(@Name)
      , GETUTCDATE()
      , @StickyDateUtc
      , 0
      , 0
      , @IsLocked
      , @IsSticky
      , @IsApproved
      , 0
      , 0
      , @ThreadStatus
      , GETUTCDATE()
    );
    
    SET @ThreadId = SCOPE_IDENTITY();
    
    SET @ErrorCode = -212;
    UPDATE  s
    SET     s.TotalThreads = TotalThreads + 1
    FROM    cms_Sections  s
      JOIN  cms_Threads   t ON s.SectionId = t.SectionId
    WHERE   t.ThreadId = @ThreadId; 
              
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @ThreadId

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