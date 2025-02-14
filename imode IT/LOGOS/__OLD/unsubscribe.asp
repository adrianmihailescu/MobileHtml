<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************

Dim g_strURL
Dim g_strArg1
Dim g_strArg2

Dim g_strHTMLLinkSubscription
Dim g_strUrlBack

'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Subscription.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()
	Call InitVariablesFromRequest()

	Call Subscription_Init( Common_GetConnectionString(), "IMG")
End Sub

Sub InitVariablesFromRequest()
	On Error Resume Next
	g_strURL = CStr(Request.QueryString("nl"))	
	If g_strURL = "" Then
		g_strURL = "http://" & Request.ServerVariables("HTTP_HOST") & "/logos/default.asp"
	End If
	
	g_strUrlBack = CStr(Request.QueryString("b"))
	If g_strUrlBack = "" Then g_strUrlBack = "default.asp?uid=UIDREQUEST"		
End Sub

Sub SelectService()
	Dim l_objRs
	Dim l_strHTML
	Dim l_strURLByTel
	
	If Subscription_SelectService( l_objRs ) Then
		While Not l_objRs.EOF
				
			l_strURLByTel = IMODE_URL_WIND & "?ci=" & l_objRs("IDByTel") & "&uid=UIDREQUEST&nl=" & g_strURL & "&rl=" & Request.ServerVariables("HTTP_HOST") & "/subscription.asp&act=rel&arg1=" & l_objRs("IDService") & "&arg2=IMG"
			'Response.Write l_strURLByTel & "<br>"
			'l_strURLByTel = "/Subscription.asp?uid=" & Common_GetUID(False) & "&act=reg&arg1=" & l_objRs("IDService") & "&arg2=" & IMODE_CONTENTGROUP_IMG
			g_strHTMLLinkSubscription = g_strHTMLLinkSubscription & "&#5910" & (6 + l_objRs.AbsolutePosition) & "; <a href='" & l_strURLByTel & "' accesskey='" & (l_objRs.AbsolutePosition + 1) & "'>Ho letto le condizioni e mi disattivo a " & l_objRs("Name") & " (" & l_objRs("Price") & " &#8364;)</a><br>" & vbCrLf

			l_objRs.Movenext
		Wend
		
		l_objRs.Close
		Set l_objRs = Nothing
	End If
	Response.Write l_strHTML
End Sub


'*************************************************
'					TREATMENT
'*************************************************

Call Page_Initialize()
Call SelectService()
%>
<html>
<head>
<title>KiweeScreens</title>

</head>

<body bgcolor="FFFFFF" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<div align="center"> 
  <p><img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"></p>
</div>
<div>Attenzione, se metti fine all'abbonamento nel corso del mese l'effetto sar&agrave; immediato 
e senza rimborso, neanche parziale, del mese in corso; inoltre, perderai gli eventuali crediti residui.<br>
</div>
<table width="100%" cellspacing="3">
<tr><td>
<hr size="1">
&#59106; <a href="conditions.asp" accesskey="1">Leggere le condizioni</a> 
</td></tr>
<% If g_strHTMLLinkSubscription <> "" Then %> 
<tr><td>
<hr size="1">
<% Response.Write g_strHTMLLinkSubscription %>
</td></tr>
<% End If %>
</table>
<br>
	<table cellspacing="3" width="100%" ID="Table1">
		<tr> 
			<td bgcolor="#FFFFCC">&#59113; <a href="<% = g_strUrlBack %>" accesskey="8">Indietro</a></td>
		</tr>
		<tr> 
			<td bgcolor="#FFFFCC">&#59114; <a href="default.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
		</tr>	
	</table>
</body>
</html>
