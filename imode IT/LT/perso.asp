<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					CONSTANTS
'*************************************************

Const IMG_PER_PAGE = 2

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico

Dim g_lngNumPage
Dim g_strUrlBack
Dim g_lngContentPerPage

Dim g_strContentGroup
Dim g_strContentType

'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Goodies.asp" -->
<!--#include virtual="/Engine/API/Subscription.asp" -->
<!--#include virtual="/Engine/Includes/Goodies_Display.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Call InitVariablesFromRequest()
	Call InitPageVariables()

	Call Goodies_Init( IMODE_DISPLAY_KEY_LT, GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR)
	Call Goodies_SetMobileType( Common_GetMobileType() )

	Call Subscription_Init( IMODE_CONNECTIONSTRING, "LT")	
	
	Set g_objDico = Common_LoadPageText("LT")
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_lngNumPage = CLng(Request.QueryString("n"))
	
	g_strContentGroup = CStr(Request.QueryString("cg"))
	g_strContentType = CStr(Request.QueryString("ct"))
	'If g_strContentGroup = "" Then g_strContentGroup = GOODIE_CONTENTGROUP_IMG
	
	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default2.asp?uid=UIDREQUEST"	
	
End Sub

Sub InitPageVariables()
	Select Case g_strContentGroup
	Case GOODIE_CONTENTGROUP_ANIM
		g_lngContentPerPage = 1
	Case Else
		g_lngContentPerPage = IMG_PER_PAGE
	End Select
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
End Sub

Sub SelectDownloads( strContentGroup, strContentType )
	Dim l_objRs
	Dim l_strXML
	Dim l_strHTML
	Dim l_objXML
	Dim l_lngItem
	Dim l_lngIdx
	Dim l_strResolution

	If strContentGroup <> "" Then
		If Subscription_SelectDownloadsByContentType( Common_GetUID(), strContentGroup, strContentType, Null, l_objRs ) Then
				If l_objRs.Recordcount > 0 Then
					l_lngItem = g_lngNumPage * g_lngContentPerPage + 1
				
				If l_lngItem > l_objRs.Recordcount Then
					l_lngItem = 1
					g_lngNumPage = 0
				End If
				
				l_strHTML = "<tr><td align='center'>" & g_objDico(strContentGroup) & "</td></tr>" & vbCrLf
				l_strResolution = g_objDico(strContentGroup & "_PREVIEW")
				
				l_objRs.AbsolutePosition = l_lngItem
				For l_lngIdx = l_lngItem To l_lngItem + g_lngContentPerPage - 1
			 
					l_strXML = ""
					Call Goodies_Init( IMODE_DISPLAY_KEY_LT, l_objRs("ContentGroup"), l_objRs("ContentType") )
					Call Goodies_GetContentInfosByContentName( l_objRs("ContentName"), l_strXML )
					
					If l_strXML <> "" Then
						Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
						l_objXML.loadXML l_strXML
						
						l_strHTML = l_strHTML & _
							"<tr>" & vbCrLf & _
								"<td align='center'><a href='view.asp?uid=UIDREQUEST&cg=" & strContentGroup & "&nav=" & Server.URLEncode("|PERSO|" & g_lngNumPage + 1) & l_objRs("ContentGroup") & "&ct=" & l_objRs("ContentType") & "&r=" & l_objXml.selectSingleNode("./Content/IDContent").Text & "'><img src='" & Goodies_GetImgPreview(l_objRs("ContentName"), l_strResolution) & "'><br>" & _
								Server.HTMLEncode(l_objXml.selectSingleNode("./Content/PropertyCollection/Property[Name/text()='Name']/Value").Text) & "</a></td>" & vbCrLf & _
							"</tr>" & vbCrLf					
					End If
					
					l_objRs.Movenext
					If l_objRs.EOF Then Exit For
				Next
				
				l_strHTML = l_strHTML & GoodiesDisplay_Pages( l_objRs.Recordcount, IMG_PER_PAGE, g_lngNumPage + 1, "perso.asp?cg=" & strContentGroup &"&ct="& strContentType & "&uid=UIDREQUEST" )

			Else
				Select Case strContentType
				Case GOODIE_CONTENTTYPE_ANIM_COLOR
					l_strHTML = "<tr><td align='left'>Non hai ancora scaricato nessuna animazione.<br>Ritrova nella tua zona personale tutte le animazioni che tu hai scaricato su Looney Tunes.</td></tr>" & vbCrLf
				Case GOODIE_CONTENTTYPE_VIDEO_CLIP
					l_strHTML = "<tr><td align='left'>Non hai ancora scaricato nessuno video.<br>Ritrova nella tua zona personale tutti i video che tu hai scaricato su Looney Tunes.</td></tr>" & vbCrLf
				Case Else
					l_strHTML = "<tr><td align='left'>Non hai ancora scaricato nessuno sfondo.<br>Ritrova nella tua zona personale tutti gli sfondi che tu hai scaricato su Looney Tunes.</td></tr>" & vbCrLf
				End Select
			End If
			
			l_objRs.Close
			Set l_objRs = Nothing
		End If
	End If

	l_strHTML = l_strHTML & "<tr><td align='center'><br>I contenuti acquistati:</td></tr>" & vbCrLf
	
	If strContentType <> GOODIE_CONTENTTYPE_IMG_COLOR Then
		l_strHTML = l_strHTML & "<tr><td align='center'><a href='perso.asp?cg=" & GOODIE_CONTENTGROUP_IMG & "&ct=" & GOODIE_CONTENTTYPE_IMG_COLOR & "&uid=UIDREQUEST'>" & g_objDico("IMG") & "</a></td></tr>" & vbCrLf
	End If
	
	If strContentType <> GOODIE_CONTENTTYPE_VIDEO_CLIP And Common_IsMobileCompatible(Common_GetMobileType(), GOODIE_CONTENTTYPE_VIDEO_CLIP) Then
		l_strHTML = l_strHTML & "<tr><td align='center'><a href='perso.asp?cg=" & GOODIE_CONTENTGROUP_VIDEO_RGT & "&ct=" & GOODIE_CONTENTTYPE_VIDEO_CLIP & "&uid=UIDREQUEST'>" & g_objDico("VIDEO_RGT") & "</a></td></tr>" & vbCrLf		
	End If

	If strContentType <> GOODIE_CONTENTTYPE_ANIM_COLOR Then
		l_strHTML = l_strHTML & "<tr><td align='center'><a href='perso.asp?cg=" & GOODIE_CONTENTGROUP_ANIM & "&ct=" & GOODIE_CONTENTTYPE_ANIM_COLOR & "&uid=UIDREQUEST'>" & g_objDico("ANIM") & "</a></td></tr>" & vbCrLf
	End If
	
	Response.Write l_strHTML
