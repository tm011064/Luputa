IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Sections_Get]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Sections_Get];
END
GO

CREATE PROCEDURE [dbo].[cms_Sections_Get]
(
  @SectionId        INT           = NULL
  , @SectionType    TINYINT       = NULL
  , @Name           NVARCHAR(128) = NULL 
  , @ApplicationId  INT           = NULL  
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    [ApplicationId]
          , [SectionId]
          , [ParentSectionId]
          , [GroupId]
          , [Name]
          , [Description]
          , [SectionType]
          , [IsActive]
          , [IsModerated]
          , [TotalContents]
          , [TotalThreads]
  FROM    [dbo].[cms_Sections]
  WHERE   (@SectionId     IS NULL OR SectionId = @SectionId)
      AND (@SectionType   IS NULL OR SectionType = @SectionType)
      AND (@ApplicationId IS NULL OR ApplicationId = @ApplicationId)
      AND (@Name          IS NULL OR LoweredName = LOWER(@Name))
  
END

GO