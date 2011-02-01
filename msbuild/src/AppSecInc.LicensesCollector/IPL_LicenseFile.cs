using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    class IPL_LicenseFile: ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "txt";
        }

        public bool IsMatch(string filename)
        {
            return filename.Equals("IPLicense.txt", StringComparison.CurrentCultureIgnoreCase);
        }

        public string GetSpec(string filename)
        {
            return "ipl-v10";
        }

        public new string GetType()
        {
            return "IPL1.0";
        }

        #endregion
    }
}
