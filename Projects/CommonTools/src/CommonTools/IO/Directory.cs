using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CommonTools.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class Directory
    {
        /// <summary>
        /// Gets a list of filenames (with absolute path) in a directory
        /// </summary>
        /// <param name="path">The absolute path of the directory to search files for</param>
        /// <param name="fileExtensions">Allowed file extensions. Must be in the following format: flv|avi|mpg|xml</param>
        /// <param name="option">formating option</param>
        /// <returns></returns>
        public static List<string> GetFilenames(string path, string fileExtensions, FilenameFormatOptions option)
        {
            List<string> returnFiles = new List<string>();
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists)
            {
                string[] filenames = System.IO.Directory.GetFiles(path);
                Regex r = new Regex(@"[\w]*.(" + fileExtensions + ")$", RegexOptions.IgnoreCase);

                foreach (string file in filenames)
                {
                    if (r.IsMatch(file))
                    {
                        switch (option)
                        {
                            case FilenameFormatOptions.FilenameOnly:
                                returnFiles.Add(file.Remove(0, file.LastIndexOf(@"\") + 1));
                                break;

                            case FilenameFormatOptions.FilenameWithoutExtension:
                                returnFiles.Add(file.Remove(file.LastIndexOf(".") + 1).Remove(0, file.LastIndexOf(@"\") + 1));
                                break;

                            case FilenameFormatOptions.FullPath:
                                returnFiles.Add(file);
                                break;
                        }
                    }
                }
            }

            return returnFiles;
        }
    }
}
