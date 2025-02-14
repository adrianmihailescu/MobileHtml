<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico
Dim g_lngIDContent
Dim g_strContentGroup
Dim g_strContentType
Dim g_lngIDService
Dim g_lngIDSendTo
Dim g_strUID

Dim g_strNameSender
Dim g_strNameRecipient
Dim g_strEmailRecipient
Dim g_strMessage
Dim g_strBody

Dim g_strReferer
Dim g_strUrlBack
Dim g_strErr
Dim g_boolSuccess

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
	Call InitPageVariables()

	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, g_strContentGroup, g_strContentType)
	Call Subscription_Init( Common_GetConnectionString(), GOODIE_CONTENTGROUP_IMG )
	'Response.Write Common_GetCurrentRelativeURL()
	
	Set g_objDico = Common_LoadPageText("SCREENS")
End Sub

Sub InitPageVariables()
	g_boolSuccess = False
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strContentGroup = CStr(Request("cg"))
	If g_strContentGroup = "" Then g_strContentGroup = GOODIE_CONTENTGROUP_IMG

	g_strContentType = CStr(Request("ct"))
	If g_strContentType = "" Then g_strContentType = GOODIE_CONTENTTYPE_IMG_COLOR
	
	g_lngIDService = CLng(Request("s"))
	
	g_strReferer = CStr(Request("ref"))
	
	g_lngIDContent = CLng(Request("c"))
	
	g_strUrlBack = CStr(Request("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "view.asp?uid=UIDREQUEST&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&r=" & g_lngIDContent	

	'Param du formulaire	
	g_strNameSender = Trim(CStr(Request("ns")))
	g_strNameRecipient = Trim(CStr(Request("nr")))
	g_strEmailRecipient = Trim(CStr(Request("e")))
	g_strMessage = Trim(CStr(Request("m")))
	
	g_strUID = Common_GetUID()	
End Sub

Sub Check()
	Dim l_objRegExp
	
	'NameSender
	If g_strNameSender = "" Then
		g_strErr = "Devi inserire il tuo nome"
		Exit Sub
	End If
	
	'NameRecipient
	If g_strNameRecipient = "" Then
		g_strErr = "Devi inserire il nome del tuo amico"
		Exit Sub
	End If	
	
	Set l_objRegExp = New RegExp
	l_objRegExp.Pattern = "^[\w\-\.]+@(libero.it|inwind.it|blu.it|iol.it|imode.fr)$"
	
	If Not l_objRegExp.Test(g_strEmailRecipient) Then
		g_strErr = "Indirizzo mail non valido"
	End If
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
End Sub


'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
If Not IsEmpty(Request.Form("e")) Then
	Call Check()
	If g_strErr = "" Then

		If Subscription_InsertSendTo(	g_strUID, g_lngIDService, g_lngIDContent, g_strContentGroup, g_strContentType, _
																		g_strMessage, g_strNameSender, g_strNameRecipient, g_strEmailRecipient, g_lngIDSendTo ) Then	
			'Insertion dans la table des SendTo OK																		
			
			If Subscription_Download( g_lngIDService, g_strUID, g_objDico(g_strContentGroup & "_CREDIT")) = 0 Then
				'Débit du compte OK
				
				If g_strMessage <> "" Then
					g_strBody = g_strMessage & "<br>" & g_strNameSender & ",<br><br>"
				End If
				
				g_strBody = g_strBody & _
										"Ciao " & g_strNameRecipient & ",<br>" & _
										g_strNameSender & " ti ha regalato un'immagine da utilizzare come sfondo o screensaver per il tuo cellulare i-mode.<br>" & _
										"Scaricala gratuitamente, cliccando sul link qui sotto.<br>" & _
										"http://" & Request.ServerVariables("HTTP_HOST") & "/logos/getfrom.asp?g=" & g_lngIDSendTo
										
				'Call Common_SendMail( "0665091358@imode.fr", "Hai ricevuto un'immagine!", g_strBody, "true", "UTF-8" )
				Call Common_SendMail( g_strEmailRecipient, g_strNameSender & " ti ha regalato uno sfondo!", g_strBody, "true", "UTF-8" )
				g_boolSuccess = True
			Else
				g_strErr = "Servizio indisponibile. Riprova pi&ugrave; tardi grazie."
			End If

		Else
			g_strErr = "Servizio indisponibile. Riprova pi&ugrave; tardi grazie."
		End If																	
	End If
End If
%>
<html>
<head>
<title>KiweeScreens</title>
</head>
<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<div align="center"><img src="../images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"></div>
<table width="100%" cellspacing="0" bgcolor="#0092FF">
	<tr>
		<td align="center">
			<font color="white">&#59091; Invia a chi vuoi tu!</font>
		</td>
	</tr>
</table>
<% If Not g_boolSuccess Then %>
<form method="post" name="Send" action="frmsendto.asp">
<input type="hidden" name="c" value="<% = g_lngIDContent %>">
<input type="hidden" name="cg" value="<% = g_strContentGroup %>">
<input type="hidden" name="ct" value="<% = g_strContentType %>">
<input type="hidden" name="s" value="<% = g_lngIDService %>">
<input type="hidden" name="uid" value="UIDREQUEST">
<table width="100%" border="0" cellspacing="0" cellpadding="1">
	<% If g_strErr <> "" Then %>
	<tr><td><font color="red"><% = g_strErr %></font></td></tr>
	<% End If %>
	<tr>
		<td>
			Il tuo nome<br>
			<input type="text" name="ns" maxlength="50" value="<% = g_strNameSender %>">
		</td>
	</tr>
	<tr>
		<td>
			Il nome del tuo amico<br>
			<input type="text" name="nr" maxlength="50" value="<% = g_strNameRecipient %>">
		</td>
	</tr>
	<tr>
		<td>
			L'indirizzo mail del tuo amico cliente i-mode<br>
			<font size="1">(@libero.it, @inwind.it, @blu.it e @iol.it)</font><br>
			<input type="text" name="e" maxlength="100" value="<% = g_strEmailRecipient %>">
		</td>
	</tr>
	<tr>
		<td>
			Se vuoi inserisci una dedica da inviare con l'immagine<br>
			<input type="text" name="m" maxlength="500" value="<% = g_strMessage %>">
		</td>
	</tr>
	<tr>
		<td><input type="submit" value="&#59091; Invia"><br><br></td>
	</tr>
	<tr>
		<td bgcolor="#0092FF">&#59091; <a href="helpsendto.asp?b=<% = Common_GetCurrentRelativeUrl() %>">Come funziona?</a></td>
	</tr>
	<tr>
		<td>
			<br><a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a>
		</td>
	</tr>
</table>
</form>
<% Else %>
<table width="100%" border="0" cellspacing="0" cellpadding="1">
	<tr><td>Lo sfondo &eacute; stato correttamente inviato al tuo amico.</td></tr>
</table>
<% End If %>
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
    <td bgcolor="#FFFFCC">&#59114; <a href="default.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
  </tr>	
</table>
</body>
</html>
<% Call Page_Terminate() %>