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
