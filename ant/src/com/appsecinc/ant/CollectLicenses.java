/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
package com.appsecinc.ant;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

import org.apache.tools.ant.BuildException;
import org.apache.tools.ant.Task;
import org.apache.tools.ant.types.Path;
import org.apache.tools.ant.util.FileUtils;
import org.tmatesoft.svn.core.SVNException;
import org.tmatesoft.svn.core.wc.SVNClientManager;
import org.tmatesoft.svn.core.wc.SVNPropertyData;
import org.tmatesoft.svn.core.wc.SVNRevision;
import org.tmatesoft.svn.core.wc.SVNWCClient;

import com.appsecinc.ant.license.License;
import com.appsecinc.ant.license.LicenseFound;
import com.appsecinc.ant.license.LicenseInfo;
import com.appsecinc.ant.license.LicenseManager;

public class CollectLicenses extends Task {
	
	private Path _src = null;
	private Path _toDir = null;
	private int _maxDepth = 1;
	private LicenseManager _manager = new LicenseManager();
	private Externals _externals = null;
	private Folders _folders = null;
	private Path _xslFile = null;

	/*
	 * Source path.
	 */	
	public void setSrc(Path path) {
        _src = path;
    }

	/*
	 * Destination path.
	 */
	public void setToDir(Path path) {
        _toDir = path;
    }
	
	public void setMaxDepth(int maxDepth) {
		_maxDepth = maxDepth;
	}
	
	private List<LicenseFound> collect(String root, String path, String product, String version, File src, int depth) {
		List<LicenseFound> licenses = new ArrayList<LicenseFound>();
		
		// System.out.println("license-files: processing " + sub);
		List<LicenseFound> licensesFound = _manager.find(root, path, product, version, src, depth);
		if (licensesFound != null) {
			super.log("found " + licensesFound.size() + " license(s) in '" + path + "'");
			licenses.addAll(licensesFound);
		}

		if (depth < _maxDepth && licenses.size() == 0) {				
			String[] files = src.list();
			for(String file : files) {
	
				if (file.startsWith("."))
					continue;
	
				File sub = new File(src, file);
				if (! sub.isDirectory())
					continue;
				
				List<LicenseFound> licensesCollected = collect(root, path + "/" + sub.getName(),
						product, version, sub, depth + 1);
				if (licensesCollected != null) {
					licenses.addAll(licensesCollected);
				}
			}
		}
		
		return licenses.size() > 0 ? licenses : null;
	}
	
	private String getVersion(String s) {
		s = s.trim();
		
		if (s.startsWith("-r")) {
			s = s.substring("-r".length()).trim();
			while(s.length() > 0 && Character.isDigit(s.charAt(0))) {
				s = s.substring(1);
			}
		}
	
		String version = "";
		String currentVersion = "";
		for(int i = 0; i < s.length(); i++) {
			char c = s.charAt(i); 
			if (Character.isDigit(c)) {
				currentVersion += c;
			} else if (c == '.') {
				if (currentVersion.length() > 0 && Character.isDigit(currentVersion.charAt(
						currentVersion.length() - 1))) {
					currentVersion += c;
				}
			} else {
				if (currentVersion.length() > version.length()) {
					version = currentVersion;
					currentVersion = "";
				}
			}
		}
		
		if (currentVersion.length() > version.length()) {
			version = currentVersion;
		}
		
		while(version.length() > 0 && version.charAt(version.length() - 1) == '.') {
			version = version.substring(0, version.length() - 1);
		}
		
		return version.length() > 0 ? version : null;
	}
	
	private Map<String, String> getExternalsVersions(File src) {
		Map<String, String> externals = new TreeMap<String, String>();
		try {
			super.log("fetching svn:externals for '" + src + "'");
			SVNClientManager clientManager = SVNClientManager.newInstance();
			SVNWCClient client = clientManager.getWCClient();
			SVNPropertyData svnExternals = client.doGetProperty(src, 
						"svn:externals", SVNRevision.WORKING, SVNRevision.WORKING);
			String svnExternalsData = svnExternals.getValue().toString();
			String[] svnExternalsLineData = svnExternalsData.split("[\\n\\r]");
			for(String svnExternal : svnExternalsLineData) {
				String[] parts = svnExternal.split("\\s", 2);
				if (parts.length == 2) {
					String version = getVersion(parts[1]);
					if (version != null) {
						System.out.println(parts[0] + ": " + version);
						externals.put(parts[0], version);
					}
				}
			}
		} catch (SVNException e) {
			super.log("warning: " + e.getMessage());
		}
		return externals;
	}
	
	public void setXslFile(Path xslFile) {
		_xslFile = xslFile;
	}
	
	public Path getXslFile() {
		return _xslFile;
	}

