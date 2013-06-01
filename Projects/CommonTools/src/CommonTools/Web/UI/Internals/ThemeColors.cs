using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

namespace CommonTools.Web.UI
{

    internal static class ThemeColors
    {
        public static string GetDefaultCellBgColor(JobViewTheme theme)
        {
            switch (theme)
            {
                case JobViewTheme.Default:
                    return "#FFFFFF";

                default:
                    return "#FFFFFF";
            }
        }

        public static string GetBooleanValueColor(JobViewTheme theme, bool value)
        {
            switch (theme)
            {
                case JobViewTheme.Default:
                    if (value)
                        return "#00FF00";
                    else
                        return "#FF0000";

                default:
                    return "#FFFFFF";
            }
        }

        public static string GetCacheItemPriorityColor(CacheViewTheme theme, CacheItemPriority priority)
        {
            switch (theme)
            {
                case CacheViewTheme.Default:
                    switch (priority)
                    {
                        case CacheItemPriority.NotRemovable:
                            return "#FF0000";
                        case CacheItemPriority.High:
                            return "#FF6900";
                        case CacheItemPriority.AboveNormal:
                            return "#FFB300";
                        case CacheItemPriority.Normal:
                            return "#FFD100";
                        case CacheItemPriority.BelowNormal:
                            return "#FFEA00";
                        case CacheItemPriority.Low:
                            return "#FFFC00";
                    }
                    return null;

                default:
                    return "#ffffff";
            }
        }

        public static string GetDefaultCellBgColor(CacheViewTheme theme)
        {
            switch (theme)
            {
                case CacheViewTheme.Default:
                    return "#FFFFFF";

                default:
                    return "#FFFFFF";
            }
        }

        public static string GetBooleanValueColor(CacheViewTheme theme, bool value)
        {
            switch (theme)
            {
                case CacheViewTheme.Default:
                    if (value)
                        return "#00FF00";
                    else
                        return "#FF0000";

                default:
                    return "#FFFFFF";
            }
        }
    }
}
