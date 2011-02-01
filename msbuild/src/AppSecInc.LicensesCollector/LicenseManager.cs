using System;
using System.Collections.Generic;
using System.IO;

namespace AppSecInc.LicensesCollector
{
  class LicenseManager
  {
    private static List<ILicenseFile> _licenseFiles = new List<ILicenseFile>();

    public LicenseManager()
    {
      _licenseFiles.Add(new LICENSE_LicenseFile());
      _licenseFiles.Add(new CPL_LicenseFile());
      _licenseFiles.Add(new CPLv1_LicenseFile());
      _licenseFiles.Add(new LICENSEHTM_LicenseFile());
      _licenseFiles.Add(new COPYING_LicenseFile());
      _licenseFiles.Add(new GFDL_LicenceFile());
      _licenseFiles.Add(new LGPL_LicenseFile());
      _licenseFiles.Add(new IPL_LicenseFile());
      _licenseFiles.Add(new IDPL_LicenseFile());
    }

    /// <summary>
    /// Find a license file in a directory.
    /// </summary>
    /// <param name="dir">Target directory</param>
    /// <returns>License file</returns>
    public List<LicenseFound> Find(String root, String node, String product, String version, String dir, int depth)
    {
      List<LicenseFound> licenses = new List<LicenseFound>();

      DirectoryInfo directory = new DirectoryInfo(dir);
      FileInfo[] files = directory.GetFiles();
      foreach (FileInfo file in files)
      {
        if (isFolder(file.FullName))
          continue;

        foreach (ILicenseFile licenseFile in _licenseFiles)
        {
          if (licenseFile.IsMatch(file.Name))
          {
            String subProduct = licenseFile.GetSpec(file.Name);
            if (!String.IsNullOrEmpty(subProduct))
            {
              licenses.Add(new LicenseFound(root, node, subProduct, product, null, licenseFile, file.FullName));
            }
            else
            {
              licenses.Add(new LicenseFound(root, node, product, null, version, licenseFile, file.FullName));
            }
          }
        }
      }

      return licenses.Count > 0 ? licenses : null;
    }

    private bool isFolder(string path)
    {
      return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
    }
  }
}
