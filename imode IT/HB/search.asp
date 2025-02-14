<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = False

'*************************************************
'					CONSTANTS
'*************************************************

Const NB_IMG = 12
Const NB_ITEMS = 12

'*************************************************
'					VARIABLES
'*************************************************

Dim g_strUrlBack
Dim g_objDico
Dim g_strContentGroup
Dim g_strContentType
Dim g_strContext
Dim g_strHrefParam
Dim g_strKeyword
Dim g_lngIndex
Dim g_strReferer
Dim file_download
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
	Set g_objDico = Common_LoadPageText( "HB" )
	
	Call InitVariablesFromRequest()
	
	Call Goodies_SetMobileType( Common_GetMobileType() )
End Sub

Sub InitVariablesFromRequest()
	Dim l_strParam
	Dim l_tabParam
	'On Error Resume Next
	g_strReferer = CStr(Request("ref"))	
	l_strParam = CStr(Request("p"))
	If l_strParam <> "" Then
		l_tabParam = Split(l_strParam, "@")
		g_strHrefParam = l_tabParam(0)
		g_strContentType = l_tabParam(1)
		g_strContentGroup = GetContentGroup(g_strContentType)
	Else
		g_strHrefParam = ""
		g_strContentType = CStr(Request("ct"))
		g_strContentGroup = CStr(Request("cg"))
	End If
	g_strContext = CStr(Request("ctxt"))		

	g_strKeyword = CStr(Request("k"))
	
	g_lngIndex = CLng(Request("n"))
	If Not IsEmpty(Request("next")) Then
    	g_lngIndex = g_lngIndex + NB_ITEMS
	End If
	If Not IsEmpty(Request("previous")) Then
		g_lngIndex = g_lngIndex - NB_ITEMS
	End If
	file_download = "view.asp"
	'if Goodies_IsPromo() then file_download = "download050.asp"
	
	'Response.Write "hrefParam : " & g_strHrefParam & "<br>"
	'Response.Write "Context : " & g_strContext & "<br>"
	'Response.Write "Keyword : " & g_strKeyword & "<br>"
End Sub

Sub Page_Terminate()
	On Error Resume Next
	Set g_objDico = Nothing
End Sub

Function GetContentGroup( strContentType )
	Select Case strContentType
	Case GOODIE_CONTENTTYPE_SOUND_FX
		GetContentGroup = GOODIE_CONTENTGROUP_SFX
	Case GOODIE_CONTENTTYPE_SOUND_POLY
		GetContentGroup = GOODIE_CONTENTGROUP_SOUND
	Case GOODIE_CONTENTTYPE_SOUND_HIFI
		GetContentGroup = GOODIE_CONTENTGROUP_SOUND
	Case GOODIE_CONTENTTYPE_IMG_COLOR
		GetContentGroup = GOODIE_CONTENTGROUP_IMG
	Case GOODIE_CONTENTTYPE_ANIM_COLOR
		GetContentGroup = GOODIE_CONTENTGROUP_ANIM
	Case GOODIE_CONTENTTYPE_VIDEO_CLIP
		GetContentGroup = GOODIE_CONTENTGROUP_VIDEO_RGT
	Case Else
		GetContentGroup = GOODIE_CONTENTGROUP_VIDEO
	End Select
End Function

