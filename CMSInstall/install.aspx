<%@ Page Language="C#" Inherits="CMSInstall_install" Theme="Default" EnableEventValidation="false"
    ValidateRequest="false" CodeFile="install.aspx.cs" %>

<%@ Register Src="~/CMSAdminControls/UI/PageElements/Help.ascx" TagName="Help" TagPrefix="cms" %>
<%@ Register Src="~/CMSInstall/Controls/WizardSteps/LicenseDialog.ascx" TagName="LicenseDialog"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSInstall/Controls/WizardSteps/SiteCreationDialog.ascx" TagName="SiteCreationDialog"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/UI/System/RequireScript.ascx" TagName="RequireScript"
    TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/AsyncControl.ascx" TagName="AsyncControl" TagPrefix="cms" %>
<%@ Register Src="~/CMSInstall/Controls/WizardSteps/WagDialog.ascx" TagName="WagDialog"
    TagPrefix="cms" %>
<%@ Register Src="Controls/LayoutPanels/Error.ascx" TagName="ErrorPanel" TagPrefix="cms" %>
<%@ Register Src="Controls/LayoutPanels/Log.ascx" TagName="LogPanel" TagPrefix="cms" %>
<%@ Register Src="Controls/LayoutPanels/Version.ascx" TagName="VersionPanel" TagPrefix="cms" %>
<%@ Register Src="Controls/LayoutPanels/Warning.ascx" TagName="WarningPanel" TagPrefix="cms" %>
<%@ Register Src="Controls/StepNavigation.ascx" TagName="StepNavigation" TagPrefix="cms" %>
<%@ Register Src="Controls/WizardSteps/UserServer.ascx" TagName="UserServer" TagPrefix="cms" %>
<%@ Register Src="Controls/WizardSteps/DatabaseDialog.ascx" TagName="DatabaseDialog"
    TagPrefix="cms" %>
<%@ Register Src="Controls/WizardSteps/DBProgress.ascx" TagName="DBProgress" TagPrefix="cms" %>
<%@ Register Src="Controls/WizardSteps/SiteProgress.ascx" TagName="SiteProgress"
    TagPrefix="cms" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ResHelper.GetFileString("General.ProductName")%>
        Database Setup </title>
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 0px;
            height: 100%;
        }
        .ButtonsPanel
        {
            padding-top: 10px;
        }
    </style>
    <script type="text/javascript">
        //<![CDATA[
        var installTimerId = 0;

        // Start timer function
        function StartInstallStateTimer(type) {
            var act = document.getElementById('activity');
            if (act != null) {
                act.style.display = 'inline';
            }
            installTimerId = setInterval("GetInstallState('false;" + type + "')", 500);
        }

        // End timer function
        function StopInstallStateTimer() {
            if (installTimerId) {
                clearInterval(installTimerId);
                installTimerId = 0;

                if (window.HideActivity) {
                    window.HideActivity();
                }

                var act = document.getElementById('activity');
                if (act != null) {
                    act.style.display = 'none';
                }
            }
        }

        // Cancel install
        function CancelImport(type) {
            GetInstallState('true;' + type);
            return false;
        }
        //]]>
    </script>
