<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico
Dim g_strUrlBack

'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Goodies.asp" -->
<!--#include virtual="/Engine/API/Subscription.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Set g_objDico = Common_LoadPageText( "SCREENS" )

	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default.asp"
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
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
	<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<div align="center"><img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"></div>
		<table width="100%" cellspacing="0" bgcolor="#0092FF">
			<tr>
				<td align="center">
					<font color="white">&#59091; Invia a chi vuoi tu!</font>
				</td>
			</tr>
		</table>		
		<table width="100%" cellspacing="0">
			<tr>
				<td>
				Invia ai tuoi amici gli sfondi pi&ugrave; cool e sexy dell'estate, i messaggi 
				d'amore e i cartoni animati, le dediche pi&ugrave; trash!
				<br>
				<br>
				I tuoi amici potranno a loro volta utilizzare le immagini ricevute come sfondo o screensaver.
				<br>
				<br>
				La ricezione dell'immagine &egrave; destinata unicamente ai clienti i-mode che dispongono di un indirizzo di posta elettronica 
				del dominio Wind (@libero.it, @inwind.it, @blu.it o @iol.it).
					<br>
					<a href="<% = g_strUrlBack %>">Indietro</a>
				</td>
			</tr>
		</table>
		<br>
<table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="#FFFFCC">
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
<% Call Page_Terminate() %>
 