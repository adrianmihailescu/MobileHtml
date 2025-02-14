<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico

Dim g_boolAlready
Dim g_lngIDContent
Dim g_strContentName

Dim g_strPreview
Dim g_strPreviewVid
Dim g_boolCompatible

Dim g_strUrlDownload
Dim g_strUrlDownload2
Dim g_strUrlSendTo

Dim g_lngCredit
Dim g_strContentGroup
Dim g_strContentType
Dim g_strReferer
Dim g_strCredit
Dim g_lngIDContentSet
Dim g_boolHasAlready

Dim g_strUrlBack
Dim g_strNav
Dim is_abonado
Dim abonado_sin_creditos
Dim g_strSize
Dim precio
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
	Set g_objDico = Common_LoadPageText("SCREENS")
	Call InitVariablesFromRequest()

	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, g_strContentGroup, g_strContentType)
	Call Subscription_Init( Common_GetConnectionString(), GOODIE_CONTENTGROUP_IMG )
	'Response.Write Common_GetCurrentRelativeURL()
	
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strContentGroup = CStr(Request.QueryString("cg"))
	If g_strContentGroup = "" Then g_strContentGroup = GOODIE_CONTENTGROUP_IMG

	g_strContentType = CStr(Request.QueryString("ct"))
	If g_strContentType = "" Then g_strContentType = GOODIE_CONTENTTYPE_IMG_COLOR
	
	If g_strContentGroup = "VIDEO" or g_strContentGroup = "VIDEO_RGT" Then
		precio = "1.5"
	else
		precio = "1"
	End If

	g_strNav = CStr(Request.QueryString("nav"))
	g_strReferer = CStr(Request.QueryString("ref"))
	g_lngIDContentSet = CLng(Request.QueryString("c"))
	g_lngIDContent = CLng(Request.QueryString("r"))
	
	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default2.asp?uid=UIDREQUEST"	
	g_lngCredit = CLng(g_objDico("CREDIT_" & g_strContentType))	
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
End Sub

