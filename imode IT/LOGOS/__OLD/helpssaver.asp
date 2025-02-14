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
<!--#include virtual="/Engine/API/Goodies.asp" -->
<!--#include virtual="/Engine/API/Subscription.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Call InitVariablesFromRequest()

	Call Goodies_SetMobileType( Common_GetMobileType() )
	Call Subscription_Init( IMODE_CONNECTIONSTRING, GOODIE_CONTENTGROUP_IMG)
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
	<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<div align="center"><img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"></div>
		<br>
		<table width="100%" cellspacing="0" cellpadding="0" border="0">
			<tr>
				<td bgcolor="#926DAA"><font color="white">Cos'&eacute; uno Sfondo Video? &#58999;</font></td>
			</tr>
		</table>
		<table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#B692AA">
			<tr>
				<td>
					I video kiweescreen sono dei contenuti video che possono essere impostati 
					in maniera da animare il tuo schermo ogni volta che lo attivi!
					<br>
					<br>
					Telefoni compatibili:<br>
					NEC N401i<br>
					NEC N410i<br>
					<br>
					Tariffa per Video:<br>
					2 crediti<br>
					<br>
				</td>
			</tr>
			<tr>
				<td bgcolor="#926DAA"><a name="activation"></a><font color="white">Come attivarlo? &#58999;</font></td>
			</tr>
			<tr>
				<td>
					Una volta scaricato, salva il video nella tua zona personale in "Video". Per 
					attribuirgli la funzione di screensaver, vai nel tuo spazio personale, poi in 
					"Video", poi in "Scaricati", seleziona il contenuto di tua scelta, premi su "menu", 
					"usa come", "Imp. come sfondo".<br>
					Fatto!
				</td>
			</tr>
		</table>
		<br>
		<table width="100%" cellspacing="3" cellpadding="0">
			<% Call Common_GetFooterScreens( "", 1 ) %>
			<tr>
				<td>&nbsp;</td>
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
