IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Insert];
END
GO



CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Insert]
(
  @Nodes          XML
  , @ThreadId     INT = NULL
  , @SectionId    INT = NULL
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
 
    DECLARE @Name                     NVARCHAR(256);
    DECLARE @Level                    INT;
    DECLARE @ContentLevelNodeId       INT;
    DECLARE @ParentContentLevelNodeId INT;
    
    SET @Level = 0;
    
    DECLARE NodesCursor CURSOR LOCAL
    FOR     SELECT  t.a.value('@n','NVARCHAR(256)') AS Name
            FROM    @Nodes.nodes('//i') t(a)
            ORDER BY t.a.value('@l','INT')            
    OPEN    NodesCursor    
    FETCH NEXT FROM NodesCursor INTO @Name
    WHILE (@@FETCH_STATUS <> -1)
    BEGIN
        
      SET @ContentLevelNodeId = NULL;
        
      SET @ErrorCode = -351;
      SELECT  @ContentLevelNodeId         = ContentLevelNodeId
      FROM    cms_ContentLevelNodes
      WHERE       LoweredName = LOWER(@Name)
        AND (     (ParentContentLevelNodeId IS NULL AND @ParentContentLevelNodeId IS NULL)
              OR  (@ParentContentLevelNodeId = ParentContentLevelNodeId) );
                
      IF ( @ContentLevelNodeId IS NULL )
      BEGIN
      
        SET @ErrorCode = -352;        
        IF ( @ParentContentLevelNodeId IS NULL )
        BEGIN
          INSERT INTO cms_ContentLevelNodes ([Name]
                                            ,[LoweredName]
                                            ,[Level]
                                            ,[ParentContentLevelNodeId]
                                            ,[BreadCrumbs]
                                            ,[BreadCrumbsSplitIndexes]
                                            ,[ThreadId]
                                            ,[SectionId])
          VALUES  ( @Name
                    , LOWER(@Name)
                    , @Level
                    , @ParentContentLevelNodeId
                    , @Name
                    , CAST(LEN(@Name) AS NVARCHAR(32))
                    , @ThreadId
                    , @SectionId)
        END
        ELSE
        BEGIN        
          INSERT INTO cms_ContentLevelNodes ([Name]
                                            ,[LoweredName]
                                            ,[Level]
                                            ,[ParentContentLevelNodeId]
                                            ,[BreadCrumbs]
                                            ,[BreadCrumbsSplitIndexes]
                                            ,[ThreadId]
                                            ,[SectionId])
          SELECT  @Name
                  , LOWER(@Name)
                  , @Level
                  , @ParentContentLevelNodeId
                  , cln.BreadCrumbs + @Name
                  , cln.BreadCrumbsSplitIndexes + ',' + CAST(LEN(@Name) AS NVARCHAR(32))
                  , @ThreadId
                  , @SectionId
          FROM    cms_ContentLevelNodes cln
          WHERE   cln.ContentLevelNodeId = @ParentContentLevelNodeId        
        END
        
        SET @ParentContentLevelNodeId = SCOPE_IDENTITY();
        
      END
      ELSE
      BEGIN
        SET @ParentContentLevelNodeId = @ContentLevelNodeId;
      END
    
      SET @Level = @Level + 1;
    
      FETCH NEXT FROM NodesCursor INTO @Name
    END
    CLOSE NodesCursor    
    DEALLOCATE NodesCursor
  
    SET @ErrorCode = @ParentContentLevelNodeId;
  
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

  RETURN @ErrorCode

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