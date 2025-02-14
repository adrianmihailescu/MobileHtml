<%
'---------------------------------------------------------------
'										CONSTANTS
'---------------------------------------------------------------

Const IMODE_CONNECTIONSTRING = "IMode"
Const IMODE_IDOPERATOR = "3"

Const IMODE_URL_WIND = "http://portal.imode.wind.it/gprs/cp/regst.cgi"
Const IMODE_URL_ACTE = "http://portal.imode.wind.it/ipei"
Const IMODE_URL = "http://imode.kiwee.it"
'Const IMODE_URL = ""

Const IMODE_BILLING_KEY = "135C00CA-2CA8-4B23-B870-4DE87B8E69FF"
Const IMODE_BILLING_KEY_1 = "D1AB89E0-4B66-47CA-B4ED-75E8055A6D01"
Const IMODE_BILLING_KEY_FREE = "D43077B7-E721-4762-83ED-2C64E6C5FB80"
Const IMODE_BILLING_KEY_1_5 = "C2B01DE9-DCD4-4E70-873F-F303D514FE16"


Const IMODE_DISPLAY_KEY_TONES = "76ABC5BC-6FCA-46A2-BFEE-C7F00136DA0C"
Const IMODE_DISPLAY_KEY_SCREENS = "eebdd96f-4e3f-4324-96e8-80e0654754cf"
Const IMODE_DISPLAY_KEY_MOOVIZ = ""
Const IMODE_DISPLAY_KEY_HB = "13AAB6DE-2823-4427-AD39-15F997800B6A"
Const IMODE_DISPLAY_KEY_LT = "2C5E01BD-7825-4807-91BF-0BF4B3AC8D3F"


Const IMODE_CONTENT_FREE_SOUND = 2432
Const IMODE_CONTENT_FREE_IMG = 3934

Const IMODE_URL_CONTENT = "http://content.k-mobile.com/"
Const IMODE_URL_REFERENCESERVICE = "http://content.k-mobile.com/V2/ws/ReferenceService.asmx/"
Const IMODE_URL_MAILSERVICE = "http://drmservice.com/pps/default.asmx/"

'---------------------------------------------------------------
'										Variables
'---------------------------------------------------------------

Dim cm_strMobileType 
Dim cm_strBrand
Dim cm_strType
Dim cm_strWidth
Dim cm_strConnectionString
Dim cm_strUID

'---------------------------------------------------------------
'										PARAMETERS
'---------------------------------------------------------------

Function Common_GetConnectionString()
	On Error Resume Next
	If CStr(cm_strConnectionString) = "" Then
		Common_GetConnectionString = IMODE_CONNECTIONSTRING
		Common_SetConnectionString IMODE_CONNECTIONSTRING
	Else
		Common_GetConnectionString = cm_strConnectionString
	End If
End Function


Sub Common_SetConnectionString( strConnectionString )
	If strConnectionString <> "" Then
		Select Case UCase(strConnectionString)
		Case "FRENCH", "SPANISH", "ITALIAN", "DUTCH", "ENGLISH"
			cm_strConnectionString = strConnectionString
		Case Else
			cm_strConnectionString = IMODE_CONNECTIONSTRING
		End Select
	End If
End Sub

Function Common_GetUID()
	Dim l_strURL
	On Error Resume Next
	'If Session("IMODE_UID") = "" Then
		If Not IsEmpty(Request("uid")) And Request("uid") <> "" Then 'Or Request.QueryString("uid") = "UIDREQUEST" Then
			cm_strUID = Request("uid")
		Else
			cm_strUID = ""
		End If
	'End If
	Common_GetUID = cm_strUID
End Function

