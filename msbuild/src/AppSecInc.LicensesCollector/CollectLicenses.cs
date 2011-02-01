using System;
using System.Collections.Generic;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using SharpSvn;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AppSecInc.LicensesCollector
{
    /// <summary>
    /// Custom MSBuild task to gather and export
    /// the third party licenses.
    /// </summary>
    public class CollectLicenses : Task
    {
        private String _src;
        [Required]
        public String Src
        {
            get { return _src; }
            set { _src = value; }
        }

        private String _toDir;
        [Required]
        public String ToDir
        {
            get { return _toDir; }
            set { _toDir = value; }
        }

        private Int32 _maxDepth = 1;
        [Required]
        public Int32 MaxDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }

        private ExternalsDictionary _externals = new ExternalsDictionary();

        private ITaskItem[] _excludedProducts;
        public ITaskItem[] ExcludedProducts
        {
            get { return _excludedProducts; }
            set { _excludedProducts = value; }
        }

        private LicenseManager _manager = new LicenseManager();
        private Folders _folders;

        private String _xslFile;
        public String XslFile
        {
            get { return _xslFile; }
            set { _xslFile = value; }
        }

        public override bool Execute()
        {
            if (_src == null)
            {
                throw new Exception("license-files: missing 'src'");
            }

            if (_toDir == null)
            {
                throw new Exception("license-files: missing 'toDir'");
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

            foreach (ITaskItem excludedProduct in ExcludedProducts)
            {
                External ex = new External();
                ex.Src = excludedProduct.GetMetadata("src");
                ex.Name = excludedProduct.GetMetadata("name");
                Boolean include;
                Boolean.TryParse(excludedProduct.GetMetadata("include"), out include);
                ex.Include = include;
                ex.ParentProduct = excludedProduct.GetMetadata("parentproduct");
                ex.License = excludedProduct.GetMetadata("license");
                ex.Version = excludedProduct.GetMetadata("version");
                _externals.AddConfiguredExternal(ex);
            }
            Log.LogMessage(MessageImportance.Normal, "license-files: collecting license files in " + _src, null);

            DirectoryInfo src = new DirectoryInfo(_src);
            DirectoryInfo toDir = new DirectoryInfo(_toDir);
            if (!toDir.Exists)
                toDir.Create();

            LicenseFilesManifest manifest = new LicenseFilesManifest();

            SortedList<String, String> externals = getExternalsVersions(src);
            SortedList<String, List<LicenseFound>> licenses = new SortedList<String, List<LicenseFound>>();
            foreach (String external in externals.Keys)
            {
                if (!isIncluded(external))
                {
                    Log.LogMessage(MessageImportance.Normal, "skipping license file in '" + external + "'", null);
                    continue;
                }
                String version;
                externals.TryGetValue(external, out version);
                List<LicenseFound> licensesCollected = collect(external, external,
                    external, version, new DirectoryInfo(src + @"\" + external), 1);
                if (licensesCollected != null)
                {
                    licenses.Add(external, licensesCollected);
                    foreach (LicenseFound licenseFound in licensesCollected)
                    {
                        manifest.Add(getLicenseInfo(external, licenseFound));
                    }
                }
            }

            foreach (String external in externals.Keys)
            {
                if (!licenses.ContainsKey(external) && isIncluded(external))
                {
                    String version;
                    externals.TryGetValue(external, out version);
                    Log.LogMessage(MessageImportance.Normal, "missing license file in '" + external + " (" + version + ")'", null);
                    manifest.Add(getLicenseInfo(external, new LicenseInfo(external, version)));
                }
            }

            foreach (List<LicenseFound> licensesFound in licenses.Values)
            {
                foreach (LicenseFound licenseFound in licensesFound)
                {
                    String licenseFilename = licenseFound.LicenseFilename;
                    FileInfo destinationFile = new FileInfo(toDir.FullName + @"\" + getLicenseFilename(licenseFilename));
                    File.Copy(licenseFound.File, destinationFile.FullName, true);
                }
            }

            FileInfo manifestFile = new FileInfo(toDir + @"\manifest.xml");
            Log.LogMessage(MessageImportance.Normal, "writing " + manifestFile.FullName, null);
            manifest.WriteTo(manifestFile, _xslFile);
            Log.LogMessage(MessageImportance.Normal, manifest.ToString(), null);

            return true;
        }

        private List<LicenseFound> collect(String root, String path, String product, String version, DirectoryInfo src, int depth)
        {
            List<LicenseFound> licenses = new List<LicenseFound>();
            List<LicenseFound> licensesFound = _manager.Find(root, path, product, version, src.FullName, depth);
            if (licensesFound != null)
            {
                Log.LogMessage(MessageImportance.Normal,
                    "found " + licensesFound.Count + " license(s) in '" + path + "'", null);
                licenses.AddRange(licensesFound);
            }

            if (depth < _maxDepth && licenses.Count == 0)
            {
                FileInfo[] files = src.GetFiles();

                foreach (FileInfo file in files)
                {
                    if (file.Name.StartsWith("."))
                        continue;

                    if (!isFolder(src + @"\" + file.Name))
                        continue;

                    DirectoryInfo sub = new DirectoryInfo(src + @"\" + file.Name);
                    List<LicenseFound> licensesCollected = collect(root, path + "/" + sub.Name,
                            product, version, sub, depth + 1);

                    if (licensesCollected != null)
                        licenses.AddRange(licensesCollected);
                }
            }

            return licenses.Count > 0 ? licenses : null;
        }

        private SortedList<String, String> getExternalsVersions(DirectoryInfo src)
        {
            SortedList<String, String> externals = new SortedList<String, String>();
            Log.LogMessage(MessageImportance.Normal, "fetching svn:externals for '{0}'", new object[] { src });

            using (SvnClient svn = new SvnClient())
            {
                String svnExternalsData;
                svn.GetProperty(src.FullName, SvnPropertyNames.SvnExternals, out svnExternalsData);
                Regex regexp = new Regex(@"^(\S+).*/((\d+[\._]?)+)$", RegexOptions.Multiline);
                MatchCollection matches = regexp.Matches(svnExternalsData.Replace("\r\n","\n"));
                foreach (Match match in matches)
                {
                    String externalName = match.Groups[1].Value;
                    String externalVersion = match.Groups[2].Value;
                    if (!String.IsNullOrEmpty(externalVersion))
                    {
                        externals.Add(externalName, externalVersion);
                    }
                }
                return externals;
            }
        }

        private String getLicenseFilename(String filename)
        {
            if (_folders != null)
            {
                foreach (Folder folder in _folders)
                {
                    filename = folder.Replace(filename);
                }
            }
            return filename;
        }

        private LicenseInfo getLicenseInfo(String external, ILicense licenseFound)
        {
            LicenseInfo licenseInfo = new LicenseInfo();
            licenseInfo.LicenseFilename = licenseFound.LicenseFilename;
            licenseInfo.LicenseType = licenseFound.LicenseType;
            licenseInfo.Product = licenseFound.Product;
            licenseInfo.ParentProduct = licenseFound.ParentProduct;
            licenseInfo.Version = licenseFound.Version;

            if (_externals != null)
            {
                External externalDefinition;
                _externals.TryGetValue(external, out externalDefinition);
                if (externalDefinition != null)
                {
                    if (String.IsNullOrEmpty(licenseInfo.ParentProduct))
                    {
                        externalDefinition.Apply(licenseInfo);
                    }
                    else
                    {
                        externalDefinition.ApplyToSubProduct(licenseInfo);
                    }
                }
                // subproduct external
                if (!String.IsNullOrEmpty(licenseInfo.ParentProduct))
                {
                    External childExternalDefinition;
                    _externals.TryGetValue(licenseInfo.Product, out childExternalDefinition);
                    if (childExternalDefinition != null)
                    {
                        childExternalDefinition.Apply(licenseInfo);
                    }
                }
            }

            if (_folders != null)
            {
                foreach (Folder folder in _folders)
                {
                    licenseInfo.Product = folder.Replace(licenseInfo.Product);
                    licenseInfo.ParentProduct = folder.Replace(licenseInfo.ParentProduct);
                    licenseInfo.LicenseFilename = folder.Replace(licenseInfo.LicenseFilename);
                }
            }

            return licenseInfo;
        }

        private Boolean isIncluded(String root)
        {
            if (_externals == null)
                return true;

            External external;
            _externals.TryGetValue(root, out external);
            if (external == null)
                return true;

            return external.Include;
        }

        void AddConfiguredExternals(ExternalsDictionary set)
        {
            if (_externals != null)
            {
                throw new Exception("Only one externals set allowed.");
            }
            _externals = set;
        }

        void AddConfiguredFolders(Folders set)
        {
            if (_folders != null)
            {
                throw new Exception("Only one folders set allowed.");
            }
            _folders = set;
        }

        private bool isFolder(string path)
        {
            return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
        }
    }
}
