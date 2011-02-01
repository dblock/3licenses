using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    class GFDL_LicenceFile : ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "txt";
        }

        public bool IsMatch(string filename)
        {
            return filename.Equals("gfdl.txt", StringComparison.CurrentCultureIgnoreCase);
        }

        public string GetSpec(string filename)
        {
            return "gfdl-v1.1";
        }

        public new string GetType()
        {
            return "GFDL1.1";
        }

        #endregion
    }
}
