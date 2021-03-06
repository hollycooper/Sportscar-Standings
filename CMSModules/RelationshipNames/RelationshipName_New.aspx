<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_RelationshipNames_RelationshipName_New"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" Title="Relationship names - new" CodeFile="RelationshipName_New.aspx.cs" %>

<%@ Register Src="~/CMSFormControls/System/LocalizableTextBox.ascx" TagName="LocalizableTextBox" TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/System/CodeName.ascx" TagName="CodeName" TagPrefix="cms" %>
<%@ Register Src="~/CMSFormControls/System/ObjectTypeSelector.ascx" TagName="ObjectTypeSelector" TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <table>
        <tr>
            <td class="FieldLabel">
                <cms:LocalizedLabel ID="lblRelationshipNameDisplayName" runat="server" EnableViewState="false"
                    ResourceString="RelationshipNames.DisplayName" />
            </td>
            <td>
                <cms:LocalizableTextBox ID="txtRelationshipNameDisplayName" runat="server" CssClass="TextBoxField"
                    MaxLength="200" EnableViewState="false" />
                <cms:CMSRequiredFieldValidator ID="RequiredFieldValidatorDisplayName" runat="server"
                    EnableViewState="false" ControlToValidate="txtRelationshipNameDisplayName:textbox" Display="dynamic" />
            </td>
        </tr>
        <tr>
            <td class="FieldLabel">
                <cms:LocalizedLabel ID="lblRelationshipNameCodeName" runat="server" EnableViewState="false"
                    ResourceString="RelationshipNames.CodeName" />
            </td>
            <td>
                <cms:CodeName ID="txtRelationshipNameCodeName" runat="server" CssClass="TextBoxField"
                    MaxLength="200" EnableViewState="false" />
                <cms:CMSRequiredFieldValidator ID="RequiredFieldValidatorCodeName" runat="server" EnableViewState="false"
                    ControlToValidate="txtRelationshipNameCodeName" Display="dynamic" />
            </td>
        </tr>
        <tr>
            <td class="FieldLabel">
                <cms:LocalizedLabel ID="lblRelationshipNameType" runat="server" EnableViewState="false"
                    ResourceString="RelationshipNames.Type" />
            </td>
            <td>
                <cms:ObjectTypeSelector ID="objectTypeSelector" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:CheckBox runat="server" ID="chkAssign" Visible="false" CssClass="CheckBoxMovedLeft" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <cms:FormSubmitButton ID="ButtonOK" runat="server" OnClick="ButtonOK_Click" EnableViewState="false" />
            </td>
        </tr>
    </table>
</asp:Content>
