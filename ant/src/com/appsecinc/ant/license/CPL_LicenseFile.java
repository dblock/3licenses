package com.appsecinc.ant.license;

public class CPL_LicenseFile implements LicenseFile {

	@Override
	public String getExtension() {
		return "txt";
	}

	@Override
	public boolean isMatch(String filename) {
		return filename.equalsIgnoreCase("cpl.txt");
	}

	@Override
	public String getSpec(String filename) {
		return "cpl";
	}

	@Override
	public String getType() {
		return "CPL";
	}
}
