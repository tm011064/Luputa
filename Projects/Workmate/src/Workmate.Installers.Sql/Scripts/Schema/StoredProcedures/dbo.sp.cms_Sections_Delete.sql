IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_Sections_Delete]
(
  @SectionId  INT
  , @DeleteLinkedThreads BIT = 1
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
  
    DECLARE @t_Threads TABLE ( ThreadId INT );
    
    INSERT INTO @t_Threads ( ThreadId )
    SELECT      lt.ThreadId
    FROM    [dbo].[cms_LinkedThreads] lt
      JOIN  [dbo].[cms_Threads] t ON lt.ThreadId = t.ThreadId  
    WHERE   t.SectionId = @SectionId
    UNION ALL
    SELECT      lt.ThreadId
    FROM    cms_Threads       t 
      JOIN  cms_Contents      c   ON t.ThreadId = c.ThreadId
      JOIN  cms_LinkedThreads lt  ON c.ContentId = lt.ContentId
    WHERE   t.SectionId = @SectionId
    
    SET @ErrorCode = -101;
    DELETE  lt 
    FROM    [cms_LinkedThreads] lt
      JOIN  @t_Threads          t   ON lt.ThreadId = t.ThreadId
    
    IF ( @DeleteLinkedThreads = 1 )
    BEGIN
    
      SET @ErrorCode = -102;
      DELETE  t
      FROM    [cms_Threads] t
        JOIN  @t_Threads    tt  ON t.ThreadId = tt.ThreadId
    
    END
                    
    SET @ErrorCode = -103;
    DELETE FROM [dbo].[cms_Sections] 
    WHERE      [SectionId] = @SectionId
	           
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