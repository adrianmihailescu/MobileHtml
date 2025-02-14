<?xml version="1.0" encoding="UTF-8" ?>
<%@ Register tagprefix="xhtml" Namespace="xhtml_v3.Tools" Assembly="xhtml_v3" %>
<%@ Page language="c#" Codebehind="perso.aspx.cs" AutoEventWireup="false" Inherits="xhtml_v3.perso" %>
<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">
<html>
<head>
	<title>Im&aacute;genes</title>
	<link rel="stylesheet" href="./xhtml.css" type="text/css" />
    <meta forua="true" http-equiv="Cache-Control" content="no-cache, max-age=0, must-revalidate, proxy-revalidate, s-maxage=0" />
</head>
<body>
	<xhtml:XhtmlTable id="tbPerso" CssClass="normal" Runat="server">
		<xhtml:XhtmlTableRow id="rowTitle" Runat="server" />
		<xhtml:XhtmlTableRow id="rowTitle2" Runat="server" />
		<xhtml:XhtmlTableRow id="rowText" Runat="server" />
	</xhtml:XhtmlTable>
	<hr/>
	<xhtml:XhtmlTable id="tbPreviews" CssClass="normal" Runat="server"/>
    <xhtml:XhtmlTable id="tbContents" CssClass="normal" Runat="server">
		<xhtml:XhtmlTableRow id="rowContents" Runat="server" />
        <xhtml:XhtmlTableRow id="rowIMG" Runat="server" />
        <xhtml:XhtmlTableRow id="rowANIM" Runat="server" />
        <xhtml:XhtmlTableRow id="rowVIDEO" Runat="server" />
	</xhtml:XhtmlTable>                    
	<hr />
	<xhtml:XhtmlTable id="tbLinks" CssClass="normal" Runat="server"/>
	<hr />
    <img src="./Images/Gpar16.gif" alt="" /> <a href="./default.aspx">Atr&aacute;s</a><br/>
    <img src="./Images/Gemo16.gif" alt="" /> <a href="http://wap.movistar.com/descargas/?ID=21">Inicio</a>      
</body>
</html>
