<% 
Option Explicit

Response.CacheControl = "Public"
'Response.Expires = 20
Response.Buffer = True

'*************************************************
'					VARIABLES
'*************************************************


'*************************************************
'					INCLUDES
'*************************************************
%>
<!--#include virtual="/Engine/Includes/Common.asp" -->
<!--#include virtual="/Engine/API/Goodies.asp" -->
<%
'*************************************************
'					FUNCTIONS
'*************************************************

Sub Page_Initialize()

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
  <img src="/images/screensmall<% = Common_GetMobileWidth() %>.gif" alt="KiweeScreens"><br>
  </div>
		<table width="100%" cellspacing="0" cellpadding="0" ID="Table1">
			<tr>
				<td bgcolor="#FFB6AA" align="center">Condizioni</td>
			</tr>
		</table>  
<div align="left">
	<p>
	La registrazione ha la durata di 1 mese, sar&agrave; automaticamente rinnovata lo stesso giorno del mese successivo per la medesima durata, 
	salvo disdetta, e comporta il pagamento dell'importo indicato nel sito stesso. La registrazione effettuata il 31 del mese scadr&agrave; l'ultimo giorno del mese successivo. 
	Per il mese di Gennaio la registrazione effettuata il 29, 30 o 31 scade l'ultimo giorno del mese di Febbraio. In caso di credito insufficiente 
	nella carta prepagata Wind alla data di scadenza della registrazione e nelle successive 24 ore, ovvero in caso di tua disdetta la registrazione non sar&agrave; rinnovata. 
	Inoltre, in caso di esaurimento o insufficienza del credito della carta pre-pagata WIND durante il periodo di validit&agrave; della registrazione, non sar&agrave; possibile
	usufruire del servizio i-mode(TM).<br>
	La disdetta pu&ograve; essere effettuata alla pagina "disattivazione differita", entro 24 ore prima della scadenza della registrazione, essa impedir&agrave; il rinnovo 
	automatico della registrazione ma non l'accesso al sito per il mese corrente.<br>
	Avrai, inoltre, la possibilit&agrave; di recedere in ogni momento, dalla registrazione effettuate, con efficacia immediata, alla pagina "disattivazione immediata". 
	L'esercizio del recesso non da diritto alla restituzione del costo di registrazione.<br>
	Il costo mensile di registrazione le verr&agrave; direttamente addebitato sulla carta prepagata o in fattura in caso di abbonamento ai servizi di telecomunicazioni Wind.<br>
	Al momento della registrazione garantisci di avere sulla tua carta prepagata Wind la disponibilit&agrave; di credito sufficiente.
	</p>
</div>
  
<table cellspacing="3" width="100%">
	<tr> 
		<td bgcolor="#FFFFCC">&#59113; <a href="subscribe.asp" accesskey="8">Indietro</a></td>
	</tr>
  <tr> 
    <td bgcolor="#FFFFCC">&#59114; <a href="default.asp?uid=UIDREQUEST" accesskey="9">KiweeScreens HomePage</a></td>
  </tr>	
</table>
</body>
</html>
