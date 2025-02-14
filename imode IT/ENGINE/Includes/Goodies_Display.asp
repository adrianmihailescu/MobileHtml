<%
'*************************************************
'					VARIABLES
'*************************************************

'*************************************************
'					CONSTANTS
'*************************************************

Const FIELD_SEPARATOR = "_@_"
Const RECORD_SEPARATOR = "_#_"

'************************************************************************
'		UTILITIES
'************************************************************************

Function GoodiesDisplay_ChangeApp( strKey )
	Dim l_datChange
	Dim l_boolChange
	
	l_boolChange = False
	If Application(strKey) = "" Then
		l_boolChange = True
	Else
		l_boolChange = (DateDiff("d", Application(strKey & "@DATE"), Date()) <> 0)
	End If
	
	l_boolChange = True
	GoodiesDisplay_ChangeApp = l_boolChange
End Function

'------------------------------------------------------------------------
'************************************************************************
'		SELECTION TOP ET HOME
'************************************************************************
'------------------------------------------------------------------------


Function GoodiesDisplay_Pages( lngNbNodes, lngNbItemsPage, lngNumPage, strURL )
	Dim l_lngNbPage
	Dim l_lngPageDisplayed
	Dim l_lngNumSerie
	Dim l_lngIdx
	Dim l_strHTML
	
	l_lngPageDisplayed = 5
	
	l_lngNbPage = ((lngNbNodes - 1) \ lngNbItemsPage) + 1
	l_lngNumSerie = ((lngNumPage - 1) \ l_lngPageDisplayed) + 1
	
	If l_lngNbPage > 1 Then
		For l_lngIdx = ((l_lngNumSerie - 1) * l_lngPageDisplayed) + 1 To (l_lngNumSerie * l_lngPageDisplayed) 
			If l_lngIdx > l_lngNbPage Then Exit For
			
			If l_lngIdx = lngNumPage Then
				l_strHTML = l_strHTML & l_lngIdx & "&nbsp;"
			Else
				l_strHTML = l_strHTML & "<a href='" & strUrl & "&n=" & l_lngIdx - 1 & "'>" & l_lngIdx & "</a>&nbsp;"
			End If
		Next
	
		If l_lngNumSerie > 1 Then
			l_strHTML = "<a href='" & strUrl & "&n=" & ((l_lngNumSerie - 1) * l_lngPageDisplayed) - 1 & "'>&lt;</a>&nbsp;" & l_strHTML
		End If
		If l_lngNumSerie * l_lngPageDisplayed < l_lngNbPage Then
			l_strHTML = l_strHTML & "<a href='" & strUrl & "&n=" & l_lngIdx - 1 & "'>&gt;</a>"
		End If
	
		If l_strHTML <> "" Then
			l_strHTML = "<tr><td align='center'>Pages :" & l_strHTML & "</td></tr>"
		End If
	End If
		
	GoodiesDisplay_Pages = l_strHTML
End Function

Function GoodiesDisplay_PagesV2( ByVal lngNumPage, ByVal lngPageCount, ByVal lngNbPageDisplayed, ByVal strURL )
	Dim l_strHTML
	Dim l_lngIdx
	
	If lngPageCount < 2 Then 
		GoodiesDisplay_PagesV2 = ""
		Exit Function
	End If
								
	If lngNumPage > lngNbPageDisplayed Then
		l_strHTML = "<a href='" & strUrl & "&n=" & (lngNbPageDisplayed * ((lngNumPage - 1)\ lngNbPageDisplayed)) & "'>&lt;</a>&nbsp;" & l_strHTML
	End If							

	For l_lngIdx = (lngNbPageDisplayed * ((lngNumPage - 1)\ lngNbPageDisplayed) + 1) To (lngNbPageDisplayed * ((lngNumPage - 1)\ lngNbPageDisplayed) + 1) + lngNbPageDisplayed - 1
		If l_lngIdx > lngPageCount Then Exit For
		
		If l_lngIdx = lngNumPage Then
			l_strHTML = l_strHTML & l_lngIdx & "&nbsp;"		
		Else
			l_strHTML = l_strHTML & "<a href='" & strUrl & "&n=" & l_lngIdx & "'>" & l_lngIdx & "</a>&nbsp;"
		End If
		
	Next                                        
	
	If l_lngIdx - 1 < lngPageCount Then
		l_strHTML = l_strHTML & "<a href='" & strUrl & "&n=" & l_lngIdx & "'>&gt;</a>"
	End If
	
	GoodiesDisplay_PagesV2 = l_strHTML
End Function

