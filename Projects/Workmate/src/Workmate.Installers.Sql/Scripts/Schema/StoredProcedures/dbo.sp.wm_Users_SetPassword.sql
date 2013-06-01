IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_SetPassword]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_SetPassword];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_SetPassword]
(
  @UserId           INT
  , @NewPassword    NVARCHAR(128)
  , @PasswordSalt   NVARCHAR(128)
  , @PasswordFormat TINYINT
)
AS
BEGIN

  UPDATE  wm_Users
  SET       [Password] = @NewPassword
          , [PasswordSalt] = @PasswordSalt
          , [PasswordFormat] = @PasswordFormat  
          , [LastPasswordChangeDateUtc] = GETUTCDATE()          
  WHERE   UserId = @UserId
  
  RETURN @@ROWCOUNT;

END

GO