<%@ Page language="c#" Codebehind="view.aspx.cs" Inherits="wap.view" AutoEventWireup="false" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>
<body Xmlns:mobile="http://schemas.microsoft.com/Mobile/WebForm">
	<mobile:form id="frmSearch" runat="server" EnableViewState="False" Title="Imágenes y Fondos">
<mobile:Panel id="pnlPreview" runat="server">
			<mobile:Image id="imgLogo" Alignment="Center" Runat="server"></mobile:Image>
		</mobile:Panel>
<mobile:Panel id="pnlView" Runat="server">
			<mobile:Image id="imgView" Alignment="Center" Runat="server"></mobile:Image>
			<mobile:Label id="lblContent" Alignment="Center" Runat="server" Font-Size="Small"></mobile:Label>
			<mobile:Link id="linkPPD" Alignment="Center" Runat="server" Font-Size="Small">Descargar por 1.5 €</mobile:Link>
		</mobile:Panel>------<BR>
<mobile:Panel id="pnlEnd" runat="server"></mobile:Panel>
    </mobile:form>
</body>
