<?xml version="1.0" encoding="UTF-8" ?>
<%@ Register tagprefix="xhtml" Namespace="xhtml.Tools" Assembly="xhtml_v4" %>
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="xhtml._default" %>
<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">
<html>
  <head>
          <title>Im&aacute;genes y Fondos</title>
          <link rel="stylesheet" href="xhtml.css" type="text/css" />
          <meta forua="true" http-equiv="Cache-Control" content="no-cache, max-age=0, must-revalidate, proxy-revalidate, s-maxage=0" />  
  </head>
<body>          
		  <a id="start" name="start" />
		  <xhtml:XhtmlTable id="tbHeader" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0"/>    
		  
		 <xhtml:XhtmlTable id="tbPub" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0" />
		 
		 <xhtml:XhtmlTable id="tbHeaderAhora" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0" />    
		<xhtml:XhtmlTable id="tbAhora" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
					<xhtml:XhtmlTableRow Runat="server" ID="rowEspecial">
						<xhtml:XhtmlTableCell Runat="server" ID="cellImg" />
						<xhtml:XhtmlTableCell Runat="server" ID="cellLink" />						
                    </xhtml:XhtmlTableRow>
					<xhtml:XhtmlTableRow Runat="server" ID="rowEspecial2">
						<xhtml:XhtmlTableCell Runat="server" ID="cellImg2" />
						<xhtml:XhtmlTableCell Runat="server" ID="cellLink2" />						
                    </xhtml:XhtmlTableRow>                    
					<xhtml:XhtmlTableRow Runat="server" ID="rowEspecial3">
						<xhtml:XhtmlTableCell Runat="server" ID="cellImg3" />
						<xhtml:XhtmlTableCell Runat="server" ID="cellLink3" />						
                    </xhtml:XhtmlTableRow>                    
          </xhtml:XhtmlTable>
			<xhtml:XhtmlTable id="tbAhora2" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" />
			 
		<xhtml:XhtmlTable id="tbHeaderDestacados" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0"/>    
		 <xhtml:XhtmlTable id="tbImages" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
					<xhtml:XhtmlTableRow Runat="server" ID="rowImages" />
                    <xhtml:XhtmlTableRow Runat="server" ID="rowImg" />
                    <xhtml:XhtmlTableRow Runat="server" ID="rowTitlesImg" />
          </xhtml:XhtmlTable>

          <xhtml:XhtmlTable id="tbTop" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" >
                    <xhtml:XhtmlTableRow Runat="server" ID="rowTop" />
          </xhtml:XhtmlTable>         
		  
         
          <xhtml:XhtmlTable id="tbHeaderEnd" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0" />    
			<xhtml:XhtmlTable id="tbEnd" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" />		 
			<xhtml:XhtmlTable id="tbEnd2" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2" />	
		
			<form method="get" action="<%=search%>">
				<input type="hidden" name="db" value="vimages" />
				<input type="text" name="q" />
				<input type="submit" value="Buscar" />
				<font color="#000000">Busca tu imagen</font>
		  </form>  
		   
		<xhtml:XhtmlTable id="tbTitleShop" Runat="server" CssClass="normal" cellspacing="0" cellpadding="0"/>
          <xhtml:XhtmlTable id="tbShops" Runat="server" CssClass="normal" cellspacing="2" cellpadding="2">
            <xhtml:XhtmlTableRow Runat="server" ID="rowTitleShops" />
			<xhtml:XhtmlTableRow Runat="server" ID="rowShop1">
				<xhtml:XhtmlTableCell Runat="server" id="cellShop1" />
			</xhtml:XhtmlTableRow>
			<xhtml:XhtmlTableRow Runat="server" ID="Xhtmltablerow1">
				<xhtml:XhtmlTableCell Runat="server" id="cellShop2" />
			</xhtml:XhtmlTableRow>
			<xhtml:XhtmlTableRow Runat="server" ID="rowShop2">
				<xhtml:XhtmlTableCell Runat="server" id="cellShop3" />
			</xhtml:XhtmlTableRow>
			<xhtml:XhtmlTableRow Runat="server" ID="Xhtmltablerow2">
				<xhtml:XhtmlTableCell Runat="server" id="cellShop4" />
			</xhtml:XhtmlTableRow>
			<xhtml:XhtmlTableRow Runat="server" ID="rowmoreshops" />	
          </xhtml:XhtmlTable>	 
          
          <xhtml:XhtmlTable id="tbHeaderApuntante" CssClass="normal" Runat="server" cellspacing="0" cellpadding="0" />    
		  <xhtml:XhtmlTable id="tbEnd3" CssClass="normal" Runat="server" cellspacing="2" cellpadding="2" />    
 
		<hr/>
		<table width="100%">
			<tr>
				<td align="center"><a href="http://www.todoloultimo.net/musica_movistar" style="color: #696969"><%=musica%></a> | <a href="http://wap.movistar.com/descargas/?ID=11" style="color: #696969">Juegos</a></td>
			</tr>
			<tr>
				<td align="center"><a href="http://10.236.7.247:13081/RBTWAP/wap/inicio.jsp" style="color: #696969">Yavoy</a> | <a href="http://www.marketingmovil.com:8080/portal/hmr/ServletAction?idaccion=3122" style="color: #696969">Humor</a> </td>
			</tr>
		</table>

		<p style="text-align:center; height:110%; background-repeat: no-repeat; background-position: top center; background-image:	url(<%=fondo%>); "> 
			<a href="http://10.132.67.244/buscador2/searcher.initsearch.do" alt="" class="imagen"><img src="<%=buscar%>" alt="" border="0" /></a>&nbsp;
			<a href="http://wap.movistar.com" alt="" class="imagen"><img src="<%=emocion%>" alt="" border="0" /></a>&nbsp;
			<a href="http://wap.movistar.com" alt="" class="imagen"><img src="<%=back%>" alt="" border="0" /></a>&nbsp;
			<a href="#start" class="imagen"><img src="<%=up%>" alt="" border="0" /></a>
		</p>
		
</body>
</html>
