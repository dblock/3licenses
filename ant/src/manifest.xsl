<?xml version="1.0" encoding="utf-8"?>
<!-- 
 Copyright (c) Application Security Inc., 2010
 All Rights Reserved
 Eclipse Public License (EPLv1)
 http://3licenses.codeplex.com/license
 -->
<html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <style type="text/css">
      body, tr { color: #000000; background-color: white; font-family: Verdana; font-size: 10pt; }
      table { width: 600px; }
      td { text-align: center; background-color: #EEEEEE; }
      td.header { font-weight: bold; }
      div.note { font-size: 8pt; }
      div.missing { color: red; }
    </style>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
  </head>
  <body>
    <ul>
      <li>
        Total Licenses: <xsl:value-of select="count(//license)" />
      </li>
    </ul>
    <table>
      <tr>
       <td class="header">Product</td>
       <td class="header">Version</td>
       <td class="header">License</td>
      </tr>
      <xsl:for-each select="//license">
        <tr>
          <td>
          	<xsl:value-of select="@productName" />
			<xsl:choose>
			  <xsl:when test="string-length(@parentProduct) &gt; 1">
			  	<div class="note">
		    	 <xsl:text>(used by </xsl:text>
		    	 <xsl:value-of select="@parentProduct" />
		    	 <xsl:text>)</xsl:text>
		    	</div>
			  </xsl:when>
			</xsl:choose>          	
          </td>
          <td><xsl:value-of select="@productVersion" /></td>
          <td>
          	<xsl:choose>
			  <xsl:when test="string-length(@filename) &gt; 1">
	            <a>
	              <xsl:attribute name="href">
	                <xsl:value-of select="@filename" />
	              </xsl:attribute>
	              <xsl:attribute name="target">_blank</xsl:attribute>            
				  <xsl:choose>
				    <xsl:when test="string-length(@license) &gt; 1">
			    	  <xsl:value-of select="@license" />
				    </xsl:when>
				    <xsl:otherwise>
	            	  <xsl:text>read</xsl:text>
				    </xsl:otherwise>
				  </xsl:choose>
	            </a>
	          </xsl:when>
	          <xsl:otherwise>
	          	<div class="missing">
	          		<xsl:text>missing</xsl:text>
	          	</div>
	          </xsl:otherwise>
	        </xsl:choose>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </body>
</html>
