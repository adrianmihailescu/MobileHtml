
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

	Call Goodies_Init( IMODE_DISPLAY_KEY_LT, GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR)
	Call Subscription_Init( IMODE_CONNECTIONSTRING, "LT")	

	Set g_objDico = Common_LoadPageText("LT")
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
	
	Call Goodies_Init( IMODE_DISPLAY_KEY_LT, strContentGroup, strContentType)	

	l_strApp = GoodiesDisplay_GetContentSet( lngIDContentSet, False )
	If l_strApp <> "" Then
		l_arrContents = Split(l_strApp, RECORD_SEPARATOR)
		
		l_lngCount = UBound(l_arrContents) - LBound(l_arrContents)		'!! l_arrContents(0) = ContentSetName
		Call Randomize()
		l_lngNumContent = Int(l_lngCount * Rnd() + 1)
		l_arrContent = Split(l_arrContents(l_lngNumContent), FIELD_SEPARATOR)
		
		l_strHTML = l_strHTML & _
			"<tr>" & vbCrLf & _
				"<td align='center'><a href='view.asp?uid=UIDREQUEST&cg=" & strContentGroup & "&ct=" & strContentType & "&nav=" & Server.URLEncode("|HOME") & "&r=" & l_arrContent(cst_fldIDContent) & "'><img src='" & Goodies_GetImgPreviewEx(l_arrContent(cst_fldContentName), "GIF", strResolution) & "'></a><br/><a href='view.asp?uid=UIDREQUEST&cg=" & strContentGroup & "&ct=" & strContentType & "&nav=" & Server.URLEncode("|HOME") & "&r=" & l_arrContent(cst_fldIDContent) & "'>" & l_arrContent(cst_fldName) & "</a>"
		
		If Common_GetMobileWidth() > 200 and false Then
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
	Call Goodies_Init( IMODE_DISPLAY_KEY_LT, strContentGroup, strContentType)	

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
						"<td>&#" & strEmoji & "; <a href='view.asp?uid=UIDREQUEST&cg=" & strContentGroup & "&ct=" & strContentType & "&nav=" & Server.URLEncode("|LINK") & "&r=" & l_arrContent(cst_fldIDContent) & "'><font color='" & g_objDico("COLOR_BODY_" & GOODIE_CONTENTGROUP_IMG) & "'>" & Server.HTMLEncode(l_arrContent(cst_fldName)) & performer
	
					
			l_strHTML = l_strHTML & "</font></a></td>" & vbCrLf & _
				"</tr>" & vbCrLf					

		Next		
	End If
	Response.Write l_strHTML
End Sub

Function DisplayComposite( lngIDContentSet, lngIndexBegin, lngIndexEnd, colorbg, color, emoji, bold )
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

	Call Goodies_Init( IMODE_DISPLAY_KEY_LT, "COMPOSITE", "")	
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
			if bold then
				l_strHTML = l_strHTML & _
					"<tr bgcolor=" & colorbg & "><td><a href='catalog.asp?cg=" & l_arrContent(cst_fldCompositeContentGroup) & "&c=" & l_arrContent(cst_fldIDComposite) & "&cgd=" & l_arrContent(cst_fldCompositeContentGroup) & "&ct=" & GetDefaultContentType(l_arrContent(cst_fldCompositeContentGroup)) & "'><font color='" & color & "'>&#" & emoji & ";&nbsp;<b>" & Server.HTMLEncode(l_arrContent(cst_fldName)) & "</b></font></a></td></tr>" & vbCrLf
			else
				l_strHTML = l_strHTML & _
					"<tr bgcolor=" & colorbg & "><td><a href='catalog.asp?cg=" & l_arrContent(cst_fldCompositeContentGroup) & "&c=" & l_arrContent(cst_fldIDComposite) & "&cgd=" & l_arrContent(cst_fldCompositeContentGroup) & "&ct=" & GetDefaultContentType(l_arrContent(cst_fldCompositeContentGroup)) & "'><font color='" & color & "'>&#" & emoji & ";&nbsp;" & Server.HTMLEncode(l_arrContent(cst_fldName)) & "</font></a></td></tr>" & vbCrLf			
			end if
		Next		
		
	End If
	
	DisplayComposite = l_strHTML
