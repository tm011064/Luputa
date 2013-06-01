using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools.Components.TextResources
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITextResourceManager
    {
        /// <summary>
        /// Gets the formatted resource text.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        string GetFormattedResourceText(string key, params object[] args);
        /// <summary>
        /// Gets the formatted resource text.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        string GetFormattedResourceText(string key, string culture, params object[] args);
        /// <summary>
        /// Gets the resource text.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetResourceText(string key);
        /// <summary>
        /// Gets the resource text.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        string GetResourceText(string key, string culture);
    }
}
