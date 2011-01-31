using System;

namespace AppSecInc.LicensesCollector
{
    class COPYING_LicenseFile : ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "txt";
        }

        public bool IsMatch(string filename)
        {
            String filenameLowerCase = filename.ToLower();
            return filenameLowerCase.Equals("copying")
                || filenameLowerCase.Equals("copying.txt")
                || filenameLowerCase.EndsWith("-copying");
        }

        public string GetSpec(string filename)
        {
            String filenameLowerCase = filename.ToLower();
            filename = filenameLowerCase.Replace("-copying", "");
            if (filenameLowerCase.Equals("copying")) return "";
            if (filenameLowerCase.Equals("copying.txt")) return "";
            return filenameLowerCase;
        }

        public new string GetType()
        {
            return "COPYING";
        }

        #endregion
    }
}