Function Common_CleanQueryString()
	Dim l_strItem
	Dim l_strQS
	
	For Each l_strItem In Request.QueryString 
		If UCase(l_strItem) <> "UID" And UCase(l_strItem) <> "B" Then
			If l_strQS <> "" Then l_strQS = l_strQS & "&"
			l_strQS = l_strQS & l_strItem & "=" & Request.QueryString(l_strItem)
		ElseIf UCase(l_strItem) = "UID" Then
			If l_strQS <> "" Then l_strQS = l_strQS & "&"
			l_strQS = l_strQS & "uid=UIDREQUEST"
		End If
	Next
	
	For Each l_strItem In Request.Form
		If UCase(l_strItem) <> "UID" And UCase(l_strItem) <> "B" Then
			If l_strQS <> "" Then l_strQS = l_strQS & "&"
			l_strQS = l_strQS & l_strItem & "=" & Request.Form(l_strItem)
		ElseIf UCase(l_strItem) = "UID" Then
			If l_strQS <> "" Then l_strQS = l_strQS & "&"
			l_strQS = l_strQS & "uid=UIDREQUEST"
		End If
	Next	
	'Response.Write l_strQs & "<br>"
	'Response.Write Request.QueryString & "<br>"
	Common_CleanQueryString = l_strQS
End Function

Function Common_GetCurrentURL()
	On Error Resume Next
	
	If Request.QueryString <> "" Then
		Common_GetCurrentURL = "http://" & Request.ServerVariables("HTTP_HOST") & Request.ServerVariables("URL") & Server.URLEncode("?" & Common_CleanQueryString())
		'Response.Write "http://" & Request.ServerVariables("HTTP_HOST") & Request.ServerVariables("URL") & Server.URLEncode("?" & Common_CleanQueryString()) & "<br>"
		'Common_GetCurrentURL = "http://" & Request.ServerVariables("HTTP_HOST") & Request.ServerVariables("URL") & Server.URLEncode("?" & Request.QueryString)
		'Response.Write "http://" & Request.ServerVariables("HTTP_HOST") & Request.ServerVariables("URL") & Server.URLEncode("?" & Request.QueryString) & "<br>" 
	Else
		Common_GetCurrentURL = "http://" & Request.ServerVariables("HTTP_HOST") & Request.ServerVariables("URL")
	End If
End Function

Function Common_GetCurrentRelativeURL()
	On Error Resume Next
	
	If Request.QueryString <> "" Or Request.Form <> "" Then
		Common_GetCurrentRelativeURL = Server.URLEncode( Request.ServerVariables("URL") & "?" & Common_CleanQueryString() )
	Else
		Common_GetCurrentRelativeURL = Server.URLEncode( Request.ServerVariables("URL") )
	End If
End Function

'---------------------------------------------------------------
'										FUNCTIONS
'---------------------------------------------------------------

Sub Common_SetCookie( strObject, strValue )
	Dim l_objNav

	Set l_objNav = Server.CreateObject("MSWC.BrowserType")
	If l_objNav.Cookies Then
		Response.Cookies(strObject) = strValue
		Response.Cookies(strObject).Expires = DateAdd("y", 1, Date())
	End If
	Set l_objNav = Nothing
End Sub

Function Common_GetMobileList( ByRef strXML )
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next

	l_strURL = IMODE_URL_REFERENCESERVICE & "GetMobileList"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send ""

	strXML = l_objXMLHTTP.responseXML.xml
	 
	Set l_objXMLHTTP = Nothing
	
End Function

Function Common_GetMobile()
	Dim l_objXMLHTTP	
	Dim l_strURL
	Dim l_objXML
	On Error Resume Next

	l_strURL = IMODE_URL_REFERENCESERVICE & "GetMobileFromUA"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "userAgent=" & Request.ServerVariables("HTTP_USER_AGENT")

	Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
	l_objXML.loadXML l_objXMLHTTP.responseXML.xml

	cm_strMobileType = l_objXML.selectSingleNode("./Mobile/MobileType").text
	cm_strBrand = l_objXML.selectSingleNode("./Mobile/Brand").text 
	cm_strType = l_objXML.selectSingleNode("./Mobile/Type").text
	cm_strWidth = l_objXML.selectSingleNode("./Mobile/ScreenWidth").text 		 
	
	Set l_objXML = Nothing	
	Set l_objXMLHTTP = Nothing
	
