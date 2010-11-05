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
package com.appsecinc.ant.license;

public class LICENSE_LicenseFile implements LicenseFile {

	@Override
	public String getExtension() {
		return "txt";
	}

	@Override
	public boolean isMatch(String filename) {
		String filenameLowerCase = filename.toLowerCase();
		return filenameLowerCase.equals("license")
			|| filenameLowerCase.equals("license.txt")
			|| filenameLowerCase.endsWith("_license.txt")
			|| filenameLowerCase.endsWith("_license")
			|| filenameLowerCase.endsWith("-license")
			|| filenameLowerCase.endsWith("-license.txt");
	}

	@Override
	public String getSpec(String filename) {
		String filenameLowerCase = filename.toLowerCase();
		filenameLowerCase = filenameLowerCase.replace(".txt", "");
		filenameLowerCase = filenameLowerCase.replace("-license", "");
		filenameLowerCase = filenameLowerCase.replace("_license", "");
		if (filenameLowerCase.equals("license")) return "";
		return filenameLowerCase;
	}

	@Override
	public String getType() {
		return null;
	}
}
