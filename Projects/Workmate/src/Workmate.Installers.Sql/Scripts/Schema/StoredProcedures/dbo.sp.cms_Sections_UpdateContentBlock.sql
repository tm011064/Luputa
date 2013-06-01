IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_UpdateContentBlock]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_UpdateContentBlock];
END
GO


CREATE PROCEDURE [dbo].[cms_Sections_UpdateContentBlock]
(
    @ApplicationId    INT
  , @Name             NVARCHAR(128)
  , @FormattedBody    NVARCHAR(MAX)
  , @SectionType      TINYINT
  , @ActiveContentBlockStatus   TINYINT
  , @InactiveContentBlockStatus TINYINT
  , @AuthorUserId     INT
  , @CreatePlaceholderIfNotExists BIT
)
AS
BEGIN  

  DECLARE @SectionId INT;
  DECLARE @ThreadId INT;
  DECLARE @ContentId INT;

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

    SELECT  @SectionId = s.SectionId
            , @ThreadId = t.ThreadId
    FROM    cms_Sections  s
      LEFT JOIN  cms_Threads   t ON s.SectionId = t.SectionId
    WHERE   s.ApplicationId = @ApplicationId
        AND s.LoweredName = LOWER(@Name);
         
    IF (@SectionId IS NULL)
    BEGIN
    
      IF (@CreatePlaceholderIfNotExists = 0)
      BEGIN
          SET @ErrorCode = -1;
          GOTO Cleanup;
      END
      ELSE
      BEGIN
      
        EXEC @SectionId = [cms_Sections_Insert] 
           @ApplicationId
          , NULL -- @ParentSectionId
          , NULL -- @GroupId
          , @Name
          , NULL -- @Description
          , @SectionType
          , 1 -- @IsActive
          , 0 -- @IsModerated
                
        IF (@SectionId <= 0)
        BEGIN
          SET @ErrorCode = @SectionId;
          GOTO Cleanup;
        END        
        
      END
                
    END
                
    IF (@ThreadId IS NULL)
    BEGIN
    
      EXEC @ThreadId = [cms_Threads_Insert] 
         @SectionId
        , NULL -- @Name
        , NULL -- @StickyDateUtc
        , 0 -- @IsLocked
        , 0 -- @IsSticky
        , 1 -- @IsApproved
        , 0 -- @ThreadStatus
    
      IF (@ThreadId <= 0)
      BEGIN
        SET @ErrorCode = @ThreadId;
        GOTO Cleanup;
      END
      
    END
    
    SET @ErrorCode = -111;
    UPDATE  cms_Contents
    SET     ContentStatus = @InactiveContentBlockStatus
    WHERE   ThreadId = @ThreadId      

    EXEC @ContentId = [cms_Contents_Insert] 
        @ThreadId -- @ThreadId
      , NULL -- @ParentContentId
      , @AuthorUserId
      , 0 -- @ContentLevel
      , NULL -- @Subject
      , @FormattedBody
      , 1 -- @IsApproved
      , 0 -- @IsLocked
      , 0 -- @ContentType
      , @ActiveContentBlockStatus -- @ContentStatus
      , '<r />' -- @ExtraInfo
      , NULL -- @UrlFriendlyName
      , NULL -- @TagXml
      , 0 -- @CreateNewThread
      , NULL -- @NewThreadSectionId
      , NULL -- @IsNewThreadApproved      
  
    IF (@ContentId <= 0)
    BEGIN
      SET @ErrorCode = @ContentId;
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

  RETURN @ContentId

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