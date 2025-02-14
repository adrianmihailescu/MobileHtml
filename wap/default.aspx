<%@ Page language="c#" Codebehind="default.aspx.cs" Inherits="wap._default" AutoEventWireup="false" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>
<body Xmlns:mobile="http://schemas.microsoft.com/Mobile/WebForm">
	<mobile:form id="frmDefault" runat="server" EnableViewState="false" Title="Imágenes y Fondos">
<mobile:Panel id="pnlPreview" runat="server">
			<mobile:Image id="imgLogo" Runat="server" Alignment="Center"></mobile:Image>
		</mobile:Panel>
<mobile:Panel id="pnlTemas" runat="server"></mobile:Panel>
<mobile:Panel id="pnlFooter" runat="server"></mobile:Panel>
<mobile:Panel id="pnlShops" runat="server"></mobile:Panel>------<BR>
<mobile:Panel id="pnlEnd" runat="server"></mobile:Panel>
	</mobile:form>
</body>