</head>
<body class="InstallBody <%=BodyClass%>">
    <form id="Form1" method="post" runat="server">
    <asp:Literal ID="ltlScript" runat="server" EnableViewState="false" />
    <cms:RequireScript ID="rqScript" runat="server" UseFileStrings="true" />
    <asp:Panel runat="server" ID="pnlBody" CssClass="InstallerBody">
        <cms:LayoutPanel ID="layPanel" runat="server" LayoutCssClass="InstallPanel">
            <asp:Label ID="lblHeader" CssClass="InstallHeader" runat="server" />
            <asp:Image ID="imgHeader" runat="server" CssClass="InstalHeader" />
            <asp:Panel runat="server" ID="pnlWizard" CssClass="InstallerContent">
                <asp:Button ID="btnHiddenNext" runat="server" CssClass="HiddenButton" OnClick="btnHiddenNext_onClick" />
                <asp:Button ID="btnHiddenBack" runat="server" CssClass="HiddenButton" OnClick="btnHiddenBack_onClick" />
                <asp:Wizard ID="wzdInstaller" runat="server" DisplaySideBar="False" OnPreviousButtonClick="wzdInstaller_PreviousButtonClick"
                    ActiveStepIndex="1" Width="100%">
                    <StepNavigationTemplate>
                        <cms:StepNavigation ID="stepNavigation" runat="server" NextPreviousVisible="True"
                            NextPreviousJS="True" />
                    </StepNavigationTemplate>
                    <StartNavigationTemplate>
                        <cms:StepNavigation ID="startStepNavigation" runat="server" NextPreviousVisible="false"
                            NextPreviousJS="True" />
                    </StartNavigationTemplate>
                    <WizardSteps>
                        <asp:WizardStep ID="stpUserServer" runat="server" StepType="Start">
                            <cms:UserServer ID="userServer" runat="server" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpDatabase" runat="server" StepType="Step">
                            <cms:DatabaseDialog ID="databaseDialog" runat="server" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpConnectionString" runat="server" AllowReturn="false" StepType="Start">
                            <asp:Panel ID="pnlConnectionString" runat="server">
                                <div class="InstallContent">
                                    <asp:Label ID="lblConnectionString" runat="server" CssClass="InstallGroupTitle" Visible="False" />
                                    <table class="InstallWizard" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblErrorConnMessage" runat="server" CssClass="connMessageError" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpDBProgress" runat="server" AllowReturn="false" StepType="Step">
                            <cms:DBProgress ID="dbProgress" runat="server" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpLicenseSetting" runat="server" AllowReturn="false" StepType="Start">
                            <div class="InstallContent">
                                <table class="InstallWizard" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <cms:LicenseDialog ID="ucLicenseDialog" runat="server" />
                                            <cms:WagDialog ID="ucWagDialog" runat="server" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpSiteCreation" runat="server" AllowReturn="false" StepType="Start">
                            <asp:Panel ID="pnlSiteCreation" runat="server">
                                <asp:Label ID="lblSiteCreation" runat="server" CssClass="InstallGroupTitle" Visible="False" />
                                <div class="InstallContentNewSite">
                                    <table class="InstallWizard" border="0" cellpadding="0" cellspacing="0">
                                        <tr style="vertical-align: top;">
                                            <td align="left">
                                                <cms:SiteCreationDialog ID="ucSiteCreationDialog" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpProgress" runat="server" AllowReturn="false" StepType="Step">
                            <cms:SiteProgress ID="siteProgress" runat="server" />
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpFinish" runat="server" StepType="Complete">
                            <asp:Panel ID="pnlFinished" runat="server">
                                <table class="InstallWizard" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="InstallCompleted" style="text-align: center">
                                            <asp:Label ID="lblCompleted" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <asp:Label ID="lblMediumTrustInfo" runat="server" Visible="false" /><br />
                                            <asp:LinkButton ID="btnWebSite" runat="server" OnClick="btnWebSite_onClick" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:WizardStep>
                        <asp:WizardStep ID="stpCollation" runat="server" StepType="Step">
                            <asp:Panel ID="pnlCollation" runat="server">
                                <div class="InstallContent">
                                    <table class="InstallWizard" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <div style="height: 305px;">
                                                    <asp:Label ID="lblCollation" runat="server" /><br />
                                                    <br />
                                                    <br />
                                                    <asp:RadioButton ID="rbChangeCollationCI" Checked="true" runat="server" GroupName="Collation" />
                                                    <br />
                                                    <br />
                                                    <asp:RadioButton ID="rbLeaveCollation" runat="server" GroupName="Collation" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </asp:WizardStep>
                    </WizardSteps>
                </asp:Wizard>
            </asp:Panel>
            <asp:Panel ID="pnlPermission" runat="server" CssClass="InstallerContent" Visible="false">
                <div class="InstallContentPermission">
                    <div style="text-align: left; padding: 0px 20px 10px 20px;">
                        <asp:Label ID="lblPermission" runat="server" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel CssClass="ButtonsPanel" ID="pnlButtons" runat="server" Visible="false">
                <div class="InstallContent">
                    <asp:Button ID="btnPermissionTest" runat="server" CssClass="XLongButton" />&nbsp;<asp:Button
                        ID="btnPermissionSkip" runat="server" CssClass="ContentButton" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlPermissionSuccess" runat="server" Visible="false">
                <div class="InstallContent" style="padding: 20px 20px 10px;">
                    <asp:Label ID="lblPermissionSuccess" runat="server" /><br />
                    <br />
                    <asp:Button ID="btnPermissionContinue" runat="server" CssClass="ContentButton" />
                </div>
            </asp:Panel>
        </cms:LayoutPanel>
        <cms:VersionPanel ID="versionPanel" runat="server" DisplaySupportLabel="True" />
        <cms:ErrorPanel ID="errorPanel" runat="server" />
        <cms:LogPanel ID="logPanel" runat="server" />
        <cms:WarningPanel ID="warningPanel" runat="server" />
    </asp:Panel>
    <asp:HiddenField ID="hdnState" runat="server" />
    <asp:HiddenField ID="hdnAdvanced" runat="server" />
    <asp:Literal ID="ltlInstallScript" runat="server" EnableViewState="false" />
    <asp:Literal ID="ltlAdvanced" runat="server" EnableViewState="false" />
    <cms:AsyncControl ID="ucAsyncControl" runat="server" PostbackOnError="false" />
    <cms:AsyncControl ID="ucDBAsyncControl" runat="server" PostbackOnError="false" UseFileStrings="True" />
    </form>
</body>
</html>