Sub Check()
	Dim l_objRs
	Dim l_boolZeroLeft
	Dim l_strHTML

	abonado_sin_creditos = False
	g_boolHasAlready = False
	is_abonado = False
	
	If Subscription_CheckDownloads_Free( Common_GetUID(), g_strContentName, Common_GetMobileType() ) Then
		g_boolHasAlready = True
		g_strURLDownload = "download.asp?ref=" & g_strReferer & "&nav=" & Server.URLEncode(g_strNav) & "&uid=UIDREQUEST&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&r=" & g_lngIDContent & "&f=1"
	Else
			g_strURLDownload = Common_GetUrlBillingScreens("reg")
			If g_strContentType = GOODIE_CONTENTTYPE_VIDEO_CLIP then
				g_strUrlDownload2 = IMODE_URL_ACTE & "?ci=023400036617&uid=UIDREQUEST&nl_ok=" & Server.HTMLEncode("http://" & Request.ServerVariables("HTTP_HOST") & "/logos/download2.asp") & "&nl_ko=" & Request.ServerVariables("HTTP_HOST") & "/default2.asp&act=SAP&arg1=" & g_lngIDContent & "&arg2=" & g_strContentGroup
			Else
				g_strUrlDownload2 = IMODE_URL_ACTE & "?ci=023400036615&uid=UIDREQUEST&nl_ok=" & Server.HTMLEncode("http://" & Request.ServerVariables("HTTP_HOST") & "/logos/download2.asp") & "&nl_ko=" & Request.ServerVariables("HTTP_HOST") & "/default2.asp&act=SAP&arg1=" & g_lngIDContent & "&arg2=" & g_strContentGroup
			End If
		If Subscription_Check( Common_GetUID(), l_objRs ) Then
			'g_strURLDownload = "subscribe.asp?uid=UIDREQUEST&nd=1&nl=" & Server.URLEncode(Common_GetCurrentUrl()) & "&b=" & Common_GetCurrentRelativeURL()
			'g_strURLDownload = Common_GetUrlBillingScreens("reg")
			If l_objRs.EOF Then
				l_strHTML = ""
				'g_strURLDownload = "nodownload.asp?nl=" & Server.URLEncode(Common_GetCurrentUrl()) & "&b=" & Common_GetCurrentRelativeURL()
			Else
				is_abonado = True
				abonado_sin_creditos = True
				l_boolZeroLeft = True
				While Not l_objRs.EOF
					If CLng(l_objRs("DownloadsLeft")) <> 0 Then
						abonado_sin_creditos = False
						l_boolZeroLeft = False
						If l_objRs("DownloadsLeft") > 10000 Then
							l_strHTML = l_strHTML & "Sei inscritto ad un abbonamento illimitato!<br>"
						Else
								l_strHTML =	"Ti restano ancora " &  l_objRs("DownloadsLeft") & " credit"
								If l_objRs("DownloadsLeft") > 1 Then 
									l_strHTML =	l_strHTML & "i"
								Else
									l_strHTML =	l_strHTML & "o"
								End If
							l_strHTML = l_strHTML & " sul tuo abbonamento a " & l_objRs("Price") & " euro.<br>"
						End If
						g_strURLDownload = "download.asp?ref=" & g_strReferer & "&nav=" & Server.URLEncode(g_strNav) & "&uid=UIDREQUEST&cg=" & g_strContentGroup & "&ct=" & g_strContentType & "&r=" & g_lngIDContent & "&s=" & l_objRs("IDService")
						'if (g_lngIDContentSet=5566 and Hour(Now)>= 18 and Hour(Now)<20 and Month(Now)=8) then g_strURLDownload = g_strURLDownload & "&f=1"
						If g_strContentType = "IMG_COLOR" Then g_strUrlSendTo = "frmsendto.asp?uid=UIDREQUEST&cg=IMG&ct=IMG_COLOR&c=" & g_lngIDContent & "&s=" & l_objRs("IDService") & "&b=" & Common_GetCurrentRelativeURL()
					End If
					l_objRs.Movenext
				Wend 
				
				'If l_boolZeroLeft Then
					'l_strHTML = "Vous avez &eacute;puis&eacute; vos cr&eacute;dits, vous pourrez t&eacute;l&eacute;charger votre s&eacute;lection d&egrave;s le mois prochain."
				'	g_strURLDownload = "nodownload.asp?zl=1&nl=" & Server.URLEncode(Common_GetCurrentUrl()) & "&nav=" & Server.URLEncode(g_strNav) & "&b=" & Common_GetCurrentRelativeURL()
				'End If
			End If
			
			l_objRs.Close
			Set l_objRs = Nothing
		End If
	End If
	Response.Write l_strHTML
End Sub

