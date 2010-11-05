package com.appsecinc.ant;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import org.apache.tools.ant.BuildException;
import org.apache.tools.ant.Task;
import org.apache.tools.ant.types.Path;
import org.apache.tools.ant.util.FileUtils;

import com.appsecinc.ant.license.LicenseInfo;

public class CombineLicenses extends Task {
	
	private List<LicenseFilesManifest> _manifests = new ArrayList<LicenseFilesManifest>();
	private Path _toDir = null;
	private Path _xslFile = null;
	
	public void setToDir(Path path) {
        _toDir = path;
    }
	
	public void setXslFile(Path xslFile) {
		_xslFile = xslFile;
	}
	
	public Path getXslFile() {
		return _xslFile;
	}

	public void addConfiguredLicenses(LicenseFilesManifest manifest) {
		_manifests.add(manifest);
	}
	
	public void execute() {
		try {
			if (_toDir == null) {
				throw new BuildException("license-files: missing 'toDir'");
			}
			
			super.log("license-manifests: collecting manifests to " + _toDir);
			
			File toDir = new File(_toDir.toString());
			if (! toDir.exists()) {
				toDir.mkdir();
			}
			
			FileUtils fileUtils = FileUtils.getFileUtils();
			LicenseFilesManifest combinedManifest = new LicenseFilesManifest();		
			for(LicenseFilesManifest manifest : _manifests) {
				int count = 0;
				for(LicenseInfo licenseInfo : manifest.getLicenses()) {
					combinedManifest.add(licenseInfo);
					count ++;
				}
				for(File licenseFile : manifest.getSrcDir().listFiles()) {
					if (! licenseFile.getName().equals("manifest.xml")) {
						File targetLicenseFile = new File(toDir, licenseFile.getName());
						fileUtils.copyFile(licenseFile, targetLicenseFile, null, true);
					}
				}
				super.log("merged " + count + " license(s) from " + manifest.getSrcDir().getPath());
			}
			
			File manifestXsl = null;
			if (_xslFile != null) {
				manifestXsl = new File(toDir, "manifest.xsl");
				super.log("copying " + manifestXsl + " to " + toDir);
				fileUtils.copyFile(new File(_xslFile.toString()), manifestXsl, null, true);
			}
			
			File manifestFile = new File(toDir, "manifest.xml");
			super.log("writing " + manifestFile.getAbsolutePath());
			combinedManifest.writeTo(manifestFile, manifestXsl != null ? manifestXsl.getName() : null);
			
		} catch (Exception e) {
			super.log("error: " + e.getMessage());
			e.printStackTrace();
			throw new BuildException(e.getMessage(), e);
		}
	}
}