Function GoodiesDisplay_GetContentSet( lngIDContentSet, boolDoja )
	Dim l_strXML
	Dim l_objXML
	Dim l_objNodes
	Dim l_objNode
	Dim l_strKey
	Dim l_strApp
	Dim l_strDoja
	On Error Resume Next
	
	If boolDoja Then l_strDoja = "@Doja" Else l_strDoja = ""	
	
	l_strKey = Goodies_GetContentType() & "@ContentSet" & lngIDContentSet & "@" & Goodies_GetMobileType() & l_strDoja
	If GoodiesDisplay_ChangeApp( l_strKey ) Then
		If Goodies_GetContentsByContentSet2( lngIDContentSet, l_strXML) Then
			'Response.Write "Web Service<br>"		
			If l_strXML <> "" Then
				Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
				l_objXML.loadXML l_strXML
				
				l_strApp = l_objXML.selectSingleNode("./ContentSet/Description").Text
				
				If Goodies_GetContentGroup() = GOODIE_CONTENTGROUP_SOUND Or boolDoja Then
					Set l_objNodes = l_objXML.selectNodes("./ContentSet/ContentCollection/Content[Preview/URL/text() != '']")
				Else
					Set l_objNodes = l_objXML.selectNodes("./ContentSet/ContentCollection/Content")
				End If
				
				For Each l_objNode In l_objNodes
				
					If Goodies_GetContentType <> GOODIE_CONTENTTYPE_SOUND_HIFI Or l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Master']/Value").Text = "1" Then
					
						l_strApp = l_strApp & RECORD_SEPARATOR & l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='ContentName']/Value").Text & FIELD_SEPARATOR & _
												l_objNode.selectSingleNode("./IDContent").Text & FIELD_SEPARATOR & _
												l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Name']/Value").Text
												
						l_strApp = l_strApp & FIELD_SEPARATOR											
						If Not IsNull(l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Performer']/Value")) Then
							l_strApp = l_strApp & l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Performer']/Value").Text
						End If
						
						l_strApp = l_strApp & FIELD_SEPARATOR											
						If Not IsNull(l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='CompositeContentGroup']/Value")) Then
							l_strApp = l_strApp & l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='CompositeContentGroup']/Value").Text
						End If					

						l_strApp = l_strApp & FIELD_SEPARATOR											
						If Not IsNull(l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='IDComposite']/Value")) Then
							l_strApp = l_strApp & l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='IDComposite']/Value").Text
						End If		
						
						l_strApp = l_strApp & FIELD_SEPARATOR											
						If Not IsNull(l_objNode.selectSingleNode("./Preview/URL")) Then
							l_strApp = l_strApp & l_objNode.selectSingleNode("./Preview/URL").Text
						End If								
						
					End If			
				Next
				
				Set l_objNode = Nothing
				Set l_objNodes = Nothing
				Set l_objXML = Nothing
			End If
			
			Application.Lock 
			Application(l_strKey & "@DATE") = Date()	
			Application(l_strKey) = l_strApp
			Application.UnLock 
		End If
	Else
		l_strApp = Application( l_strKey )
	End If

	GoodiesDisplay_GetContentSet = l_strApp
End Function

Function GoodiesDisplay_Search( strKeyword )
	Dim l_strXML
	Dim l_objXML
	Dim l_objNode
	Dim l_strApp
	
	If Goodies_SearchContent( strKeyword, l_strXML) Then
		If l_strXML <> "" Then
			Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
			l_objXML.loadXML l_strXML
				
			l_strApp = strKeyword
				
			For Each l_objNode In l_objXML.selectNodes("./ContentSet/ContentCollection/Content")
				l_strApp = l_strApp & RECORD_SEPARATOR & l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='ContentName']/Value").Text & FIELD_SEPARATOR & _
										l_objNode.selectSingleNode("./IDContent").Text & FIELD_SEPARATOR & _
										l_objNode.selectSingleNode("./PropertyCollection/Property[Name/text()='Name']/Value").Text
			Next
			
			Set l_objNode = Nothing
			Set l_objXML = Nothing
		End If
	End If			

	GoodiesDisplay_Search = l_strApp
End Function

Function GoodiesDisplay_GetContentSets( intType, boolDoja )
	Dim l_strXML
	Dim l_objXML
	Dim l_objNode, l_objNodes
	Dim l_strKey
	Dim l_strApp
	Dim l_strDoja
	
	If boolDoja Then l_strDoja = "@Doja" Else l_strDoja = ""
	
	l_strKey = Goodies_GetContentType() & "@ContentSetType" & intType & "@" & Goodies_GetMobileType() & l_strDoja
	'Response.Write "Key : " & Goodies_GetContentType() & "@ContentSet" & intType & "@" & Goodies_GetMobileType() & "<br>"
	'Response.Write GoodiesDisplay_ChangeApp( l_strKey )

	If GoodiesDisplay_ChangeApp( l_strKey ) Then

		If Goodies_GetContentSets( "All", intType, l_strXML) Then
			If l_strXML <> "" Then
				Set l_objXML = Server.CreateObject("MSXML2.DomDocument")
				l_objXML.loadXML l_strXML
				
				'If CStr(intType) = "1" Then 
					Set l_objNodes = l_objXML.selectNodes("./CatalogDisplay/ContentSetCollection/ContentSet[Priority/text()='0']")											
				'Else
				'	Set l_objNodes = l_objXML.selectNodes("./CatalogDisplay/ContentSetCollection/ContentSet")
				'End If
				
				For Each l_objNode In l_objNodes
					If l_strApp <> "" Then l_strApp = l_strApp & RECORD_SEPARATOR
				
					l_strApp = l_strApp & l_objNode.selectSingleNode("./IDContentSet").Text & FIELD_SEPARATOR & _
											l_objNode.selectSingleNode("./Name").Text & FIELD_SEPARATOR & _
											l_objNode.selectSingleNode("./Count").Text

				Next
				
				Set l_objNode = Nothing
				Set l_objXML = Nothing
			End If
			
			Application.Lock 
			Application(l_strKey & "@DATE") = Date()	
			Application(l_strKey) = l_strApp
			Application.UnLock 
		End If
	Else
		l_strApp = Application( l_strKey )
	End If
	
	GoodiesDisplay_GetContentSets = l_strApp
End Function
%>


