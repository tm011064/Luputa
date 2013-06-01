IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[wm_Users_GetUserModel]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[wm_Users_GetUserModel];
END
GO

CREATE PROCEDURE [dbo].[wm_Users_GetUserModel]
(
  @UserId INT
)
AS
BEGIN

  SELECT  u.UserId
          , u.Email
          , u.Status
          , u.DateCreatedUtc
          , u.UserName
          
          , u.LastActivityDateUtc
          , u.LastLoginDateUtc
          , u.LastPasswordChangeDateUtc
          , u.LastLockoutDateUtc
          
          , u.FailedPasswordAttemptCount
          , u.UniqueId
          , u.UserNameDisplayMode
          
          , u.ExtraInfo
          , u.ProfileImageId
          , u.TimeZoneInfoId
          , u.FirstName
          , u.LastName
          
          , u.Gender
  FROM    wm_Users u
  WHERE   u.UserId = @UserId

END

GO