<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico
Dim g_lngIDSendTo
Dim g_strUID

Dim g_lngIDService
Dim g_lngIDContent
Dim g_strContentName
Dim g_strContentGroup
Dim g_strContentType
Dim g_strPreview

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

	Call Subscription_Init( Common_GetConnectionString(), GOODIE_CONTENTGROUP_IMG )
	'Response.Write Common_GetCurrentRelativeURL()
	
	Set g_objDico = Common_LoadPageText("SCREENS")
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next

	g_lngIDSendTo = CLng(Request.QueryString("g"))
End Sub

Sub InitPageVariables()
	g_lngIDService = 0
	g_lngIDContent = 0
	g_strContentGroup = ""
	g_strContentType = ""
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
End Sub

Sub GetSendToInfo()
	Dim l_objRs
	
	If Subscription_SelectSendTo(	g_lngIDSendTo, l_objRs ) Then
		If Not l_objRs.EOF Then
			g_lngIDContent = CLng(l_objRs("IDContent"))
			g_strContentType = CStr(l_objRs("ContentType"))
			g_strContentGroup = CStr(l_objRs("ContentGroup"))
			g_lngIDService = CLng(l_objRs("IDService"))
			g_strUID = CStr(l_objRs("UID"))

			'Response.Write g_lngIDContent & g_strContentGroup & g_strContentType
		End If
		
		l_objRs.Close
		Set l_objRs = Nothing

	End If
End Sub

Sub GetContent()
	Dim l_strXMl
	Dim l_objXML
	On Error Resume Next
	
	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, g_strContentGroup, g_strContentType)	
	If Goodies_GetContentInfos( g_lngIDContent, Common_GetMobileType(), l_strXML ) Then
		Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
		l_objXML.loadXML l_strXML
			
		g_strContentName = l_objXML.selectSingleNode("./Content/ContentName").text 
		
		Select Case g_strContentType
		Case GOODIE_CONTENTTYPE_ANIM_COLOR
			g_strPreview = Goodies_GetImgPreview( g_strContentName, "GIF_A_PR_40x40x8" )
		Case Else
			If UCase(Common_GetMobileType()) = "TOSHIBATS21I" Then
				g_strPreview = Goodies_GetImgPreviewEx( g_strContentName, "GIF", "GIF_PREVIEW_IMODE.2" )
			Else
				g_strPreview = Goodies_GetImgPreviewEx( g_strContentName, "JPG", "JPG_PREVIEW_IMODE.2" )
			End If
		End Select
		
		Set l_objXML = Nothing
	End If
End Sub


'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
Call GetSendToInfo()
If g_lngIDContent <> 0 Then Call GetContent()
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
			<font color="white">&#59091; Il tuo sfondo gratis!</font>
		</td>
	</tr>
</table>
<% If g_strContentName <> "" Then %>
<table>
	<tr>
		<td>
			<img src="<% = g_strPreview %>">
			<br>
			 &#59086; <a href="downloadfrom.asp?c=<% = g_lngIDContent %>&cg=<% = g_strContentGroup %>&ct=<% = g_strContentType %>&s=<% = g_lngIDService %>&g=<% = g_lngIDSendTo %>&u=<% = g_strUID %>&uid=UIDREQUEST">Scarica</a> &#59095;
		</td>
	</tr>
	<tr>
		<td><br>
		Per scoprire tutte le altre Immagini e le Animazioni dell'estate, 
		clicca <a href="/logos/default.asp?uid=UIDREQUEST">KiweeScreens</a>, 
		numero 1 su i-mode!! E anche tu potrai inviare le immagini pi&ugrave;
		divertenti e stravaganti ai tuoi amici!!
		</td>
	</tr>
</table>
<% Else %>
<table>
	<tr>
		<td>
		Hai gi&agrave; scaricato il tuo sfondo gratuito.
		<br>
		Per scaricare e regalare ai tuoi amici altri magnifici sfondi vai su <a href="catalog.asp?cg=COMPOSITE&cgd=IMG&ct=IMG_COLOR">KiweeScreens</a>.
		</td>
	</tr>
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