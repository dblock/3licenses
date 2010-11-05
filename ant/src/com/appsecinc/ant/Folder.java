package com.appsecinc.ant;

public class Folder {
	
	private String _src = "";
	private String _name = "";
	
	public String getSrc() {
		return _src;
	}
	
	public void setSrc(String src) {
		_src = src;
	}
	
	public String getName() {
		return _name;
	}
	
	public void setName(String name) {
		_name = name;
	}

	public String replace(String value) {
		if (value == null) {
			return value;
		} else {
			return value
				.replace(_src, _name == null ? "" : _name)
				.replace(_src.replace('/', '-'), _name == null ? "" : _name);		
		}
	}
}