	public void execute() {
		try {
			if (_src == null) {
				throw new BuildException("license-files: missing 'src'");
			}
	
			if (_toDir == null) {
				throw new BuildException("license-files: missing 'toDir'");
			}
			
			super.log("license-files: collecting license files in " + _src);
			
			File src = new File(_src.toString());
			if (! src.exists() || ! src.isDirectory()) {
				throw new BuildException("license-files: missing 'src' " + _src);
			}
	
			File toDir = new File(_toDir.toString());
			if (! toDir.exists()) {
				toDir.mkdir();
			}
			
			LicenseFilesManifest manifest = new LicenseFilesManifest();
						
			Map<String, String> externals = getExternalsVersions(src);
			Map<String, List<LicenseFound>> licenses = new TreeMap<String, List<LicenseFound>>();
			for(String external : externals.keySet()) {
				
				if (! isIncluded(external)) {
					super.log("skipping license file in '" + external + "'");
					continue;
				} 

				String version = externals.get(external);
				List<LicenseFound> licensesCollected = collect(external, external, 
						external, version, new File(src, external), 1);				
				if (licensesCollected != null) {
					licenses.put(external, licensesCollected);
					
					for(LicenseFound licenseFound : licensesCollected) {
						manifest.add(getLicenseInfo(external, licenseFound));
					}
				}
			}
			
			FileUtils fileUtils = FileUtils.getFileUtils();
			
			for(String external : externals.keySet()) {
				if (! licenses.containsKey(external) && isIncluded(external)) {
					String version = externals.get(external);
					super.log("missing license file in '" + external + " (" + version + ")'");
					manifest.add(getLicenseInfo(external, new LicenseInfo(external, version)));
				}
			}

			for(List<LicenseFound> licensesFound : licenses.values()) {
				for(LicenseFound licenseFound : licensesFound) {
					String licenseFilename = licenseFound.getLicenseFilename();
					File destinationFile = new File(toDir, getLicenseFilename(licenseFilename));
					fileUtils.copyFile(licenseFound.getFile(), destinationFile, null, true);
				}
			}
			
			File manifestXsl = null;
			if (_xslFile != null) {
				manifestXsl = new File(toDir, "manifest.xsl");
				super.log("copying " + manifestXsl);
				fileUtils.copyFile(new File(_xslFile.toString()), manifestXsl, null, true);
			}
			
			File manifestFile = new File(toDir, "manifest.xml");
			super.log("writing " + manifestFile.getPath());
			manifest.writeTo(manifestFile, manifestXsl != null ? manifestXsl.getName() : null);
			
			super.log(manifest.toString());
			
		} catch (Exception e) {
			super.log("error: " + e.getMessage());
			e.printStackTrace();
			throw new BuildException(e.getMessage(), e);
		}
	}
	
	private String getLicenseFilename(String filename) {
		if (_folders != null) {
			for(Folder folder : _folders) {
				filename = folder.replace(filename);
			}
		}		
		return filename;
	}
	
	private LicenseInfo getLicenseInfo(String external, License licenseFound) {
		LicenseInfo licenseInfo = new LicenseInfo();
		licenseInfo.setLicenseFilename(licenseFound.getLicenseFilename());
		licenseInfo.setLicenseType(licenseFound.getLicenseType());
		licenseInfo.setProduct(licenseFound.getProduct());
		licenseInfo.setParentProduct(licenseFound.getParentProduct());
		licenseInfo.setVersion(licenseFound.getVersion());
		
		// external re-definitions don't apply to sub-products
		String parentProduct = licenseInfo.getParentProduct();
		if (_externals != null) {
			// product external
			External externalDefinition = _externals.get(external);
			if (externalDefinition != null) {
				if (parentProduct == null || parentProduct.length() == 0) {
					externalDefinition.apply(licenseInfo);
				} else {
					externalDefinition.applyToSubProduct(licenseInfo);					
				}
			}
			// subproduct external
			if (parentProduct != null && parentProduct.length() > 0) {
				External childExternalDefinition = _externals.get(licenseInfo.getProduct());
				if (childExternalDefinition != null) {
					childExternalDefinition.apply(licenseInfo);
				}
			}			
		}
		
		if (_folders != null) {
			for(Folder folder : _folders) {
				licenseInfo.setProduct(folder.replace(licenseInfo.getProduct()));
				licenseInfo.setParentProduct(folder.replace(licenseInfo.getParentProduct()));
				licenseInfo.setLicenseFilename(folder.replace(licenseInfo.getLicenseFilename()));
			}
		}
	
		return licenseInfo;
	}
	
	private boolean isIncluded(String root) {
		if (_externals == null)
			return true;
		
		External external = _externals.get(root);
		if (external == null)
			return true;
		
		return external.getInclude();	
	}
	
	public void addConfiguredExternals(Externals set) {
		if (_externals != null) {
			throw new RuntimeException("Only one externals set allowed.");
		}
		_externals = set;
	}
	
	public void addConfiguredFolders(Folders set) {
		if (_folders != null) {
			throw new RuntimeException("Only one externals set allowed.");
		}
		_folders = set;
	}

}