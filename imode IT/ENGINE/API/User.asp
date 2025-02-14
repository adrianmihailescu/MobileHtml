<% 
'_________________________________________________________________________________________________________
'
' Variables

Dim user_lngErrNumber
Dim user_strLanguage

'_____________________________________________________________________________________________
'
' Properties
'

Function User_GetLastError()
	User_GetLastError = CLng( user_lngErrNumber )
End Function

Sub User_SetLastError( rhs )
	If CLng( rhs ) <> 0 Or user_lngErrNumber <= 0 Then user_lngErrNumber = CLng( rhs )
End Sub

Sub User_ClearLastError()
	Err.Clear
	user_lngErrNumber = 0
End Sub

'_________________________________________________________________________________________________________
'
' Init 
'
Sub User_Init( ByVal strLanguage )
	user_strLanguage = strLanguage
End Sub

'_________________________________________________________________________________________________________

Function User_Visit_Insert( ByVal strUID, ByVal idSite, ByVal mobile )
	Dim l_objSP
	User_ClearLastError
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	If (Not Isnull(strUID)) And (Cstr(strUID) <> "") And (Cstr(strUID) <> "UIDREQUEST") Then
	
		l_objSP.Language = user_strLanguage
		l_objSP.CommandText = "PIMode_User_Visit"
		l_objSP.AddParameter "@UID", CStr(strUID)
		l_objSP.AddParameter "@IDSite", CInt(idSite)
		If (Not IsNull(mobile)) Then
			l_objSP.AddParameter "@MobileType", CStr(mobile)
		End If
		
		l_objSP.Execute
	End If	
	Set l_objSP = Nothing
	User_SetLastError Err.number 
	User_Visit_Insert = (User_GetLastError = 0)
End Function

Function User_Info( ByVal strUID, ByVal idSite, ByRef objSub )
	Dim l_objSP
	User_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = user_strLanguage
	l_objSP.CommandText = "PIMode_User_Info"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@IDSite", CInt(idSite)
	
	Set objSub = l_objSP.Execute
	Set l_objSP = Nothing
	
	User_SetLastError Err.number 
	User_Info = (User_GetLastError = 0)	
End Function

Function User_Add_Info ( ByVal strUID, ByVal idSite, ByVal nombre, ByVal apellidos, ByVal edad, ByVal sexo, ByVal preferencias, ByVal tel )
Dim l_objSP
	User_ClearLastError
	
	Set l_objSP = Server.CreateObject("KDataLayer.StoredProc")
	l_objSP.Language = user_strLanguage
	l_objSP.CommandText = "PIMode_User_Insert"
	l_objSP.AddParameter "@UID", CStr(strUID)
	l_objSP.AddParameter "@IDSite", CInt(idSite)
	If (Not IsNull(nombre)) Then
		l_objSP.AddParameter "@Nombre", CStr(nombre)
	End If
	If (Not IsNull(apellidos)) Then
		l_objSP.AddParameter "@Apellidos", CStr(apellidos)
	End If	
	If (Not IsNull(edad)) Then
		l_objSP.AddParameter "@Edad", CInt(edad)
	End If
	If (Not IsNull(sexo)) Then
		l_objSP.AddParameter "@Sexo", CStr(sexo)
	End If	
	If (Not IsNull(preferencias)) Then
		l_objSP.AddParameter "@Preferences", CStr(preferencias)
	End If
	If (Not IsNull(tel)) Then
		l_objSP.AddParameter "@Num_Tel", CStr(tel)
	End If	
	
	l_objSP.Execute
	Set l_objSP = Nothing
	
	User_SetLastError Err.number 
	User_Add_Info = (User_GetLastError = 0)
End Function

%>