using System;

namespace AppSecInc.LicensesCollector
{
    class CPL_LicenseFile : ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "txt";
        }

        public bool IsMatch(string filename)
        {
            return filename.Equals("cpl.txt", StringComparison.CurrentCultureIgnoreCase);
        }

        public string GetSpec(string filename)
        {
            return "cpl";
        }

        public new string GetType()
        {
            return "CPL";
        }

        #endregion
    }
}
