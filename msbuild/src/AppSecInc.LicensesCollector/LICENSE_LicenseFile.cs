using System;

namespace AppSecInc.LicensesCollector
{
    class LICENSE_LicenseFile : ILicenseFile
    {
        #region ILicenseFile Members

        public string GetExtension()
        {
            return "txt";
        }

        public bool IsMatch(string filename)
        {
            String filenameLowerCase = filename.ToLower();
            return filenameLowerCase.Equals("license")
                || filenameLowerCase.Equals("license.txt")
                || filenameLowerCase.EndsWith("_license.txt")
                || filenameLowerCase.EndsWith("_license")
                || filenameLowerCase.EndsWith("-license")
                || filenameLowerCase.EndsWith("-license.txt");
        }

        public string GetSpec(string filename)
        {
            String filenameLowerCase = filename.ToLower();
            filenameLowerCase = filenameLowerCase.Replace(".txt", "");
            filenameLowerCase = filenameLowerCase.Replace("-license", "");
            filenameLowerCase = filenameLowerCase.Replace("_license", "");
            if (filenameLowerCase.Equals("license")) return "";
            return filenameLowerCase;
        }

        public new string GetType()
        {
            return null;
        }

        #endregion
    }
}
