IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Departments_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Departments_Get];
END
GO

CREATE PROCEDURE [dbo].[wm_Departments_Get]
(
  @DepartmentId     INT
  , @ApplicationId  INT
  , @OfficeId       INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [DepartmentId]
          , [ParentDepartmentId]
          , [Name]
          , [OfficeId]
  FROM    [dbo].[wm_Departments]
  WHERE   ([DepartmentId] IS NULL OR [DepartmentId] = @DepartmentId)
      AND ([ApplicationId] IS NULL OR [ApplicationId] = @ApplicationId)
      AND ([OfficeId] IS NULL OR [OfficeId] = @OfficeId)
  
END

GO