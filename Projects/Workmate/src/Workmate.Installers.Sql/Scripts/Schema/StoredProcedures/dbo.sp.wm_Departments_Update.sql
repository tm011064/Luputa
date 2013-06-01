IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Update];
END
GO


CREATE PROCEDURE [dbo].[wm_Departments_Update]
(
  @DepartmentId         INT
  , @ParentDepartmentId INT
  , @Name               NVARCHAR(256)
  , @OfficeId           INT
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE  [dbo].[wm_Departments] 
  SET       [ParentDepartmentId]  = @ParentDepartmentId
          , [Name]                = @Name
          , [OfficeId]            = @OfficeId
  WHERE   [DepartmentId]  = @DepartmentId
	
  RETURN @@ROWCOUNT;
  
END

GO