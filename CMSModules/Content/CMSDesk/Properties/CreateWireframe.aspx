<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_Content_CMSDesk_Properties_CreateWireframe"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/EmptyPage.master" 
    EnableEventValidation="false" CodeFile="CreateWireframe.aspx.cs" %>

<%@ Register Src="~/CMSModules/Content/CMSDesk/New/TemplateSelection.ascx" TagName="TemplateSelection" TagPrefix="cms" %>
<%@ Register Src="~/CMSModules/Content/Controls/EditMenu.ascx" TagName="editmenu"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <div class="NewPageDialog">
        <asp:Panel ID="pnlContent" runat="server" CssClass="PageContentFrame">
            <div class="PTSelection">
                <cms:CMSPanel ID="pnlMenu" runat="server" FixedPosition="true">
                    <cms:editmenu ID="menuElem" ShortID="m" runat="server" ShowProperties="false" ShowSpellCheck="true" IsLiveSite="false" HelpTopicName="CMS_Content_Templates" />
                </cms:CMSPanel>
                <div style="padding: 10px 10px 0px 10px;">
                    <asp:Label ID="lblError" runat="server" CssClass="ErrorLabel" EnableViewState="false" />
                </div>
                <table cellpadding="0" cellspacing="0" class="Table" border="0">
                    <tr class="HeaderRow">
                        <td class="LeftBorder">
                        </td>
                        <td style="vertical-align: top;" class="Header">
                            <cms:LocalizedLabel ID="lblPageName" runat="server" ResourceString="Wireframe.SelectTemplate" />
                        </td>
                        <td class="RightBorder">
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" class="Table" border="0">
                    <tr class="Row">
                        <td style="vertical-align: top;" class="Content">
                            <cms:TemplateSelection ID="selTemplate" ShortID="s" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class="Footer">
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
