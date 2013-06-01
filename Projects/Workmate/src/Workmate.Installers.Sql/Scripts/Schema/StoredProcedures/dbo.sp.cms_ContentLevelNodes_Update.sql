IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Update];
END
GO



CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Update]
(
  @ContentLevelNodeId INT
  , @Name             NVARCHAR(256)
)
AS
BEGIN
  
  IF ( NOT EXISTS ( SELECT * FROM cms_ContentLevelNodes WHERE ContentLevelNodeId = @ContentLevelNodeId ) )
  BEGIN
    RETURN -1003; 
  END
  
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
 
    SET @ErrorCode = -361;
    -- first, rename
    UPDATE  cms_ContentLevelNodes
    SET     Name = @Name
            , LoweredName = LOWER(@Name)
    WHERE   ContentLevelNodeId = @ContentLevelNodeId
    
    DECLARE @ThreadId     INT
            , @SectionId  INT;
    SELECT  @ThreadId = ThreadId
            , @SectionId = SectionId
    FROM    cms_ContentLevelNodes
    WHERE   ContentLevelNodeId = @ContentLevelNodeId
    
    EXEC @ErrorCode = cms_ContentLevelNodes_Cleanup @ThreadId, @SectionId 
    IF ( @ErrorCode <> 0 )
    BEGIN
      GOTO Cleanup;
    END
    
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  RETURN 0

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