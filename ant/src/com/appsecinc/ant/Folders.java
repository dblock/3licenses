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
