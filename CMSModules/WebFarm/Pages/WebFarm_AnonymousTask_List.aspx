<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_WebFarm_Pages_WebFarm_AnonymousTask_List" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Web farm server - List" Theme="Default" CodeFile="WebFarm_AnonymousTask_List.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniSelector/UniSelector.ascx" TagName="UniSelector"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:CMSUpdatePanel ID="pnlUpdate" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlSites" runat="server" CssClass="PageHeaderLine">
                <table style="width: 100%;">
                    <tr>
                        <td>
                        </td>
                        <td class="TextRight">
                            <cms:CMSButton ID="btnRunTasks" runat="server" CssClass="ContentButton" OnClick="btnRunTasks_Click" />
                            <cms:CMSButton ID="btnEmptyTasks" runat="server" CssClass="LongButton" OnClick="btnEmptyTasks_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlContent" runat="server" CssClass="PageContent">
                <cms:MessagesPlaceHolder runat="server" ID="plcMess" IsLiveSite="false" />
                <cms:UniGrid runat="server" ID="UniGrid" GridName="WebFarm_AnonymousTask_List.xml" OrderBy="TaskCreated" IsLiveSite="false" />
            </asp:Panel>
        </ContentTemplate>
    </cms:CMSUpdatePanel>
</asp:Content>
