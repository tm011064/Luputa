using System;
using System.Web.Caching;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This interface enforces all properties needed for 
    /// </summary>
    public interface IUrlRewriteItem
    {
        /// <summary>
        /// Gets the pattern.
        /// </summary>
        /// <value>The pattern.</value>
        string Pattern { get; set; }
        /// <summary>
        /// Gets the vanity.
        /// </summary>
        /// <value>The vanity.</value>
        string Vanity { get; set; }
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        string Path { get; set; }
        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        string Url { get; set; }
        /// <summary>
        /// Gets the parent name used for breadcrumb navigation.
        /// </summary>
        /// <value>The URL.</value>
        string ParentName { get; set; }
        /// <summary>
        /// Gets the full virtual path.
        /// </summary>
        /// <value>The full virtual path.</value>
        string FullVirtualPath { get; set; }
        /// <summary>
        /// Gets the breadcrumb title.
        /// </summary>
        /// <value>The breadcrumb title.</value>
        string BreadcrumbTitle { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is HTTPS.
        /// </summary>
        /// <value><c>true</c> if this instance is HTTPS; otherwise, <c>false</c>.</value>
        bool IsHttps { get; set; }
        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }
    }
}
