<% 
'_________________________________________________________________________________________________________
'
' Constantes

Const WIND_OPERATOR = 3

'_________________________________________________________________________________________________________
'
' Variables

Dim sub_strLanguage
Dim sub_strContentGroup

Dim sub_lngErrNumber

'_____________________________________________________________________________________________
'
' Properties
'

Function Subscription_GetLastError()
	Subscription_GetLastError = CLng( sub_lngErrNumber )
End Function

Sub Subscription_SetLastError( rhs )
	If CLng( rhs ) <> 0 Or sub_lngErrNumber <= 0 Then sub_lngErrNumber = CLng( rhs )
End Sub

Sub Subscription_ClearLastError()
	Err.Clear
	sub_lngErrNumber = 0
End Sub

'_________________________________________________________________________________________________________
'
' Init 
'
Sub Subscription_Init( ByVal strLanguage, ByVal strContentGroup )
	sub_strLanguage = strLanguage
	sub_strContentGroup = strContentGroup
End Sub

'_________________________________________________________________________________________________________

Function Subscription_Add( ByVal lngIDService, ByVal strUID, ByVal strSpecialOfferDate )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIModeWind_Subscription_Add"
	l_objSP.AddParameter "@IDService", CLng(lngIDService)
	l_objSP.AddParameter "@UID", CStr(strUID)
	If strSpecialOfferDate <> "" Then
		l_objSP.AddParameter "@SpecialOfferDay", CLng(Left(strSpecialOfferDate, 2))
		l_objSP.AddParameter "@SpecialOfferMonth", CLng(Mid(strSpecialOfferDate, 3, 2))
		l_objSP.AddParameter "@SpecialOfferYear", CLng(Right(strSpecialOfferDate, 4))
	End If
	
	l_objSP.Execute
	
	Set l_objSP = Nothing
	
	Subscription_SetLastError Err.number 
	Subscription_Add = (Subscription_GetLastError = 0)
End Function

Function Subscription_Cancel( ByVal lngIDService, ByVal strUID )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Subscription_Cancel"
	l_objSP.AddParameter "@IDService", CLng(lngIDService)
	l_objSP.AddParameter "@UID", CStr(strUID)
	
	l_objSP.Execute
	
	Set l_objSP = Nothing
	
	Subscription_SetLastError Err.number 
	Subscription_Cancel = (Subscription_GetLastError = 0)
End Function

Function Subscription_Check( ByVal strUID, ByRef objSub )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Subscription_Check"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	l_objSP.AddParameter "@IDOperator", WIND_OPERATOR
	
	Set objSub = l_objSP.Execute
	
	Set l_objSP = Nothing
	
	Subscription_SetLastError Err.number 
	Subscription_Check = (Subscription_GetLastError = 0)
End Function

Function Subscription_IsUIDSubscribed( ByVal strUID )
	Dim l_objRs
	
	If Subscription_Check( strUID, l_objRs ) Then
		Subscription_IsUIDSubscribed = Not l_objRs.EOF
	Else
		Subscription_IsUIDSubscribed = False
	End If
End Function

Function Subscription_ProcessDownload( ByVal lngIDService, ByVal strUID, ByVal strContentName, ByVal strContentGroup, ByVal strContentType, ByVal strMobileType, ByVal lngIDTicket, ByVal strReferer, ByVal lngCredit )
	Subscription_ClearLastError
	
	If Subscription_Download( lngIDService, strUID, lngCredit ) = 0 Then
		Call Subscription_InsertDownload( lngIDService, strUID, strContentName, strContentGroup, strContentType, strMobileType, lngIDTicket, strReferer )
		Subscription_ProcessDownload = True
	Else
		Subscription_ProcessDownload = False
	End If
End Function

Function Subscription_Download( ByVal lngIDService, ByVal strUID, ByVal lngCredit )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Subscription_Download"
	l_objSP.AddParameter "@IDService", CLng(lngIDService)
	l_objSP.AddParameter "@UID", CStr(strUID)
	If Not IsNull(lngCredit) Then l_objSP.AddParameter "@Credit", CLng(lngCredit)
	
	l_objSP.Execute
	
	Subscription_SetLastError Err.number 	
	If Subscription_GetLastError = 0 Then
		Subscription_Download = l_objSP.Parameters("@Result").Value
	Else		
		Subscription_Download = Subscription_GetLastError()
	End If
	
	Set l_objSP = Nothing
End Function

