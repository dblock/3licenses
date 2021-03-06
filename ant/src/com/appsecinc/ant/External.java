/*******************************************************************************
* 3Licenses (http://3licenses.codeplex.com)
* 
* Copyright (c) 2010 Application Security, Inc.
* 
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*
* Contributors:
*     Application Security, Inc.
*******************************************************************************/
package com.appsecinc.ant;

import com.appsecinc.ant.license.LicenseInfo;

public class External {
	
	private String _src;
	private boolean _include = true;
	private String _name;
	private String _url;
	private String _license;
	private String _parentproduct;
	private String _version;
	
	public boolean getInclude() { 
		return _include;
	}
	
	public void setInclude(boolean include) {
		_include = include;
	}
	
	public String getSrc() {
		return _src;
	}
	
	public void setSrc(String src) {
		_src = src;
	}
	
	public String getName() {
		return _name;
	}
	
	public void setName(String name) {
		_name = name;
	}
	
	public String getUrl() {
		return _url;
	}
	
	public void setUrl(String url) {
		_url = url;
	}
	
	public String getLicense() {
		return _license;
	}
	
	public void setLicense(String license) {
		_license = license;
	}
	
	public String getParentProduct() {
		return _parentproduct;
	}
	
	public void setSubProduct(String name) {
		_parentproduct = name;
	}
	
	public String getVersion() {
		return _version;
	}
	
	public void setVersion(String version) {
		_version = version;
	}

	public void apply(LicenseInfo licenseInfo) {
		if (_name != null) licenseInfo.setProduct(_name);
		if (_url != null) licenseInfo.setUrl(_url);
		if (_license != null) licenseInfo.setLicenseType(_license);
		if (_parentproduct != null) licenseInfo.setParentProduct(_parentproduct);
		if (_version != null) licenseInfo.setVersion(_version);
	}

	public void applyToSubProduct(LicenseInfo licenseInfo) {
		if (_name != null) licenseInfo.setParentProduct(_name);
	}
}
