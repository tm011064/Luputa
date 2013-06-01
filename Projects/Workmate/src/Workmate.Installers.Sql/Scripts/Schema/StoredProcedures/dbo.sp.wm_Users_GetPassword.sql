IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetPassword]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetPassword];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetPassword]
(
  @ApplicationId                INT
  , @UserName                   NVARCHAR(256)
  , @Email                      NVARCHAR(256)
)
AS
BEGIN

  DECLARE @ErrorCode INT;
  SET     @ErrorCode = 0;
  
  IF (@UserName IS NULL AND @Email IS NULL)
  BEGIN
    SET @ErrorCode = -1;
    RETURN @ErrorCode;  
  END

  SELECT  u.[Password]
          , u.[Status]
          , u.[UserId]
          , u.[Email]
          , u.[DateCreatedUtc]
          , u.[LastLoginDateUtc]
          , u.[PasswordFormat]
          , u.[PasswordSalt]
          , u.[ProfileImageId]
          , u.[UserName]
          , u.[TimeZoneInfoId]
  FROM    wm_Users u
  WHERE   u.ApplicationId = @ApplicationId
      AND (@UserName  IS NULL OR LOWER(@UserName) = u.[LoweredUserName])
      AND (@Email     IS NULL OR LOWER(@Email) = u.[LoweredEmail])
      
  RETURN 0;

END

GO