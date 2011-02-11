using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace AppSecInc.LicensesCollector
{
  /// <summary>
  /// Custom MSBuild task to combine
  /// third party licenses.
  /// </summary>
  public class CombineLicenses : Task
  {
    #region fields
    private String _xslFile;
    private String _toDir;
    private ITaskItem[] _manifests;

    #endregion

    public override bool Execute()
    {
      try
      {
        if (_toDir == null)
        {
          throw new Exception("license-manifests: missing 'toDir'");
        }

        if (_xslFile == null)
        {
          //Look in the same folder as the dll file.
          Uri codeBasePath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), UriKind.Absolute);
          String path = codeBasePath.AbsolutePath;
          String xslFilePath = path + @"\manifest.xsl";
          FileInfo xslFile = new FileInfo(xslFilePath);
          _xslFile = xslFile.FullName;
        }

        Log.LogMessage(MessageImportance.Normal, "license-manifests: collecting manifests to " + _toDir, null);

        Directory.CreateDirectory(_toDir);

        LicenseFilesManifest combinedManifest = new LicenseFilesManifest();
        foreach (ITaskItem item in _manifests)
        {
          int count = 0;
          LicenseFilesManifest manifest = new LicenseFilesManifest();
          manifest.SetSrcDir(item.GetMetadata("Fullpath"));
          foreach (LicenseInfo licenseInfo in manifest.GetLicenses())
          {
            combinedManifest.Add(licenseInfo);
            count++;
          }
          foreach (FileInfo licenseFile in manifest.SrcDir.GetFiles())
          {
            if (!licenseFile.Name.Equals("manifest.xml"))
            {
              String targetLicenseFile = Path.Combine(_toDir, licenseFile.Name);
              File.Copy(licenseFile.FullName, targetLicenseFile, true);
            }
          }
          Log.LogMessage(MessageImportance.Normal, "merged " + count + " license(s) from " + manifest.SrcDir.FullName);
        }

        String manifestXsl = null;
        if (_xslFile != null)
        {
          manifestXsl = Path.Combine(_toDir, "manifest.xsl");
          Log.LogMessage(MessageImportance.Normal, "copying " + _xslFile + " to " + _toDir);
          File.Copy(_xslFile, manifestXsl, true);
        }

        FileInfo manifestFile = new FileInfo(Path.Combine(_toDir, "manifest.xml"));
        Log.LogMessage(MessageImportance.Normal, "writing " + manifestFile);
        combinedManifest.WriteTo(manifestFile, manifestXsl != null ? manifestXsl : null);

        return true;
      }
      catch (Exception e)
      {
        Log.LogMessage(MessageImportance.High, "error: " + e.Message);
        Console.WriteLine(e.StackTrace);
        throw;
      }
    }

    #region properties
    [Required]
    public String ToDir
    {
      get { return _toDir; }
      set { _toDir = value; }
    }

    public String XslFile
    {
      get { return _xslFile; }
      set { _xslFile = value; }
    }

    public ITaskItem[] Manifests
    {
      get { return _manifests; }
      set { _manifests = value; }
    }

    #endregion

  }
}
