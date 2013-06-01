IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Contents_GetAllWithSectionName]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Contents_GetAllWithSectionName];
END
GO

CREATE PROCEDURE cms_Contents_GetAllWithSectionName
(
  @ApplicationId    INT
  , @SectionType    TINYINT  
  , @ContentStatus  TINYINT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT  s.LoweredName
          , c.FormattedBody
  FROM    cms_Contents  c
    JOIN  cms_Threads   t ON c.ThreadId = t.ThreadId
    JOIN  cms_Sections  s ON t.SectionId = s.SectionId
  WHERE   s.ApplicationId = @ApplicationId
      AND s.SectionType = @SectionType
      AND c.ContentStatus = @ContentStatus

END
GO