Sub Display()
	Dim l_objDoc
	Dim l_strXml
	Dim l_objNode
	Dim l_strHTML, l_strHTML2
	Dim l_boolResult
	Dim g_boolPrevious, g_boolNext
	Dim l_strTitle
	Dim cont 
	'On Error Resume Next
	
	If g_strKeyword <> "" Then
		If g_strContext <> "" Then
			Call Goodies_Init( IMODE_DISPLAY_KEY_HB, null, null )

			If Goodies_ExaSearchRefine( g_strContext, g_strHrefParam, g_lngIndex, l_strXML) Then
					
				Set l_objDoc = Server.CreateObject("MSXML2.DomDocument")
				l_objDoc.loadXML(l_strXml)
				
				l_strHTML = "Ricerca: " & Server.HTMLEncode(g_strKeyword) & " - " & Server.HTMLEncode(g_objDico(g_strContentType)) & "<br>"
				cont = 0
				For Each l_objNode In l_objDoc.selectNodes("./SearchResult/ContentSet/ContentCollection/Content")	
					cont = cont +1
					if cont > NB_ITEMS then Exit For
					If g_strContentType = "SOUND_HIFI" And l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Master']/Value").text = "1" Then
						l_strTitle = l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Name']/Value").text & " / " & l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Performer']/Value").text
					Else
						l_strTitle = Server.HTMLEncode(l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Name']/Value").text)
					End If
					
					If (GetContentGroup(g_strContentType)="IMG" or GetContentGroup(g_strContentType)="ANIM" or GetContentGroup(g_strContentType)="VIDEO_RGT" or GetContentGroup(g_strContentType)="VIDEO") Then
					          l_strHTML = l_strHTML & g_objDico("EMOJI_" & g_strContentGroup) & " <a href='" & file_download & "?uid=UIDREQUEST&ref=" & g_strReferer & "&nav=" & Server.URLEncode("|SEARCH|" & g_strKeyword & "|" & CInt((g_lngIndex MOD CInt(NB_ITEMS-1)) + 1)) & "&cg=" & GetContentGroup(g_strContentType) & "&ct=" & g_strContentType & "&r=" & l_objNode.selectSingleNode("./IDContent").text & "'>" & l_strTitle & "</a><br>" & vbCrLf
					          'l_strHTML = l_strHTML & g_objDico("EMOJI_" & g_strContentGroup) & " <a href='downlaod.asp?ref=" & g_strReferer & "&nav=" & Server.URLEncode("|SEARCH|" & g_strKeyword & "|" & CInt((g_lngIndex MOD CInt(NB_ITEMS-1)) + 1)) & "&cg=" & GetContentGroup(g_strContentType) & "&ct=" & g_strContentType & "&r=" & l_objNode.selectSingleNode("./IDContent").text & "&s=100'>" & l_strTitle & "</a><br>" & vbCrLf
					Else
					          l_strHTML = l_strHTML & g_objDico("EMOJI_" & g_strContentGroup) & " <a href='/ringtones/download.asp?ref=" & g_strReferer & "&nav=" & Server.URLEncode("SEARCH_IMG|" & g_strKeyword & "|" & CInt((g_lngIndex MOD CInt(NB_ITEMS-1)) + 1)) & "&cg=" & GetContentGroup(g_strContentType) & "&ct=" & g_strContentType & "&r=" & l_objNode.selectSingleNode("./IDContent").text & "'>" & l_strTitle & "</a><br>" & vbCrLf
                                                  End If
                                                  
					'l_strHTML = l_strHTML & g_objDico("EMOJI_" & g_strContentType) & " <a href='view.asp?ref=" & g_strReferer & "&nav=" & Server.URLEncode("|SEARCH|" & g_strKeyword & "|" & CInt((g_lngIndex MOD CInt(NB_ITEMS-1)) + 1)) & "&cg=" & GetContentGroup(g_strContentType) & "&ct=" & g_strContentType & "&r=" & l_objNode.selectSingleNode("./IDContent").text & "'>" & l_strTitle & "</a><br>" & vbCrLf
					
					
				Next
				
				l_strHTML = l_strHTML & "<input type='hidden' name='p' value='" & g_strHrefParam & "@" & g_strContentType & "'>" & vbCrLf & _
																"<input type='hidden' name='ref' alue='" & g_strReferer & "'>" & vbCrLf & _
																"<input type='hidden' name='ctxt' value='" & g_strContext & "'>" & vbCrLf & _
																"<input type='hidden' name='k' value='" & g_strKeyword  & "'>" & vbCrLf
						
				g_boolPrevious = (g_lngIndex > 0)
				g_boolNext = ((g_lngIndex+1) + NB_ITEMS < CLng(l_objDoc.DocumentElement.Attributes.getNamedItem("TotalHits").Value))
						
				If g_boolNext or g_boolPrevious Then
				          l_strHTML = l_strHTML & "<input type='hidden' name='n' value='" & g_lngIndex & "'>"
				End If 
						
				'If CLng(l_objDoc.DocumentElement.Attributes.getNamedItem("CurrentSetEnd").Text) + 1 < CLng(l_objDoc.DocumentElement.Attributes.getNamedItem("TotalHits").Text) Then
				If g_boolNext Then
					l_strHTML = l_strHTML & "<input type='submit' name='next' value='Successivo'><br>" & vbCrLf
					
				End If
				
				'If CLng(l_objDoc.DocumentElement.Attributes.getNamedItem("CurrentSetStart").Text) > 0 Then
				If g_boolPrevious Then
					l_strHTML = l_strHTML & "<input type='submit' name='previous' value='Precedente'><br>" & vbCrLf					
				End If

																
				'If CLng(l_objDoc.DocumentElement.Attributes.getNamedItem("CurrentSetEnd").Text) + 1 < CLng(l_objDoc.DocumentElement.Attributes.getNamedItem("TotalHits").Text) Then
				'	l_strHTML = l_strHTML & "<input type='hidden' name='n' value='" & g_lngIndex + NB_ITEMS & "'><input type='submit' value='Siguientes'><br>" & vbCrLf
					
				'End If
				'If CLng(l_objDoc.DocumentElement.Attributes.getNamedItem("CurrentSetStart").Text) > 0 Then
				'	l_strHTML = l_strHTML & "<input type='hidden' name='n' value='" & g_lngIndex - NB_ITEMS & "'><input type='submit' value='Anteriores'><br>" & vbCrLf
					
				'End If
				
			Else
			End If
		Else
			If g_strContentType = "" Then

				'Pas de context, pas de content type : Recherche globale
				Call Goodies_Init( IMODE_DISPLAY_KEY_HB, null, null )
				If Goodies_ExaSearchExt( g_strKeyword, 0, l_strXML) Then
					
					Set l_objDoc = Server.CreateObject("MSXML2.DomDocument")
					l_objDoc.loadXML(l_strXml)
					l_boolResult = False
					l_strHTML2 = ""
					l_strHTML = "Ricerca: " & Server.HTMLEncode(g_strKeyword) & "<br><br>"
					l_strHTML = l_strHTML & "<input type='hidden' name='ctxt' value='" & l_objDoc.selectSingleNode("./SearchResult/Context").text & "'>" & vbCrLf & _
																	"<input type='hidden' name='ref' value='" & g_strReferer  & "'>" & vbCrLf & _
																	"<input type='hidden' name='k' value='" & g_strKeyword  & "'>" & vbCrLf
					

					For Each l_objNode In l_objDoc.selectNodes("./SearchResult/CriteriaHits/CriteriaGroup[@Name='ContentType']/Criterias/Criteria")
						'If (l_objNode.Attributes.getNamedItem("Name").Text = "IMG_COLOR" Or l_objNode.Attributes.getNamedItem("Name").Text = "ANIM_COLOR" Or l_objNode.Attributes.getNamedItem("Name").Text = "VIDEO_SCREENSAVERSOUND" Or l_objNode.Attributes.getNamedItem("Name").Text = "FLASH_SCREENSAVER") _
						'	And Common_IsMobileCompatible(Common_GetMobileType(), l_objNode.Attributes.getNamedItem("Name").Text) Then
						If Common_IsMobileCompatible(Common_GetMobileType(), l_objNode.Attributes.getNamedItem("Name").Text) and not(IsEmpty(g_objDico(l_objNode.Attributes.getNamedItem("Name").Text))) Then
					          	If (GetContentGroup(l_objNode.Attributes.getNamedItem("Name").Text)="IMG" or GetContentGroup(l_objNode.Attributes.getNamedItem("Name").Text)="ANIM" or GetContentGroup(l_objNode.Attributes.getNamedItem("Name").Text)="VIDEO_RGT" or GetContentGroup(l_objNode.Attributes.getNamedItem("Name").Text)="VIDEO") Then
					                    	l_strHTML = l_strHTML & "<input type='radio' name='p' value='" & l_objNode.Attributes.getNamedItem("Refine").Text & "@" & l_objNode.Attributes.getNamedItem("Name").Text & "'"
								If Not l_boolResult Then l_strHTML = l_strHTML & " checked"
								l_strHTML = l_strHTML & "> " & Server.HTMLEncode(g_objDico(l_objNode.Attributes.getNamedItem("Name").Text)) & " : " & l_objNode.Attributes.getNamedItem("Count").Text & " risultati<br>" & vbCrLf
					                    Else
								l_strHTML2 = l_strHTML2 & "<input type='radio' name='p' value='" & l_objNode.Attributes.getNamedItem("Refine").Text & "@" & l_objNode.Attributes.getNamedItem("Name").Text & "'"
								l_strHTML2 = l_strHTML2 & "> " & Server.HTMLEncode(g_objDico(l_objNode.Attributes.getNamedItem("Name").Text)) & " : " & l_objNode.Attributes.getNamedItem("Count").Text & " risultati<br>" & vbCrLf
							End If
							l_boolResult = True
						End If
					Next
					
					'if Not(IsEmpty(l_strHTML2)) then 
					'if Not(l_strHTML2="") then 
                     '                                       l_strHTML2 = "<br>y en Kiwee M&uacute;sica...<br>" & l_strHTML2
                      '                                      l_strHTML = l_strHTML & l_strHTML2
                       '                           end if
                                                  
					If l_boolResult Then
						l_strHTML = l_strHTML & "<input type='submit' value='Cerca'>" & vbCrLf					
					Else
						l_strHTML = l_strHTML & "La tua ricerca non ha dato risultati." & vbCrLf
					End If
					
					Set l_objDoc = Nothing
				End If			
				
			End If
		End If
	Else
	'Pas de keyword
		l_strHTML = l_strHTML & "Inserisci il termine da cercare:<br>" & vbCrLf & _
											"<input type='hidden' name='ref' value='" & g_strReferer & "'><br>" & vbCrLf & _
											"<input type='text' name='k'><br>" & vbCrLf & _
											"<input type='submit' value='Cerca'>" & vbCrLf
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
		<title>Hanna Barbera</title>
	</head>
        <body bgcolor="#00F000" text="#ffffff" link="#ffffff" style="font-family:Verdana, Arial, Helvetica, sans-serif; font-size:x-small;">
		<div align="center"><img src="/images/hb<% = Common_GetMobileWidth() %>.gif" alt="Hanna Barbera" /><br/>
		<table width="100%" cellspacing="0" bgcolor="<% = g_objDico("COLOR_HEADER_IMG") %>" ID="Table2">
			<tr>
				<td align="center">
					<font color="white">Cerca</font>
				</td>
			</tr>
		</table>			
	
		<form action="search.asp" method="post" ID="Form1">
		<% Call Display() %>
		</form>
		<% If g_strKeyword <> "" Then %>
		<br><a href="search.asp?ref=<%=g_strReferer%>">Altra ricerca</a>
		<% End If %>
		<br>
		<table width="100%" cellspacing="3" ID="Table1">
		<tr> 
			<td><hr color="<% = g_objDico("COLOR_HEADER_BAR1") %>"></td>
		</tr>
		<tr>
			<td>&#59115; <a href="default2.asp?uid=UIDREQUEST&ref=<%=g_strReferer%>" accesskey="0">Hanna Barbera HomePage</a></td>
		</tr>
	</table>
	</body>
</html>
<% Call Page_Terminate() %>
