/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
package com.appsecinc.ant.license;

import java.io.File;

public class LicenseFound implements License {
	private LicenseFile _licenseFile;
	private File _file;
	private String _root;
	private String _path;
	private String _version;
	private String _product;
	private String _parent;
	
	public LicenseFile getLicenseFile() {
		return _licenseFile;
	}
	
	public File getFile() {
		return _file;
	}
	
	public String getRoot() {
		return _root;
	}
	
	public String getPath() {
		return _path;
	}
	
	@Override
	public String getVersion() {
		return _version;
	}	
	
	public LicenseFound(String root, String path, String product, String parent, String version, 
			LicenseFile licenseFile, File file) {
		_licenseFile = licenseFile;
		_file = file;
		_root = root;
		_path = path;
		_version = version;
		_product = product;
		_parent = parent;
	}

	@Override
	public String getParentProduct() {
		return _parent;
	}
	
	@Override
	public String getLicenseFilename() {
		StringBuilder sb = new StringBuilder();
		sb.append(_path);
		if (_version != null) {
			sb.append("_");
			sb.append(_version);
		}
		if (_parent != null && _parent.length() > 0) {
			sb.append("-");
			sb.append(_product);
		}
		sb.append(".");
		sb.append(_licenseFile.getExtension());
		return sb.toString().replace("/", "-");
	}

	@Override
	public String getLicenseType() {
		return _licenseFile.getType();
	}

	@Override
	public String getProduct() {
		return _product;
	}

	@Override
	public String getUrl() {
		return null;
	}
}
