IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetNumberOfUsersOnline]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetNumberOfUsersOnline];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetNumberOfUsersOnline]
(
  @ApplicationId              INT
  , @MinutesSinceLastInActive INT
)
AS
BEGIN

    DECLARE @DateActive DATETIME
    SELECT  @DateActive = DATEADD(MINUTE, -(@MinutesSinceLastInActive), GETUTCDATE());

    DECLARE @NumOnline INT;
    
    SELECT  @NumOnline = COUNT(*)
    FROM      wm_Users u        (NOLOCK)
            , wm_Applications a (NOLOCK)
    WHERE     u.ApplicationId = a.ApplicationId                  
          AND u.LastActivityDateUtc > @DateActive
          AND u.ApplicationId = @ApplicationId;
            
    RETURN(@NumOnline)
    
END

GO