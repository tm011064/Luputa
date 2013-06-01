using System;
using System.Collections.Generic;

namespace CommonTools.Web.Navigation
{
    /// <summary>
    /// This interface enforces all properties needed for 
    /// </summary>
    public interface ISiteMapMenuItem
    {
        /// <summary>
        /// Gets the name of the rewrite item.
        /// </summary>
        /// <value>The name of the rewrite item.</value>
        string UrlRewriteItemName { get; set; }
        /// <summary>
        /// Gets the name of the <see cref="IUrlRewriteItem"/>.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
        /// <summary>
        /// Gets the breadcrumb title.
        /// </summary>
        /// <value>The breadcrumb title.</value>
        string BreadcrumbTitle { get; set; }
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }
        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>The child nodes.</value>
        List<ISiteMapMenuItem> ChildNodes { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is the root node.
        /// </summary>
        /// <value><c>true</c> if this instance is root; otherwise, <c>false</c>.</value>
        bool IsRoot { get; set; }
        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>The parent node.</value>
        ISiteMapMenuItem ParentNode { get; set; }
        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>The parent node.</value>
        IUrlRewriteItem UrlRewriteItem { get; set; }
    }
}
