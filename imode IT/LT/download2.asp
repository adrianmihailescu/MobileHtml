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
Dim g_lngIDService

Dim g_strResult
Dim g_strUrl
Dim g_strContentName
Dim g_lngIDTicket
Dim g_strReferer
Dim g_lngCredit

Dim g_boolFree
Dim g_strNav

Dim g_strContentGroup
Dim g_strContentType
Dim bk

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
	Set g_objDico = Common_LoadPageText("LT")
	Call InitVariablesFromRequest()
	Call InitPageVariables()
	
	Call Goodies_Init( IMODE_DISPLAY_KEY_LT, g_strContentGroup, g_strContentType)
	Call Subscription_Init( Common_GetConnectionString(), GOODIE_CONTENTGROUP_IMG )
End Sub

Sub Page_Terminate() 
	'On Error Resume Next
	Set g_objDico = Nothing
End Sub

Sub InitVariablesFromRequest()
	'On Error Resume Next
	g_strContentGroup = CStr(Request.QueryString("arg2"))
	If g_strContentGroup = "" Then g_strContentGroup = "IMG"		
	
	If g_strContentGroup = "IMG" Then 
		g_strContentType = "IMG_COLOR"
	ElseIf g_strContentGroup = "ANIM" Then 
		g_strContentType = "ANIM_COLOR"
	ElseIf g_strContentGroup = "SFX" Then 
		g_strContentType = "SOUND_FX"
	ElseIf g_strContentGroup = "VIDEO" Then 
		g_strContentType = "VIDEO_DWL"
	ElseIf g_strContentGroup = "VIDEO_RGT" Then 
		g_strContentType = "VIDEO_CLIP"
	End If
	
	g_lngIDContent = CLng(Request.QueryString("arg1"))
	if (g_strContentGroup="VIDEO" or g_strContentGroup = "VIDEO_RGT") Then
		g_lngIDService = 50
		g_strReferer = "Pay_1_5"
		bk = IMODE_BILLING_KEY_1_5
	else
		g_lngIDService = 49
		g_strReferer = "Pay_1"
		bk = IMODE_BILLING_KEY_1
	end if	
End Sub


Sub InitPageVariables()
	'On Error Resume Next
	g_lngCredit = CLng(g_objDico("CREDIT_" & g_strContentType))
	If g_lngCredit = 0 Then g_lngCredit = 1
End Sub

Sub GetExtendedTicketFile()
	Dim l_strXMl
	Dim l_objXML
	On Error Resume Next
	If Goodies_GetContentTicketFileEx( bk, g_strReferer, g_lngIDContent, Common_GetMobileType(), l_strXML) Then
		Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
		l_objXML.loadXML l_strXML
			
		g_strResult = l_objXML.selectSingleNode("./KMWebService/ResultCode").text
		g_strUrl = l_objXML.selectSingleNode("./KMWebService/Ticket/Data/block").text		
		g_strContentName = l_objXML.selectSingleNode("./KMWebService/Ticket/Title").text 
		g_lngIDTicket = l_objXML.selectSingleNode("./KMWebService/Ticket/IDTicket").text 
		
		g_strUrl = IMODE_URL & "/logos/temp/" & g_strUrl
				
		Set l_objXML = Nothing
	Else
		g_strResult = "NOK"
	End If
End Sub

Sub GetExtendedTicket()
	Dim l_strXMl
	Dim l_objXML
	On Error Resume Next
	
	If Goodies_GetContentTicketExtended( bk, g_strReferer, g_lngIDContent, Common_GetMobileType(), l_strXML) Then
		Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
		l_objXML.loadXML l_strXML
			
		g_strResult = l_objXML.selectSingleNode("./KMWebService/ResultCode").text
		g_strUrl = l_objXML.selectSingleNode("./KMWebService/Ticket/URL").text 
		g_strContentName = l_objXML.selectSingleNode("./KMWebService/Ticket/Title").text 
		g_lngIDTicket = l_objXML.selectSingleNode("./KMWebService/Ticket/IDTicket").text 
		
		Set l_objXML = Nothing
	Else
		g_strResult = "NOK1"
	End If
End Sub

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()

If Instr(1, Common_GetMobileType(), "SAGEM") <> 0 Then 
	Call GetExtendedTicketFile() 
Else 
	Call GetExtendedTicket()
End If

If g_strResult = "OK" Then
		Call Subscription_InsertDownload( g_lngIDService, Common_GetUID(), g_strContentName, g_strContentGroup, g_strContentType, Common_GetMobileType(), g_lngIDTicket, g_strReferer )
	If (g_strContentGroup = "VIDEO_RGT" Or g_strContentGroup = "SOUND" Or g_strContentGroup = "SFX") And g_strResult = "OK" Then
		Response.Redirect g_strUrl
	End If
End If
%>
<html>
<head>
<title>Looney Tunes</title>
</head>
<body bgcolor="#99CCFF" text="#ffffff" link="#ffffff" style="font-family:Verdana, Arial, Helvetica, sans-serif; font-size:x-small;">
	<p align="center"> 
	  <% If g_strResult <> "OK" Then %> 
	            Si &eacute; verificato un problema tecnico. Alcun credito ti &eacute; stato fatturato.
    <% Else %> 
		<% If (g_strContentGroup = "VIDEO_RGT" Or g_strContentGroup = "SOUND" Or g_strContentGroup = "SFX") And g_strResult = "OK" Then %>
			<br>Per salvare il video, dopo averlo scaricato, utilizza l'apposita funzione del menu 'opzioni' del tuo telefono<br>
			<a href="<% = g_strURL %>">Scarica</a><br>
		<% Else %>
			  <br>
			  Per salvare l'immagine, vai su Menu - Salva / Registra l'immagine. L'immagine appare in un quadro colorato, clicca sull'immagine e fai Salva.
				<br>
			  <img src="<% = g_strURL %>"> 
		<% End If %> 
	<% End If %>
  </p>
	<table width="100%" cellspacing="3">
		<tr> 
			   <td bgcolor="#0000FF">&#59114; <a href="default2.asp?uid=UIDREQUEST" accesskey="9">Looney Tunes HomePage</a></td>
		</tr>	
	</table>
</body>
</html>
<% Call Page_Terminate() %>