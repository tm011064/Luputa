IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_Update];
END
GO


CREATE PROCEDURE [dbo].[cms_Contents_Update]
(
  @ContentId        INT
  , @ThreadId         INT
  , @ParentContentId  INT
  , @AuthorUserId     INT
  , @ContentLevel     SMALLINT
  , @Subject          NVARCHAR(256)
  , @FormattedBody    NVARCHAR(MAX)
  , @IsApproved       BIT
  , @IsLocked         BIT
  , @ContentType      TINYINT
  , @ContentStatus    TINYINT
  , @ExtraInfo        XML
  , @BaseContentId    INT
  , @UrlFriendlyName  NVARCHAR(256)
  , @TagXml           XML
)
AS
BEGIN
  
  IF (      @UrlFriendlyName IS NOT NULL
       AND  EXISTS (  SELECT  c.* 
                      FROM    cms_Contents  c
                        JOIN  cms_Threads   t ON c.ThreadId = @ThreadId
                      WHERE   LoweredUrlFriendlyName = LOWER(@UrlFriendlyName)
                          AND c.ContentId <> @ContentId ))
  BEGIN
    RETURN -1003; -- name not unique
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
  
    SET @ErrorCode = -321;    
    UPDATE  [dbo].[cms_Contents] 
    SET       [ThreadId]        = @ThreadId
            , [ParentContentId] = @ParentContentId
            , [AuthorUserId]    = @AuthorUserId
            , [ContentLevel]    = @ContentLevel
            , [Subject]         = @Subject
            , [FormattedBody]   = @FormattedBody
            , [IsApproved]      = @IsApproved
            , [IsLocked]        = @IsLocked
            , [ContentType]     = @ContentType
            , [ContentStatus]   = @ContentStatus
            , [ExtraInfo]       = @ExtraInfo
            , [BaseContentId]   = @BaseContentId
    WHERE     [ContentId] = @ContentId
      
    EXEC @ErrorCode = cms_ContentTag_InsertUpdateDelete @ContentId, @TagXml
    IF (@ErrorCode <> 0)
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