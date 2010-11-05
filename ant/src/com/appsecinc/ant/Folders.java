/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
package com.appsecinc.ant;

import java.util.ArrayList;

public class Folders extends ArrayList<Folder> {

	private static final long serialVersionUID = 1L;

	public Folders() {
    	
    }

    public void addConfiguredFolder(Folder folder) {
		add(folder);
	}
}
