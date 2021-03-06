<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeparateDB.aspx.cs" Inherits="CMSInstall_SeparateDB"
    Theme="Default" MasterPageFile="~/CMSMasterPages/UI/Dialogs/ModalDialogPage.master"
    Title="Contact management database separation" %>

<%@ Register Src="~/CMSAdminControls/AsyncControl.ascx" TagName="AsyncControl" TagPrefix="cms" %>
<%@ Register Src="Controls/StepNavigation.ascx" TagName="StepNavigation" TagPrefix="cms" %>
<%@ Register Src="Controls/WizardSteps/UserServer.ascx" TagName="UserServer" TagPrefix="cms" %>
<%@ Register Src="Controls/WizardSteps/DatabaseDialog.ascx" TagName="DatabaseDialog"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSInstall/Controls/WizardSteps/AsyncProgress.ascx" TagName="AsyncProgress"
    TagPrefix="cms" %>
<%@ Register Src="Controls/WizardSteps/SeparationFinished.ascx" TagName="SeparationFinished"
    TagPrefix="cms" %>
<asp:Content ID="cntContent" ContentPlaceHolderID="plcContent" runat="Server">
    <asp:Panel runat="server" ID="pnlBody">
        <cms:LayoutPanel ID="layPanel" runat="server" LayoutCssClass="InstallPanel SeparatePanel">
            <asp:Label ID="lblHeader" CssClass="InstallHeader" runat="server" />
            <asp:Image ID="imgHeader" runat="server" CssClass="InstalHeader" />
            <asp:Panel runat="server" ID="pnlWizard" CssClass="InstallerContent">
                <asp:Wizard ID="wzdInstaller" runat="server" DisplaySideBar="False" OnNextButtonClick="wzdInstaller_NextButtonClick"
                    Width="100%">
                    <StepNavigationTemplate>
                        <cms:StepNavigation ID="stepNavigation" runat="server" NextPreviousVisible="True" />
                    </StepNavigationTemplate>
                    <StartNavigationTemplate>
                        <cms:StepNavigation ID="startStepNavigation" runat="server" />
                    </StartNavigationTemplate>
                    <FinishNavigationTemplate>
                        <cms:StepNavigation ID="finishNavigation" runat="server" NextButton-ResourceString="general.finish"
                            FinishStep="True" />
                    </FinishNavigationTemplate>
                    <WizardSteps>
                        <asp:WizardStep ID="stpUserServer" runat="server" StepType="Start">
                            <cms:UserServer ID="userServer" runat="server" SameServerEnabled="True" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpDatabase" runat="server" StepType="Step">
                            <cms:DatabaseDialog ID="databaseDialog" runat="server" IsDBSeparation="True" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpProgress" runat="server" AllowReturn="false" StepType="Step">
                            <cms:AsyncProgress ID="progress" runat="server" IsSeparation="True" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpFinish" runat="server" StepType="Finish">
                            <cms:SeparationFinished ID="separationFinished" runat="server" IsSeparation="true" />
                        </asp:WizardStep>
                    </WizardSteps>
                </asp:Wizard>
            </asp:Panel>
        </cms:LayoutPanel>
    </asp:Panel>
    <cms:AsyncControl ID="asyncControl" runat="server" PostbackOnError="False" />
</asp:Content>
