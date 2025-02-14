<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************


'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Subscription.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()

End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next

End Sub


Sub Page_Terminate()
	On Error Resume Next
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
		<div align="center">
			<img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens">
		</div>
		<table width="100%" cellspacing="0" cellpadding="0">
			<tr>
				<td bgcolor="#FFB6AA" align="center">Offerta e Condizioni</td>
			</tr>
		</table>
		<p>
			<!--- Abbonamento KiweeScreens: 3 crediti / 2 &#8364; (I.V.A. inclusa) al mese<br>-->
			- Abbonamento KiweeScreens : <blink>3 crediti</blink> / 2 &#8364; (I.V.A. inclusa) al mese<br>
			(1 credito per Sfondo o Animazione o 1 &#8364;)<br>(2 crediti o 1,5 &#8364; per 1 Video)
			<br><br>Rinventa il tuo schermo tutte le volte che vuoi, scopri ogni giorno nuovi kiweescreens!   <!--, 2 crediti per Video)-->

		</p>
		<table width="100%" cellspacing="3">
			<tr>
				<td>
					&#59017; <a href="conditions.asp" accesskey="1">Condizioni</a>
				</td>
			</tr>
			<tr>
				<td>&#59147; <a href="<% = Common_GetUrlBillingScreens("reg") %>">Registrazione</a></td>
			</tr>				
		</table>
		<br>
		<table cellspacing="3" width="100%">
			<tr> 
				<td bgcolor="#FFFFCC">&#59114; <a href="default2.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
			</tr>	
		</table>
	</body>
</html>
<% Call Page_Terminate() %>
