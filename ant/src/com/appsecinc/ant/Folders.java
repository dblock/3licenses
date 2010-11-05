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
