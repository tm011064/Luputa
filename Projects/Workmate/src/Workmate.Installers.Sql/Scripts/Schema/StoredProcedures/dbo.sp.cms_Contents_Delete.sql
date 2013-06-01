IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Delete];
END
GO


CREATE PROCEDURE [dbo].[cms_Contents_Delete]
(
    @ContentId            INT
    , @DeleteLinkedThreads BIT = 1
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
  
    DECLARE @ThreadId INT;
    DECLARE @LinkedThreadId INT;
  
    SET @ErrorCode = -301;    
    SELECT  @ThreadId = ThreadId
    FROM    [cms_Contents]
    WHERE   ContentId = @ContentId
    
    IF ( @ThreadId IS NULL )
    BEGIN
      SET @ErrorCode = -1003;
      GOTO Cleanup;
    END
    
    SET @ErrorCode = -302;   
    SELECT  @LinkedThreadId = ThreadId
    FROM    cms_LinkedThreads
    WHERE   ContentId = @ContentId
    
    IF ( @LinkedThreadId IS NOT NULL )
    BEGIN
    
      SET @ErrorCode = -303;      
      DELETE FROM cms_LinkedThreads
      WHERE       ContentId = @ContentId; 
      
      IF ( @DeleteLinkedThreads = 1 )
      BEGIN
      
        SET @ErrorCode = -304;      
        DELETE FROM cms_Threads
        WHERE       ThreadId = @LinkedThreadId;
      
      END         
    
    END
	           	  
    SET @ErrorCode = -305
    SELECT  @ThreadId = ThreadId
    FROM    [cms_Contents]
    WHERE   ContentId = @ContentId
	           
    SET @ErrorCode = -306
    UPDATE  cms_Threads
    SET     TotalContents = TotalContents - 1
    WHERE   ThreadId = @ThreadId;  
    
    SET @ErrorCode = -307
    UPDATE  s
    SET     s.TotalContents = s.TotalContents - 1
    FROM    cms_Sections  s
      JOIN  cms_Threads   t ON s.SectionId = t.SectionId
    WHERE   t.ThreadId = @ThreadId; 
    
    SET @ErrorCode = -308;      
    DELETE FROM [dbo].[cms_Contents] 
    WHERE       [ContentId] = @ContentId
    
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