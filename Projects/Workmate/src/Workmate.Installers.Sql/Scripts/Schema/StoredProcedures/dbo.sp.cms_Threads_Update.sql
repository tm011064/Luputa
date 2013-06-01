IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Threads_Update]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Threads_Update];
END
GO

CREATE PROCEDURE [dbo].[cms_Threads_Update]
(
  @ThreadId       INT
  , @SectionId      INT
  , @Name           NVARCHAR(32)
  , @LastViewedDateUtc DATETIME
  , @StickyDateUtc  DATETIME
  , @IsLocked       BIT
  , @IsSticky       BIT
  , @IsApproved     BIT
  , @ThreadStatus   INT
)
AS
BEGIN

  SET NOCOUNT ON;

  UPDATE    [dbo].[cms_Threads] 
  SET       [SectionId]       = @SectionId
          , [Name]            = @Name
          , [LoweredName]     = LOWER(@Name)
          , [LastViewedDateUtc]  = @LastViewedDateUtc
          , [StickyDateUtc]      = @StickyDateUtc
          , [IsLocked]        = @IsLocked
          , [IsSticky]        = @IsSticky
          , [IsApproved]      = @IsApproved
          , [ThreadStatus]    = @ThreadStatus
  WHERE     [ThreadId]  = @ThreadId
	
  RETURN @@ROWCOUNT;
  
END

GO