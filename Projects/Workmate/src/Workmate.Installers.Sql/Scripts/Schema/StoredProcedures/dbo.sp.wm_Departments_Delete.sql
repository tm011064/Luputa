IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Delete]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Delete];
END
GO


CREATE PROCEDURE [dbo].[wm_Departments_Delete]
(
    @DepartmentId INT
)
AS
BEGIN

  DELETE FROM [dbo].[wm_Departments] 
  WHERE      [DepartmentId]  = @DepartmentId
	
  RETURN @@ROWCOUNT;
  
END

GO