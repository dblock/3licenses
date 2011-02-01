using System;

namespace AppSecInc.LicensesCollector
{
    interface ILicenseFile
    {
        String GetExtension();
        Boolean IsMatch(String filename);
        String GetSpec(String filename);
        String GetType();
    }
}
