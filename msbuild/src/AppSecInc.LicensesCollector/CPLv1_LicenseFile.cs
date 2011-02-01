using System;

namespace AppSecInc.LicensesCollector
{
    class CPLv1_LicenseFile : ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "html";
        }

        public bool IsMatch(string filename)
        {
            return filename.Equals("cpl-v10.html", StringComparison.CurrentCultureIgnoreCase);
        }

        public string GetSpec(string filename)
        {
            return "cpl-v10";
        }

        public new string GetType()
        {
            return "CPL1.0";
        }

        #endregion
    }
}
