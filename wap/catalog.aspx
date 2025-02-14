<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>
<%@ Page language="c#" Codebehind="catalog.aspx.cs" Inherits="wap.catalog" AutoEventWireup="false" %>
<HEAD>
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" content="C#">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/Mobile/Page">
</HEAD>
<body Xmlns:mobile="http://schemas.microsoft.com/Mobile/WebForm">
	<mobile:Form id="frmCatalog" runat="server" Title="Imágenes y Fondos">
<mobile:Panel id="pnlPreview" runat="server">
			<mobile:Image id="imgLogo" Runat="server" Alignment="Center"></mobile:Image>
		</mobile:Panel>
<mobile:Panel id="pnlCatalog" runat="server"></mobile:Panel>------<BR>
<mobile:Panel id="pnlEnd" runat="server"></mobile:Panel>
	</mobile:Form>
</body>
