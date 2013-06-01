IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_UnlockUser]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_UnlockUser];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_UnlockUser]
(
  @UserId INT
)
AS
BEGIN
    
  UPDATE  wm_Users
  SET     [Status] = (
                        SELECT  CASE 
                                  WHEN u.[Status] = 6 THEN 1  -- was Locked, so Valid
                                  WHEN u.[Status] = 7 THEN 5  -- was LockedAwaitingEmailVerification, so AwaitingEmailVerification
                                  ELSE 1
                                END
                        FROM    wm_Users u
                        WHERE   u.UserId = @UserId
                      )
          , LastLockoutDateUtc = CONVERT( DATETIME, '17540101', 112 )
          , FailedPasswordAttemptCount = 0
  WHERE   UserId = @UserId

  RETURN @@ROWCOUNT;

END

GO