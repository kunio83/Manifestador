using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Manifestador
{
    class Program
    {
        private const string _pathTextRoot = "//*[local-name()='organizations']/*[local-name()='organization']";


        static void Main(string[] args)
        {
            args = new string[] { @"C:\Users\Kunio\Documents\visual studio 2017\Projects\Manifestador\Manifestador\bin\Debug\Comercialización de productos y servicios en pymes.zip" };  //  @"C:\Users\Kunio\Documents\visual studio 2017\Projects\Manifestador\Manifestador\bin\Debug"

            //comentando
            if (args.Length > 0)// && args[0].Contains(".zip"))
            {
                string pathZip;// = args[0];
                string newDir;
                string pathRoot;// = FileHelper.GetFileRoot(pathZip);
                string fileName;// = FileHelper.GetFileName(pathZip);
                string pathCopy;
                string pathNewFile;
                string newName;
                ZipperHelper zipper;// = new ZipperHelper(pathZip);

                foreach (string p in args)
                {


                    pathZip = p;
                    pathRoot = FileHelper.GetFileRoot(pathZip);
                    fileName = FileHelper.GetFileName(pathZip);
                    newDir = pathRoot + "\\" + fileName;
                    pathCopy = newDir + "\\" + fileName + ".zip";
                    

                    //Creo el directorio pertinente
                    FileHelper.CreateDir(newDir);
                    FileHelper.Copy(pathZip, pathCopy);

                    zipper = new ZipperHelper(pathCopy);

                    //Obtengo el Manifest
                    string pathManifet = zipper.GetManifest();
                    XmlHelper xmlHelper = new XmlHelper(pathManifet, _pathTextRoot);

                    //Obtengo los Nodos
                    XmlNodeList nodos = xmlHelper.GetNodesByName("item");

                    //Vaciolo antes de empezar
                    xmlHelper.DeleteNodes(nodos);

                    for (int i = 0; i < nodos.Count; i++)
                    {
                        //hago la copia
                        newName = fileName + " U" + (i + 1).ToString() + ".zip";
                        pathNewFile = newDir + "\\" + newName;
                        FileHelper.Copy(pathCopy, pathNewFile);

                        //Hago el manifest
                        xmlHelper.AddNode(nodos[i]);

                        //Reemplazo el manifest en la copia
                        zipper.AddFile(pathNewFile, pathManifet);

                        //Recomprimo por las dudas
                        zipper.ReCompress(pathNewFile);

                        //vacio el manifest
                        xmlHelper.DeleteNode(nodos[i]);
                    }
                }
            }
        }
    }
}
