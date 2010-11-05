package com.appsecinc.ant.license;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

public class LicenseManager {
	private static List<LicenseFile> _licenseFiles = new ArrayList<LicenseFile>();
	
	public LicenseManager() {
		_licenseFiles.add(new LICENSE_LicenseFile());
		_licenseFiles.add(new CPL_LicenseFile());
		_licenseFiles.add(new CPLv1_LicenseFile());
		_licenseFiles.add(new LICENSEHTM_LicenseFile());
		_licenseFiles.add(new COPYING_LicenseFile());
	}
	
	/**
	 * Find a license file in a directory.
	 * @param dir
	 *  Target directory.
	 * @return
	 *  License file.
	 */
	public List<LicenseFound> find(String root, String node, String product, String version, File dir, int depth) {
		List<LicenseFound> licenses = new ArrayList<LicenseFound>();
		String[] files = dir.list();
		for(String file : files) {
			
			File sub = new File(dir, file);
			if (sub.isDirectory())
				continue;

			for(LicenseFile licenseFile : _licenseFiles) {
				if (licenseFile.isMatch(file)) {					
					String subProduct = licenseFile.getSpec(file);
					if (subProduct != null && subProduct.length() > 0) {
						licenses.add(new LicenseFound(root, node, subProduct, product, null, licenseFile, sub));
					} else {					
						licenses.add(new LicenseFound(root, node, product, null, version, licenseFile, sub));
					}
				}
			}
		}
		
		return licenses.size() > 0 ? licenses : null;
	}
}
