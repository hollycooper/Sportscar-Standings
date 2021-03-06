<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConversionsByVariations.aspx.cs"
    MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="A/B test variations source pages"
    Theme="Default" Inherits="CMSModules_OnlineMarketing_Pages_Tools_ABTest_ConversionsByVariations"
    EnableEventValidation="false" %>

<%@ Register Src="~/CMSModules/OnlineMarketing/FormControls/SelectVariation.ascx"
    TagPrefix="cms" TagName="VariationsSelector" %>
<%@ Register Src="~/CMSModules/WebAnalytics/Controls/SelectGraphTypeAndPeriod.ascx"
    TagName="GraphType" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/OnlineMarketing/FormControls/SelectABTest.ascx" TagName="ABTests"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/WebAnalytics/Controls/GraphPreLoader.ascx" TagName="GraphPreLoader"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>
<%@ Register Src="~/CMSModules/WebAnalytics/Controls/ReportHeader.ascx" TagName="ReportHeader"
    TagPrefix="cms" %>
<asp:Content ID="cntHeader" runat="server" ContentPlaceHolderID="plcBeforeContent">
    <cms:ReportHeader runat="server" ID="reportHeader" />
    <asp:Panel runat="server" ID="pnlDisabled" CssClass="PageHeaderLine">
        <cms:DisabledModule runat="server" ID="ucDisabledModule" />
    </asp:Panel>
</asp:Content>
<asp:Content runat="server" ID="cntBody" ContentPlaceHolderID="plcContent">
    <div class="PageHeaderLine">
        <cms:GraphPreLoader runat="server" ID="ucGraphPreLoader" />
        <cms:GraphType runat="server" ID="ucGraphType" />
    </div>
    <div class="ReportBody">
        <cms:MessagesPlaceHolder ID="plcMess" runat="server" IsLiveSite="false" />
        <table>
            <tr>
                <td>
                    <cms:LocalizedLabel ID="lblTests" runat="server" ResourceString="om.abtest" DisplayColon="true" />
                </td>
                <td>
                    <cms:ABTests runat="server" ID="ucABTests" />
                </td>
                <td>
                    &nbsp;&nbsp;
                    <cms:LocalizedLabel ID="lblConversions" runat="server" ResourceString="abtesting.variants"
                        DisplayColon="true" />
                </td>
                <td>
                    <cms:VariationsSelector ID="ucSelectVariation" runat="server" />
                </td>
            </tr>
        </table>
        <br />
        <asp:PlaceHolder runat="server" ID="pnlDisplayReport"></asp:PlaceHolder>
    </div>
</asp:Content>
