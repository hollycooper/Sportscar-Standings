<%@ Control Language="C#" AutoEventWireup="True" Inherits="CMSModules_Integration_Controls_UI_Connectors_List"
    CodeFile="List.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<cms:UniGrid runat="server" ID="gridElem" ObjectType="integration.connector" OrderBy="ConnectorDisplayName"
    Columns="ConnectorID,ConnectorEnabled,ConnectorDisplayName,ConnectorName" IsLiveSite="false"
    EditActionUrl="Edit.aspx?connectorId={0}">
    <GridActions Parameters="ConnectorID">
        <ug:Action Name="edit" Caption="$General.Edit$" Icon="Edit.png" />
        <ug:Action Name="#delete" Caption="$General.Delete$" Icon="Delete.png" Confirmation="$General.ConfirmDelete$"
            ModuleName="CMS.Integration" Permissions="modify" />
        <ug:Action Name="status" Caption="$General.Status$" Icon="Warning.png" ExternalSourceName="status" />
    </GridActions>
    <GridColumns>
        <ug:Column Source="ConnectorDisplayName" Caption="$integration.connectorname$" Wrap="false"
            Width="100%" />
        <ug:Column Source="ConnectorEnabled" Caption="$general.enabled$" Wrap="false" ExternalSourceName="#yesno" />
    </GridColumns>
</cms:UniGrid>
