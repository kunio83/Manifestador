using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;

namespace Manifestador
{
    public class ZipperHelper
    {
        private string _tempPath = Environment.GetEnvironmentVariable("TEMP");

        private string _pathZip = string.Empty;
        private const string _manifestName = "imsmanifest.xml";
        private string _manifestPath = string.Empty;
        private Process _winrar;
        public ZipperHelper(String pathZip)
        {
            _pathZip = pathZip;
            _winrar = new Process();
        }

        public string GetManifest()
        {
            //descomprimo el manifest en temp
            using (ZipArchive archive = ZipFile.OpenRead(_pathZip))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name == _manifestName)
                    {
                        _manifestPath = Path.Combine(_tempPath, entry.FullName);
                        entry.ExtractToFile(_manifestPath,true);
                        break;
                    }
                }
                return _manifestPath;
            }
        }

        public void AddFile(string pathZip, string pathFile)
        {
            using (ZipArchive zip = ZipFile.Open(pathZip, ZipArchiveMode.Update))
            {
                zip.GetEntry(_manifestName).Delete();
                zip.CreateEntryFromFile(pathFile, _manifestName);
            }
        }

        public void ReCompress(string pathFile)
        {
            string fileName = FileHelper.GetFileName(pathFile);
            string dirTarget = _tempPath + "\\" + fileName;

            //si no existe la carpeta en temp, la creo
            if (!Directory.Exists(dirTarget))
                Directory.CreateDirectory(dirTarget);

            //extraigo todos los archivos en la carpeta en temp
            using (ZipArchive zip = ZipFile.Open(pathFile, ZipArchiveMode.Read))
            {
                zip.ExtractToDirectory(dirTarget);
            }

            //Borro el archivo origen
            if (File.Exists(pathFile))
                File.Delete(pathFile);

            //Comprimo todo lo extraido y lo guardo en el mismo arhicvo de origen usando la dll de windows
            using (ZipArchive zip = ZipFile.Open(pathFile,ZipArchiveMode.Create))
            {
                //zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                //zip.AddFiles(Directory.GetFiles(dirTarget), "");
                foreach (string s in Directory.GetFiles(dirTarget))
                {
                    zip.CreateEntryFromFile(s, Path.GetFileName(s));
                }
            }

            //Process winrar = new Process();
            //winrar.StartInfo.FileName = @"Winrar\winrar.exe";
            //winrar.StartInfo.Arguments = String.Format("a -afzip -r -ep -u \"{0}\" \"{1}\\*.*\"", pathFile, dirTarget);
            //winrar.Start();
            //winrar.WaitForExit();

            //borro la carpeta en temp
            if (Directory.Exists(dirTarget))
                Directory.Delete(dirTarget,true);
        }
    }
}
