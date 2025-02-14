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
	Call User_Visit_Insert (Common_GetUID(), 40, Common_GetMobileType())
	Response.Redirect "http://imode.kiwee.it/logos/default2.asp"

%>
