<%@ Page Title="" Language="C#" MasterPageFile="~/CMSAPIExamples/Pages/APIExamplesPage.Master"
    Theme="Default" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="CMSAPIExamples_Code_Development_Countries_Default" %>

<%@ Register Src="~/CMSAPIExamples/Controls/APIExample.ascx" TagName="APIExample" TagPrefix="cms" %>
<%@ Register Assembly="CMS.UIControls" Namespace="CMS.UIControls" TagPrefix="cms" %>
<asp:Content ID="contentLeft" ContentPlaceHolderID="plcLeftContainer" runat="server">
    <%-- Country --%>
    <cms:APIExamplePanel ID="pnlCreateCountry" runat="server" GroupingText="Country">
        <cms:APIExample ID="apiCreateCountry" runat="server" ButtonText="Create country" InfoMessage="Country 'My new country' was created." />
        <cms:APIExample ID="apiGetAndUpdateCountry" runat="server" ButtonText="Get and update country" APIExampleType="ManageAdditional" InfoMessage="Country 'My new country' was updated." ErrorMessage="Country 'My new country' was not found." />
        <cms:APIExample ID="apiGetAndBulkUpdateCountries" runat="server" ButtonText="Get and bulk update countries" APIExampleType="ManageAdditional" InfoMessage="All countries matching the condition were updated." ErrorMessage="Countries matching the condition were not found." />
    </cms:APIExamplePanel>
    <%-- State --%>
    <cms:APIExamplePanel ID="pnlCreateState" runat="server" GroupingText="State">
        <cms:APIExample ID="apiCreateState" runat="server" ButtonText="Create state" InfoMessage="State 'My new state' was created." ErrorMessage="Country 'My new country' was not found." />
        <cms:APIExample ID="apiGetAndUpdateState" runat="server" ButtonText="Get and update state" APIExampleType="ManageAdditional" InfoMessage="State 'My new state' was updated." ErrorMessage="Country 'My new country' or state 'My new state' were not found." />
        <cms:APIExample ID="apiGetAndBulkUpdateStates" runat="server" ButtonText="Get and bulk update states" APIExampleType="ManageAdditional" InfoMessage="All states matching the condition were updated." ErrorMessage="States matching the condition were not found." />
    </cms:APIExamplePanel>
</asp:Content>
<asp:Content ID="contentRight" ContentPlaceHolderID="plcRightContainer" runat="server">
    <%-- State --%>
    <cms:APIExamplePanel ID="pnlDeleteState" runat="server" GroupingText="State">
        <cms:APIExample ID="apiDeleteState" runat="server" ButtonText="Delete state" APIExampleType="CleanUpMain" InfoMessage="State 'My new state' and all its dependencies were deleted." ErrorMessage="State 'My new state' was not found." />
    </cms:APIExamplePanel>
    <%-- Country --%>
    <cms:APIExamplePanel ID="pnlDeleteCountry" runat="server" GroupingText="Country">
        <cms:APIExample ID="apiDeleteCountry" runat="server" ButtonText="Delete country" APIExampleType="CleanUpMain" InfoMessage="Country 'My new country' and all its dependencies were deleted." ErrorMessage="Country 'My new country' was not found." />
    </cms:APIExamplePanel>
</asp:Content>
