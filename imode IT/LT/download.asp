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
	Call Subscription_Init( Common_GetConnectionString(), "LT" )
End Sub


Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strContentGroup = CStr(Request.QueryString("cg"))
	If g_strContentGroup = "" Then g_strContentGroup = GOODIE_CONTENTGROUP_IMG		
	
	g_strContentType = CStr(Request.QueryString("ct"))
	If g_strContentType = "" Then g_strContentType = GOODIE_CONTENTTYPE_IMG_COLOR
	
	g_lngIDContent = CLng(Request.QueryString("r"))
	g_lngIDService = CLng(Request.QueryString("s"))
	g_strReferer = CStr(Request.QueryString("ref"))
	g_strNav = CStr(Request.QueryString("nav"))
		
	g_boolFree = CBool(Request.QueryString("f"))
	If g_boolFree Then 
		bk = IMODE_BILLING_KEY_FREE
	Else
		bk = IMODE_BILLING_KEY
	End If
End Sub


Sub InitPageVariables()
	g_lngCredit = CLng(g_objDico("CREDIT_" & g_strContentType))	
End Sub

Sub GetExtendedTicketFile()
	Dim l_strXMl
	Dim l_objXML
	On Error Resume Next
	
	If Goodies_GetContentTicketFileEx( bk, Server.URLEncode("LT" & g_strNav), g_lngIDContent, Common_GetMobileType(), l_strXML) Then
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
	
	If Goodies_GetContentTicketExtended( bk, Server.URLEncode("LT" & g_strNav), g_lngIDContent, Common_GetMobileType(), l_strXML) Then
		Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
		l_objXML.loadXML l_strXML
			
		g_strResult = l_objXML.selectSingleNode("./KMWebService/ResultCode").text
		g_strUrl = l_objXML.selectSingleNode("./KMWebService/Ticket/URL").text 
		g_strContentName = l_objXML.selectSingleNode("./KMWebService/Ticket/Title").text 
		g_lngIDTicket = l_objXML.selectSingleNode("./KMWebService/Ticket/IDTicket").text 
		
		Set l_objXML = Nothing
	Else
		g_strResult = "NOK"
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
	'If Not g_boolFree Then g_boolFree = Subscription_CheckDownloads( Common_GetUID(), g_strContentName )
	If Not g_boolFree Then g_boolFree = Subscription_CheckDownloads_Free( Common_GetUID(), g_strContentName, Common_GetMobileType() )
	
	If Not g_boolFree Then
		If Not Subscription_ProcessDownload( g_lngIDService, Common_GetUID(), g_strContentName, g_strContentGroup, g_strContentType, Common_GetMobileType(), g_lngIDTicket, g_strReferer, g_lngCredit ) <> 0 Then
			g_strResult = "NOK"
		End If
	Else
		g_lngIDService = 0
		Call Subscription_InsertDownload( g_lngIDService, Common_GetUID(), g_strContentName, g_strContentGroup, g_strContentType, Common_GetMobileType(), g_lngIDTicket, g_strReferer )
	End If
	
	If g_strContentGroup = GOODIE_CONTENTGROUP_VIDEO_RGT And g_strResult = "OK" Then
		Response.Redirect g_strUrl
	End If
End If
%>
<html>
<head>
<title>Looney Tunes</title>
</head>
<body bgcolor="#99CCFF" text="#ffffff" link="#ffffff" style="font-family:Verdana, Arial, Helvetica, sans-serif; font-size:x-small;">
<p align="center"> <% If g_strResult <> "OK" Then %> 
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
