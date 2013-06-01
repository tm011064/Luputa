IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Insert]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Insert];
END
GO

CREATE PROCEDURE [dbo].[cms_Sections_Insert]
(
    @ApplicationId    INT
  , @ParentSectionId  INT
  , @GroupId          INT
  , @Name             NVARCHAR(128)
  , @Description      NVARCHAR(1024)
  , @SectionType      TINYINT
  , @IsActive         BIT
  , @IsModerated      BIT
)
AS
BEGIN

  SET NOCOUNT ON;

  INSERT INTO [dbo].[cms_Sections] 
  (
      [ApplicationId]
    , [ParentSectionId]
    , [GroupId]
    , [Name]
    , [LoweredName]
    , [Description]
    , [SectionType]
    , [IsActive]
    , [IsModerated]
    , [TotalContents]
    , [TotalThreads]	
  ) 
  VALUES
  (
      @ApplicationId
    , @ParentSectionId
    , @GroupId
    , @Name
    , LOWER(@Name)
    , @Description
    , @SectionType
    , @IsActive
    , @IsModerated
    , 0
    , 0
  );
    
  RETURN SCOPE_IDENTITY();
  
END

GO