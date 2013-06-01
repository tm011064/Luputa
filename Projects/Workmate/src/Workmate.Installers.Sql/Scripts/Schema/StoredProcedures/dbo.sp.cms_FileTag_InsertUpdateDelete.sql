IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_FileTag_InsertUpdateDelete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_FileTag_InsertUpdateDelete];
END
GO


CREATE PROCEDURE [dbo].[cms_FileTag_InsertUpdateDelete]
(
  @FileId  INT
  , @TagXml    XML
)
AS
BEGIN
  
  CREATE TABLE #TagsFromXml
  (
    Tag NVARCHAR(32)
    , LoweredTag NVARCHAR(32)
  );
    
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

    -- we expect small data, so use xpath over openxml
    SET @ErrorCode = -571;
    INSERT INTO #TagsFromXml (Tag, LoweredTag)
    SELECT  x.Tag
            , LOWER(x.Tag)
    FROM  
    (
      SELECT t.a.value('@t','NVARCHAR(32)') AS Tag
      FROM @TagXml.nodes('//r') t(a)
    ) x
    
    SET @ErrorCode = -572;    
    INSERT INTO cms_Tags (Tag, LoweredTag)
    SELECT  tfx.Tag, tfx.LoweredTag
    FROM    #TagsFromXml tfx
      WHERE NOT EXISTS ( SELECT LoweredTag FROM cms_Tags WHERE LoweredTag = tfx.LoweredTag)
  
    SET @ErrorCode = -573;
    DELETE FROM cms_FileTag
    WHERE       FileId = @FileId
    
    SET @ErrorCode = -574;
    INSERT INTO cms_FileTag (FileId, TagId)
    SELECT      @FileId, t.TagId
    FROM        cms_Tags      t
          JOIN  #TagsFromXml  tfx ON t.LoweredTag = tfx.LoweredTag
  
  END TRY
  BEGIN CATCH
    GOTO Cleanup;
  END CATCH
  
  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
    COMMIT TRANSACTION
  END

  DROP TABLE #TagsFromXml;
  RETURN 0

Cleanup:

  IF( @TranStarted = 1 )
  BEGIN
    SET @TranStarted = 0
  	ROLLBACK TRANSACTION
  END
        
  EXEC wm_SqlErrorLog_LogLastError @ErrorCode;

  DROP TABLE #TagsFromXml;
  RETURN @ErrorCode

END


GO