Sub GetContent()
	Dim l_strXMl
	Dim l_objXML
	On Error Resume Next
	g_strSize = ""
	g_boolCompatible = True
	
	If Goodies_GetContentInfos( g_lngIDContent, Common_GetMobileType(), l_strXML ) Then
		Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
		l_objXML.loadXML l_strXML
			
		g_strContentName = l_objXML.selectSingleNode("./Content/PropertyCollection/Property[Name/text()='ContentName']/Value").text 
		g_strSize  = l_objXML.selectSingleNode("./Content/ContentGroup/ContentTypeCollection/ContentType[Name/text()='" + g_strContentType + "']/DataRawSize").text 
				
		Select Case g_strContentType
		Case GOODIE_CONTENTTYPE_ANIM_COLOR
			g_strPreview = Goodies_GetImgPreview( g_strContentName, "GIF_A_PR_40x40x8" )
		Case GOODIE_CONTENTTYPE_VIDEO_CLIP
			g_strPreview = Goodies_GetImgPreview( g_strContentName, "GIF_A_PR_67x45" )
			If UCase(Common_GetMobileType()) = "NEC401I" Then
				g_strPreviewVid = Goodies_GetURL( g_strContentName ) & "/PREVIEW/3GPAAC/" & g_strContentName & ".3GPAAC_176x144x15.3gp"
			ElseIf UCase(Common_GetMobileType()) = "SAMSUNGS401I" Then
				g_strPreviewVid = Goodies_GetURL( g_strContentName ) & "/PREVIEW/3GPAMR/" & g_strContentName & ".3GPAMR_176x144x15.3gp"
			Else
				g_strPreviewVid = Goodies_GetURL( g_strContentName ) & "/PREVIEW/3GPAAC/" & g_strContentName & ".3GPAAC_176x144x15x64.3gp"
			End If		
		Case Else
			'If UCase(Common_GetMobileType()) = "TOSHIBATS21I" Then
				g_strPreview = Goodies_GetImgPreviewEx( g_strContentName, "GIF", "GIF_PREVIEW_IMODE.2" )
			'Else
			'	g_strPreview = Goodies_GetImgPreviewEx( g_strContentName, "JPG", "JPG_PREVIEW_IMODE.2" )
			'End If
		End Select
		If CLng(g_strSize) > 0 Then g_strSize = Fix(CLng(g_strSize) / 1024)
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
<div align="center"><img src="../images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"></div>
<table width="100%" cellspacing="0" bgcolor="<% = g_objDico(g_strContentGroup & "_COLOR_DARK") %>">
	<tr>
		<td align="center">
			<font color="white"><% = g_objDico(g_strContentGroup & "_EMOJI") & " " & g_objDico(g_strContentGroup) %></font>
		</td>
	</tr>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1">

  <% Check() %> 
  <% If g_strPreview <> "" Then %> 
  	<tr>	
		<td align="center"><img src="<% = g_strPreview %>"></td>
	</tr>
  <% End If %>


	<% Select Case g_strContentType
			Case GOODIE_CONTENTTYPE_VIDEO_CLIP %>
			<tr>
				<td align="center">
				<object declare id="declaration" data="<% = g_strPreviewVid %>" type="video/3gpp" VIEWASTEXT>
				<param name="count" value="0">
				</object>
				&#59025; <a href="#declaration">Breve estatto</a></td>
			</tr>
  <% End Select %>

  <% If Not g_boolHasAlready Then %>
		<tr>
			<td align="center">
				Vale <% = g_lngCredit %> credit<% If g_lngCredit > 1 Then Response.Write "i." Else Response.Write "o." End If %>
			</td>
		</tr>
			<% If is_abonado and not abonado_sin_creditos Then %>
			<tr>
				<td align="center">
					&#59086; <a href="<% = g_strUrlDownload %>">Scarica</a> 
				</td>
			</tr>
			<% end if %>
			<% If g_strUrlDownload2 <> "" Then %>
			<tr>
				<td align="center">
				<% If abonado_sin_creditos Then %>
					Hai terminato i tuoi crediti. Se non vuoi attendere il rinnovo mensile, puoi scaricare questo contenuto a soli <%=precio%>&#8364;!
				<% ElseIf is_abonado then %>
					Se non vuoi consumare i crediti, scarica questo contenuto a soli <%=precio%>&#8364; 
				<% ElseIf g_strContentType = GOODIE_CONTENTTYPE_VIDEO_CLIP then %>
					Puoi scaricare questo singolo contenuto a soli <%=precio%>&#8364;						
				<% Else %>
					Puoi scaricare questo singolo contenuto a soli <%=precio%>&#8364;						
				<% End If %>
				</td>
			</tr>
			<tr>
				<td align="center">&#59086; <a href="<% = g_strUrlDownload2 %>">Scarica (<%=precio%>&#8364;)</td>
			</tr>
			<% End If %>
			<% If not abonado_sin_creditos and not is_abonado Then %>
				<tr>
					<td align="center">
						Oppure registrati a soli 2 &#8364; / mese per avere 3 crediti!<br>
						&#59086; <a href="<% = g_strUrlDownload %>">Registrati</a>  (2&#8364; / mese)
					</td>
				</tr>
			<% End  If %>			
	<% Else %>
		<tr>
			<td align="center">
				Hai gi&agrave; scaricato questo contenuto. Per recuperarlo clicca su "Scarica", non ti sar&agrave; addebbitato alcun credito.
				<br>&#59086; <a href="<% = g_strUrlDownload %>">Scarica</a>
			</td>
		</tr>
	<% End If %>
  
  
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="1" bgcolor="<% = g_objDico(g_strContentGroup & "_COLOR_DARK") %>">
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
<% If Not IsEmpty(Request("ms")) Then  %>
  <tr> 
    <td bgcolor="#FFFFCC">&#59114; <a href="../msengine/default.asp?ms=1" accesskey="9">Natale HomePage</a></td>
  </tr>	
<% Else %>
	<tr> 
		<td bgcolor="#FFFFCC">&#59113; <a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a></td>
	</tr>
<% End If %>
  <tr> 
    <td bgcolor="#FFFFCC">&#59114; <a href="default2.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
  </tr>	
</table>
</body>
</html>
<% Call Page_Terminate() %>