/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
package com.appsecinc.ant.license;

public interface LicenseFile {
	public String getExtension();
	public boolean isMatch(String filename);
	public String getSpec(String filename);
	public String getType();
}
