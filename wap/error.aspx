<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>
<%@ Page language="c#" Codebehind="error.aspx.cs" Inherits="wap.error" AutoEventWireup="false" %>

<body Xmlns:mobile="http://schemas.microsoft.com/Mobile/WebForm">
    <mobile:Form id=Form1 runat="server" Title="Imágenes y Fondos">
<mobile:Panel id=pnlPreview runat="server">
<mobile:Image id=imgLogo Runat="server" Alignment="Center"></mobile:Image></mobile:Panel>
<mobile:Panel id=pnlError Runat="server"></mobile:Panel>------<BR>
<mobile:Panel id=pnlEnd runat="server"></mobile:Panel>
    </mobile:Form>
</body>
