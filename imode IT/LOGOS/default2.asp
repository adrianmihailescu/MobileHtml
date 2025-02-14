
<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico

Dim g_lngDownloadsLeft
Dim g_strSubscription
Dim g_boolHasSubscription

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

	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR)
	Call Subscription_Init( IMODE_CONNECTIONSTRING, GOODIE_CONTENTGROUP_IMG)	

	Set g_objDico = Common_LoadPageText("SCREENS")
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
	
End Sub

Function Check()
	Dim l_objRs
	On Error Resume Next

	Check = False			
	g_lngDownloadsLeft = 0

	If Common_GetUID() <> "" Then
		If Subscription_Check( Common_GetUID(), l_objRs ) Then
			If Not l_objRs.EOF Then
				While Not l_objRs.EOF
					g_lngDownloadsLeft = g_lngDownloadsLeft + l_objRs("DownloadsLeft")
					l_objRs.Movenext
				Wend
				Check = True
			End If
			l_objRs.Close
			Set l_objRs = Nothing
		End If
	End If
	
	If g_lngDownloadsLeft > 10000 Then
		g_strSubscription = "Sei iscritto ad un abbonamento illimitato!"
	ElseIf g_lngDownloadsLeft > 0 Then
		g_strSubscription =	"Hai ancora " &  g_lngDownloadsLeft & " credit"
		If g_lngDownloadsLeft > 1 Then 
			g_strSubscription =	g_strSubscription & "i"
		Else
			g_strSubscription =	g_strSubscription & "o"
		End If
	End If
	
End Function

Sub Display( strContentGroup, strContentType, lngIDContentSet, strResolution )
	Dim l_arrContents, l_arrContent
	Dim l_lngCount
	Dim l_lngNumContent, l_lngNumContent2
	Dim l_strHTML
	Dim l_strApp

	'----- Contenu du tableau -------
	Const cst_fldContentName = 0
	Const cst_fldIDContent = 1
	Const cst_fldName = 2
	'--------------------------------	
	
	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, strContentGroup, strContentType)	

	l_strApp = GoodiesDisplay_GetContentSet( lngIDContentSet, False )
	If l_strApp <> "" Then
		l_arrContents = Split(l_strApp, RECORD_SEPARATOR)
		
		l_lngCount = UBound(l_arrContents) - LBound(l_arrContents)		'!! l_arrContents(0) = ContentSetName
		Call Randomize()
		l_lngNumContent = Int(l_lngCount * Rnd() + 1)
		l_arrContent = Split(l_arrContents(l_lngNumContent), FIELD_SEPARATOR)
		
		l_strHTML = l_strHTML & _
			"<tr>" & vbCrLf & _
				"<td align='center'><a href='view.asp?uid=UIDREQUEST&cg=" & strContentGroup & "&ct=" & strContentType & "&nav=" & Server.URLEncode("|HOME") & "&r=" & l_arrContent(cst_fldIDContent) & "'><img src='" & Goodies_GetImgPreviewEx(l_arrContent(cst_fldContentName), "GIF", strResolution) & "'></a>"
		
		If Common_GetMobileWidth() > 200 Then
			l_lngNumContent2 = l_lngNumContent
			While l_lngNumContent = l_lngNumContent2
				l_lngNumContent2 = Int(l_lngCount * Rnd() + 1)
			Wend
			l_arrContent = Split(l_arrContents(l_lngNumContent2), FIELD_SEPARATOR)			
				
			l_strHTML = l_strHTML & _
					"&nbsp;<a href='view.asp?uid=UIDREQUEST&cg=" & strContentGroup & "&ct=" & strContentType & "&nav=" & Server.URLEncode("|HOME") & "&r=" & l_arrContent(cst_fldIDContent) & "'><img src='" & Goodies_GetImgPreviewEx(l_arrContent(cst_fldContentName), "GIF", strResolution) & "'></a>" & vbCrLf
			
		End If

		l_strHTML = l_strHTML & _
						"<br></td>" & vbCrLf & _
					"</tr>" & vbCrLf		
		
	End If
	
	Response.Write l_strHTML
End Sub

