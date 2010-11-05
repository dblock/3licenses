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
