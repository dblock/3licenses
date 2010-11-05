/**
 * Copyright (c) Application Security Inc., 2010
 * All Rights Reserved
 * Eclipse Public License (EPLv1)
 * http://3licenses.codeplex.com/license
 */
package com.appsecinc.ant;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.StringWriter;
import java.util.Collection;
import java.util.SortedMap;
import java.util.TreeMap;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.OutputKeys;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NamedNodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import com.appsecinc.ant.license.LicenseInfo;

public class LicenseFilesManifest {
	private File _srcDir;
	private SortedMap<String, LicenseInfo> _licenses = new TreeMap<String, LicenseInfo>();
	
	public void add(LicenseInfo license) {
		_licenses.put(license.getKey(), license);
	}
	
	public void setSrcDir(String src) throws ParserConfigurationException, SAXException, IOException {
		_srcDir = new File(src);
		
		if (! _srcDir.exists()) {
			throw new FileNotFoundException(_srcDir.getPath());
		}
		
		loadXml(new File(_srcDir, "manifest.xml"));
	}
	
	public File getSrcDir() {
		return _srcDir;
	}
	
	public void loadXml(File src) throws ParserConfigurationException, SAXException, IOException {
		DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
		DocumentBuilder docBuilder = factory.newDocumentBuilder();
		Document doc = docBuilder.parse(src);
		Element root = doc.getDocumentElement(); // licenses
		NodeList licensesNodes = root.getChildNodes();
		for(int nodeIndex = 0; nodeIndex < licensesNodes.getLength(); nodeIndex++) {
			Node licenseNode = licensesNodes.item(nodeIndex);
			if (licenseNode.getNodeType() != Node.ELEMENT_NODE) {
				continue;
			}
			
			NamedNodeMap licenseAttributes = licenseNode.getAttributes();
			LicenseInfo licenseInfo = new LicenseInfo();
			for(int attributeIndex = 0; attributeIndex < licenseAttributes.getLength(); attributeIndex++) {
				Node attribute = licenseAttributes.item(attributeIndex);
				if (attribute.getNodeName().equals("productName")) {
					licenseInfo.setProduct(attribute.getNodeValue());
				} else if (attribute.getNodeName().equals("productVersion")) {
					licenseInfo.setVersion(attribute.getNodeValue());
				} else if (attribute.getNodeName().equals("parentProduct")) {
					licenseInfo.setParentProduct(attribute.getNodeValue());
				} else if (attribute.getNodeName().equals("filename")) {
					licenseInfo.setLicenseFilename(attribute.getNodeValue());
				} else if (attribute.getNodeName().equals("licenseType")) {
					licenseInfo.setLicenseType(attribute.getNodeValue());
				} else if (attribute.getNodeName().equals("url")) {
					licenseInfo.setUrl(attribute.getNodeValue());
				}
			}			
			add(licenseInfo);
		}
	}

	public Collection<LicenseInfo> getLicenses() {
		return _licenses.values();		
	}

	public Document getXml(String xslFile) throws ParserConfigurationException {
		DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
		DocumentBuilder docBuilder = factory.newDocumentBuilder();
		Document doc = docBuilder.newDocument();
		Element root = doc.createElement("licenses");
		doc.appendChild(root);
		if (xslFile != null && ! xslFile.isEmpty()) {
			Node pi = doc.createProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"" + xslFile + "\"");
			doc.insertBefore(pi, root);
		}
		for(LicenseInfo license : _licenses.values()) {
			Element licenseElement = doc.createElement("license");
			licenseElement.setAttribute("productName", license.getProduct());
			licenseElement.setAttribute("productVersion", license.getVersion());
			if (license.getParentProduct() != null) {
				licenseElement.setAttribute("parentProduct", license.getParentProduct());
			}
			if (license.getLicenseFilename() != null) {
				licenseElement.setAttribute("filename", license.getLicenseFilename());
			}
			if (license.getLicenseType() != null) {
				licenseElement.setAttribute("licenseType", license.getLicenseType());
			}
			if (license.getUrl() != null) {
				licenseElement.setAttribute("url", license.getUrl());
			}
			root.appendChild(licenseElement);
		}
		return doc;
	}
	
	public String toString() {
		StringBuilder sb = new StringBuilder();
		for(LicenseInfo license : _licenses.values()) {
			sb.append(license.getProduct());
			if (license.getVersion() != null) {
				sb.append(" " + license.getVersion());
			}
			if (license.getParentProduct() != null) {
				sb.append(" included by ");
				sb.append(license.getParentProduct());
			}
			if (license.getLicenseFilename() != null) {
				sb.append(": ");
				sb.append(license.getLicenseFilename());
			}
			if (license.getLicenseType() != null) {
				sb.append(" [" + license.getLicenseType() + "]");
			}
			sb.append("\r\n");
		}
		return sb.toString();
	}

	public void writeTo(File manifestFile, String xslFile) throws ParserConfigurationException, TransformerException, IOException {
		TransformerFactory transfac = TransformerFactory.newInstance();
		Transformer trans = transfac.newTransformer();
        StringWriter sw = new StringWriter();
        StreamResult result = new StreamResult(sw);
        Document xml = getXml(xslFile);
        DOMSource source = new DOMSource(xml);
        trans.setOutputProperty(OutputKeys.INDENT, "yes");
        trans.setOutputProperty("{http://xml.apache.org/xslt}indent-amount", "4");
        trans.transform(source, result);
        String xmlString = sw.toString();
	    OutputStream f0;
		byte buf[] = xmlString.getBytes();
		f0 = new FileOutputStream(manifestFile.getPath());
		for(int i=0; i < buf.length; i++) {
		   f0.write(buf[i]);
		}
		f0.close();
		buf = null;		
	}	
}
