using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    class LICENSEHTM_LicenseFile : ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "html";
        }

        public bool IsMatch(string filename)
        {
            String filenameLowerCase = filename.ToLower();
            return filenameLowerCase.Equals("license.html")
                || filenameLowerCase.Equals("license.htm")
                || filenameLowerCase.EndsWith("-license.htm")
                || filenameLowerCase.EndsWith("-license.html");
        }

        public string GetSpec(string filename)
        {
            String filenameLowerCase = filename.ToLower();
            filenameLowerCase = filenameLowerCase.Replace(".html", "");
            filenameLowerCase = filenameLowerCase.Replace(".htm", "");
            filenameLowerCase = filenameLowerCase.Replace("-license", "");
            filenameLowerCase = filenameLowerCase.Replace("_license", "");
            if (filenameLowerCase.Equals("license")) return "";
            return filenameLowerCase;
        }

        public new string GetType()
        {
            return "HTML";
        }

        #endregion
    }
}
