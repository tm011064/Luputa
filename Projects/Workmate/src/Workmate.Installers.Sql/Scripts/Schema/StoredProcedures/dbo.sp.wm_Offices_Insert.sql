IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Insert];
END
GO


CREATE PROCEDURE [dbo].[wm_Offices_Insert]
(
    @ApplicationId  INT
  , @Name           NVARCHAR(256)
  , @Description    NVARCHAR(512)
  , @ExtraInfo      XML
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[wm_Offices] 
  (
      [ApplicationId]
    , [Name]
    , [Description]
    , [ExtraInfo]	
  ) 
  VALUES
  (
      @ApplicationId
    , @Name
    , @Description
    , @ExtraInfo
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO