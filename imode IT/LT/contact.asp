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
	If g_strUrlBack = "" Then g_strUrlBack = "default2.asp?uid=UIDREQUEST"	
End Sub

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
%>
<html>
<head>
<title>Looney Tunes</title>

</head>

<body bgcolor="#99CCFF" text="#ffffff" link="#ffffff" style="font-family:Verdana, Arial, Helvetica, sans-serif; font-size:x-small;">
<div align="center"> 
	<img src="../images/lt<% = Common_GetMobileWidth() %>.gif" alt="Looney Tunes">
  <table width="100%" cellspacing="3">
    <tr> 
      <td bgcolor="#0000FF"> 
        <div align="center">Contatti</div>
      </td>
    </tr>
  </table>
</div>
<p align="left">
Questo servizio &eacute; fornito dalla societ&agrave; AG Interactive, societ&agrave;
per azioni con capitale sociale di 1 300 000 euro, registrata al Registro 
di commercio di Parigi con il numero B424.802.734 e la cui sede sociale 
&eacute; situata al 174, quai de Jemmapes, 75010 Parigi.
<br>
Per qualsiasi domanda riguardante il Servizio vi preghiamo di scriverci 
all'indirizzo: AG Interactive - Service Client - 174, Quai de Jemmapes - 75010 Parigi - Francia o per email a <a href="mailto:kiweeservizioclienti@ag.com">kiweeservizioclienti@ag.com</a> (Indicando il seguente codice: <% = Common_GetUID() %>).<br>
</p>
  

<table cellspacing="3" width="100%">
	<tr> 
		<td>&nbsp;</td>
	</tr>		
	<tr>
		<td bgcolor="#0000FF">&#59147; <a href="<% = Common_GetUrlBillingLT("reg") %>">Registrazione</a><br>
			&#59029; <a href="<% = Common_GetUrlBillingLT("rel") %>">Disattivazione</a>
		</td>
	</tr>
	<tr> 
		<td bgcolor="#0000FF">&#59113; <a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a></td>
	</tr>
  <tr> 
    <td bgcolor="#0000FF">&#59114; <a href="default2.asp?uid=UIDREQUEST" accesskey="9">Looney Tunes HomePage</a></td>
  </tr>	
</table>
</body>
</html>
