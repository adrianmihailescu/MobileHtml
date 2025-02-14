<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					CONSTANTS
'*************************************************

Const NB_ROW = 14

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico

Dim g_strContentGroup
Dim g_strContentType

Dim g_strContentSetType
Dim g_lngNumPage
Dim g_strUrlBack

Dim g_strThemas

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
	Call InitVariablesFromRequest()

	Set g_objDico = Common_LoadPageText("SCREENS")
	
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strContentGroup = CStr(Request.QueryString("cg"))
	If g_strContentGroup = "" Then g_strContentGroup = GOODIE_CONTENTGROUP_IMG

	g_strContentType = CStr(Request.QueryString("ct"))
	If g_strContentType = "" Then g_strContentType = GOODIE_CONTENTTYPE_IMG_COLOR
	
	g_lngNumPage = CLng(Request.QueryString("n"))
	If g_lngNumPage = 0 Then g_lngNumPage = 1
	
	g_strContentSetType = CStr(Request.QueryString("t"))
	If g_strContentSetType = "" Then g_strContentSetType = "1"
	
	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default.asp?uid=UIDREQUEST"	
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
End Sub

Function DisplayContentSets( strType, lngNumPage )
	Dim l_strApp
	Dim l_arrContentSets
	Dim l_arrContentSet
	Dim l_lngIdx
	Dim l_strHTML
	Dim l_lngNbRow

	'----- Contenu du tableau -------
	Const cst_fldIDContentSet = 0
	Const cst_fldDescription = 1
	Const cst_fldCount = 2
	'--------------------------------	
	
	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, g_strContentGroup, g_strContentType)	

	l_strApp = GoodiesDisplay_GetContentSets( strType, False )
	
	'Response.Write Server.HTMLEncode(l_strApp)
	
	If l_strApp <> "" Then
	
		l_arrContentSets = Split(l_strApp, RECORD_SEPARATOR)
		If strType = "0" Then l_lngNbRow = NB_ROW Else l_lngNbRow = NB_ROW * 4
		
		For l_lngIdx = l_lngNbRow * (lngNumPage - 1) To l_lngNbRow * lngNumPage - 1
			If l_lngIdx > UBound(l_arrContentSets) Or l_lngIdx < LBound(l_arrContentSets) Then Exit For		
			l_arrContentSet = Split(l_arrContentSets(l_lngIdx), FIELD_SEPARATOR)

			l_strHTML = l_strHTML & "<a href='catalog.asp?cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&c=" & l_arrContentSet(cst_fldIDContentSet) & "'>" & Server.HTMLEncode(l_arrContentSet(cst_fldDescription)) & "</a><br>"
		Next

		If strType = "0" Then 
			If l_lngIdx <= UBound(l_arrContentSets) Then
				l_strHTML = l_strHTML & "<br><a href='categories.asp?t=" & strType & "&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&n=" & lngNumPage + 1 & "'>Temi seguenti &gt;</a>"
			'ElseIf strType = "1" Then
			'	l_strHTML = l_strHTML & "<br><a href='categories.asp?t=0&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&n=1'>Temi seguenti &gt;</a>"
			End If 

			If lngNumPage > 1 Then
				l_strHTML = l_strHTML & "<br><a href='categories.asp?t=" & strType & "&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&n=" & lngNumPage - 1 & "'>&lt; Temi precedenti</a>"
			'ElseIf strType = "0" And g_strContentGroup <> GOODIE_CONTENTGROUP_VIDEO Then
			'	l_strHTML = l_strHTML & "<br><a href='categories.asp?t=1&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&n=1'>&lt; Temi precedenti</a>"
			End If
		End If
	End If
	
	DisplayContentSets = l_strHTML
End Function

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()

%>
<html>
<head>
<title>Kiweescreens</title>
</head>
<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<div align="center"><img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="Kiwee Screens"></div>
<table width="100%" cellspacing="0" bgcolor="<% = g_objDico(g_strContentGroup & "_COLOR_DARK") %>">
	<tr>
		<td align="center">
			<font color="white"><% = g_objDico(g_strContentGroup & "_EMOJI") & " " & g_objDico(g_strContentGroup) %>: Catalogo</font>
		</td>
	</tr>
</table>
<table width="100%" cellspacing="0" bgcolor="<% = g_objDico(g_strContentGroup & "_COLOR_LIGHT") %>">
	<tr>
		<td>
			&#59106; <a href="catalog.asp?cg=<% = g_strContentGroup %>&ct=<% = g_strContentType %>&c=<% = g_objDico("TOP_" & g_strContentGroup) %>" accesskey="1">Top</a><br>
			&#59107; <a href="catalog.asp?cg=<% = g_strContentGroup %>&ct=<% = g_strContentType %>&c=<% = g_objDico("NEW_" & g_strContentGroup) %>" accesskey="2">Novit&agrave;</a><br>
		</td>
	</tr>
	<tr>
		<td><br>Il catalogo <% = g_objDico(g_strContentGroup) %>:</td>
	</tr>
	<tr>
		<td>
				<% If g_lngNumPage = 1 Then %> 
					<% g_strThemas = DisplayContentSets( "1", 1 ) 
					If g_strThemas <> "" Then 
						Response.Write g_strThemas & "<br>"
					End If %>
				<% End If %>
				<% = DisplayContentSets( "0", g_lngNumPage ) %>
				<br>
		</td>
	</tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="<% = g_objDico(g_strContentGroup & "_COLOR_DARK") %>" ID="Table1">
	<% Call Common_GetFooterScreens( "", 3 ) %>
</table>
<table cellspacing="3" width="100%">
	<tr> 
		<td>&nbsp;</td>
	</tr>		
	<tr>
		<td bgcolor="#FFFFCC">&#59147; <a href="subscribe.asp?uid=UIDREQUEST">Registrazione</a><br>
			&#59029; <a href="unsubscribe.asp?uid=UIDREQUEST">Disattivazione</a>
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