<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_strUrlBack

'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Goodies_Constant.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Call InitVariablesFromRequest()
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default.asp?uid=UIDREQUEST"	
End Sub

'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
%>
<html>
<head>
<title>KiweeScreens</title>

</head>

<body bgcolor="#FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<div align="center"> 
  <img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens">
  <table width="100%" cellspacing="3">
    <tr> 
      <td bgcolor="#FFB6AA"> 
        <div align="center">Tel Compatibili VIDEO</div>
      </td>
    </tr>
  </table>
</div>
<p align="left">
I Terminali che consentono la visione dei video sono:
<ul>
	<li>MITSUBISHIM430i</li>
	<li>MOTOROLAL7i</li>
	<li>MOTOROLAV3XXi</li>
	<li>NEC401i</li>
	<li>NEC411i</li>
	<li>NECN412i</li>
	<li>NECN500i</li>
	<li>NOKIAN70</li>
	<li>SAMSUNGS400i</li>
	<li>SAMSUNGS401i</li>
	<li>SAMSUNGS410i</li>
	<li>SAMSUNGS501i</li>
	<li>SAMSUNGS720i</li>
	<li>SAMSUNGS730i</li>
	<li>SAMSUNGZ320i</li>
	<li>SAMSUNGZ650i</li>
	<li>SONYERICSSONK550im</li>
	<li>SONYERICSSONK610im</li>
	<li>SONYERICSSONZ1010</li>
</ul>
</p>

<table cellspacing="3" width="100%">
	<tr> 
		<td>&nbsp;</td>
	</tr>		
	<tr> 
		<td bgcolor="#FFFFCC">&#59113; <a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a></td>
	</tr>
  <tr> 
    <td bgcolor="#FFFFCC">&#59114; <a href="default.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
  </tr>	
</table>
</body>
</html>