End Function

Function Common_GetMobileType()
	On Error Resume Next

	If CStr(cm_strMobileType) = "" Then
		Call Common_GetMobile()			
	End If
	
	If IsEmpty(Request.QueryString("test")) Then
		Common_GetMobileType = cm_strMobileType
	Else
		Common_GetMobileType = Request.QueryString("test")
	End If
End Function

Function Common_GetMobileWidth()
	On Error Resume Next

	If CStr(cm_strWidth) = "" Then
		Call Common_GetMobile()			
	End If
	Common_GetMobileWidth = cm_strWidth
End Function

Function Common_GetMobileName()
	On Error Resume Next

	If CStr(cm_strBrand) = "" Then
		Call Common_GetMobile()			
	End If
	Common_GetMobileName = cm_strBrand & " " & cm_strType
End Function


Function Common_IsMobileCompatible( ByVal strMobileType, ByVal strContentType )
	Dim l_objXMLHTTP	
	Dim l_objDOMDoc
	Dim l_strURL
	Dim l_datChange
	Dim l_boolChange
	Dim l_strKey
	Dim l_boolIsCompatible

	On Error Resume Next

	l_strKey = strMobileType & "@" & strContentType
	
	l_boolChange = False
	If Application(l_strKey) = "" Then
		l_boolChange = True
	Else
		l_boolChange = (DateDiff("d", Application(l_strKey & "@DATE"), Date()) <> 0)
	End If
	
	If l_boolChange Then
			
		l_strURL = IMODE_URL_REFERENCESERVICE & "GetMobileCaps"

		Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
		Set l_objDOMDoc = Server.CreateObject("Msxml2.DOMDocument")
		
		l_objXMLHTTP.open "POST", l_strURL, False
		l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
		l_objXMLHTTP.send "strMobileType=" & strMobileType & "&strContentType=" & strContentType
		l_objDOMDoc.loadXML (l_objXMLHTTP.responseXML.xml)

		If Err.number = 0 Then		
			l_boolIsCompatible = CBool(l_objDOMDoc.Text)
		Else
			l_boolIsCompatible = False
		End If

		Set	l_objDOMDoc = Nothing
		Set l_objXMLHTTP = Nothing
		
		Application.Lock
		Application(l_strKey & "@DATE") = Date()		
		Application(l_strKey) = l_boolIsCompatible
		Application.UnLock 
	Else
		l_boolIsCompatible = CBool(Application(l_strKey))
	End If
	
	Common_IsMobileCompatible = l_boolIsCompatible
	
End Function

Function Common_GetEmoji( strEmoji, strReplace )
	Select Case Common_GetMobileType()
	Case "NOKIA6620", "NOKIA3650", "SONYERICSSONZ1010", "SONYERICSSONZ1010MR3"
		Common_GetEmoji = strReplace
	Case Else
		Common_GetEmoji = strEmoji
	End Select
End Function

Function Common_LoadSite( strMiniSite )
	Dim l_objXmlDoc
	Dim l_objNode
	Dim l_objDico
	Dim l_strKey
	Dim l_lngIdx
	Dim l_lngIDLanguage
	
	'On Error Resume Next

	Set l_objDico = Server.CreateObject("Scripting.Dictionary")
	
	Set l_objXmlDoc = Server.CreateObject("MSXML2.DomDocument")
	l_objXmlDoc.async = False
	l_objXmlDoc.load( Server.MapPath("/Engine/Xml/parameters.xml" ) )
		
	Set l_objNode = l_objXmlDoc.selectSingleNode( "./parameters/minisites" )

	For l_lngIdx = 0 To l_objNode.childNodes.length - 1
		l_objDico.Add l_objNode.childNodes.item(l_lngIdx).nodeName, l_objNode.childNodes.item(l_lngIdx).Text
	Next

	Set l_objNode = l_objXmlDoc.selectSingleNode( "./parameters/SITE[@id='" & strMiniSite & "']" )

	For l_lngIdx = 0 To l_objNode.childNodes.length - 1
		l_objDico.Add l_objNode.childNodes.item(l_lngIdx).nodeName, l_objNode.childNodes.item(l_lngIdx).Text
	Next	
	
	Set l_objNode = Nothing
	Set l_objXmlDoc = Nothing
	
	'Dim name
	'for each name in l_objDico
	'	Response.Write name & " : " & server.HTMLEncode(l_objDico(name)) & "<br>"
	'next

	Set Common_LoadSite = l_objDico

	Set l_objDico = Nothing	
	