Function Subscription_InsertDownload( ByVal lngIDService, ByVal strUID, ByVal strContentName, ByVal strContentGroup, ByVal strContentType, ByVal strMobileType, ByVal lngIDTicket, ByVal strReferer )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Downloads_Insert"
	l_objSP.AddParameter "@IDService", CLng(lngIDService)
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentName", CStr(strContentName)
	l_objSP.AddParameter "@ContentGroup", CStr(strContentGroup)
	l_objSP.AddParameter "@ContentType", CStr(strContentType)
	l_objSP.AddParameter "@MobileType", CStr(strMobileType)
	l_objSP.AddParameter "@IDTicket", CLng(lngIDTicket)
	l_objSP.AddParameter "@Referer", CStr(strReferer)
	
	l_objSP.Execute
	
	Set l_objSP = Nothing

	Subscription_SetLastError Err.number 	
	Subscription_InsertDownload = Subscription_GetLastError()

End Function

Function Subscription_SelectService( ByRef objSrv )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Service_SelectByContentGroup"
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	l_objSP.AddParameter "@IDOperator", WIND_OPERATOR
	
	Set objSrv = l_objSP.Execute
	
	Set l_objSP = Nothing
	
	Subscription_SetLastError Err.number 
	Subscription_SelectService = (Subscription_GetLastError = 0)
End Function

Function Subscription_Newsletter( ByVal strUID, ByVal strEmail, ByVal bytSubscription )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Newsletter_Insert"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	If Not IsNull(strEmail) Then l_objSP.AddParameter "@Email", CStr(strEmail)
	l_objSP.AddParameter "@Enabled", CByte(bytSubscription)
	
	l_objSP.Execute
	
	Set l_objSP = Nothing
	
	Subscription_SetLastError Err.number 
	Subscription_Newsletter = (Subscription_GetLastError = 0)
End Function

Function Subscription_CheckNewsletter( ByVal strUID )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Newsletter_Check"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	
	l_objSP.Execute
	
	If Err.number <> 0 Then
		Subscription_CheckNewsletter = ""
	Else
		Subscription_CheckNewsletter = CStr(l_objSP.Parameters("@Email"))
	End If
	Set l_objSP = Nothing	
End Function

Function Subscription_CheckDownloads( ByVal strUID, ByVal strContentName )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Downloads_Check"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentName", CStr(strContentName)
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	
	l_objSP.Execute
	
	If Err.number <> 0 Then
		Subscription_CheckDownloads = False
	Else
		Subscription_CheckDownloads = CBool(l_objSP.Parameters("@Result"))
	End If
	Set l_objSP = Nothing	
End Function

Function Subscription_CheckDownloads_Free( ByVal strUID, ByVal strContentName, ByVal strMobileType )
	Dim l_objSP
	'On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Downloads_Check_Free"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentName", CStr(strContentName)
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	l_objSP.AddParameter "@MobileType", CStr(strMobileType)
	l_objSP.Execute
	
	If Err.number <> 0 Then
		Subscription_CheckDownloads_Free = False
	Else
		Subscription_CheckDownloads_Free = CBool(l_objSP.Parameters("@Result"))
	End If
	Set l_objSP = Nothing	
End Function

Function Subscription_SelectDownloads( ByVal strUID, ByRef strReferer, ByRef objRs )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Downloads_Select"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	If Not IsNull( strReferer ) Then l_objSP.AddParameter "@Referer", CStr(strReferer)
	
	Set objRs = l_objSP.Execute
	
	Set l_objSP = Nothing	

	Subscription_SetLastError Err.number 
	Subscription_SelectDownloads = (Subscription_GetLastError = 0)
End Function

Function Subscription_SelectDownloadsByContentGroup( ByVal strUID, ByVal strContentGroup, ByRef strReferer, ByRef objRs )
	Dim l_objSP
	'On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Downloads_Select"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	l_objSP.AddParameter "@ContentGroupDwld", CStr(strContentGroup)
	If Not IsNull( strReferer ) Then l_objSP.AddParameter "@Referer", CStr(strReferer)	
	
	Set objRs = l_objSP.Execute
	
	Set l_objSP = Nothing	

	Subscription_SetLastError Err.number 
	Subscription_SelectDownloadsByContentGroup = (Subscription_GetLastError = 0)
End Function

Function Subscription_SelectDownloadsByContentType( ByVal strUID, ByVal strContentGroup, ByVal strContentType, ByRef strReferer, ByRef objRs )
	Dim l_objSP
	'On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Downloads_Select"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@ContentGroup", CStr(sub_strContentGroup)
	l_objSP.AddParameter "@ContentGroupDwld", CStr(strContentGroup)
	If Not IsNull(strContentType) Then l_objSP.AddParameter "@ContentTypeDwld", CStr(strContentType)
	If Not IsNull( strReferer ) Then l_objSP.AddParameter "@Referer", CStr(strReferer)	
	
	Set objRs = l_objSP.Execute
	
	Set l_objSP = Nothing	

	Subscription_SetLastError Err.number 
	Subscription_SelectDownloadsByContentType = (Subscription_GetLastError = 0)
