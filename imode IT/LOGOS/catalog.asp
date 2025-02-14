<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					CONSTANTS
'*************************************************

Const NB_IMG = 12

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico

Dim g_lngNumPage
Dim g_lngNumPageOtherDisplay

Dim g_lngItemPerRow
Dim g_lngItemPerPage

Dim g_lngIDContentSet
Dim g_bytViewImg

Dim g_strHTMLContent
Dim g_strContentSetName

Dim g_strContentGroup
Dim g_strContentType
Dim g_strContentGroupDisplay

Dim g_strUrlBack

Dim g_lngIDContentSetTop
Dim g_lngIDContentSetNew

Dim g_lngAccessKey

'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Goodies.asp" -->
<!--#include virtual="/Engine/Includes/Goodies_Display.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Set g_objDico = Common_LoadPageText("SCREENS")

	Call InitVariablesFromRequest()
	Call InitPageVariables()

	Call Goodies_SetMobileType( Common_GetMobileType() )
	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, g_strContentGroup, g_strContentType)
		
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strContentGroup = CStr(Request.QueryString("cg"))
	If g_strContentGroup = "" Then g_strContentGroup = GOODIE_CONTENTGROUP_IMG
	
	g_strContentType = CStr(Request.QueryString("ct"))
	g_strContentGroupDisplay = CStr(Request.QueryString("cgd"))
	If g_strContentGroupDisplay = "" Then g_strContentGroupDisplay = g_strContentGroup	
		
	g_lngNumPage = CLng(Request.QueryString("n"))
	If g_lngNumPage = 0 Then g_lngNumPage = 1
	
	g_lngIDContentSet = CLng(Request.QueryString("c"))
	If g_lngIDContentSet = 0 Then g_lngIDContentSet = g_objDico("COMPOSITE_" & g_strContentGroupDisplay)
	
	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default2.asp?uid=UIDREQUEST"		

	If IsEmpty(Request.QueryString("v")) Then
		g_bytViewImg = 1
	Else
		g_bytViewImg = CByte(Request.QueryString("v"))
	End If
End Sub

Sub InitPageVariables()

	If g_strContentGroup = "COMPOSITE" Then
		g_lngItemPerPage = 10
		g_lngItemPerRow = 1
	Else
		If Common_GetMobileWidth() > 200 Then
			g_lngItemPerPage = CLng(g_objDico(g_strContentGroup & "_ITEM_PAGE200"))
			g_lngItemPerRow = CLng(g_objDico(g_strContentGroup & "_ITEM_ROW200"))
		Else
			g_lngItemPerPage = CLng(g_objDico(g_strContentGroup & "_ITEM_PAGE"))
			g_lngItemPerRow = CLng(g_objDico(g_strContentGroup & "_ITEM_ROW"))	
		End If

		If g_bytViewImg = 1 Then
			g_lngNumPageOtherDisplay = ((g_lngNumPage - 1) * g_lngItemPerPage) \ NB_IMG + 1
		Else
			g_lngItemPerRow = 1
			g_lngNumPageOtherDisplay = ((g_lngNumPage - 1) * NB_IMG) \ g_lngItemPerPage + 1
			g_lngItemPerPage = NB_IMG
		End If
	End If
	
	g_lngIDContentSetNew = CLng(g_objDico("NEW_" & g_strContentGroupDisplay))
	g_lngIDContentSetTop = CLng(g_objDico("TOP_" & g_strContentGroupDisplay))
	
	g_lngAccessKey = 1
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
End Sub

