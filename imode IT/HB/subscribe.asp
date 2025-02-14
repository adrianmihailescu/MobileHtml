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
		<title>Hanna Barbera</title>
	</head>
<body bgcolor="#00F000" text="#ffffff" link="#ffffff" style="font-family:Verdana, Arial, Helvetica, sans-serif; font-size:x-small;">
		<div align="center">
			<img src="../images/hb<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"><br>
		</div>
		<table width="100%" cellspacing="0" cellpadding="0">
			<tr>
				<td bgcolor="#008000" align="center">Offerta e Condizioni</td>
			</tr>
		</table>
		<p>
			<!--- Abbonamento KiweeScreens: 6 crediti / 2 &#8364; (I.V.A. inclusa) al mese<br>-->
			- Abbonamento Hanna Barbera : <blink>6 crediti</blink> / 2 &#8364; (I.V.A. inclusa) al mese<br>
			(2 crediti per Sfondo o Animazione o 1 &#8364;) <br>(3 crediti o 1,5 &#8364; per 1 Video)
		</p>
		<table width="100%" cellspacing="3">
			<tr>
				<td>
					&#59017; <a href="conditions.asp" accesskey="1">Condizioni</a>
				</td>
			</tr>
		</table>
		<br>
		<table cellspacing="3" width="100%">
			<tr> 
				<td bgcolor="#008000">&#59114; <a href="default2.asp?uid=UIDREQUEST" accesskey="9">Hanna Barbera HomePage</a></td>
			</tr>	
		</table>
	</body>
</html>
<% Call Page_Terminate() %>
