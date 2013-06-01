IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Groups_InsertOrUpdate]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Groups_InsertOrUpdate];
END
GO

CREATE PROCEDURE [dbo].[cms_Groups_InsertOrUpdate]
(
    @GroupId      INT
  , @Name         NVARCHAR(256)
  , @Description  NVARCHAR(1024)
  , @GroupType    TINYINT
)
AS
BEGIN

  SET NOCOUNT ON;

  BEGIN TRAN

    UPDATE  [dbo].[cms_Groups] WITH (SERIALIZABLE)
    SET       [Name]        = @Name
            , [Description] = @Description
            , [GroupType]   = @GroupType
    WHERE     [GroupId] = @GroupId;
	
    IF ( @@ROWCOUNT = 0 )
    BEGIN
      INSERT INTO [dbo].[cms_Groups] 
      (
          [Name]
        , [Description]
        , [GroupType]	
        , [GroupId]
      ) 
      VALUES
      (
          @Name
        , @Description
        , @GroupType
        , @GroupId
      ); 
    END
    
  COMMIT TRAN
  
  RETURN 0;
  
END

GO