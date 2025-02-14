<%@ Page language="c#" Codebehind="sf.aspx.cs" Inherits="wap._sf" AutoEventWireup="false" %>
<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>

<body Xmlns:mobile="http://schemas.microsoft.com/Mobile/WebForm">
	<mobile:Form id="frmCatalog" runat="server" Title="San Fermín">
		<mobile:Panel id=pnlPreview runat="server">
			<mobile:Image id=imgLogo Alignment="Center" Runat="server"/>
		</mobile:Panel>
		<mobile:Panel id=pnlCatalog runat="server"></mobile:Panel>
		------<BR>
		<mobile:Panel id=pnlEnd runat="server"></mobile:Panel>
	</mobile:Form>
</body>