Function Display()
	Dim l_arrContents, l_arrContent
	Dim l_strHTML
	Dim l_strApp
	Dim l_strUrl
	Dim l_strMimeType
	Dim l_strResolution
	Dim l_lngIdx
	Dim l_strPages
	Dim l_strPicto

	'----- Contenu du tableau -------
	Const cst_fldContentName = 0
	Const cst_fldIDContent = 1
	Const cst_fldName = 2
	Const cst_fldCompositeContentGroup = 4
	Const cst_fldIDComposite = 5
	Const cst_fldPreview = 6
	'--------------------------------	

	l_strApp = GoodiesDisplay_GetContentSet( g_lngIDContentSet, False )
	
	'Response.Write "App : " & Server.HTMLEncode(l_strApp)
	If l_strApp <> "" Then
		l_arrContents = Split(l_strApp, RECORD_SEPARATOR)
		
		g_strContentSetName = l_arrContents(0)
	
		l_strUrl = "catalog.asp?uid=UIDREQUEST&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&c=" & g_lngIDContentSet & "&cgd=" & g_strContentGroupDisplay & "&v=" & g_bytViewImg

		If UBound(l_arrContents) = 0 Then 
			If (g_strContentGroup = "VIDEO_RGT" Or g_strContentGroupDisplay = "VIDEO_RGT") And Not Common_IsMobileCompatible(Common_GetMobileType(), "VIDEO_CLIP") Then
				l_strHTML = l_strHTML & "<tr><td>Il tuo terminale non &egrave; abilitato alla visione del video.<br>I Terminali che consentono la visione dei video sono:<ul><li>MITSUBISHIM430i</li><li>MOTOROLAL7i</li><li>MOTOROLAV3XXi</li><li>NEC401i</li><li>NEC411i</li><li>NECN412i</li><li>NECN500i</li><li>NOKIAN70</li><li>SAMSUNGS400i</li><li>SAMSUNGS401i</li><li>SAMSUNGS410i</li><li>SAMSUNGS501i</li><li>SAMSUNGS720i</li><li>SAMSUNGS730i</li><li>SAMSUNGZ320i</li><li>SAMSUNGZ650i</li><li>SONYERICSSONK550im</li><li>SONYERICSSONK610im</li><li>SONYERICSSONZ1010</li></ul></td></tr>"
			Else
				l_strHTML = l_strHTML & "<tr><td>Nessun risultato</td></tr>"
			End If
		End If
		
		For l_lngIdx = g_lngItemPerPage * (g_lngNumPage - 1) + 1 To g_lngItemPerPage * g_lngNumPage
			If l_lngIdx > UBound(l_arrContents) Or l_lngIdx < LBound(l_arrContents) + 1 Then Exit For
			l_arrContent = Split(l_arrContents(l_lngIdx), FIELD_SEPARATOR)
			
			If l_arrContent(cst_fldIDComposite) <> "" Then
			
				l_strPicto = l_arrContent(cst_fldPreview)
				If l_strPicto = "" Then
					l_strPicto = "/IMAGES/picto.gif"
				End If
			'Response.Write GetDefaultType(l_arrContent(cst_fldCompositeContentGroup))
				l_strHTML = l_strHTML & _
						"<tr><td><font color='" & g_objDico(g_strContentGroupDisplay & "_COLOR_DARK") & "'><li>&nbsp;<a href='catalog.asp?uid=UIDREQUEST&cg=" & l_arrContent(cst_fldCompositeContentGroup) & "&ct=" & GetDefaultType(l_arrContent(cst_fldCompositeContentGroup)) & "&c=" & l_arrContent(cst_fldIDComposite) & "&cgd=" & g_strContentGroupDisplay & "&b=" & Server.URLEncode(l_strUrl & "&n=" & g_lngNumPage) & "'>" & Server.HTMLEncode(l_arrContent(cst_fldName)) & "</a></font></td></tr>" & vbCrLf
			Else			
				l_strMimeType = "GIF"	
				l_strResolution = g_objDico(g_strContentGroup & "_PREVIEW")
				
				If l_lngIdx Mod g_lngItemPerRow = 1 Or g_lngItemPerRow = 1 Then l_strHTML = l_strHTML & "<tr>" & vbCrLf				
				
				If g_bytViewImg = 1 Then
					l_strHTML = l_strHTML & _
							"<td align='center'><a href='view.asp?uid=UIDREQUEST&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&c=" & g_lngIDContentSet & "&r=" & l_arrContent(cst_fldIDContent) & "&nav=" & Server.URLEncode("|CONTENTSET|" & g_strContentSetName & "|" & g_lngNumPage) & "&b=" & Server.URLEncode(l_strUrl & "&n=" & g_lngNumPage) & "'><img src='" & Goodies_GetImgPreviewEx(l_arrContent(cst_fldContentName), l_strMimeType, l_strResolution) & "'>"
							
					If g_lngItemPerRow = 1 Then
						l_strHTML = l_strHTML & "<br>" & Server.HTMLEncode(l_arrContent(cst_fldName)) & "</a></td>" & vbCrLf
					End If
				Else
					l_strHTML = l_strHTML & _
							"<td align='center'><a href='view.asp?uid=UIDREQUEST&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&c=" & g_lngIDContentSet & "&r=" & l_arrContent(cst_fldIDContent) & "&nav=" & Server.URLEncode("|CONTENTSET|" & g_strContentSetName & "|" & g_lngNumPage) & "&b=" & Server.URLEncode(l_strUrl & "&n=" & g_lngNumPage) & "'>" & Server.HTMLEncode(l_arrContent(cst_fldName)) & "</a></td>" & vbCrLf
				End If
				
				If l_lngIdx Mod g_lngItemPerRow = 0  Or g_lngItemPerRow = 1 Then l_strHTML = l_strHTML & "</tr>" & vbCrLf						
					
			End If
		Next		
		
		l_strPages = GoodiesDisplay_PagesV2( g_lngNumPage, ((UBound(l_arrContents) - 1) \ g_lngItemPerPage) + 1, 5, l_strUrl )

		If l_strPages <> "" Then
			l_strHTML = l_strHTML & "<tr><td>Pagine :" & l_strPages & "</td></tr>"
		End If	
		
	End If
	
	Display = l_strHTML
