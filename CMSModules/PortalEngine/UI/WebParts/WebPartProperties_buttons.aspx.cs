using System;

using CMS.GlobalHelper;
using CMS.UIControls;

public partial class CMSModules_PortalEngine_UI_WebParts_WebPartProperties_buttons : CMSWebPartPropertiesPage
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        chkRefresh.Text = GetString("WebpartProperties.Refresh");

        ltlScript.Text += ScriptHelper.GetScript("function GetRefreshStatus() { return document.getElementById('" + chkRefresh.ClientID + "').checked; }");

        // Set button texts
        btnOk.Text = GetString("general.ok");
        btnApply.Text = GetString("general.apply");
        btnCancel.Text = GetString("general.cancel");

        // Set button click events
        btnCancel.OnClientClick = "SendEvent('close'); return false;";
        btnApply.OnClientClick = "SendEvent('apply', GetRefreshStatus()); return false;";
        btnOk.OnClientClick = "SendEvent('ok', GetRefreshStatus()); return false;";

        string action = QueryHelper.GetString("tab", "properties");

        switch (action)
        {
            case "properties":
                break;

            case "code":
                break;

            case "binding":
                chkRefresh.Visible = false;
                btnApply.Visible = false;
                btnOk.Visible = false;
                btnCancel.Text = GetString("WebpartProperties.Close");
                break;
        }
    }
}