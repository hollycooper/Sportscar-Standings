<%@ Page Language="C#" AutoEventWireup="true" CodeFile="List.aspx.cs" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Account list" Inherits="CMSModules_ContactManagement_Pages_Tools_Account_List"
    Theme="Default" EnableEventValidation="false" %>

<%@ Register Src="~/CMSModules/ContactManagement/Controls/UI/Account/List.ascx" TagName="AccountList"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Sites/SiteOrGlobalSelector.ascx" TagName="SiteOrGlobalSelector"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/PageElements/HeaderActions.ascx" TagName="HeaderActions"
    TagPrefix="cms" %>
<asp:Content ID="cntControls" runat="server" ContentPlaceHolderID="plcSiteSelector">
    <cms:LocalizedLabel runat="server" ID="lblSite" EnableViewState="false" DisplayColon="true"
        ResourceString="General.Site" />
    <cms:SiteOrGlobalSelector ID="SiteOrGlobalSelector" ShortID="s" runat="server" />
</asp:Content>
<asp:Content ID="cntActions" runat="server" ContentPlaceHolderID="plcActions">
    <cms:CMSUpdatePanel ID="pnlActons" runat="server">
        <ContentTemplate>
            <div class="LeftAlign">
                <cms:HeaderActions ID="hdrActions" runat="server" IsLiveSite="false" />
            </div>
            <cms:LocalizedLabel ID="lblWarnNew" runat="server" ResourceString="com.chooseglobalorsite"
                EnableViewState="false" Visible="false" CssClass="ActionsInfoLabel" />
            <div class="ClearBoth">
            </div>
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:AccountList ID="listElem" runat="server" IsLiveSite="false" />
</asp:Content>
