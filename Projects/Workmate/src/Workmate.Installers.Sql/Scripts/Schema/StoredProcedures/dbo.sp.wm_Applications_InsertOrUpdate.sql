IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Applications_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Applications_InsertOrUpdate];
END
GO

CREATE PROCEDURE [dbo].[wm_Applications_InsertOrUpdate]
(
    @ApplicationName        NVARCHAR(256)
  , @Description            NVARCHAR(512)
  , @ExtraInfo              XML
)
AS
BEGIN

  SET NOCOUNT ON;
  
  DECLARE @ApplicationId INT;

  BEGIN TRAN

    UPDATE    [dbo].[wm_Applications] WITH (SERIALIZABLE)
    SET       [Description] = @Description
              , [ExtraInfo] = @ExtraInfo
    WHERE     [LoweredApplicationName] = LOWER(@ApplicationName);
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      INSERT INTO [dbo].[wm_Applications] 
      (
          [ApplicationName]
        , [LoweredApplicationName]
        , [Description]	
        , [ExtraInfo]	
      ) 
      VALUES
      (
          @ApplicationName
        , LOWER(@ApplicationName)
        , @Description
        , @ExtraInfo
      );
      
      SET @ApplicationId = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
      SELECT    @ApplicationId = ApplicationId
      FROM      [dbo].[wm_Applications]
      WHERE     [LoweredApplicationName] = LOWER(@ApplicationName);
    END
    
  COMMIT TRAN
  
  RETURN @ApplicationId;
    
END

GO