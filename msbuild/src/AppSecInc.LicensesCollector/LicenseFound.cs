using System;
using System.IO;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    class LicenseFound : ILicense
    {
        private ILicenseFile _licenseFile;
        public ILicenseFile LicenseFile
        {
            get
            {
                return _licenseFile;
            }
        }

        private String _file;
        public String File
        {
            get
            {
                return _file;
            }
        }

        private String _root;
        public String Root
        {
            get
            {
                return _root;
            }
        }

        private String _path;
        public String Path
        {
            get
            {
                return _path;
            }
        }

        private String _version;
        public String Version
        {
            get
            {
                return _version;
            }
        }

        private String _product;
        public String Product
        {
            get
            {
                return _product;
            }
        }

        public LicenseFound(String root, String path, String product,
            String version, ILicenseFile licenseFile, String file)
        {
            _licenseFile = licenseFile;
            _file = file;
            _root = root;
            _path = path;
            _version = version;
            _product = product;
        }

        #region ILicense Members


        public string SubProduct
        {
            get
            {
                String spec = LicenseFile.GetSpec(File);
                if (!String.IsNullOrEmpty(spec))
                    return spec;
                return null;
            }
        }

        public string LicenseFilename
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Path);
                if (Version != null)
                {
                    sb.Append("-");
                    sb.Append(Version);
                }
                String spec = SubProduct;
                if (String.IsNullOrEmpty(spec))
                {
                    sb.Append("-");
                    sb.Append(spec);
                }
                sb.ToString().Replace("/", "-");
                sb.Append("-license");
                if (!String.IsNullOrEmpty(_licenseFile.GetType()))
                {
                    sb.Append("-");
                    sb.Append(_licenseFile.GetType());
                }
                sb.Append(".");
                sb.Append(_licenseFile.GetExtension());
                return sb.ToString();
            }
        }

        public string LicenseType
        {
            get
            {
                return _licenseFile.GetType();
            }
        }

        public string Url
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}
