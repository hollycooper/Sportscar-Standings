<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSModules_DocumentTypes_Pages_Development_DocumentType_Edit_Transformation_New"
    ValidateRequest="false" Theme="Default" EnableEventValidation="false" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="New transformation" CodeFile="DocumentType_Edit_Transformation_New.aspx.cs" %>

<%@ Register Src="~/CMSFormControls/Filters/DocTypeFilter.ascx" TagName="DocTypeFilter"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/PageElements/Help.ascx" TagName="Help" TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/Macros/MacroEditor.ascx" TagName="MacroEditor"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/Layouts/TransformationCode.ascx" TagName="TransformationCode"
    TagPrefix="cms" %>
<%@ Register TagPrefix="cms" Namespace="CMS.ExtendedControls" Assembly="CMS.UIControls" %>
<asp:Content ID="cntControls" runat="server" ContentPlaceHolderID="plcControls">
    <cms:DocTypeFilter runat="server" ID="filter" RenderTableTag="true" EnableViewState="true" />
</asp:Content>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:UIForm runat="server" ID="editElem" ObjectType="cms.transformation" DefaultFieldLayout="Inline"
        CssClass="Transformation" RedirectUrlAfterCreate="">
        <LayoutTemplate>
            <cms:FormCategory runat="server" ID="pnlGeneral" DefaultFieldLayout="TwoColumns" ResourceString="general.general">
                <cms:FormField runat="server" ID="fDisplayName" Field="TransformationName" FormControl="TextBoxControl"
                    ResourceString="transformationlist.transformationname" DisplayColon="true" />
            </cms:FormCategory>
            <cms:FormCategory runat="server" ID="pnlCategory" ResourceString="objecttype.cms_transformation"
                CssClass="TransformationCode" DefaultFieldLayout="Inline">
                <cms:FormField runat="server" ID="fCode" Field="TransformationCode">
                    <cms:TransformationCode runat="server" ID="ucTransfCode" />
                </cms:FormField>
            </cms:FormCategory>
            <cms:FormSubmitButton runat="server" ID="btnSubmit" />
        </LayoutTemplate>
    </cms:UIForm>
</asp:Content>