End Function

Function Subscription_CheckEmail( ByVal strUID )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Email_Check"
	l_objSP.AddParameter "@UID", CStr(strUID)
	
	l_objSP.Execute
	
	If Err.number <> 0 Then
		Subscription_CheckEmail = ""
	Else
		Subscription_CheckEmail = CStr(l_objSP.Parameters("@Email"))
	End If
	Set l_objSP = Nothing	
End Function

Function Subscription_InsertEmail( ByVal strUID, ByVal strEmail )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Email_Insert"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@Email", CStr(strEmail)
	
	l_objSP.Execute
	
	Set l_objSP = Nothing	

	Subscription_SetLastError Err.number 
	Subscription_InsertEmail = (Subscription_GetLastError = 0)
End Function

Function Subscription_UpdateEmail( ByVal strUID, ByVal strEmail )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Email_Update"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@Email", CStr(strEmail)
	
	l_objSP.Execute
	
	Set l_objSP = Nothing	

	Subscription_SetLastError Err.number 
	Subscription_UpdateEmail = (Subscription_GetLastError = 0)
End Function

Function Subscription_SelectSubscriptions( ByVal strUID, ByVal lngIDService, ByVal strSubscriptionDate, ByRef objRs )
	Dim l_objSP
	'On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_Subscription_GetSubscriptions"
	l_objSP.AddParameter "@UID", CStr(strUID)
	If Not IsNull(lngIDService) Then l_objSP.AddParameter "@IDService", CLng(lngIDService)
	If Not IsNull(strSubscriptionDate) Then l_objSP.AddParameter "@SubscriptionDate", strSubscriptionDate
	
	Set objRs = l_objSP.Execute
	
	Set l_objSP = Nothing	

	Subscription_SetLastError Err.number 
	Subscription_SelectSubscriptions = (Subscription_GetLastError = 0)
End Function

Function Subscription_InsertSendTo(	ByVal strUID, ByVal lngIDService, ByVal lngIDContent, ByVal strContentGroup, ByVal strContentType, _
																		ByVal strMessage, ByVal strNameSender, ByVal strNameRecipient, ByVal strEmailRecipient, ByRef lngIDSendTo )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_SendTo_Insert"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@IDService", CLng(lngIDService)	
	l_objSP.AddParameter "@IDContent", CStr(lngIDContent)
	l_objSP.AddParameter "@ContentGroup", CStr(strContentGroup)
	l_objSP.AddParameter "@ContentType", CStr(strContentType)
	If strNameSender <> "" Then l_objSP.AddParameter "@NameSender", CStr(strNameSender)
	If strNameRecipient <> "" Then l_objSP.AddParameter "@NameRecipient", CStr(strNameRecipient)
	l_objSP.AddParameter "@EmailRecipient", CStr(strEmailRecipient)
	
	l_objSP.Execute
	
	lngIDSendTo = CLng(l_objSP.Parameters("@IDSendTo"))
	
	Set l_objSP = Nothing

	Subscription_SetLastError Err.number 	
	Subscription_InsertSendTo = (Subscription_GetLastError = 0)

End Function

Function Subscription_SelectSendTo(	ByVal lngIDSendTo, ByRef objRs )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_SendTo_Select"
	l_objSP.AddParameter "@IDSendTo", CLng(lngIDSendTo)
	
	Set objRs = l_objSP.Execute
	
	Set l_objSP = Nothing	

	Subscription_SetLastError Err.number 
	Subscription_SelectSendTo = (Subscription_GetLastError = 0)
End Function

Function Subscription_UpdateSendTo(	ByVal lngIDSendTo, ByVal lngIDTicket, ByVal strUID )
	Dim l_objSP
	On Error Resume Next
	Subscription_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = sub_strLanguage
	l_objSP.CommandText = "PIMode_SendTo_Update"
	l_objSP.AddParameter "@IDSendTo", CLng(lngIDSendTo)
	l_objSP.AddParameter "@IDTicket", CLng(lngIDTicket)
	l_objSP.AddParameter "@UID", CStr(strUID)
	
	l_objSP.Execute
	
	Set l_objSP = Nothing

	Subscription_SetLastError Err.number 	
	Subscription_UpdateSendTo = (Subscription_GetLastError = 0)

End Function
%>
