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
	If g_strUrlBack = "" Then g_strUrlBack = "default2.asp?uid=UIDREQUEST"	
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
<div align="center"> 
	<img src="../images/hb<% = Common_GetMobileWidth() %>.gif" alt="Hanna Barbera">
  <table width="100%" cellspacing="3">
    <tr> 
      <td bgcolor="#008000"> 
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
		<td bgcolor="#008000">&#59113; <a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a></td>
	</tr>
  <tr> 
    <td bgcolor="#008000">&#59114; <a href="default2.asp?uid=UIDREQUEST" accesskey="9">Hanna Barbera HomePage</a></td>
  </tr>	
</table>
</body>
</html>
