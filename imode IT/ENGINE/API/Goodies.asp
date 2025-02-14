<!-- #include virtual="/Engine/API/Goodies_Constant.asp" -->
<% 

'_________________________________________________________________________________________________________
'
' Variables

Dim gd_strContentGroup
Dim gd_strContentType
Dim gd_strConnectionString
Dim gd_strDisplayKey
Dim gd_strMobileType

'_____________________________________________________________________________________________
'
' Properties
'
Dim gd_lngErrNumber

Function Goodies_GetLastError()
	Goodies_GetLastError = CLng( gd_lngErrNumber )
End Function

Sub Goodies_SetLastError( rhs )
	If CLng( rhs ) <> 0 Or gd_lngErrNumber <= 0 Then gd_lngErrNumber = CLng( rhs )
End Sub

Sub Goodies_ClearLastError()
	Err.Clear
	gd_lngErrNumber = 0
End Sub

Sub Goodies_SetMobileType( strMobileType )
	gd_strMobileType = strMobileType
End Sub

Function Goodies_GetMobileType()
	Goodies_GetMobileType = gd_strMobileType
End Function

Function Goodies_GetContentGroup()
	Goodies_GetContentGroup = gd_strContentGroup
End Function

Function Goodies_GetContentType()
	Goodies_GetContentType = gd_strContentType
End Function


'_________________________________________________________________________________________________________
'
' Init 
'
Sub Goodies_Init( ByVal strDisplayKey, ByVal strContentGroup, ByVal strContentType )
	gd_strDisplayKey = strDisplayKey
	gd_strContentGroup = strContentGroup
	gd_strContentType = strContentType
End Sub

'_________________________________________________________________________________________________________
'
' Functions
'
Function Goodies_FormatPathPreviewByContentGroup( strContentName, strTypePreview )
	Dim l_strUrlImg
	On Error Resume Next

	l_strUrlImg = Goodies_GetURL( strContentName ) & "/preview/GIF/" & strContentName & "." & strTypePreview & ".gif"
	
	Goodies_FormatPathPreviewByContentGroup = l_strUrlImg
End Function


Function Goodies_GetURL( strContentName )
	Dim l_strUrl
	
	l_strUrl = Left(strContentName, 1)
	
	l_strUrl = "http://content.k-mobile.com/V2/Data/" & gd_strContentGroup & "/" & l_strUrl & "/" & strContentName
	
	Goodies_GetURL = l_strUrl
End Function

Function Goodies_GetImgPreview( ByVal strContentName, ByVal strResolution )
	Goodies_GetImgPreview = Goodies_GetImgPreviewEx( strContentName, "GIF", strResolution )
End Function

Function Goodies_GetImgPreviewEx( ByVal strContentName, ByVal strMimeType, ByVal strResolution )
	Goodies_GetImgPreviewEx = Goodies_GetURL(strContentName) &  "/PREVIEW/" & strMimeType & "/" & strContentName & "." & strResolution & "." & strMimeType
End Function

'------------------------------------------------------------------------

