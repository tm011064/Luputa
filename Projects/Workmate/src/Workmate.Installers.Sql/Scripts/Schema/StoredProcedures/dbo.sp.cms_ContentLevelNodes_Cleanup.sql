IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_ContentLevelNodes_Cleanup]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_ContentLevelNodes_Cleanup];
END
GO


CREATE PROCEDURE [dbo].[cms_ContentLevelNodes_Cleanup]
(
    @ThreadId     INT = NULL
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
    
    -- now, clean up duplicates in case the name already existed. This is done via merging the nodes
    DECLARE @ContentLevelNodesToRemove TABLE 
    (
      ContentLevelNodeId          INT
    );
    DECLARE @Duplicates TABLE 
    (
      ContentLevelNodeId          INT
      , MasterContentLevelNodeId  INT
    );

    DECLARE @MaxLevel INT;
    DECLARE @CurrentLevel INT;

    SET     @CurrentLevel = 0;
    SELECT  @MaxLevel = MAX([Level])
    FROM    cms_ContentLevelNodes

    IF ( @MaxLevel IS NOT NULL )
    BEGIN

      WHILE ( @CurrentLevel <= @MaxLevel )
      BEGIN
       
        -- have a clean table
        DELETE FROM @Duplicates;
       
        INSERT INTO @Duplicates (ContentLevelNodeId, MasterContentLevelNodeId)
        SELECT  nested.ContentLevelNodeId, nested.MasterContentLevelNodeId
        FROM
        (
          SELECT  cln.ContentLevelNodeId
                  , ( SELECT TOP 1 cln2.ContentLevelNodeId 
                      FROM    cms_ContentLevelNodes cln2
                      WHERE   cln2.LoweredName = dup.LoweredName 
                          AND cln2.ParentContentLevelNodeId = dup.ParentContentLevelNodeId ) AS MasterContentLevelNodeId
          FROM
          (
          
            SELECT    LoweredName
                      , ParentContentLevelNodeId
            FROM      cms_ContentLevelNodes
            WHERE     [Level] = @CurrentLevel
                  AND ((@ThreadId IS NULL AND ThreadId IS NULL) OR (ThreadId = @ThreadId))
                  AND ((@SectionId IS NULL AND SectionId IS NULL) OR (SectionId = @SectionId))
            GROUP BY  LoweredName, ParentContentLevelNodeId
            HAVING COUNT(LoweredName) > 1
            
          ) AS dup
            JOIN  cms_ContentLevelNodes cln ON      cln.LoweredName = dup.LoweredName 
                                                AND cln.ParentContentLevelNodeId = dup.ParentContentLevelNodeId
        ) AS nested
        WHERE nested.ContentLevelNodeId <> nested.MasterContentLevelNodeId
          
        SET @ErrorCode = -371;
        -- update the content records which reference the duplicated node       
        UPDATE  c
        SET     c.ContentLevelNodeId = dup.MasterContentLevelNodeId
        FROM    cms_Contents c
          JOIN  @Duplicates  dup  ON c.ContentLevelNodeId = dup.ContentLevelNodeId
          
        SET @ErrorCode = -372;
        -- now, update all children of the duplicated node to reference the new parent node
        UPDATE  cln
        SET     cln.ParentContentLevelNodeId = dup.MasterContentLevelNodeId
        FROM    cms_ContentLevelNodes cln
          JOIN  @Duplicates           dup ON cln.ParentContentLevelNodeId = dup.ContentLevelNodeId

        SET @ErrorCode = -373;
        -- remember which duplicates need to be removed
        INSERT INTO @ContentLevelNodesToRemove
        SELECT      ContentLevelNodeId
        FROM        @Duplicates
        
        SET @CurrentLevel = @CurrentLevel + 1;
        
      END

    END

    SET @ErrorCode = -374;
    -- finally, remove all duplicates
    DELETE cln
    FROM    cms_ContentLevelNodes       cln
      JOIN  @ContentLevelNodesToRemove  dup ON cln.ContentLevelNodeId = dup.ContentLevelNodeId
  
    SET @CurrentLevel = 0;
    WHILE ( @CurrentLevel <= @MaxLevel )
    BEGIN -- update breadcrumbs

      SET @ErrorCode = -375;
      UPDATE  cln
      SET     cln.BreadCrumbs = ISNULL ( p.BreadCrumbs, '' ) + cln.Name
              , cln.BreadCrumbsSplitIndexes = ISNULL ( p.BreadCrumbsSplitIndexes, '' ) 
                                              + CASE 
                                                  WHEN @CurrentLevel = 0 THEN ''
                                                  ELSE ','
                                                END
                                              + CAST(LEN(cln.Name) AS NVARCHAR(32))
      FROM    cms_ContentLevelNodes cln
        LEFT  JOIN  cms_ContentLevelNodes p   ON cln.ParentContentLevelNodeId = p.ContentLevelNodeId
      WHERE   cln.[Level] = @CurrentLevel     
          AND ((@ThreadId IS NULL AND cln.ThreadId IS NULL) OR (cln.ThreadId = @ThreadId))
          AND ((@SectionId IS NULL AND cln.SectionId IS NULL) OR (cln.SectionId = @SectionId))
      
      SET @CurrentLevel = @CurrentLevel + 1;
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