End Function


Function Common_LoadThemas( strContentGroup, lngNumThema )
	Dim l_objXmlDoc
	Dim l_objNodes
	Dim l_objDico
	Dim l_strKey
	Dim l_lngIdx
	
	'On Error Resume Next

	Set l_objDico = Server.CreateObject("Scripting.Dictionary")
	
	Set l_objXmlDoc = Server.CreateObject("MSXML2.DomDocument")
	l_objXmlDoc.async = False
	l_objXmlDoc.load( Server.MapPath("/Engine/Xml/home.xml" ) )
		
	Set l_objNodes = l_objXmlDoc.selectNodes( "./contentgroups/contentgroup[@id='" & strContentGroup & "']/thema" & lngNumThema & "/thema" )

	Set Common_LoadThemas = l_objNodes

	Set l_objNodes = Nothing
	Set l_objXmlDoc = Nothing
	
End Function

Function Common_GetFooterTones( strContentType, lngFirstAccessKey )
	Dim l_lngAccessKey

	l_lngAccessKey = lngFirstAccessKey

	If strContentType <> GOODIE_CONTENTTYPE_SOUND_POLY Then
		Response.Write "<tr><td bgcolor='#FFFFCC'>&#59" & (105 + l_lngAccessKey) & "; <a href='catalog.asp?cg=COMPOSITE&ctd=SOUND_POLY&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Poli Music</a></td></tr>"	
		l_lngAccessKey = l_lngAccessKey + 1
	End If

	If Common_IsMobileCompatible(Common_GetMobileType(), "SOUND_HIFI") And strContentType <> GOODIE_CONTENTTYPE_SOUND_HIFI And Not Common_IsMobileCompatible(Common_GetMobileType(), "SOUND_VIDEO") Then 
		Response.Write "<tr><td bgcolor='#FFFFCC'>&#59" & (105 + l_lngAccessKey) & "; <a href='catalog.asp?cg=COMPOSITE&ctd=" & GOODIE_CONTENTTYPE_SOUND_HIFI & "&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Real Music</a></td></tr>"
		l_lngAccessKey = l_lngAccessKey + 1
	End If
	
	If Common_IsMobileCompatible(Common_GetMobileType(), "SOUND_VIDEO") And strContentType <> "SOUND_VIDEO" Then 
		Response.Write "<tr><td bgcolor='#FFFFCC'>&#59" & (105 + l_lngAccessKey) & "; <a href='catalog.asp?cg=COMPOSITE&ctd=SOUND_VIDEO&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Real Music</a></td></tr>"
		l_lngAccessKey = l_lngAccessKey + 1
	End If
	
	If Common_IsMobileCompatible(Common_GetMobileType(), "SOUND_FX") And strContentType <> GOODIE_CONTENTTYPE_SOUND_FX Then 
		Response.Write "<tr><td bgcolor='#FFFFCC'>&#59" & (105 + l_lngAccessKey) & "; <a href='catalog.asp?cg=COMPOSITE&ctd=" & GOODIE_CONTENTTYPE_SOUND_FX & "&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Effetti Speciali</a></td></tr>"
		l_lngAccessKey = l_lngAccessKey + 1
	End If
	
	If Common_IsMobileCompatible(Common_GetMobileType(), "VIDEO_CLIP") And strContentType <> GOODIE_CONTENTTYPE_VIDEO_CLIP Then
		Response.Write "<tr><td bgcolor='#FFFFCC'>&#59" & (105 + l_lngAccessKey) & "; <a href='catalog.asp?cg=COMPOSITE&ctd=" & GOODIE_CONTENTTYPE_VIDEO_CLIP & "&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>VideoSuonerie</a></td></tr>"
		l_lngAccessKey = l_lngAccessKey + 1	
	End If

