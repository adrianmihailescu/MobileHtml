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
	<table width="100%" cellspacing="0" bgcolor="<% = g_objDico("VIDEO_RGT_COLOR_DARK") %>">
		<tr>
			<td align="center">
				<% = g_objDico("VIDEO_RGT_EMOJI") %> <a name="part1"></a><font color="white">Cos'&eacute; una VideoSuoneria?</font>
			</td>
		</tr>
	</table>		
		<table width="100%" cellspacing="0">
			<tr>
				<td>
				Le VideoSuonerie Kiwee sono video (immagini e suoni di alta qualit&agrave;) che puoi impostare come sfondo o suoneria sul tuo cellulare.
				Se impostato come suoneria il tuo video apparir&agrave; sullo schermo del tuo cellulare quando ricevi una chiamata. Se impostato come sfondo il 
				tuo video apparir&agrave; ogni volta che apri lo sportellino del tuo cellulare.
				Scarica e divertiti con le videosuonerie Kiwee.
					<br>
					<a href="<% = g_strUrlBack %>">Indietro</a>
				</td>
			</tr>
		</table>
	<table width="100%" cellspacing="0" bgcolor="<% = g_objDico("VIDEO_RGT_COLOR_DARK") %>">
		<tr>
			<td align="center">
				<% = g_objDico("VIDEO_RGT_EMOJI") %> <a name="part2"></a><font color="white">Come attivarla?</font>
			</td>
		</tr>
	</table>		
		<table width="100%" cellspacing="0">
			<tr>
				<td>
					Una volta scaricata, salva la VideoSuoneria nella tua zona personale in "Video". Per attribuirgli la funzione di sfondo o suoneria, 
					vai nel tuo spazio personale, poi in "Video", poi in "Scaricati", seleziona il contenuto di tua scelta, premi su "menu", "usa come", 
					"Imp. come sfondo"o "Imp. come Suoneria" . Fatto!
					<br>
					<a href="<% = g_strUrlBack %>">Indietro</a>
				</td>
			</tr>
		</table>		
		<br>
<table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="<% = g_objDico("VIDEO_RGT_COLOR_DARK") %>">
	<% Call Common_GetFooterScreens( "", 1 ) %>
</table>
<br>
<marquee bgcolor="#0092FF"><font color="white"><% = g_objDico("TXT_TONES1") %></marquee>
<blink>&#59101;</blink> <a href="/ringtones/catalog.asp?cg=COMPOSITE&ctd=SOUND_POLY"><% = g_objDico("TXT_TONES2") %></a>
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
 