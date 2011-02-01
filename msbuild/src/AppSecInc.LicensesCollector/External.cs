using System;

namespace AppSecInc.LicensesCollector
{
    public class External
    {
        public String Src { get; set; }
        public Boolean Include { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public String License { get; set; }
        public String ParentProduct { get; set; }
        public String Version { get; set; }

        public void Apply(LicenseInfo licenseInfo)
        {
            if (Name != null) licenseInfo.Product = Name;
            if (Url != null) licenseInfo.Url = Url;
            if (License != null) licenseInfo.LicenseType = License;
            if (ParentProduct != null) licenseInfo.ParentProduct = ParentProduct;
            if (Version != null) licenseInfo.Version = Version;
        }

        public void ApplyToSubProduct(LicenseInfo licenseInfo)
        {
          if (Name != null) licenseInfo.ParentProduct = Name;
        }

    }
}
