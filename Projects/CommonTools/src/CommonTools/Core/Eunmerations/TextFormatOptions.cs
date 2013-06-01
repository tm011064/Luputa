using System;

namespace CommonTools
{
    /// <summary>
    /// Options to format a string with
    /// </summary>
    public enum TextFormatOptions : byte
    {
        /// <summary>
        /// Option to remove all line breaks.
        /// </summary>
        RemoveLineBreaks,
        /// <summary>
        /// Option to remove all javascript script tags.
        /// </summary>
        RemoveScriptTags,
        /// <summary>
        /// Option to remove characters that can be used for SQL injections.
        /// </summary>
        RemoveBadSQLCharacters,
        /// <summary>
        /// Option to remove the following tags:
        /// abbr|acronym|address|applet|area|base|basefont|bdo|big|body|button|
        /// caption|center|cite|code|col|colgroup|dd|del|dir|div|dfn|dl|dt|embed|
        /// fieldset|font|form|frame|frameset|head|html|iframe|img|input|ins|
        /// isindex|kbd|label|legend|link|map|menu|meta|noframes|noscript|object|
        /// optgroup|option|param|pre|q|s|samp|script|select|small|span|strike|
        /// style|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|var|xmp
        /// </summary>
        RemoveBadTags,
        /// <summary>
        /// Option to remove all html/xml tags from a specified string.
        /// </summary>
        RemoveAllTags,
        /// <summary>
        /// Option to transform to a querystring-safe representation.
        /// </summary>
        SafeQuerystringParameter,
        /// <summary>
        /// This method secures script tags by replacing the html tag brackets.
        /// </summary>
        DefuseScriptTags,
    }
}