Function Goodies_GetContentInfos(ByVal lngIDContent, ByVal strMobileType, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_objDOMDoc
	Dim l_strURL
	'On Error Resume Next
			
	l_strURL = GOODIE_URL_CATALOGSERVICE & "GetContentInfos"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	Set l_objDOMDoc = Server.CreateObject("Msxml2.DOMDocument")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	l_objXMLHTTP.send "displayKey=" & gd_strDisplaykey &"&contentID=" & lngIDContent & _
						"&mobileType=" & strMobileType & "&contentType=" & gd_strContentType
	l_objDOMDoc.loadXML (l_objXMLHTTP.responseXML.xml)
		
	strXML = l_objXMLHTTP.responseXML.xml

	Set	l_objDOMDoc = Nothing
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_GetContentInfos = (Goodies_GetLastError = 0)
End Function

'-----------------------------------------
'	Utilisé dans Perso.asp
Function Goodies_GetContentInfosByContentName(ByVal strContentName, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_objDOMDoc
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_CATALOGSERVICE & "GetContentInfosByContentName"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	Set l_objDOMDoc = Server.CreateObject("Msxml2.DOMDocument")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	l_objXMLHTTP.send "displayKey=" & gd_strDisplaykey &"&contentName=" & strContentName & _
						"&mobileType=" & gd_strMobileType & "&contentType=" & gd_strContentType
	l_objDOMDoc.loadXML (l_objXMLHTTP.responseXML.xml)
		
	strXML = l_objXMLHTTP.responseXML.xml

	Set	l_objDOMDoc = Nothing
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_GetContentInfosByContentName = (Goodies_GetLastError = 0)
End Function

'-----------------------------------------
'	Utilisé dans Search.asp
Function Goodies_GetContentInfosByTrackingCode(ByVal strTrackingCode, ByVal strMobileType, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_objDOMDoc
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_CATALOGSERVICE & "GetContentInfosByTrackingCode"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	Set l_objDOMDoc = Server.CreateObject("Msxml2.DOMDocument")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	l_objXMLHTTP.send "displayKey=" & gd_strDisplaykey &"&trackingCode=" & strTrackingCode & _
						"&mobileType=" & strMobileType
	l_objDOMDoc.loadXML (l_objXMLHTTP.responseXML.xml)
		
	strXML = l_objXMLHTTP.responseXML.xml

	Set	l_objDOMDoc = Nothing
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_GetContentInfosByTrackingCode = (Goodies_GetLastError = 0)
End Function

Function Goodies_GetContentsByContentSet2( ByVal lngIDContentSet, ByRef strXML) 
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next
	l_strURL = GOODIE_URL_CATALOGSERVICE & "GetContentsByContentSet2"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "displayKey=" & gd_strDisplayKey & "&contentSetID=" & lngIDContentSet & _
						"&contentGroup=" & gd_strContentGroup & "&contentType=" & gd_strContentType & _
						"&mobileType=" & gd_strMobileType
										
	strXML = l_objXMLHTTP.responseXML.xml
	
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_GetContentsByContentSet2 = (Goodies_GetLastError = 0)	

End Function

Function Goodies_GetContentTicketExtended( ByVal strBillingKey, ByVal strReferer, ByVal lngIDContent, ByVal strMobileType, _
											ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_SENDSERVICE & "getContentTicketExtended"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "strBillingKey=" & strBillingKey & "&strDisplayKey=" & gd_strDisplayKey & _
						"&strReferer=" & strReferer & "&IDContent=" & lngIDContent & "&ContentType=" & gd_strContentType & _
						"&strMobileType=" & strMobileType

	strXML = l_objXMLHTTP.responseXML.xml
		
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_GetContentTicketExtended = (Goodies_GetLastError = 0)	
End Function

Function Goodies_SearchContent( ByVal strKeyword, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_CATALOGSERVICE & "SearchContent"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "displayKey=" & gd_strDisplayKey & "&contentGroup=" & gd_strContentGroup & _
						"&contentType=" & gd_strContentType & "&mobileType=" & gd_strMobileType & _
						"&keyWord=" & strKeyword
						
						
	strXML = l_objXMLHTTP.responseXML.xml
		
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_SearchContent = (Goodies_GetLastError = 0)	
End Function

Function Goodies_GetContentSets(ByVal strVisibility, ByVal intContentSetType, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_CATALOGSERVICE & "GetContentSet"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	l_objXMLHTTP.send "displayKey=" & gd_strDisplaykey & "&visibility=" & strVisibility & "&contentSetType=" & intContentSetType & _
			 "&contentGroup=" & gd_strContentGroup & "&contentType=" & gd_strContentType & "&mobileType=" & gd_strMobileType
			 
	'Response.Write "displayKey=" & gd_strDisplaykey & "&visibility=" & strVisibility & "&contentSetType=" & intContentSetType & _
	'		 "&contentGroup=" & gd_strContentGroup & "&contentType=" & gd_strContentType & "&mobileType=" & gd_strMobileType
			 
	strXML = l_objXMLHTTP.responseXML.xml

	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_GetContentSets = (Goodies_GetLastError = 0)
End Function

Function Goodies_GetContentCompatibilities(ByVal lngIDContent, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_CATALOGSERVICE & "GetContentCompatibilitiesByContentID"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	l_objXMLHTTP.send "displayKey=" & gd_strDisplaykey & "&contentID=" & lngIDContent
			 
	strXML = l_objXMLHTTP.responseXML.xml

	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_GetContentCompatibilities = (Goodies_GetLastError = 0)
End Function

Function Goodies_GetContentTicketFileEx( ByVal strBillingKey, ByVal strReferer, ByVal lngIDContent, ByVal strMobileType, _
											ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_SENDSERVICE & "GetContentTicketFileEx"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "billingKey=" & strBillingKey & "&displayKey=" & gd_strDisplayKey & _
						"&referer=" & strReferer & "&contentID=" & lngIDContent & "&contentType=" & gd_strContentType & _
						"&mobileType=" & strMobileType

	strXML = l_objXMLHTTP.responseXML.xml
		
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_GetContentTicketFileEx = (Goodies_GetLastError = 0)	
End Function

Function Goodies_ExaSearchExt( ByVal strKeyword, ByVal lngStartElement, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_strURL
	'On Error Resume Next

	l_strURL = GOODIE_URL_CATALOGSERVICE & "ExaSearchExt"
	
	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "displayKey=" & gd_strDisplayKey & "&contentGroup=" & gd_strContentGroup & _
						"&contentType=" & gd_strContentType & "&mobileType=" & gd_strMobileType & "&keyword=" & strKeyword & _
						"&startElement=" & lngStartElement
	strXML = l_objXMLHTTP.responseXML.xml
	'Response.Write(strXML)
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_ExaSearchExt = (Goodies_GetLastError = 0)	
End Function

Function Goodies_ExaSearchRefine( ByVal strContext, ByVal strHrefParam, ByVal lngStartElement, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_CATALOGSERVICE & "ExaSearchRefine"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "displayKey=" & gd_strDisplayKey & "&context=" & Server.URLEncode(strContext) & _
						"&hrefParam=" & Server.URLEncode(strHrefParam) & "&mobileType=" & gd_strMobileType & _
						"&startElement=" & lngStartElement
						
	

	strXML = l_objXMLHTTP.responseXML.xml
		
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_ExaSearchRefine = (Goodies_GetLastError = 0)	
End Function

Function Goodies_ExaSearch( ByVal strKeyword, ByVal lngStartElement, ByRef strXML)
	Dim l_objXMLHTTP	
	Dim l_strURL
	On Error Resume Next
			
	l_strURL = GOODIE_URL_CATALOGSERVICE & "ExaSearch"

	Set l_objXMLHTTP = Server.CreateObject("MSXML2.XMLHTTP")
	
	l_objXMLHTTP.open "POST", l_strURL, False
	l_objXMLHTTP.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
	
	l_objXMLHTTP.send "displayKey=" & gd_strDisplayKey & "&contentGroup=" & gd_strContentGroup & _
						"&contentType=" & gd_strContentType & "&mobileType=" & gd_strMobileType & "&keyword=" & strKeyword & _
						"&startElement=" & lngStartElement

	strXML = l_objXMLHTTP.responseXML.xml
		
	Set l_objXMLHTTP = Nothing
	
	Goodies_SetLastError Err.Number
	Goodies_ExaSearch = (Goodies_GetLastError = 0)	
End Function
%>
