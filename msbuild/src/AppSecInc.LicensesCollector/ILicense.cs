using System;

namespace AppSecInc.LicensesCollector
{
    interface ILicense
    {
        String Product { get; }
        String SubProduct { get; }
        String Version { get; }
        String LicenseFilename { get; }
        String LicenseType { get; }
        String Url { get; }
    }
}
