<%@ Page Language="C#" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" AutoEventWireup="true"
    Inherits="CMSModules_BannerManagement_Tools_Category_Category_Edit_Banners" Title="Banners"
    Theme="Default" CodeFile="Category_Edit_Banners.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>
<asp:Content ContentPlaceHolderID="plcContent" runat="Server">
    <cms:UniGrid ID="gridBanners" runat="server" OrderBy="BannerDisplayName" ObjectType="cms.banner"
        EditActionUrl="~/CMSModules/BannerManagement/Tools/Banner/Banner_Edit.aspx?id={0}&siteid={?siteid?}&categoryid={?categoryid?}"
        IsLiveSite="false" Columns="BannerID, BannerDisplayName, BannerEnabled, BannerFrom, BannerTo, BannerType, BannerUrl, BannerWeight, BannerHitsLeft, BannerClicksLeft, BannerSiteID" RememberStateByParam="CategoryID">
        <GridActions Parameters="BannerID">
            <ug:Action Name="edit" Caption="$General.Edit$" Icon="Edit.png" />
            <ug:Action Name="delete" Caption="$General.Delete$" Icon="Delete.png" Confirmation="$general.confirmdelete$" ExternalSourceName="delete" />
        </GridActions>
        <GridColumns>
            <ug:Column Source="BannerDisplayName" Caption="$general.name$" Wrap="false">
                <Filter Type="text" />
            </ug:Column>
            <ug:Column Source="BannerType" ExternalSourceName="BannerType" Caption="$banner.bannernew.type$"
                Wrap="false">
            </ug:Column>
            <ug:Column Source="BannerUrl" Caption="$general.url$" Wrap="false" MaxLength="40">
                <Filter Type="text" />
            </ug:Column>
            <ug:Column Source="BannerWeight" Caption="$banner.bannernew.weight$" Wrap="false">
                <Filter Type="double" />
            </ug:Column>
            <ug:Column Source="BannerHitsLeft" ExternalSourceName="hitsclicksleft" Caption="$banner.bannernew.hitsleft$" Wrap="false">
            </ug:Column>
            <ug:Column Source="BannerClicksLeft" ExternalSourceName="hitsclicksleft" Caption="$banner.bannernew.clicksleft$" Wrap="false">
            </ug:Column>
            <ug:Column Source="BannerEnabled" ExternalSourceName="#yesno" Caption="$general.enabled$"
                Wrap="false">
                <Filter Type="bool" />
            </ug:Column>
            <ug:Column Source="BannerFrom" Caption="$banner.bannerfrom$" Wrap="false">
            </ug:Column>
            <ug:Column Source="BannerTo" Caption="$banner.bannerto$" Wrap="false">
            </ug:Column>
            <ug:Column Width="100%">
            </ug:Column>
        </GridColumns>
        <GridOptions DisplayFilter="true" />
    </cms:UniGrid>
</asp:Content>
