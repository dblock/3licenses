/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
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
