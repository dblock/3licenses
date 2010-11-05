package com.appsecinc.ant.license;

public interface LicenseFile {
	public String getExtension();
	public boolean isMatch(String filename);
	public String getSpec(String filename);
	public String getType();
}
