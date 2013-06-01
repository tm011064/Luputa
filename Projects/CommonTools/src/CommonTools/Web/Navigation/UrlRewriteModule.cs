using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// Module for rewriting urls
    /// </summary>
    public class UrlRewriteModule : System.Web.IHttpModule
    {
        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose() { }

        private SiteMapUrls _SiteMapUrls = null;

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            _SiteMapUrls = SiteMapUrls.Instance();
            context.BeginRequest += new System.EventHandler(Rewrite_BeginRequest);
        }

        #region rewrite request
        /// <summary>
        /// Handles the BeginRequest event of the Rewrite control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Rewrite_BeginRequest(object sender, System.EventArgs args)
        {
            string rewrittenPath = _SiteMapUrls.GetMatchingRewrite(HttpContext.Current.Request.Url.AbsolutePath);
            if (!string.IsNullOrEmpty(rewrittenPath))
                HttpContext.Current.RewritePath(rewrittenPath, false);
        }
        #endregion
    }
}