Function DisplayComposite( lngIDContentSet, lngIndexBegin, lngIndexEnd )
	Dim l_arrContents, l_arrContent
	Dim l_strHTML
	Dim l_strApp
	Dim l_lngIdx
	Dim l_strPicto

	'----- Contenu du tableau -------
	Const cst_fldName = 2
	Const cst_fldCompositeContentGroup = 4
	Const cst_fldIDComposite = 5
	Const cst_fldPreview = 6
	'--------------------------------	

	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, "COMPOSITE", "")	
	l_strApp = GoodiesDisplay_GetContentSet( lngIDContentSet, False )
	'Response.Write "App : " & Server.HTMLEncode(l_strApp)
	If l_strApp <> "" Then
		l_arrContents = Split(l_strApp, RECORD_SEPARATOR)
		
		For l_lngIdx = lngIndexBegin To lngIndexEnd
			If l_lngIdx > UBound(l_arrContents) Or l_lngIdx < LBound(l_arrContents) + 1 Then Exit For
			l_arrContent = Split(l_arrContents(l_lngIdx), FIELD_SEPARATOR)
			
			l_strPicto = l_arrContent(cst_fldPreview)
			If l_strPicto = "" Then
				l_strPicto = "/IMAGES/picto.gif"
			End If			
			
			l_strHTML = l_strHTML & _
					"<tr><td><font color='" & g_objDico(l_arrContent(cst_fldCompositeContentGroup) & "_COLOR_DARK") & "'><li>&nbsp;<a href='catalog.asp?cg=" & l_arrContent(cst_fldCompositeContentGroup) & "&c=" & l_arrContent(cst_fldIDComposite) & "&cgd=" & l_arrContent(cst_fldCompositeContentGroup) & "&ct=" & GetDefaultContentType(l_arrContent(cst_fldCompositeContentGroup)) & "'>" & Server.HTMLEncode(l_arrContent(cst_fldName)) & "</a></font></td></tr>" & vbCrLf

		Next		
		
	End If
	
	DisplayComposite = l_strHTML
End Function

Sub DisplayTitulos( lngIDContentSet, strContentGroup, strContentType, strEmoji, nb )
	Dim l_arrContents, l_arrContent
	Dim l_lngCount
	Dim l_lngIdx, jour
	Dim l_strHTML
	Dim l_strApp, i, performer
	'On Error Resume Next
	'----- Contenu du tableau -------
	Const cst_fldContentName = 0
	Const cst_fldIDContent = 1
	Const cst_fldName = 2
	Const cst_fldPerformer = 3
	'--------------------------------	
	Call Goodies_Init( IMODE_DISPLAY_KEY_SCREENS, strContentGroup, strContentType)	

	l_strApp = GoodiesDisplay_GetContentSet( lngIDContentSet,False )

	If l_strApp <> "" Then
		
		l_arrContents = Split(l_strApp, RECORD_SEPARATOR)
		'jour = Day(Date)
		jour = Second(Time)

		'l_lngIdx = LBound(l_arrContents) + 1  '!! l_arrContents(0) = ContentSetName
		For i=0 to nb
		          l_lngIdx = (jour + i) MOD (UBound(l_arrContents))
                    	l_arrContent = Split(l_arrContents(l_lngIdx + 1), FIELD_SEPARATOR)        

				l_strHTML = l_strHTML & _
					"<tr>" & vbCrLf & _
						"<td>&#" & strEmoji & "; <a href='view.asp?uid=UIDREQUEST&cg=" & strContentGroup & "&ct=" & strContentType & "&nav=" & Server.URLEncode("|LINK") & "&r=" & l_arrContent(cst_fldIDContent) & "'>" & Server.HTMLEncode(l_arrContent(cst_fldName)) & performer
	
					
			l_strHTML = l_strHTML & "</a></td>" & vbCrLf & _
				"</tr>" & vbCrLf					

		Next		
	End If
	Response.Write l_strHTML
End Sub

Function GetDefaultContentType( strContentGroup )
	Select Case strContentGroup
	Case "IMG"
		GetDefaultContentType = "IMG_COLOR"
	Case "ANIM"
		GetDefaultContentType = "ANIM_COLOR"
	End Select
