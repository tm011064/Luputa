IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Offices_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Offices_Update];
END
GO


CREATE PROCEDURE [dbo].[wm_Offices_Update]
(
  @OfficeId       INT
  , @Name           NVARCHAR(256)
  , @Description    NVARCHAR(512)
  , @ExtraInfo      XML
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE  [dbo].[wm_Offices] 
  SET       [Name]          = @Name
          , [Description]   = @Description
          , [ExtraInfo]     = @ExtraInfo
  WHERE   [OfficeId]  = @OfficeId
	
  RETURN @@ROWCOUNT;
  
END

GO