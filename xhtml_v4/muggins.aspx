<?xml version="1.0" encoding="UTF-8" ?>
<%@ Register tagprefix="xhtml" Namespace="xhtml_v3.Tools" Assembly="xhtml_v3" %>
<%@ Page language="c#" Codebehind="muggins.aspx.cs" AutoEventWireup="false" Inherits="xhtml_v3.muggins" %>
<!DOCTYPE html PUBLIC "-//WAPFORUM//DTD XHTML Mobile 1.0//EN" "http://www.wapforum.org/DTD/xhtml-mobile10.dtd">
<html>
  <head>
                    <title>Im&aacute;genes</title>
                    <link rel="stylesheet" href="./xhtml.css" type="text/css" />
                    <meta forua="true" http-equiv="Cache-Control" content="no-cache, max-age=0, must-revalidate, proxy-revalidate, s-maxage=0" />
  </head>
          <body>
                    <Xhtml:XhtmlTable id="tbHeader" Runat="server" CssClass="normal" /> 
                    <a id="start" name="start" />
                    <Xhtml:XhtmlTable id="tbTitle" Runat="server" CssClass="normal">
						<Xhtml:XhtmlTableRow id="rowTitle" Runat="server" />
						<Xhtml:XhtmlTableRow id="rowPreviews" Runat="server" />						
                    </Xhtml:XhtmlTable>
                    
                    <% if (showForm1) { %>
                    <form method="post" action="./muggins.aspx">
						<table width="90%">
							<tr><td class="IMG">Tu sexo:</td></tr>
							<tr><td align="left"><select name="sex" >
								<option value="1" selected>Chico</option>
								<option value="2">Chica</option>
							</select></td></tr>
							<tr><td class="IMG">Color de pelo:</td></tr>
							<tr><td align="left"><select name="color">
								<option value="1" selected>Rubio</option>
								<option value="2">Casta&ntilde;o</option>
								<option value="3">Moreno</option>
							</select></td></tr>
							<tr><td class="IMG">Estilo:</td></tr>
							<tr><td align="left"><select name="style" >
								<option value="1" selected>Moderno</option>
								<option value="2">Cl&aacute;sico</option>
								<option value="3">Alternativo</option>
							</select></td></tr>							
							<tr><td align="left"><input type="submit" class="submit" value="Continuar" /></td></tr>
						</table>
					</form>
                    <% } else if (showForm2) {%>
                    <form method="post" action="./muggins.aspx">				
						<input type="hidden" name="a" value="<%= answer %>" /> 
						<table width="90%">
							<tr><td class="IMG">Tus complementos:</td></tr>
							<tr><td align="left"><select name="complementos">
								<% if (uno) { %><option value="1" selected>Tele</option><%}%>
								<% if (dos) { %><option value="2">Perro</option><%}%>
								<% if (tres) { %><option value="3">Gafas de sol</option><%}%>
								<% if (cuatro) { %><option value="4">Malet&iacute;n</option><%}%>
								<% if (cinco) { %><option value="5">Bal&oacute;n</option><%}%>
								<% if (seis) { %><option value="6">Gorra</option><%}%>
								<% if (siete) { %><option value="7">iPod</option><%}%>
								<% if (ocho) { %><option value="8">Cerveza</option><%}%>
							</select></td></tr>
							<tr><td align="left">	<input type="submit" class="submit" value="OK" /></td></tr>
						</table>
					</form>
					<% } else if (showForm3) {%>
                    <form method="post" action="./muggins.aspx">				
						<input type="hidden" name="a" value="<%= answer %>" /> 
						<table width="90%">
							<tr><td class="IMG">Tus complementos:</td></tr>
							<tr><td align="left"><select name="complementos">
								<% if (uno) { %><option value="1" selected>M&oacute;vil</option><%}%>
								<% if (dos) { %><option value="2">Tele</option><%}%>
								<% if (tres) { %><option value="3">Gafas de sol</option><%}%>
								<% if (cuatro) { %><option value="4">Bufanda</option><%}%>
								<% if (cinco) { %><option value="5">Mochila</option><%}%>
								<% if (seis) { %><option value="6">Gato</option><%}%>
								<% if (siete) { %><option value="7">iPod</option><%}%>
								<% if (ocho) { %><option value="8">Perro</option><%}%>
								<% if (nueve) { %><option value="9">Gorra</option><%}%>
							</select></td></tr>
							<tr><td align="left">	<input type="submit" class="submit" value="OK" /></td></tr>
						</table>
					</form>						
                    <% } %>
                    <Xhtml:XhtmlTable id="tbMuggins" Runat="server" CssClass="normal" />
					<table width="100%">
						<tr>
							<td align="center"><a href="./default.aspx" style="color: #696969">Im&aacute;genes y Fondos</a></td>
						</tr>
					</table>
					<p style="text-align:center; height:110%; background-repeat: no-repeat; background-position: top center; background-image:	url(<%=fondo%>); "> 
						<a href="http://10.132.67.244/buscador2/searcher.initsearch.do" class="imagen"><img src="<%=buscar%>" alt="" border="0"></a>&nbsp;
						<a href="http://wap.movistar.com" class="imagen"><img src="<%=emocion%>" alt="" border="0"></a>&nbsp;
						<a href="<%=atras%>" class="imagen"><img src="<%=back%>" alt="" border="0"></a>&nbsp;
						<a href="#start" class="imagen"><img src="<%=up%>" alt="" border="0"></a>
					</p>
          </body>
</html>
