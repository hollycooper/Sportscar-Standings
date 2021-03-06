<%@ Page Title="" Language="C#" MasterPageFile="~/CMSAPIExamples/Pages/APIExamplesPage.Master"
    Theme="Default" AutoEventWireup="true" Inherits="CMSAPIExamples_Code_Tools_ImportExport_Default"
    CodeFile="Default.aspx.cs" %>

<%@ Register Src="~/CMSAPIExamples/Controls/APIExample.ascx" TagName="APIExample"
    TagPrefix="cms" %>
<%@ Register Assembly="CMS.UIControls" Namespace="CMS.UIControls" TagPrefix="cms" %>
<asp:Content ID="contentLeft" ContentPlaceHolderID="plcLeftContainer" runat="server">
    <asp:Label ID="importExportLabel" runat="server" Text="Note: Import and export of site may take much time."></asp:Label>
    <%-- Import --%>
    <cms:APIExamplePanel ID="pnlImport" runat="server" GroupingText="Import">
        <cms:APIExample ID="apiImportObject" runat="server" ButtonText="Import object" InfoMessage="User 'My new imported user' was imported." />
        <cms:APIExample ID="apiImportSite" runat="server" ButtonText="Import site" InfoMessage="Site 'My new imported site' was imported."
            ErrorMessage="There is alrady site with name 'MyNewImportedSite'" />
    </cms:APIExamplePanel>
    <%-- Export --%>
    <cms:APIExamplePanel ID="pnlExport" runat="server" GroupingText="Export">
        <cms:APIExample ID="apiExportObject" runat="server" ButtonText="Export object" InfoMessage="User 'My new imported user' was exported."
            ErrorMessage="There is already exported package with the same name or the user 'My new imported user' was not found." />
        <cms:APIExample ID="apiExportSite" runat="server" ButtonText="Export site" InfoMessage="Site 'My new imported site' was exported."
            ErrorMessage="There is already exported package with the same name or the site 'My new imported site' was not found." />
    </cms:APIExamplePanel>
</asp:Content>
<asp:Content ID="contentRight" ContentPlaceHolderID="plcRightContainer" runat="server">
    <%-- Delete imported objects --%>
    <cms:APIExamplePanel ID="pnlDeleteImported" runat="server" GroupingText="Delete imported objects">
        <cms:APIExample ID="apiDeleteImportedObject" runat="server" ButtonText="Delete imported object"
            APIExampleType="CleanUpMain" InfoMessage="User 'My new imported user' was deleted."
            ErrorMessage="User 'My new imported user' was not found." />
        <cms:APIExample ID="apiDeleteImportedSite" runat="server" ButtonText="Delete imported site"
            APIExampleType="CleanUpMain" InfoMessage="Site 'My new imported site' was deleted."
            ErrorMessage="Site 'My new imported site' was not found." />
        <cms:APIExample ID="apiDeletePackages" runat="server" ButtonText="Delete exported packages"
            APIExampleType="CleanUpMain" InfoMessage="All API example export packages were deleted." ErrorMessage="Some export packages were not deleted." />
    </cms:APIExamplePanel>
</asp:Content>
