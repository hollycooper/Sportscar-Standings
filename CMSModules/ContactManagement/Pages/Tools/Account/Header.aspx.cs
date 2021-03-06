using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.FormEngine;
using CMS.GlobalHelper;
using CMS.OnlineMarketing;
using CMS.UIControls;

// Edited object
[EditedObject(OnlineMarketingObjectType.ACCOUNT, "accountid")]
public partial class CMSModules_ContactManagement_Pages_Tools_Account_Header : CMSContactManagementAccountsPage
{
    #region "Variables"

    private bool isDialogMode = false;

    #endregion


    #region "Page events"

    protected void Page_PreInit(object sender, EventArgs e)
    {
        isDialogMode = QueryHelper.GetBoolean("dialogmode", false);

        // Modify header master page in modal dialog
        if (isDialogMode)
        {
            Page.MasterPageFile = "~/CMSMasterPages/UI/Dialogs/TabsHeader.master";
        }
        else
        {
            Page.MasterPageFile = "~/CMSMasterPages/UI/TabsHeader.master";
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (EditedObject != null)
        {
            // Register script for unimenu button selection
            AddMenuButtonSelectScript(this, "Accounts", null, "menu");

            // Get account info object
            AccountInfo ai = (AccountInfo)EditedObject;
            string append = null;

            // Check permission
            AccountHelper.AuthorizedReadAccount(ai.AccountSiteID, true);

            // Check if running under site manager (and distribute "site manager" flag to other tabs)
            string siteManagerParam = string.Empty;
            if (IsSiteManager)
            {
                siteManagerParam = "&issitemanager=1";
            }
            
            // Set default help topic
            SetHelp("onlinemarketing_account_general", "helpTopic");

            // register scripts in modal dialog
            if (isDialogMode)
            {
                RegisterModalPageScripts();
            }

            // Append '(merged)' behind account name in breadcrumbs
            if (ai.AccountMergedWithAccountID != 0)
            {
                append = " " + GetString("om.account.mergedsuffix");
            }
            // Append '(global)'
            else if (ai.AccountSiteID == 0)
            {
                append = " " + GetString("om.account.globalsuffix");
            }

            // Modify header appearance in modal dialog (display title instead of breadcrumbs)
            if (QueryHelper.GetBoolean("dialogmode", false))
            {
                CurrentMaster.Title.TitleText = GetString("om.account.edit") + " - " + HTMLHelper.HTMLEncode(ai.AccountName) + append;
                CurrentMaster.Title.TitleImage = GetImageUrl("Objects/OM_Account/object.png");
            }
            else
            {
                // Get url for breadcrumbs
                string url = ResolveUrl("~/CMSModules/ContactManagement/Pages/Tools/Account/List.aspx");
                url = URLHelper.AddParameterToUrl(url, "siteid", SiteID.ToString());
                if (IsSiteManager)
                {
                    url = URLHelper.AddParameterToUrl(url, "issitemanager", "1");
                }

                CurrentPage.InitBreadcrumbs(2);
                CurrentPage.SetBreadcrumb(0, GetString("om.account.list"), url, "_parent", null);
                CurrentPage.SetBreadcrumb(1, HTMLHelper.HTMLEncode(CMSContext.ResolveMacros("{%EditedObject.DisplayName%}")) + append, null, null, null);
            }

            // Check if account has any custom fields
            int i = 0;

            FormInfo formInfo = FormHelper.GetFormInfo(ai.ClassName, false);
            if (formInfo.GetFormElements(true, false, true).Any())
            {
                i = 1;
            }

            // Initialize tabs
            InitTabs("content");
            SetTab(0, GetString("general.general"), "Tab_General.aspx?accountid=" + ai.AccountID + siteManagerParam, "SetHelpTopic('helpTopic', 'onlinemarketing_account_general');");
            if (i > 0)
            {
                // Add tab for custom fields
                SetTab(1, GetString("general.customfields"), "Tab_CustomFields.aspx?accountid=" + ai.AccountID + siteManagerParam, "SetHelpTopic('helpTopic', 'onlinemarketing_account_customfields');");
            }

            // Display contacts tab only if user is authorized to read contacts
            if (ContactHelper.AuthorizedReadContact(ai.AccountSiteID, false) || AccountHelper.AuthorizedReadAccount(ai.AccountSiteID, false))
            {
                SetTab(1 + i, GetString("om.contact.list"), "Tab_Contacts.aspx?accountid=" + ai.AccountID + siteManagerParam, "SetHelpTopic('helpTopic', 'onlinemarketing_account_contacts');");
            }

            // Hide last 2 tabs if the account is merged
            if (ai.AccountMergedWithAccountID == 0)
            {
                SetTab(2 + i, GetString("om.account.subsidiaries"), "Tab_Subsidiaries.aspx?accountid=" + ai.AccountID + siteManagerParam, "SetHelpTopic('helpTopic', 'onlinemarketing_account_subsidiaries');");
                SetTab(3 + i, GetString("om.account.merge"), "Tab_Merge.aspx?accountid=" + ai.AccountID + siteManagerParam, "SetHelpTopic('helpTopic', 'onlinemarketing_account_merge');");
            }

            // Data.com
            SetTab(4 + i, "Data.com", "Tab_DataCom.aspx?accountid=" + ai.AccountID + siteManagerParam, "SetHelpTopic('helpTopic', 'onlinemarketing_account_datacom');");
        }
    }

    #endregion
}