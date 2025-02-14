<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_strPreview
Dim g_strContentDesc
Dim g_strUrlBack

'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Goodies.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Call InitVariablesFromRequest()

	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR)
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default.asp?uid=UIDREQUEST"	
End Sub

Sub GetContent()
	Dim l_strXMl
	Dim l_objXML
	
	If Goodies_GetContentInfos( IMODE_CONTENT_FREE_IMG, Common_GetMobileType(), l_strXML ) Then
		Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
		l_objXML.loadXML l_strXML
			
		g_strPreview = l_objXML.selectSingleNode("./Content/ContentGroup/ContentTypeCollection/ContentType[Name/text()='" + GOODIE_CONTENTTYPE_IMG_COLOR + "']/Preview/URL").text
		
		Set l_objXML = Nothing
	End If
End Sub

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
Call GetContent()
%>
<html>
<head>
<title>KiweeScreens</title>

</head>

<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<div align="center"> 
  <p><img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"><br>
    &nbsp;&nbsp; <br>
    Gratis ! Halloween</p>
  <p><img src="<% = g_strPreview %>"><br>
  </p>
  <p>
    &#59095; <a href="download.asp?uid=UIDREQUEST&nav=<%= Server.URLEncode("|FREE") %>&r=<% = IMODE_CONTENT_FREE_IMG %>&f=1">Scarica ora</a> 
  </p>
  <p><a href="catalog.asp?cg=IMG&ct=IMG_COLOR&c=2867">Scopri tutto Halloween!</a><br/>
  <a href="catalog.asp?cg=IMG&ct=IMG_COLOR&c=4398">Halloween con Titti, Bugs Bunny & Co!</a></p>
</div>
<table width="100%" cellspacing="0" bgcolor="#FFFFCC">
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
		<td bgcolor="#FFFFCC">&#59113; <a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a></td>
	</tr>
  <tr> 
    <td bgcolor="#FFFFCC">&#59114; <a href="default.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
  </tr>	
</table>
</body>
</html>
