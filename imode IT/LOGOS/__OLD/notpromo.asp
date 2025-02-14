<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_objDico
Dim g_strReferer


'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Goodies_Constant.asp" -->
<!--#include virtual="/Engine/API/Subscription.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Set g_objDico = Common_LoadPageText( "SCREENS" )
	Call InitVariablesFromRequest()
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strReferer = CStr(Request.QueryString("ref"))		
End Sub

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()

%>
<html>
<head>
<title>Kiwee Screens</title>

</head>

<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<div align="center"> 
 
    <p><img src="/images/screens<% = Common_GetMobileWidth() %>.gif" alt="Kiweescreens"></p>


  <table width="100%" cellspacing="3">
    <tr> 
      <td bgcolor="#FFB600" align="center">
        <font color="black">La scarica gratuita è reservata ai abbonati di KiweeScreens</font>
      </td>
    </tr>
  </table>

 <table width="100%" cellspacing="3" ID="Table2">
	<tr> 
		<td align="center">
                              <a href="/logos/subscribe.asp">Abbonati e approfittati di questa promozione.</a> Anche, i tuoi accreditamenti saranno duplicati nel primo mese di abbonamento!
		</td>
	</tr>
	<tr>
	          <td align="center"><a href="/logos/subscribe.asp">Abbonati</a></td>
	</tr>
</table>
  
 
 <table width="100%" cellspacing="3" ID="Table1">
	<tr> 
		<td><hr color="<% = g_objDico("COLOR_HEADER_BAR1") %>"></td>
	</tr>
	<tr>
                    <td>&#59115; <a href="/logos/default.asp?ref=<%=g_strReferer%>" accesskey="0">KiweeScreens HomePage</a></td>	
	</tr>
  </table>
</body>
</html>
