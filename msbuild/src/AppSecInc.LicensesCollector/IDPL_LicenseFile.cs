using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    class IDPL_LicenseFile : ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "txt";
        }

        public bool IsMatch(string filename)
        {
            return filename.Equals("IDPLicense.txt", StringComparison.CurrentCultureIgnoreCase);
        }

        public string GetSpec(string filename)
        {
            return "idpl-v10";
        }

        public new string GetType()
        {
            return "IDPL1.0";
        }

        #endregion
    }
}
