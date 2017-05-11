using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Manifestador
{

    public class XmlHelper
    {
        private XmlNode _root;
        private XmlDocument _xml;
        private XmlNamespaceManager _mgr;

        public XmlHelper(string pathXML, string pathTextRoot)
        {
            _xml = new XmlDocument();
            _xml.Load(pathXML);
            _mgr = new XmlNamespaceManager(_xml.NameTable);
            _mgr.AddNamespace("", "http://schemas.microsoft.com/appx/2010/manifest");
            _root = _xml.DocumentElement.SelectSingleNode(pathTextRoot);
        }

        public XmlNodeList GetNodesByName(string nodeName)
        {
            return _root.SelectNodes("//*[local-name()='organizations']/*[local-name()='organization']/*[local-name()='"+ nodeName + "']");
        }

        public void DeleteNodes(XmlNodeList nodos)
        {
            for (int i = 0; i < nodos.Count; i++)
            {
                _root.RemoveChild(nodos[i]);
            }
            _xml.Save((new Uri(_xml.BaseURI).LocalPath));
        }

        public void DeleteNode(XmlNode nodo)
        {
            _root.RemoveChild(nodo);
            _xml.Save((new Uri(_xml.BaseURI).LocalPath));
        }

        public void AddNode(XmlNode nodo)
        {
            _root.AppendChild(nodo);
            _xml.Save( (new Uri(_xml.BaseURI).LocalPath));
        }
    }
}



    