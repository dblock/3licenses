/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
package com.appsecinc.ant.license;

public interface License {
	String getProduct();
	String getParentProduct();
	String getVersion();
	String getLicenseFilename(); 
	String getLicenseType();
	String getUrl();
}
