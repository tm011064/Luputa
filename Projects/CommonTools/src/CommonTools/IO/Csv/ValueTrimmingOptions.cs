using System;

namespace CommonTools.IO.Csv
{
    /// <summary>
    /// 
    /// </summary>
	[Flags]
	public enum ValueTrimmingOptions
	{
        /// <summary>
        /// 
        /// </summary>
		None = 0,
        /// <summary>
        /// 
        /// </summary>
		UnquotedOnly = 1,
        /// <summary>
        /// 
        /// </summary>
		QuotedOnly = 2,
        /// <summary>
        /// 
        /// </summary>
		All = UnquotedOnly | QuotedOnly
	}
}