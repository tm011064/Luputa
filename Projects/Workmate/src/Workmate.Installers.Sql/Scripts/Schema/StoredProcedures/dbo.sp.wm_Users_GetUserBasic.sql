IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetUserBasic]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetUserBasic];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetUserBasic]
(
  @UniqueId             UNIQUEIDENTIFIER = NULL
  , @Email              NVARCHAR(256) = NULL
  , @UserName           NVARCHAR(256) = NULL
  , @UserId             INT = NULL
  , @ApplicationId      INT
  , @UpdateLastActivity BIT
)
AS
BEGIN

  IF (    @UpdateLastActivity = 1
      AND (    @UniqueId IS NOT NULL  
            OR @UserId IS NOT NULL 
            OR @Email IS NOT NULL 
            OR @UserName IS NOT NULL ))
  BEGIN
  
    UPDATE  wm_Users
    SET     LastActivityDateUtc = GETUTCDATE()
    WHERE   (@UniqueId  IS NULL OR UniqueId = @UniqueId)
        AND (@UserId    IS NULL OR UserId = @UserId)
        AND (@Email     IS NULL OR (ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email)))
        AND (@UserName  IS NULL OR (ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)))
    
  END

  SELECT  u.UserId
          , u.Email
          , u.Status
          , u.DateCreatedUtc
          , u.UserName
          , u.LastActivityDateUtc
          , u.LastLoginDateUtc
          , u.ProfileImageId
          , u.TimeZoneInfoId
  FROM    wm_Users u
  WHERE   (@UniqueId  IS NULL OR u.UniqueId = @UniqueId)
      AND (@UserId    IS NULL OR u.UserId = @UserId)
      AND (@Email     IS NULL OR (u.ApplicationId = @ApplicationId AND u.LoweredEmail = LOWER(@Email)))
      AND (@UserName  IS NULL OR (u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)))
      OR  (     @UniqueId IS NULL  
            AND @UserId IS NULL 
            AND @Email IS NULL 
            AND @UserName IS NULL
            AND u.ApplicationId = @ApplicationId) -- get all per application...

END

GO