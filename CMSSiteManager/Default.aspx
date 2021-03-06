<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSSiteManager_Default" CodeFile="Default.aspx.cs"
    MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" Theme="Default" Title="Site Manager" %>

<%@ Register TagPrefix="cms" Namespace="CMS.UIControls" Assembly="CMS.UIControls" %>
<%@ Register Src="~/CMSAdminControls/UI/ScreenLock/ScreenLockDialog.ascx" TagName="ScreenLockDialog"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/AdvancedPopupHandler.ascx" TagName="AdvancedPopupHandler"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UILayout runat="server" ID="layoutElem">
        <Panes>
            <cms:UILayoutPane ID="paneHeader" runat="server" Direction="North" RenderAs="Div"
                ControlPath="~/CMSAdminControls/UI/Header.ascx" Resizable="false" TogglerLengthOpen="33"
                TogglerLengthClosed="33" />
            <cms:UILayoutPane ID="cmsdesktop" runat="server" Direction="Center" RenderAs="Iframe"
                ModuleName="CMS.SiteManager" />
            <cms:UILayoutPane ID="paneFooter" runat="server" ControlPath="~/CMSModules/SystemDevelopment/DevMenu.ascx"
                Direction="South" RenderAs="Div" InitClosed="true" />
        </Panes>
    </cms:UILayout>
    <cms:AdvancedPopupHandler runat="server" ID="popupHandler" />
    <cms:ScreenLockDialog ID="screenLockDialogElem" runat="server" />
</asp:Content>
