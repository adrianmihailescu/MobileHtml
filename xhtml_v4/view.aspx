<?xml version="1.0" encoding="UTF-8" ?>
<%@ Page language="c#" Codebehind="view.aspx.cs" AutoEventWireup="false" Inherits="xhtml.view" %>
<%@ Register tagprefix="xhtml" Namespace="xhtml.Tools" Assembly="xhtml_v4" %>

<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">
<html>
          <head>
                    <title>Im&aacute;genes</title>
                    <link rel="stylesheet" href="./xhtml.css" type="text/css" />
                    <meta forua="true" http-equiv="Cache-Control" content="no-cache, max-age=0, must-revalidate, proxy-revalidate, s-maxage=0" />
          </head>
          <body>
                     <a id="start" name="start" />
                    <Xhtml:XhtmlTable id="tbHeader" Runat="server" CssClass="normal" />
                    <Xhtml:XhtmlTable id="tbTitle" Runat="server" CssClass="normal" />
                    <Xhtml:XhtmlTable id="tbPreviews" Runat="server" CssClass="normal">
						<Xhtml:XhtmlTableRow id="rowPreview" Runat="server" />
						<Xhtml:XhtmlTableRow id="rowLinkPPD" Runat="server" />
                    </Xhtml:XhtmlTable>

                    <Xhtml:XhtmlTable id="tbContentset" Runat="server" CssClass="normal"/>
                    <Xhtml:XhtmlTable id="tbLinks" Runat="server" CssClass="normal"/>

                    <hr />
                    <table width="100%">
						<tr>
							<td align="center"><a href="./default.aspx" style="color: #696969">Im&aacute;genes y Fondos</a></td>
						</tr>
					</table>
					<p style="text-align:center; height:110%; background-repeat: no-repeat; background-position: top center; background-image:	url(<%=fondo%>); "> 
						<a href="http://10.132.67.244/buscador2/searcher.initsearch.do" class="imagen"><img src="<%=buscar%>" alt="" border="0" /></a>&nbsp;
						<a href="http://wap.movistar.com" class="imagen"><img src="<%=emocion%>" alt="" border="0" /></a>&nbsp;
						<a href="<%=atras%>" class="imagen"><img src="<%=back%>" alt="" border="0" /></a>&nbsp;
						<a href="#start" class="imagen"><img src="<%=up%>" alt="" border="0" /></a>
					</p>                                        
          </body>
</html>
