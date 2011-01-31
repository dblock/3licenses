using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Xsl;

namespace AppSecInc.LicensesCollector
{
    class LicenseFilesManifest
    {
        private DirectoryInfo _srcDir;
        public DirectoryInfo SrcDir
        {
            get
            {
                return _srcDir;
            }
        }
        public void SetSrcDir(String src)
        {
            _srcDir = new DirectoryInfo(src);

            if (!_srcDir.Exists)
                throw new FileNotFoundException(_srcDir.FullName);

            LoadXml(new FileInfo(_srcDir.FullName + @"\" + "manifest.xml"));
        }
        private SortedList<String, LicenseInfo> _licenses = new SortedList<String, LicenseInfo>();

        public void Add(LicenseInfo license)
        {
            if (!_licenses.ContainsKey(license.GetKey()))
                _licenses.Add(license.GetKey(), license);
        }

        public void LoadXml(FileInfo src)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(src.FullName);
            XmlElement root = doc.DocumentElement;
            XmlNodeList licensesNodes = root.ChildNodes;
            for (int nodeIndex = 0; nodeIndex < licensesNodes.Count; nodeIndex++)
            {
                XmlNode licenseNode = licensesNodes.Item(nodeIndex);
                if (licenseNode.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                XmlNamedNodeMap licenseAttributes = licenseNode.Attributes;
                LicenseInfo licenseInfo = new LicenseInfo();
                for (int attributeIndex = 0; attributeIndex < licenseAttributes.Count; attributeIndex++)
                {
                    XmlNode attribute = licenseAttributes.Item(attributeIndex);
                    if (attribute.LocalName.Equals("productName"))
                        licenseInfo.Product = attribute.Value;
                    else if (attribute.LocalName.Equals("includedProduct"))
                        licenseInfo.SubProduct = attribute.Value;
                    else if (attribute.LocalName.Equals("filename"))
                        licenseInfo.LicenseFilename = attribute.Value;
                    else if (attribute.LocalName.Equals("licenseType"))
                        licenseInfo.LicenseType = attribute.Value;
                    else if (attribute.LocalName.Equals("url"))
                        licenseInfo.Url = attribute.Value;
                }

                Add(licenseInfo);
            }
        }

        public ICollection<LicenseInfo> GetLicenses()
        {
            return _licenses.Values;
        }

        public XmlDocument GetXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("licenses");
            doc.AppendChild(root);

            foreach (LicenseInfo license in _licenses.Values)
            {
                XmlElement licenseElement = doc.CreateElement("license");
                licenseElement.SetAttribute("productName", license.Product);
                licenseElement.SetAttribute("productVersion", license.Version);
                if (license.SubProduct != null)
                    licenseElement.SetAttribute("filename", license.LicenseFilename);
                if (license.LicenseType != null)
                    licenseElement.SetAttribute("license", license.LicenseType);

                root.AppendChild(licenseElement);
            }

            return doc;
        }

        public String ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (LicenseInfo license in _licenses.Values)
            {
                sb.Append(license.Product);
                sb.Append(" " + license.Version);

                if (license.SubProduct != null)
                {
                    sb.Append(" included by ");
                    sb.Append(license.SubProduct);
                }
                if (license.LicenseFilename != null)
                {
                    sb.Append(": ");
                    sb.Append(license.LicenseFilename);
                }
                if (license.LicenseType != null)
                {
                    sb.Append(" [" + license.LicenseType + "]");
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public void WriteTo(FileInfo manifestFile, String xslFile)
        {
            //Save the manifest.xml file
            XmlDocument document = GetXml();
            document.Save(manifestFile.FullName);

            //Load the xsl file (manifest.xsl)
            XslTransform transformer = new XslTransform();
            transformer.Load(xslFile);
            
            //Create the stream for output and transform the document
            XmlTextWriter writer = new XmlTextWriter(manifestFile.DirectoryName +
                @"\licenses.html", null);
            transformer.Transform(document, null, writer);

            writer.Close();
        }
    }
}
