IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cms_Files_GetProfileImage]') AND type in (N'P', N'PC'))
BEGIN
  DROP PROCEDURE [dbo].[cms_Files_GetProfileImage];
END
GO



CREATE PROCEDURE [dbo].[cms_Files_GetProfileImage]
(
  @UserId     INT
)
AS
BEGIN

  SET NOCOUNT ON;

  SELECT    f.[FileId]
          , f.[UserId]
          , f.[FileType]
          , f.[DateCreatedUtc]
          , f.[FileName]
          , f.[Content]
          , f.[ContentType]
          , f.[ContentSize]
          , f.[FriendlyFileName]
          , f.[Height]
          , f.[Width]
  FROM    [dbo].[cms_Files] f
    JOIN  [dbo].[wm_Users]  u ON f.FileId = u.ProfileImageId
  WHERE   u.UserId = @UserId
  
END

GO