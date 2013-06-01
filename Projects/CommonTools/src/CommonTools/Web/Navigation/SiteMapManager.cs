using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This class provides utility methods that can be used with sitemap objects.
    /// </summary>
    public static class SiteMapManager
    {
        /// <summary>
        /// Resolves the rewrite item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// A resolved url path, string.Empty if an error occurred
        /// </returns>
        public static string ResolveRewriteItem(IUrlRewriteItem item)
        {
            return ResolveItem(null, null, null, item);
        }
        /// <summary>
        /// Resolves the rewrite item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="args">The args to format the resulting string with.</param>
        /// <returns>
        /// A resolved url path, string.Empty if an error occurred
        /// </returns>
        public static string ResolveRewriteItem(IUrlRewriteItem item, params object[] args)
        {
            return ResolveItem(null, null, null, item, args);
        }


        /// <summary>
        /// Resolves the rewrite item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="host">The host (www.mysite.com).</param>
        /// <param name="context">The current context.</param>
        /// <param name="control">The sending control control.</param>
        /// <returns>
        /// A resolved url path, string.Empty if an error occurred
        /// </returns>
        public static string ResolveRewriteItem(HttpContext context, Control control, string host, IUrlRewriteItem item)
        {
            return ResolveItem(host, context, control, item);
        }
        /// <summary>
        /// Resolves the rewrite item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="context">The current context.</param>
        /// <param name="control">The sending control control.</param>
        /// <returns>
        /// A resolved url path, string.Empty if an error occurred
        /// </returns>
        public static string ResolveRewriteItem(HttpContext context, Control control, IUrlRewriteItem item)
        {
            return ResolveItem(null, context, control, item);
        }
        /// <summary>
        /// Resolves the rewrite item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="context">The current context.</param>
        /// <param name="control">The sending control control.</param>
        /// <param name="args">The args to format the resulting string with.</param>
        /// <returns>
        /// A resolved url path, string.Empty if an error occurred
        /// </returns>
        public static string ResolveRewriteItem(HttpContext context, Control control, IUrlRewriteItem item, params object[] args)
        {
            return ResolveItem(null, context, control, item, args);
        }


        /// <summary>
        /// Resolves the rewrite item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="host">The host (www.mysite.com).</param>
        /// <param name="context">The current context.</param>
        /// <param name="control">The sending control control.</param>
        /// <param name="args">The args to format the resulting string with.</param>
        /// <returns>
        /// A resolved url path, string.Empty if an error occurred
        /// </returns>
        public static string ResolveRewriteItem(HttpContext context, Control control, string host, IUrlRewriteItem item, params object[] args)
        {
            return ResolveItem(host, context, control, item, args);
        }

        /// <summary>
        /// Resolves the rewrite item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="host">The host (www.mysite.com).</param>
        /// <param name="context">The current context.</param>
        /// <param name="control">The sending control control.</param>
        /// <param name="args">The args to format the resulting string with.</param>
        /// <returns>
        /// A resolved url path, string.Empty if an error occurred
        /// </returns>
        private static string ResolveItem(string host, HttpContext context, Control control, IUrlRewriteItem item, params object[] args)
        {
            string resolvedPath = string.Empty;

            if (context == null)
                context = HttpContext.Current;

            if (context != null)
            {
                if (control == null)
                    control = context.CurrentHandler as Control;

                if (control != null)
                {
                    if (item.IsHttps)
                    {
                        resolvedPath = string.Format("https://{0}{1}", string.IsNullOrEmpty(host) ? context.Request.Url.Host : host, control.ResolveUrl(item.FullVirtualPath));
                    }
                    else
                    {
                        resolvedPath = control.ResolveUrl(item.FullVirtualPath);
                    }

                    if (args != null && args.Length > 0)
                        resolvedPath = string.Format(resolvedPath, args);
                }
            }

            return resolvedPath;
        }

    }
}