End Function

Function Common_GetFooterScreens( strContentType, lngFirstAccessKey )
	Dim l_lngAccessKey	

	l_lngAccessKey = lngFirstAccessKey
	
	If strContentType <> "IMG_COLOR" Then 
		Response.Write "<tr><td>&#59" & (105 + l_lngAccessKey) & "; <a href='catalog.asp?cg=COMPOSITE&cgd=IMG&ct=IMG_COLOR&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Sfondi</a></td></tr>"	
		l_lngAccessKey = l_lngAccessKey + 1
	End If
	
	If strContentType <> "ANIM_COLOR" And Common_IsMobileCompatible(Common_GetMobileType(), "ANIM_COLOR") Then 
		Response.Write "<tr><td>&#59" & (105 + l_lngAccessKey) & "; <a href='catalog.asp?cg=COMPOSITE&cgd=ANIM&ct=ANIM_COLOR&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Animazioni</a></td></tr>"
		l_lngAccessKey = l_lngAccessKey + 1
	End If
	
	If strContentType <> "VIDEO_CLIP" And Common_IsMobileCompatible(Common_GetMobileType(), "VIDEO_CLIP") Then 
		Response.Write "<tr><td>&#59" & (105 + l_lngAccessKey) & "; <a href='catalog.asp?cg=COMPOSITE&cgd=VIDEO_RGT&ct=VIDEO_CLIP&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Video</a></td></tr>"
		l_lngAccessKey = l_lngAccessKey + 1
	End If	
	
End Function

Function Common_GetFooterMooviz( strContentType )
	Dim l_lngAccessKey
	
	l_lngAccessKey = 1

	If strContentType <> GOODIE_CONTENTTYPE_VIDEO_CLIP Then
		Response.Write "<tr><td bgcolor='#FFFFCC'>&#59" & (105 + l_lngAccessKey) & "; <a href='categories.asp?cg=" & GOODIE_CONTENTGROUP_VIDEO_RGT & "&ct=" & GOODIE_CONTENTTYPE_VIDEO_CLIP & _
																	"&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Les Sonneries Digital Vid&eacute;o</a></td></tr>"
		l_lngAccessKey = 2
	End If
	
	If strContentType <> GOODIE_CONTENTTYPE_VIDEO_SSAVER Then
		Response.Write "<tr><td bgcolor='#FFFFCC'>&#59" & (105 + l_lngAccessKey) & "; <a href='categories.asp?cg=" & GOODIE_CONTENTGROUP_VIDEO & "&ct=" & GOODIE_CONTENTTYPE_VIDEO_SSAVER & _
																	"&b=" & Common_GetCurrentRelativeURL() & "' accesskey='" & l_lngAccessKey & "'>Les Fonds d'Ecran Vid&eacute;o</a></td></tr>"
	End If

End Function

Function Common_LoadPageText( strPage )
	Dim l_objXmlDoc
	Dim l_objNodes
	Dim l_objDico
	Dim l_strKey
	Dim l_lngIdx
	Dim l_lngIDLanguage
	
	'On Error Resume Next

	Set l_objDico = Server.CreateObject("Scripting.Dictionary")
	
	Set l_objXmlDoc = Server.CreateObject("MSXML2.DomDocument")
	l_objXmlDoc.async = False
	l_objXmlDoc.load( Server.MapPath("/Engine/Xml/parameters.xml" ) )
		
	Set l_objNodes = l_objXmlDoc.selectNodes( "./parameters/page[@id='" & strPage & "']/language[@id='6']/object" )

	For l_lngIdx = 0 To l_objNodes.length - 1
		l_objDico.Add l_objNodes.item(l_lngIdx).attributes.getNamedItem("id").Text, l_objNodes.item(l_lngIdx).Text
	Next											
		
	Set l_objNodes = Nothing
	Set l_objXmlDoc = Nothing
	
	'Dim name
	'for each name in l_objDico
	'	Response.Write name & " : " & server.HTMLEncode(l_objDico(name)) & "<br>"
	'next

	Set Common_LoadPageText = l_objDico

	Set l_objDico = Nothing	
	
