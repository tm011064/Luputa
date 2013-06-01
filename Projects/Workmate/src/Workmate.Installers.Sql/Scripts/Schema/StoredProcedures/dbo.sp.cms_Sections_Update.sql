IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Update];
END
GO

CREATE PROCEDURE [dbo].[cms_Sections_Update]
(
    @SectionId        INT
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

  UPDATE  [dbo].[cms_Sections] 
  SET       [ParentSectionId] = @ParentSectionId
          , [GroupId]         = @GroupId
          , [Name]            = @Name
          , [LoweredName]     = LOWER(@Name)
          , [Description]     = @Description
          , [SectionType]     = @SectionType
          , [IsActive]        = @IsActive
          , [IsModerated]     = @IsModerated
  WHERE     [SectionId] = @SectionId
	
  RETURN @@ROWCOUNT;
  
END

GO