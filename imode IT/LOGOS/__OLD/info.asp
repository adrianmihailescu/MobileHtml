<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_strUrlBack

'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Goodies_Constant.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Call InitVariablesFromRequest()
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default.asp?uid=UIDREQUEST"	
End Sub

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
%>
<html>
<head>
<title>KiweeScreens</title>

</head>

<body bgcolor="#FFFFFF">
<div align="center"> 
  <img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens">
  <table width="100%" cellspacing="3">
    <tr> 
      <td bgcolor="#FFFFCC"> 
        <div align="center">Info Editore</div>
      </td>
    </tr>
  </table>
</div>
<p align="left">
Questo servizio &eacute; fornito dalla societ&agrave; AG Interactive, societ&agrave;
per azioni con capitale sociale di 1 300 000 euro, registrata al Registro 
di commercio di Parigi con il numero B424.802.734 e la cui sede sociale 
&eacute; situata al 174, quai de Jemmapes, 75010 Parigi.
</p>
  
<table width="100%" cellspacing="0">
  <% Call Common_GetFooterScreens( "", 1 ) %>
</table>
<table cellspacing="3" width="100%">
	<tr> 
		<td>&nbsp;</td>
	</tr>		
	<tr>
		<td bgcolor="#FFFFCC">&#59147; <a href="<% = Common_GetUrlBillingScreens("reg") %>">Registrazione</a><br>
			&#59029; <a href="<% = Common_GetUrlBillingScreens("rel") %>">Disattivazione</a>
		</td>
	</tr>
	<tr> 
		<td bgcolor="#FFFFCC">&#59113; <a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a></td>
	</tr>
  <tr> 
    <td bgcolor="#FFFFCC">&#59114; <a href="default.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
  </tr>	
</table>
</body>
</html>