End Function

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
g_boolHasSubscription = Check()
%>
<html>
	<head>
		<title>Kiweescreens</title>
	</head>
	<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<div align="center"><img src="/images/screens<% = Common_GetMobileWidth() %>.gif" alt="Kiweescreens"><br>
			
			<!--<marquee bgcolor="#FFFFCC">3 crediti (Sfondi, Animazioni, Video) per soli 2 euro!</marquee>-->
			<marquee bgcolor="#FFFFCC"><a href="http://portal.imode.wind.it/imodePASS50/index.html">Offerta traffico</a>: 1000&#128; di traffico e 50% di sconto sul sito!</marquee>
			</div>

			<% If g_boolHasSubscription Then %>
			<table bgcolor="white" align="center" width="100%" cellspacing="0" cellpadding="0" ID="Table1">
				<tr align="center">
					<td>
						<% = g_strSubscription %>
					</td>
				</tr>
			</table>
			<% End If %>			
		<% If Not g_boolHasSubscription Then %>
			<!-- VIDEO -->			
			<table width="100%" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td bgcolor="<% = g_objDico("VIDEO_RGT_COLOR_DARK") %>"><% = g_objDico("VIDEO_RGT_EMOJI") %> Incredibile!!!! <font color="white"><blink>&#58999;</blink></font><br><a href="catalog.asp?cg=COMPOSITE&cgd=VIDEO_RGT&c=<% = g_objDico("COMPOSITE_VIDEO_RGT") %>&ct=VIDEO_CLIP"><b>Video !!</b></a></td>
				</tr>
				
			 </table>
				<br>		
		<% End If %>
		
		<table cellspacing="0" cellpadding="0" border="0" width="100%">
			<% = DisplayComposite( 3801, 1, 2 ) %>
			<tr><td>&nbsp;</td></tr>
			<% Call Display( GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR, g_objDico("TOP_IMG"), "GIF_PREVIEW_IMODE") %>	
			<% Call DisplayTitulos(g_objDico("TOP_IMG"), GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR, "59086", 3 ) %>
		</table>
		
		<b>&#59100; Cerca</b>
		<form method="GET" name="form1" action="search.asp" ID="Form2">
			<input type="text" size="7" name="k" istyle="3" ID="Text2">
		    <input type="submit" value="ok" id="Submit2" name=submit1>
		</form>	
		<table cellspacing="0" cellpadding="0" border="0" width="100%">
	       <tr>
				<td bgcolor="#ED5F23"><blink><font color="white">Che schermo vuoi?</font></blink></td>
			</tr>
			<tr>
				<td><a href="catalog.asp?cg=COMPOSITE&cgd=IMG&ct=IMG_COLOR&c=<% = g_objDico("COMPOSITE_IMG") %>">>>Sfondi</a></td>
			</tr>
			<tr>
				<td><a href="catalog.asp?cg=COMPOSITE&cgd=ANIM&ct=ANIM_COLOR&c=<% = g_objDico("COMPOSITE_ANIM") %>">>>Animazioni</a></td>
			</tr>			
		</table>
		<marquee bgcolor="#2400AA"><font color="white">Titti Calcio Garfield Orsetti del Cuore Dragonito</font></marquee>

		<!------------------------- COMPOSITE ------------------------>		
		<table cellspacing="0" cellpadding="0" border="0" width="100%">
		<% Call Display( GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR, g_objDico("TOP_IMG"), "GIF_PREVIEW_IMODE") %>		
		</table>
		<table cellspacing="0" cellpadding="0" border="0" width="100%">
			<tr>
				<td bgcolor="#0092FF"><font color="white">&#59140; La Vetrina TOP:</font></td>
			</tr>
		</table>
		<table cellspacing="0" cellpadding="0" border="0" width="100%">
			<% = DisplayComposite( 3801, 3, 4 ) %>
			<% If Common_IsMobileCompatible( Common_GetMobileType(), GOODIE_CONTENTTYPE_ANIM_COLOR ) Then
				Response.Write DisplayComposite( 3803, 1, 1 ) 
			End If %>
		</table>
		<br>
		<% If g_boolHasSubscription Then %>
			<!-- VIDEO -->			
					&#58999; <a href="catalog.asp?cg=COMPOSITE&cgd=VIDEO_RGT&c=<% = g_objDico("COMPOSITE_VIDEO_RGT") %>&ct=VIDEO_CLIP">Le VideoSuonerie</a><br>
		<% End If %>		
		<!--&#59095; <a href="free.asp">Un'immagine gratis!!</a>
		<br>-->
		<table cellspacing="0" width="100%" ID="Table10">
			<tr>
				<td bgcolor="#FFFFCC">&#59116; <a href="perso.asp?uid=UIDREQUEST">Zona personale</a></td>
			</tr>
		</table>
		<table cellspacing="3" width="100%">
			<tr>
				<td bgcolor="#FFFFCC">&#59017; <a href="subscribe.asp">Offerta e Condizioni</a></td>
			</tr>
			<tr>
				<td bgcolor="#FFFFCC">&#59147; <a href="<% = Common_GetUrlBillingScreens("reg") %>">Registrazione</a><br>
					&#59029; <a href="<% = Common_GetUrlBillingScreens("rel") %>">Disattivazione</a>
				</td>
			</tr>
			<tr>
				<td bgcolor="#FFFFCC">&#59091; <a href="contact.asp?uid=UIDREQUEST">Contatti</a></td>
			</tr>
			<tr>
				<td bgcolor="#FFFFCC">&#59114; <a href="compatibili.asp?uid=UIDREQUEST" accesskey="9">Tel Compatibili</a></td>
			</tr>
			<tr>
				<td bgcolor="#FFFFCC">&#59115; <a href="http://portal.imode.wind.it/gprs/mn/main.htm" accesskey="0">
						&#59089;-menu</a>
				</td>
			</tr>			
		</table>
	</body>
</html>
<% Call Page_Terminate() %>
