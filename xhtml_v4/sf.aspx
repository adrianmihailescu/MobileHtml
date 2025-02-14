<?xml version="1.0" encoding="UTF-8" ?>
<%@ Page language="c#" Codebehind="sf.aspx.cs" AutoEventWireup="false" Inherits="xhtml.sf" %>
<%@ Register tagprefix="xhtml" Namespace="xhtml.Tools" Assembly="xhtml_v3" %>
<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">
<html>
  <head>
                    <title>San Ferm&iacute;n</title>
                    <link rel="stylesheet" href="xhtml.css" type="text/css" />
                    <meta forua="true" http-equiv="Cache-Control" content="no-cache, max-age=0, must-revalidate, proxy-revalidate, s-maxage=0" />
  </head>
          <body>
                    <a id="start" name="start" />
                    <Xhtml:XhtmlTable id="tbHeader" Runat="server" CssClass="normal" />
                    <Xhtml:XhtmlTable id="tbTitle" Runat="server" CssClass="normal" />

					<Xhtml:XhtmlTable id="Xhtmltable1" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
						<Xhtml:XhtmlTableRow id="rowJuego" runat="server">
							<Xhtml:XhtmlTableCell Runat="server" ID="cellImgTV" />
							<Xhtml:XhtmlTableCell Runat="server" ID="cellLinkTV" />		
						</Xhtml:XhtmlTableRow>
						<Xhtml:XhtmlTableRow id="Xhtmltablerow1" runat="server">
							<Xhtml:XhtmlTableCell Runat="server" ID="cellImgGame" />
							<Xhtml:XhtmlTableCell Runat="server" ID="cellLinkGame" />		
						</Xhtml:XhtmlTableRow>
						<Xhtml:XhtmlTableRow id="Xhtmltablerow2" runat="server">
							<Xhtml:XhtmlTableCell Runat="server" ID="cellImgImg" />
							<Xhtml:XhtmlTableCell Runat="server" ID="cellLinkImg" />		
						</Xhtml:XhtmlTableRow>
					</Xhtml:XhtmlTable>
                    
                    <Xhtml:XhtmlTable id="tbImagenes" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" />
                    
                    <Xhtml:XhtmlTable id="Xhtmltable2" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
						<Xhtml:XhtmlTableRow id="Xhtmltablerow3" runat="server">
							<Xhtml:XhtmlTableCell Runat="server" ID="cellImgMusic" />
							<Xhtml:XhtmlTableCell Runat="server" ID="cellLinkMusic" />		
						</Xhtml:XhtmlTableRow>
					</Xhtml:XhtmlTable>
                    
                    <Xhtml:XhtmlTable id="tbMusic" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" />
                    
                    <Xhtml:XhtmlTable id="Xhtmltable3" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
						<Xhtml:XhtmlTableRow id="Xhtmltablerow4" runat="server">
							<Xhtml:XhtmlTableCell Runat="server" ID="cellImgAnim" />
							<Xhtml:XhtmlTableCell Runat="server" ID="cellLinkAnim" />		
						</Xhtml:XhtmlTableRow>
					</Xhtml:XhtmlTable>
                    
                    <Xhtml:XhtmlTable id="tbAnims" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" />
                    
                    <Xhtml:XhtmlTable id="Xhtmltable5" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
						<Xhtml:XhtmlTableRow id="Xhtmltablerow5" runat="server">
							<Xhtml:XhtmlTableCell Runat="server" ID="cellImgVideo" />
							<Xhtml:XhtmlTableCell Runat="server" ID="cellLinkVideo" />		
						</Xhtml:XhtmlTableRow>
					</Xhtml:XhtmlTable>
                    
                    <hr />
                    <table width="100%">
						<tr>
							<td align="center"><a href="http://www.marketingmovil.com:8080/portal/ServletAction?idaccion=2994" style="COLOR: #696969">Especial Verano</a></td>
						</tr>
					</table>
					<p style="BACKGROUND-POSITION:center top; BACKGROUND-IMAGE:url(<%=fondo%>); BACKGROUND-REPEAT:no-repeat; HEIGHT:110%; TEXT-ALIGN:center"> 
						<a href="http://10.132.67.244/buscador2/searcher.initsearch.do" class="imagen"><img src="<%=buscar%>" alt="" border="0" ></a>&nbsp;
						<a href="http://wap.movistar.com" class="imagen"><img src="<%=emocion%>" alt="" border="0" ></a>&nbsp;
						<a href="<%=atras%>" class="imagen"><img src="<%=back%>" alt="" border="0" ></a>&nbsp;
						<a href="#start" class="imagen"><img src="<%=up%>" alt="" border="0" ></a>
					</p>   
		</body>
</html>
