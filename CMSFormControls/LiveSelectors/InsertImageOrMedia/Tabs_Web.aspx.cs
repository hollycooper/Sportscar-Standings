﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.ExtendedControls;
using CMS.GlobalHelper;
using CMS.UIControls;
using CMS.SettingsProvider;
using CMS.CMSHelper;

public partial class CMSFormControls_LiveSelectors_InsertImageOrMedia_Tabs_Web : CMSLiveModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool checkUI = ValidationHelper.GetBoolean(SettingsHelper.AppSettings["CKEditor:PersonalizeToolbarOnLiveSite"], false);
        if (checkUI)
        {
            // Check UIProfile
            if (!CMSContext.CurrentUser.IsAuthorizedPerUIElement("CMS.WYSIWYGEditor", "InsertImageOrMedia"))
            {
                RedirectToCMSDeskUIElementAccessDenied("CMS.WYSIWYGEditor", "InsertImageOrMedia");
            }
            else if (!CMSContext.CurrentUser.IsAuthorizedPerUIElement("CMS.MediaDialog", "WebTab"))
            {
                RedirectToCMSDeskUIElementAccessDenied("CMS.MediaDialog", "WebTab");
            }
        }

        if (QueryHelper.ValidateHash("hash"))
        {
            ScriptHelper.RegisterJQuery(Page);
            CMSDialogHelper.RegisterDialogHelper(Page);
        }
        else
        {
            webContentSelector.StopProcessing = true;
            webContentSelector.Visible = false;
            string url = ResolveUrl("~/CMSMessages/Error.aspx?title=" + GetString("dialogs.badhashtitle") + "&text=" + GetString("dialogs.badhashtext") + "&cancel=1");
            ltlScript.Text = ScriptHelper.GetScript("if (window.parent != null) { window.parent.location = '" + url + "' }");
        }
    }
}