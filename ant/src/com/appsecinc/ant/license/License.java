package com.appsecinc.ant.license;

public interface License {
	String getProduct();
	String getParentProduct();
	String getVersion();
	String getLicenseFilename(); 
	String getLicenseType();
	String getUrl();
}
