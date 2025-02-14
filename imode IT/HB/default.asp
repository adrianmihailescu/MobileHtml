<% 

'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/User.asp" -->
<%

	Call User_Init( Common_GetConnectionString() )
	'Call User_Init( "ImodeTest")
	Call User_Visit_Insert (Common_GetUID(), 45, Common_GetMobileType())
	Response.Redirect "http://imode.kiwee.it/hb/default2.asp"

%>
