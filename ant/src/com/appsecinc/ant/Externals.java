/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
package com.appsecinc.ant;

import java.util.HashMap;

public class Externals extends HashMap<String, External> {

	private static final long serialVersionUID = 1L;

	public Externals() {
    	
    }

    public void addConfiguredExternal(External external) {
		if (containsKey(external.getSrc())) {
			throw new RuntimeException("Duplicate external '" + external.getSrc() + "'");
		}
		
		put(external.getSrc(), external);
	}   
}
