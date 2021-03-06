<%@ Page Language="C#" AutoEventWireup="true" Inherits="CMSFormControls_Macros_ConditionBuilderDialog"
    ValidateRequest="false" Theme="Default" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master"
    Title="Edit macro condition" CodeFile="ConditionBuilder.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/Macros/MacroDesigner.ascx" TagName="MacroDesigner"
    TagPrefix="cms" %>
<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <cms:MacroDesigner runat="server" ID="designerElem" ShortID="d" />
    <asp:Literal runat="server" ID="ltlScript" EnableViewState="false" />
</asp:Content>
<asp:Content ID="plcFooter" ContentPlaceHolderID="plcFooter" runat="server">
    <div class="Buttons FloatRight">
        <cms:LocalizedButton ID="btnInsert" runat="server" CssClass="SubmitButton" ResourceString="general.ok" />
        <cms:LocalizedButton ID="btnClose" runat="server" CssClass="SubmitButton" CausesValidation="false"
            ResourceString="general.close" OnClientClick="return CloseDialog();" /></div>
</asp:Content>
