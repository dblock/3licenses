using System;
using System.IO;
using System.Text;

namespace AppSecInc.LicensesCollector
{
  class LicenseFound : ILicense
  {
    private ILicenseFile _licenseFile;
    public ILicenseFile LicenseFile
    {
      get
      {
        return _licenseFile;
      }
    }

    private String _file;
    public String File
    {
      get
      {
        return _file;
      }
    }

    private String _root;
    public String Root
    {
      get
      {
        return _root;
      }
    }

    private String _path;
    public String Path
    {
      get
      {
        return _path;
      }
    }

    private String _version;
    public String Version
    {
      get
      {
        return _version;
      }
    }

    private String _product;
    public String Product
    {
      get
      {
        return _product;
      }
    }

    private String _parentProduct;
    public String ParentProduct
    {
      get
      {
        return _parentProduct;
      }
    }

    public string LicenseType
    {
      get
      {
        return _licenseFile.GetType();
      }
    }

    public string Url
    {
      get
      {
        return null;
      }
    }

    public LicenseFound(String root, String path, String product, String parent,
        String version, ILicenseFile licenseFile, String file)
    {
      _licenseFile = licenseFile;
      _file = file;
      _root = root;
      _path = path;
      _version = version;
      _product = product;
      _parentProduct = parent;
    }

    #region ILicense Members
    public string LicenseFilename
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        sb.Append(Path);
        if (Version != null)
        {
          sb.Append("_");
          sb.Append(Version);
        }
        if (!String.IsNullOrEmpty(_parentProduct))
        {
          sb.Append("-");
          sb.Append(_product);
        }
        sb.Append(".");
        sb.Append(_licenseFile.GetExtension());
        return sb.ToString().Replace("/", "-");
      }
    }

    #endregion
  }
}
