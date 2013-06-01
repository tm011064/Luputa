IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Insert];
END
GO


CREATE PROCEDURE [dbo].[wm_Departments_Insert]
(
    @ApplicationId      INT
  , @ParentDepartmentId INT
  , @Name               NVARCHAR(256)
  , @OfficeId           INT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[wm_Departments] 
  (
      [ApplicationId]
    , [ParentDepartmentId]
    , [Name]
    , [OfficeId]
  ) 
  VALUES
  (
      @ApplicationId
    , @ParentDepartmentId
    , @Name
    , @OfficeId
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO