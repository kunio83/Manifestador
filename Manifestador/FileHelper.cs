using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Manifestador
{
    public class FileHelper
    {
        public static void Copy(string pathSource, string pathTarget)
        {
            File.Copy(pathSource, pathTarget, true);
        }

        public static string GetFileName(string pathFile)
        {
            return Path.GetFileNameWithoutExtension(pathFile);
        }

        public static string GetFileRoot(string pathFile)
        {
            return Path.GetDirectoryName(pathFile);
        }

        public static void CreateDir(string pathDir)
        {
            Directory.CreateDirectory(pathDir);
        }
    }
}