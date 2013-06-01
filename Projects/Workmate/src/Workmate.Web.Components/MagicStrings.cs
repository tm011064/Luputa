using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Web.Components
{
  public sealed class MagicStrings
  {
    public const string DATATOKENS_ROUTENAME = "_wm_rn";
    public const string DATATOKENS_MENUINFO = "_wm_mi";
    public const string DATATOKENS_BREADCRUMB = "_wm_bc";

    public const string REQUEST_CONTEXT_DATA_KEY = "_wm_rcd";

    public const string WIKI_LANGUAGE_SECTION = "Wiki Languages";

    public const string PROFILE_IMAGE_FEMALE_FILENAME = "profileimage_female.png";
    public const string PROFILE_IMAGE_MALE_FILENAME = "profileimage_male.png";

    public const string APPLICATIONGROUP_NAME_WORKMATE = "wm";

    public const string FOLDER_IMAGES_PROFILE_NORMAL = @"profile\normal\";
    public const string FOLDER_IMAGES_PROFILE_TINY = @"profile\tiny\";

    public const string FILE_PROFILEIMAGE_PREFIX = @"pi_";

    public static string FormatRouteName(string applicationGroup, string routeName)
    {
      return applicationGroup + "_" + routeName;
    }


    public static string FormatProfileImagePath(ProfileImageSize profileImageSize, int imageId, string imageBaseUrl)
    {
      switch (profileImageSize)
      {
        case ProfileImageSize.Normal:
          return imageBaseUrl + "profile/normal/" + MagicStrings.FILE_PROFILEIMAGE_PREFIX + imageId + ".png";
        case ProfileImageSize.Tiny:
          return imageBaseUrl + "profile/tiny/" + MagicStrings.FILE_PROFILEIMAGE_PREFIX + imageId + ".png";

        default: throw new NotImplementedException();
      }
    }
  }
}
