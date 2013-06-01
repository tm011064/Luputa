IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Insert];
END
GO


CREATE PROCEDURE [dbo].[cms_Contents_Insert]
(
    @ThreadId         INT
  , @ParentContentId  INT
  , @AuthorUserId     INT
  , @ContentLevel     SMALLINT
  , @Subject          NVARCHAR(256)
  , @FormattedBody    NVARCHAR(max)
  , @IsApproved       BIT
  , @IsLocked         BIT
  , @ContentType      TINYINT
  , @ContentStatus    TINYINT
  , @ExtraInfo        XML
  , @UrlFriendlyName  NVARCHAR(256)
  , @TagXml           XML
  
  , @CreateLinkedThread           BIT = 0
  , @LinkedThreadSectionId        INT = NULL
  , @IsLinkedThreadEnabled        BIT = 0
  , @LinkedThreadRelationshipType TINYINT = NULL  
  
  , @ContentLevelNodesXml   XML = NULL
)
AS
BEGIN
  
  IF (      @UrlFriendlyName IS NOT NULL
       AND  EXISTS (  SELECT  c.* 
                      FROM    cms_Contents  c
                        JOIN  cms_Threads   t ON c.ThreadId = @ThreadId
                      WHERE   LoweredUrlFriendlyName = LOWER(@UrlFriendlyName)))
  BEGIN
    RETURN -1003; -- name not unique
  END
    
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

    DECLARE @ContentLevelNodeId INT;
    SET     @ContentLevelNodeId = NULL;
    
    IF ( @ContentLevelNodesXml IS NOT NULL )
    BEGIN
    
      SET @ErrorCode = -316;
      EXEC @ContentLevelNodeId = cms_ContentLevelNodes_Insert @ContentLevelNodesXml
      
      IF ( @ContentLevelNodeId <= 0 )
      BEGIN
        SET @ErrorCode = @ContentLevelNodeId;
        GOTO Cleanup;
      END
          
    END

    SET @ErrorCode = -311;
    INSERT INTO [dbo].[cms_Contents] 
    (
        [ThreadId]
      , [ParentContentId]
      , [AuthorUserId]
      , [ContentLevel]
      , [Subject]
      , [FormattedBody]
      , [DateCreatedUtc]
      , [IsApproved]
      , [IsLocked]
      , [TotalViews]
      , [ContentType]
      , [RatingSum]
      , [TotalRatings]
      , [ContentStatus]
      , [ExtraInfo]
      , [BaseContentId]	
      , [ContentLevelNodeId]
    ) 
    VALUES
    (
        @ThreadId
      , @ParentContentId
      , @AuthorUserId
      , @ContentLevel
      , @Subject
      , @FormattedBody
      , GETUTCDATE()
      , @IsApproved
      , @IsLocked
      , 0
      , @ContentType
      , 0
      , 0
      , @ContentStatus
      , @ExtraInfo
      , NULL
      , @ContentLevelNodeId
    );
    
    SET @ContentId = SCOPE_IDENTITY();
    
    EXEC @ErrorCode = cms_ContentTag_InsertUpdateDelete @ContentId, @TagXml
    IF (@ErrorCode <> 0)
    BEGIN
      GOTO Cleanup;
    END
    
    SET @ErrorCode = -312
    UPDATE  cms_Threads
    SET     TotalContents = TotalContents + 1
    WHERE   ThreadId = @ThreadId;  
    
    SET @ErrorCode = -313
    UPDATE  s
    SET     s.TotalContents = s.TotalContents + 1
    FROM    cms_Sections  s
      JOIN  cms_Threads   t ON s.SectionId = t.SectionId
    WHERE   t.ThreadId = @ThreadId;  
    
        
    IF( @CreateLinkedThread = 1
        AND @LinkedThreadRelationshipType IS NOT NULL
        AND EXISTS (SELECT SectionId FROM cms_Sections WHERE SectionId = @LinkedThreadSectionId))
    BEGIN
            
      DECLARE @LinkedThreadId INT;
      SET @ErrorCode = -314;
      EXEC @LinkedThreadId = cms_Threads_Insert 
        @LinkedThreadSectionId
        , ''
        , NULL 
        , 0
        , 0
        , 1
        , 0 -- ThreadStatus?
    
      IF (@LinkedThreadId <= 0)
      BEGIN
        SET @ErrorCode = @LinkedThreadId;
        GOTO Cleanup;
      END
      
      SET @ErrorCode = -315;
      INSERT INTO [cms_LinkedThreads]
             ([ThreadId]
             ,[ContentId]
             ,[RelationshipType]
             ,[IsEnabled])
       VALUES
             (@LinkedThreadId
             ,@ContentId
             ,@LinkedThreadRelationshipType
             ,@IsLinkedThreadEnabled)
    
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