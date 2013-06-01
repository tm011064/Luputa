IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_Threads_Delete]
(
    @ThreadId               INT
    , @DeleteLinkedThreads  BIT = 1
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
  
    DECLARE @t_Threads TABLE ( ThreadId INT );
    
    INSERT INTO @t_Threads  ( ThreadId )
    SELECT      lt.ThreadId
    FROM    cms_Contents      c   
      JOIN  cms_LinkedThreads lt  ON c.ContentId = lt.ContentId
    WHERE   c.ThreadId = @ThreadId
    
    SET @ErrorCode = -201;
    DELETE  FROM    [cms_LinkedThreads] 
    WHERE   ThreadId = @ThreadId
    
    IF ( @DeleteLinkedThreads = 1 )
    BEGIN
    
      SET @ErrorCode = -202;
      DELETE  lt 
      FROM    [cms_LinkedThreads] lt
        JOIN  @t_Threads          t   ON lt.ThreadId = t.ThreadId
    
      SET @ErrorCode = -203;
      DELETE  t
      FROM    [cms_Threads] t
        JOIN  @t_Threads    tt  ON t.ThreadId = tt.ThreadId
    
    END
    
    SET @ErrorCode = -204;
    DELETE FROM [dbo].[cms_Threads] 
    WHERE       [ThreadId]  = @ThreadId
    
    SET @ErrorCode = @@ROWCOUNT;
	           
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN @ErrorCode;

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