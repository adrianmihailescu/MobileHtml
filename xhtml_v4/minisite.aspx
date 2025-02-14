<?xml version="1.0" encoding="UTF-8" ?>
<%@ Page language="c#" Codebehind="minisite.aspx.cs" AutoEventWireup="false" Inherits="xhtml.minisite" %>
<%@ Register tagprefix="xhtml" Namespace="xhtml.Tools" Assembly="xhtml_v4" %>
<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">
<html>
	<head>
		<title>
			<%= title %>
		</title>
		<link rel="stylesheet" href="<%=css%>" type="text/css" />
			<meta forua="true" http-equiv="Cache-Control" content="no-cache, max-age=0, must-revalidate, proxy-revalidate, s-maxage=0" />
	</head>
	<body>
		<a id="start" name="start" />
			<xhtml:XhtmlTable id="tbHeader" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0" />
			<xhtml:XhtmlTable id="tbHeaderDestacados" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0" />
			<xhtml:XhtmlTable id="tbImages" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
				<xhtml:XhtmlTableRow Runat="server" ID="rowImages" />
				<xhtml:XhtmlTableRow Runat="server" ID="rowImg" />
				<xhtml:XhtmlTableRow Runat="server" ID="rowTitlesImg" />
			</xhtml:XhtmlTable>
			<xhtml:XhtmlTable id="tbTop" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2"></xhtml:XhtmlTable>
			<xhtml:XhtmlTable id="tbHeader2" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0" />
			<xhtml:XhtmlTable id="tbAnims" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
				<xhtml:XhtmlTableRow Runat="server" ID="rowAnims" />
			</xhtml:XhtmlTable>
			<xhtml:XhtmlTable id="tbHeader3" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0" />
			<xhtml:XhtmlTable id="tbVideos" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
				<xhtml:XhtmlTableRow Runat="server" ID="rowVideos" />
			</xhtml:XhtmlTable>
			<xhtml:XhtmlTable id="tbTemas" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" />
			<xhtml:XhtmlTable id="tbLinks" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" />
			<hr />
			<table width="100%">
				<tr>
					<td align="center"><a href="./default.aspx" style="color: #696969">Im&aacute;genes y Fondos</a></td>
				</tr>
			</table>
			<p style="background-position:center top; background-image:url(<%=fondo%>); background-repeat:no-repeat; height:110%; text-align:center">
				<a href="http://10.132.67.244/buscador2/searcher.initsearch.do" alt="" class="imagen">
					<img src="<%=buscar%>" alt="" border="0" ></a>&nbsp; <a href="http://wap.movistar.com" alt="" class="imagen">
					<img src="<%=emocion%>" alt="" border="0" ></a>&nbsp; <a href="http://wap.movistar.com" alt="" class="imagen">
					<img src="<%=back%>" alt="" border="0" ></a>&nbsp; <a href="#start" class="imagen">
					<img src="<%=up%>" alt="" border="0" ></a>
			</p>
	</body>
</html>