End Function

Function GetDefaultType( strContentGroup )
	Select Case strContentGroup
	Case "COMPOSITE"
		GetDefaultType = ""
	Case "IMG"
		GetDefaultType = "IMG_COLOR"
	Case "ANIM"
		GetDefaultType = "ANIM_COLOR"
	Case "VIDEO_RGT"
		GetDefaultType = "VIDEO_CLIP"
	End Select
End Function

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
g_strHTMLContent = Display()

%>
<html>
<head>
<title>KiweeScreens</title>
</head>
<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<div align="center"><img src="../images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"><br></div>
<table width="100%" cellspacing="0" bgcolor="<% = g_objDico(g_strContentGroupDisplay & "_COLOR_DARK") %>">
	<tr>
		<td align="center">
			<font color="white"><% = g_objDico(g_strContentGroupDisplay & "_EMOJI") & " " & g_objDico(g_strContentGroupDisplay) %>: <% = Server.HTMLEncode(g_strContentSetName) %></font>
		</td>
	</tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1">
	<% = g_strHTMLContent %>
	<% If g_strContentGroup <> "COMPOSITE" Then %>
	<tr>
		<td align="center" colspan="<% = g_lngItemPerRow %>">
		<% If g_bytViewImg = 1 Then %> 
			<a href="catalog.asp?uid=UIDREQUEST&c=<% = g_lngIDContentSet %>&cg=<% = g_strContentGroupDisplay %>&ct=<% = g_strContentType %>&n=<% = g_lngNumPageOtherDisplay %>&v=0">Visualizza per lista</a> 
		<% Else %> 
			<a href="catalog.asp?uid=UIDREQUEST&c=<% = g_lngIDContentSet %>&v=1&cg=<% = g_strContentGroupDisplay %>&ct=<% = g_strContentType %>&n=<% = g_lngNumPageOtherDisplay %>">Visualizza diaporama</a> 
		<% End If %>
		</td>
	</tr>
	<% End If %>

	<tr>
		<td colspan="<% = g_lngItemPerRow %>">
		<% If g_lngIDContentSet <> g_lngIDContentSetTop And g_lngIDContentSetTop <> 0 Then %>
		&#59<% = 105 + g_lngAccessKey %>; <a href="catalog.asp?uid=UIDREQUEST&cg=<% = g_strContentGroupDisplay %>&ct=<% = g_strContentType %>&c=<% = g_lngIDContentSetTop %>" accesskey="<% = g_lngAccessKey %>">Top</a><br>
		<% g_lngAccessKey = g_lngAccessKey + 1
		End If %>
		<% If g_lngIDContentSet <> g_lngIDContentSetNew And g_lngIDContentSetNew <> 0 Then %>
		&#59<% = 105 + g_lngAccessKey %>; <a href="catalog.asp?uid=UIDREQUEST&cg=<% = g_strContentGroupDisplay %>&ct=<% = g_strContentType %>&c=<% = g_lngIDContentSetNew %>" accesskey="<% = g_lngAccessKey %>">Novit&agrave;</a><br>
		<% g_lngAccessKey = g_lngAccessKey + 1
		End If %>
		<% If g_lngIDContentSet = g_lngIDContentSetTop Or g_lngIDContentSet = g_lngIDContentSetNew Then %>
		&#59<% = 105 + g_lngAccessKey %>; <a href="catalog.asp?uid=UIDREQUEST&cg=COMPOSITE&cgd=<% = g_strContentGroupDisplay %>&ct=<% = g_strContentType %>&c=<% = g_objDico("COMPOSITE_" & g_strContentGroupDisplay) %>" accesskey="<% = g_lngAccessKey %>">Il catalogo completo</a><br>
		<% g_lngAccessKey = g_lngAccessKey + 1
		End If %>		
		</td>
	</tr>
	<tr>
		<td>
			<b>&#59100; <a href="search.asp">Cerca</a></b>
		</td>
	</tr>		
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="<% = g_objDico(g_strContentGroupDisplay & "_COLOR_DARK") %>">
	<% Call Common_GetFooterScreens( "", g_lngAccessKey ) %>
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
		<td bgcolor="#FFFFCC">&#59113; <a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a></td>
	</tr>
  <tr> 
    <td bgcolor="#FFFFCC">&#59114; <a href="default2.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
  </tr>	
</table>
</body>
</html>
<% Call Page_Terminate() %>