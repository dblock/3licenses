/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
package com.appsecinc.ant.license;

public class LicenseInfo implements License {
	private String _product;
	private String _parentProduct;
	private String _version;
	private String _licenseFilename;
	private String _licenseType;
	private String _url;
	
	public LicenseInfo() {
		
	}
	
	public LicenseInfo(String product, String version){
		_product = product;
		_version = version;
	}
	
	@Override
	public String getProduct() {
		return _product;
	}
	
	public void setProduct(String product) {
		_product = product;
	}
	
	@Override
	public String getParentProduct() {
		return _parentProduct;
	}
	
	public void setParentProduct(String subProduct) {
		_parentProduct = subProduct;
	}
	
	@Override
	public String getVersion() {
		return _version;
	}
	
	public void setVersion(String version) {
		_version = version; 
	}
	
	@Override
	public String getLicenseFilename() { 
		return _licenseFilename;
	}
	
	public void setLicenseFilename(String filename) {
		_licenseFilename = filename;
	}
	
	@Override
	public String getLicenseType() {
		return _licenseType;
	}
	
	public void setLicenseType(String type) {
		_licenseType = type;
	}
	
	@Override
	public String getUrl() {
		return _url;
	}
	
	public void setUrl(String url) {
		_url = url;
	}
	
	public String getKey() {
		StringBuilder sb = new StringBuilder();
		// parent product
		String parentProduct = getParentProduct();
		if (parentProduct != null && parentProduct.length() > 0) {
			sb.append(parentProduct);
			sb.append("/");
		}
		// product
		sb.append(getProduct());
		// version
		String version = getVersion();
		if (version != null && version.length() > 0) {
			sb.append("/" + version);
		}
		return sb.toString();
	}
}