End Function

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
		<title>Looney Tunes</title>
	</head>
	<body bgcolor="#99CCFF" text="#ffffff" link="#ffffff" style="font-family:Verdana, Arial, Helvetica, sans-serif; font-size:x-small;">
		<div align="center">
			<img src="../images/lt<% = Common_GetMobileWidth() %>.gif" alt="Looney Tunes" /><br/>
			<!--<marquee bgcolor="#0000FF">748 sfondi, 64 video &amp; 10 animazioni Looney Tunes</marquee>-->
			<marquee><font color="black">6 crediti (Sfondi, Animazioni, Video) per soli 2 euro!</font></marquee>
			<marquee bgcolor="#0000FF"><a href="http://portal.imode.wind.it/imodePASS50/index.html">Offerta traffico</a>: 1000&#128; di traffico e 50% di sconto sul sito!</marquee>
			
			<!--<marquee bgcolor="#FFFFCC">3 crediti (Sfondi, Animazioni, Video) per soli 2 euro!</marquee>-->
			</div>
		</div>
		
		<% If g_boolHasSubscription Then %>
			<table align="center" width="100%">
				<tr align="center">
					<td>
						<% = g_strSubscription %>
					</td>
				</tr>
			</table>
		<% End If %>		
			
		<table border="0" width="100%">
			<% = DisplayComposite( g_objDico("COMPOSITE_IMG"), 1, 1, "red", "white", "59140", false ) %>
			<% Call Display( GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR, g_objDico("TOP_IMG"), "GIF_PREVIEW_IMODE") %>	
		</table>
		<table border="0" width="100%" cellpadding="0" cellspacing="0">
			<% Call DisplayTitulos(g_objDico("TOP_IMG"), GOODIE_CONTENTGROUP_IMG, GOODIE_CONTENTTYPE_IMG_COLOR, "59086", 3 ) %>			
			<% = DisplayComposite( g_objDico("COMPOSITE_IMG"), 2, 5, "white", "black", "59134", false ) %>
			<tr bgcolor="red">
				<td><a href="catalog.asp?cg=COMPOSITE&cgd=IMG&ct=IMG_COLOR&c=<% = g_objDico("COMPOSITE_IMG") %>">>> +Cartoni</a></td>
			</tr>
		</table>

		<table border="0" width="100%" border="red">
			<tr>
				<td bgcolor="yellow"><a href="catalog.asp?cg=COMPOSITE&cgd=VIDEO_RGT&ct=VIDEO_CLIP&c=5681"><font color="#000000">>> Video &#58999;</font></a></td>
			</tr>		
			<tr>
				<td bgcolor="orange"><a href="catalog.asp?cg=ANIM&ct=ANIM_COLOR&c=4895">>> Animazioni</a></td>
			</tr>					
		</table>
		<b>&#59100; Cerca</b>
		<form method="GET" name="form1" action="search.asp" ID="Form2">
			<input type="text" size="7" name="k" istyle="3" ID="Text2">
		    <input type="submit" value="ok" id="Submit2" name=submit1>
		</form>	
		<table cellspacing="3" width="100%" ID="Table10">
			<tr>
				<td bgcolor="#0000FF"><font color="#FFFFFF">&#59116; </font><a href="perso.asp?uid=UIDREQUEST"><font color="#FFFFFF">Zona personale</font></font></a></td>
			</tr>

			<tr>
				<td bgcolor="#0000FF"><font color="#FFFFFF">&#59017; </font><a href="subscribe.asp"><font color="#FFFFFF">Offerta e Condizioni</font></a></td>
			</tr>
			<tr>
				<td bgcolor="#0000FF"><font color="#FFFFFF">&#59147; </font><a href="<% = Common_GetUrlBillingLT("reg") %>"><font color="#FFFFFF">Registrazione</font></a><br>
					&#59029; <a  href="<% = Common_GetUrlBillingLt("rel") %>"><font color="#FFFFFF">Disattivazione</font></a>
				</td>
			</tr>
			<tr>
				<td bgcolor="#0000FF"><font color="#FFFFFF">&#59091; </font><a href="contact.asp?uid=UIDREQUEST"><font color="#FFFFFF">Contatti</font></a></td>
			</tr>
			<tr>
				<td bgcolor="#0000FF"><font color="#FFFFFF">&#59114; </font><a href="compatibili.asp?uid=UIDREQUEST" accesskey="9"><font color="#FFFFFF">Tel Compatibili</font></a></td>
			</tr>
			<tr>
				<td bgcolor="#0000FF"><font color="#FFFFFF">&#59115; </font><a href="http://portal.imode.wind.it/gprs/mn/main.htm" accesskey="0">
						<font color="#FFFFFF">&#59089;-menu</font></a>
				</td>
			</tr>			
		</table>
	</body>
</html>
<% Call Page_Terminate() %>
