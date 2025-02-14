<%@ Register TagPrefix="mobile" Namespace="System.Web.UI.MobileControls" Assembly="System.Web.Mobile" %>
<%@ Page language="c#" Codebehind="alertas.aspx.cs" Inherits="wap.alertas" AutoEventWireup="false" %>

<body Xmlns:mobile="http://schemas.microsoft.com/Mobile/WebForm">
    <mobile:Form id=Form1 runat="server" Title="Im&aacute;genes y Fondos">
    <mobile:Panel id=pnlPreview runat="server">
    <mobile:Image ID="imgLogo" Runat="server" Alignment="Center" />
</mobile:Panel>
<mobile:Panel id=pnlAlertas Runat="server"></mobile:Panel>
------<br/>
<mobile:Panel id="pnlEnd" runat="server"></mobile:Panel>
    </mobile:Form>
</body>
