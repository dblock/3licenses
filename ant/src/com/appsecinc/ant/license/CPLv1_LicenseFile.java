package com.appsecinc.ant.license;

public class CPLv1_LicenseFile implements LicenseFile {

	@Override
	public String getExtension() {
		return "html";
	}

	@Override
	public boolean isMatch(String filename) {
		return filename.equalsIgnoreCase("cpl-v10.html");
	}

	@Override
	public String getSpec(String filename) {
		return "cpl-v10";
	}
	
	@Override
	public String getType() {
		return "CPL 1.0";
	}

}
