<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_DocumentTypes_Pages_Development_DocumentType_List"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Document Types - Document Type List"
    CodeFile="DocumentType_List.aspx.cs" %>

<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UniGrid runat="server" ID="uniGrid" ShortID="g" OrderBy="ClassDisplayName" IsLiveSite="false" ObjectType="cms.documenttype" Columns="ClassID, ClassDisplayName, ClassName">
        <GridActions Parameters="ClassID">
            <ug:Action Name="edit" Caption="$General.Edit$" Icon="Edit.png" />
            <ug:Action Name="delete" Caption="$General.Delete$" Icon="Delete.png" Confirmation="$General.ConfirmDelete$" />
        </GridActions>
        <GridColumns>
            <ug:Column Source="ClassDisplayName" Caption="$general.displayname$" Wrap="false"
                Localize="true">
                <Filter Type="text" />
            </ug:Column>
            <ug:Column Source="ClassName" Caption="$general.codename$" Wrap="false">
                <Filter Type="text" />
            </ug:Column>
            <ug:Column Source="ClassID" Caption="$general.sitename$" Wrap="false" Visible="false">
                <Filter Type="site" />
            </ug:Column>
            <ug:Column Width="100%" />
        </GridColumns>
        <GridOptions DisplayFilter="true" />
    </cms:UniGrid>
</asp:Content>
