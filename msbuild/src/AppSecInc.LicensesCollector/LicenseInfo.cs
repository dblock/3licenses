using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    class LicenseInfo : ILicense
    {
        private String _product;
        public String Product
        {
            get
            {
                return _product;
            }
            set
            {
                _product = value;
            }
        }

        private String _subProduct;
        public String SubProduct
        {
            get
            {
                return _subProduct;
            }
            set
            {
                _subProduct = value;
            }
        }

        private String _version;
        public String Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        private String _licenseFilename;
        public String LicenseFilename
        {
            get
            {
                return _licenseFilename;
            }
            set
            {
                _licenseFilename = value;
            }
        }

        private String _licenseType;
        public String LicenseType
        {
            get
            {
                return _licenseType;
            }
            set
            {
                _licenseType = value;
            }
        }

        private String _url;
        public String Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public LicenseInfo()
        {
        }

        public LicenseInfo(String product, String version)
        {
            _product = product;
            _version = version;
        }

        public String GetKey()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Product);
            if (Product != null)
            {
                sb.Append("/" + SubProduct);
            }
            if (Version != null)
            {
                sb.Append("/" + Version);
            }
            return sb.ToString();
        }
    }
}
