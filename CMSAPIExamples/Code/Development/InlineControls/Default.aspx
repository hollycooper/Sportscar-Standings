<%@ Page Title="" Language="C#" MasterPageFile="~/CMSAPIExamples/Pages/APIExamplesPage.Master"
    Theme="Default" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CMSAPIExamples_Code_Development_InlineControls_Default" %>

<%@ Register Src="~/CMSAPIExamples/Controls/APIExample.ascx" TagName="APIExample" TagPrefix="cms" %>
<%@ Register Src="~/CMSAPIExamples/Controls/APIExampleSection.ascx" TagName="APIExampleSection" TagPrefix="cms" %>
<%@ Register Assembly="CMS.UIControls" Namespace="CMS.UIControls" TagPrefix="cms" %>
<asp:Content ID="contentLeft" ContentPlaceHolderID="plcLeftContainer" runat="server">
    <%-- Inline control --%>
    <cms:APIExamplePanel ID="pnlCreateInlineControl" runat="server" GroupingText="Inline control">
        <cms:APIExample ID="apiCreateInlineControl" runat="server" ButtonText="Create control" InfoMessage="Control 'My new control' was created." />
        <cms:APIExample ID="apiGetAndUpdateInlineControl" runat="server" ButtonText="Get and update control" APIExampleType="ManageAdditional" InfoMessage="Control 'My new control' was updated." ErrorMessage="Control 'My new control' was not found." />
        <cms:APIExample ID="apiGetAndBulkUpdateInlineControls" runat="server" ButtonText="Get and bulk update controls" APIExampleType="ManageAdditional" InfoMessage="All controls matching the condition were updated." ErrorMessage="Controls matching the condition were not found." />
    </cms:APIExamplePanel>
   
    <%-- Inline control on site --%>
    <cms:APIExamplePanel ID="pnlAddInlineControlToSite" runat="server" GroupingText="Inline control on site">
        <cms:APIExample ID="apiAddInlineControlToSite" runat="server" ButtonText="Add control to site" InfoMessage="Control 'My new control' was added to current site." ErrorMessage="Control 'My new control' was not found." />
    </cms:APIExamplePanel>
</asp:Content>
<asp:Content ID="contentRight" ContentPlaceHolderID="plcRightContainer" runat="server">
    <%-- Inline control on site --%>
    <cms:APIExamplePanel ID="pnlRemoveInlineControlFromSite" runat="server" GroupingText="Inline control on site">
        <cms:APIExample ID="apiRemoveInlineControlFromSite" runat="server" ButtonText="Remove control from site" APIExampleType="CleanUpMain" InfoMessage="Control 'My new control' was removed from current site." ErrorMessage="Control 'My new control' was not found." />
    </cms:APIExamplePanel>
   
    <%-- Inline control --%>
    <cms:APIExamplePanel ID="pnlDeleteInlineControl" runat="server" GroupingText="Inline control">
        <cms:APIExample ID="apiDeleteInlineControl" runat="server" ButtonText="Delete control" APIExampleType="CleanUpMain" InfoMessage="Control 'My new control' and all its dependencies were deleted." ErrorMessage="Control 'My new control' was not found." />
    </cms:APIExamplePanel>
</asp:Content>