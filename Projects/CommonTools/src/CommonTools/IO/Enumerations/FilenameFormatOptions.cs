using System;

namespace CommonTools.IO
{
    /// <summary>
    /// 
    /// </summary>
    public enum FilenameFormatOptions : byte
    {
        /// <summary>
        /// Returns files in full path format ( c:\mypath\in\folder\myfile.txt )
        /// </summary>
        FullPath = 0,
        /// <summary>
        /// Returns filenames only ( c:\mypath\in\folder\myfile.txt -> myfile.txt )
        /// </summary>
        FilenameOnly = 1,
        /// <summary>
        /// Returns filenames without extensions ( c:\mypath\in\folder\myfile.txt -> myfile )
        /// </summary>
        FilenameWithoutExtension = 2,
    }
}