End Sub

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
%>
<html>
<head>
<title>Looney Tunes</title>

</head>

<body bgcolor="#99CCFF" text="#ffffff" link="#ffffff" style="font-family:Verdana, Arial, Helvetica, sans-serif; font-size:x-small;">
<div align="center"> 
	<img src="../images/lt<% = Common_GetMobileWidth() %>.gif" alt="Looney Tunes">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr bgcolor="#0000FF"> 
      <td>
        <div align="center">Zona personale</div>
      </td>
    </tr>
  </table>
  
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="1">
<% If Not Subscription_IsUIDSubscribed( Common_GetUID() ) Then %>
	<tr>
		<td>
			Tutti i prodotti Looney Tunes che scarichi restano sempre disponibili in questa zona, a volont&agrave;!
			<br>
		</td>
	</tr>	
<% Else
	Call SelectDownloads( g_strContentGroup, g_strContentType ) 
End If %>
</table>
<br>
<table width="100%" cellspacing="3">
  <tr> 
    <td>&nbsp;</td>
  </tr>
	<tr>
		<td bgcolor="#0000FF"><font color="#FFFFFF">&#59147; </font><a href="<% = Common_GetUrlBillingScreens("reg") %>"><font color="#FFFFFF">Registrazione</font></a><br>
			<font color="#FFFFFF">&#59029; </font><a href="<% = Common_GetUrlBillingScreens("rel") %>"><font color="#FFFFFF">Disattivazione</font></a>
		</td>
	</tr>
	<tr> 
		<td bgcolor="#0000FF"><font color="#FFFFFF">&#59113; </font><a href="<% = g_strUrlBack %>" accesskey="8"><font color="#FFFFFF">Indietro</font></a></td>
	</tr>
  <tr> 
    <td bgcolor="#0000FF"><font color="#FFFFFF">&#59114; </font><a href="default2.asp?uid=UIDREQUEST" accesskey="9"><font color="#FFFFFF">Looney Tunes HomePage</font></a></td>
  </tr>	
</table>
</body>
</html>
<% Call Page_Terminate() %>