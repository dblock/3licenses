package com.appsecinc.ant.license;

public class COPYING_LicenseFile implements LicenseFile {

	@Override
	public String getExtension() {
		return "txt";
	}

	@Override
	public boolean isMatch(String filename) {
		String filenameLowerCase = filename.toLowerCase();
		return filenameLowerCase.equals("copying")
			|| filenameLowerCase.equals("copying.txt")
			|| filenameLowerCase.endsWith("-copying");
	}

	@Override
	public String getSpec(String filename) {
		String filenameLowerCase = filename.toLowerCase();
		filename = filenameLowerCase.replace("-copying", "");
		if (filenameLowerCase.equals("copying")) return "";
		if (filenameLowerCase.equals("copying.txt")) return "";
		return filenameLowerCase;
	}

	@Override
	public String getType() {
		return null;
	}
}
