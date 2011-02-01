using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    public class LicenseInfo : ILicense
    {
        private String _product;
        public String Product
        {
            get { return _product; } 
            set { if (value != null) _product = value.Replace('_', ' '); }
        }
        
        private String _parentProduct;
        public String ParentProduct
        {
            get { return _parentProduct; }
            set { if (value != null) _parentProduct = value.Replace('_', ' '); }
        }

        private String _version;
        public String Version
        {
            get { return _version; }
            set { if (value != null) _version = value.Replace('_', '.'); }
        }

        public String LicenseFilename { get; set; }
        public String LicenseType { get; set; }
        public String Url { get; set; }

        public LicenseInfo()
        {
        }

        public LicenseInfo(String product, String version)
        {
            Product = product;
            Version = version;
        }

        public String GetKey()
        {
            StringBuilder sb = new StringBuilder();
            // parent product
            if (!String.IsNullOrEmpty(ParentProduct))
            {
                sb.Append(ParentProduct).Append("/");
            }
            sb.Append(Product);
            if (Version != null)
            {
                sb.Append("/" + Version);
            }
            return sb.ToString();
        }

    }
}