End Function

Function Common_IsVideo()
	Common_IsVideo = (Common_GetMobileType() = "NEC401i" Or Common_GetMobileType() = "MITSUBISHIM430i" Or Common_GetMobileType() = "MITSUBISHIM420i" Or Common_GetMobileType() = "_IE" Or Common_GetMobileType() = "SAMSUNGS410i")
	'Common_IsVideo = Common_IsMobileCompatible(Common_GetMobileType(), "SOUND_VIDEO")
End Function

Function Common_GetUrlBillingScreens(strType)
	Dim l_strURLByTel
	l_strURLByTel = IMODE_URL_WIND & "?ci=00234000366&uid=UIDREQUEST&nl=" & Server.HTMLEncode("http://" & Request.ServerVariables("HTTP_HOST") & "/logos/default2.asp?uid=UIDREQUEST") & "&rl=" & Request.ServerVariables("HTTP_HOST") & "/subscription.asp&act=" & strType & "&arg1=40&arg2=IMG"
	Common_GetUrlBillingScreens = l_strURLByTel
End Function

Function Common_GetUrlBillingTones(strType)
	Dim l_strURLByTel
	l_strURLByTel = IMODE_URL_WIND & "?ci=00234000371&uid=UIDREQUEST&nl=" & Server.HTMLEncode("http://" & Request.ServerVariables("HTTP_HOST") & "/ringtones/default.asp?uid=UIDREQUEST") & "&rl=" & Request.ServerVariables("HTTP_HOST") & "/subscription.asp&act=" & strType & "&arg1=41&arg2=SOUND"
	Common_GetUrlBillingTones = l_strURLByTel
End Function

Function Common_GetUrlBillingHb(strType)
	Dim l_strURLByTel
	l_strURLByTel = IMODE_URL_WIND & "?ci=00234000414&uid=UIDREQUEST&nl=" & Server.HTMLEncode("http://" & Request.ServerVariables("HTTP_HOST") & "/hb/default2.asp?uid=UIDREQUEST") & "&rl=" & Request.ServerVariables("HTTP_HOST") & "/subscription.asp&act=" & strType & "&arg1=45&arg2=HB"
	Common_GetUrlBillingHb = l_strURLByTel
End Function

Function Common_GetUrlBillingLt(strType)
	Dim l_strURLByTel
	l_strURLByTel = IMODE_URL_WIND & "?ci=00234000415&uid=UIDREQUEST&nl=" & Server.HTMLEncode("http://" & Request.ServerVariables("HTTP_HOST") & "/lt/default2.asp?uid=UIDREQUEST") & "&rl=" & Request.ServerVariables("HTTP_HOST") & "/subscription.asp&act=" & strType & "&arg1=48&arg2=LT"
	Common_GetUrlBillingLt = l_strURLByTel
End Function

'-------------------------
' Mail Sender
'-------------------------

Sub Common_SendMail( strTo, strSubject, strBody, boolIsHtml, strEncoding )
	Dim l_strUrl
	Dim l_objXMLHTTP
  On Error Resume Next

	l_strURL = IMODE_URL_MAILSERVICE & "SendMail"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "billingKey=" & IMODE_BILLING_KEY & "&to=" & Server.URLEncode(strTo) & _
						"&subject=" & Server.URLEncode(strSubject) & "&body=" & Server.URLEncode(strBody) & "&isHtml=" & boolIsHtml & _
						"&encoding=" & strEncoding

	Set l_objXMLHTTP = Nothing
End Sub


'---------------------------------------------------------------
'										PARAMETERS
'---------------------------------------------------------------

%>