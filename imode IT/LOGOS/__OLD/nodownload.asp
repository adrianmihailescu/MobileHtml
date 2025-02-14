<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_strUrlBack
Dim g_strNl

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
	'Call InitPageVariables()

End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strNl = CStr(Request.QueryString("nl"))

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
<div align="center"><img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"></div>
<p>
<% If IsEmpty(Request.QueryString("zl")) Then %>
Per scaricare questo prodotto, abbonati a Kiweescreens!
<% Else %>
Per scaricare subito altri KiweeScreens, vai su Disattivazione, scegli la Disattivazione Immediata e poi vai su Registrazione. 
<% End If %>
<br>
<a href="<% = Common_GetUrlBillingScreens("rel") %>">Disattivazione</a>
</p>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
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
