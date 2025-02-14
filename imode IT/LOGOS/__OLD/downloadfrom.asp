<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_lngIDSendTo
Dim g_strUIDSender
Dim g_lngIDContent
Dim g_lngIDService

Dim g_strResult
Dim g_strUrl
Dim g_strContentName
Dim g_lngIDTicket
Dim g_strReferer
Dim g_lngCredit

Dim g_boolFree

Dim g_strContentGroup
Dim g_strContentType

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
End Sub


Sub InitVariablesFromRequest()
	On Error Resume Next
	g_lngIDSendTo = CLng(Request.QueryString("g"))
	g_strUIDSender = CStr(Request.QueryString("u"))
	g_strContentGroup = CStr(Request.QueryString("cg"))
	If g_strContentGroup = "" Then g_strContentGroup = GOODIE_CONTENTGROUP_IMG		
	
	g_strContentType = CStr(Request.QueryString("ct"))
	If g_strContentType = "" Then g_strContentType = GOODIE_CONTENTTYPE_IMG_COLOR
	
	g_lngIDContent = CLng(Request.QueryString("c"))
	g_lngIDService = CLng(Request.QueryString("s"))
	
End Sub


Sub InitPageVariables()
	
	g_strReferer = "SendTo"
End Sub

Sub GetExtendedTicketFile()
	Dim l_strXMl
	Dim l_objXML
	On Error Resume Next
	
	If Goodies_GetContentTicketFileEx( IMODE_BILLING_KEY, g_strReferer, g_lngIDContent, Common_GetMobileType(), l_strXML) Then
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
	
	If Goodies_GetContentTicketExtended( IMODE_BILLING_KEY, g_strReferer, g_lngIDContent, Common_GetMobileType(), l_strXML) Then
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
If UCase(Common_GetMobileType) = "SAGEMSG321I" Or UCase(Common_GetMobileType) = "SAGEMSG341I" Or UCase(Common_GetMobileType) = "SAGEMSG322I" Then Call GetExtendedTicketFile() Else Call GetExtendedTicket()
If g_strResult = "OK" Then
	Call Subscription_InsertDownload( g_lngIDService, g_strUIDSender, g_strContentName, g_strContentGroup, g_strContentType, Common_GetMobileType(), g_lngIDTicket, g_strReferer )
	Call Subscription_UpdateSendTo(	g_lngIDSendTo, g_lngIDTicket, Common_GetUID() )
	
	If g_strContentGroup = GOODIE_CONTENTGROUP_VIDEO And g_strResult = "OK" Then
		Response.Redirect g_strUrl
	End If
End If
%>
<html>
<head>
<title>KiweeScreens</title>
</head>
<body bgcolor="#FFFFFF">
<p align="center"> <% If g_strResult <> "OK" Then %> 
	Si &eacute; verificato un problema tecnico. 
  <% Else %> <br>
  Per salvare l'immagine, vai su Menu - Salva / Registra l'immagine. L'immagine appare in un quadro colorato, clicca sull'immagine e fai Salva.
	<br>
  <img src="<% = g_strURL %>"> <% End If %> </p>
<table width="100%" cellspacing="3">
  <tr> 
    <td bgcolor="#FFFFCC">&#59114; <a href="default.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
  </tr>	
</table>
</body>
</html>
