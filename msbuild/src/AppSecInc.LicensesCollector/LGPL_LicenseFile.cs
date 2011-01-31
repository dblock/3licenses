using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppSecInc.LicensesCollector
{
    class LGPL_LicenseFile : ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "txt";
        }

        public bool IsMatch(string filename)
        {
            return filename.Equals("lgpl.txt", StringComparison.CurrentCultureIgnoreCase);
        }

        public string GetSpec(string filename)
        {
            return "lgpl-v21";
        }

        public new string GetType()
        {
            return "LGPL2.1";
        }

        #endregion
    }
}
