using System;

namespace AppSecInc.LicensesCollector
{
  interface ILicense
  {
    String ParentProduct { get; }
    String Product { get; }
    String Version { get; }
    String LicenseFilename { get; }
    String LicenseType { get; }
    String Url { get; }
  }
}
