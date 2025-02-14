<%@ Page language="c#" Codebehind="shops.aspx.cs" Inherits="wap.shops" AutoEventWireup="false" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>
<body Xmlns:mobile="http://schemas.microsoft.com/Mobile/WebForm">
	<mobile:Form id="Form1" runat="server" Title="Imágenes y Fondos">
<mobile:Panel id="pnlPreview" runat="server">
			<mobile:Image id="imgLogo" Alignment="Center" Runat="server"></mobile:Image>
		</mobile:Panel>
<mobile:Panel id="pnlShops" Runat="server"></mobile:Panel>------<BR>
<mobile:Panel id="pnlEnd" runat="server"></mobile:Panel>
    </mobile:Form>
</body>
