<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>
<%@ Page language="c#" Codebehind="minisite.aspx.cs" Inherits="wap.minisite" AutoEventWireup="false" %>
<body Xmlns:mobile="http://schemas.microsoft.com/Mobile/WebForm">
	<mobile:form id="frmDefault" runat="server" EnableViewState="false" Title="Imágenes y Fondos">
<mobile:Panel id="pnlPreview" runat="server">
			<mobile:Image id="imgLogo" Alignment="Center" Runat="server"></mobile:Image>
		</mobile:Panel>------<BR>
<mobile:Panel id="pnlEnd" runat="server"></mobile:Panel>
	</mobile:form>
</body>
