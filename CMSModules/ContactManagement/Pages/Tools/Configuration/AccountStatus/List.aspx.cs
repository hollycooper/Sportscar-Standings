using System;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.OnlineMarketing;
using CMS.SettingsProvider;
using CMS.UIControls;

// Title
[Title("Objects/OM_AccountStatus/object.png", "om.accountstatus.list", "onlinemarketing_accountstatus_list")]
public partial class CMSModules_ContactManagement_Pages_Tools_Configuration_AccountStatus_List : CMSContactManagementAccountStatusPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // In cmsdesk "siteID is available in control
        if (!IsSiteManager)
        {
            // Init site filter when user is authorized for global and site account statuses
            if (AuthorizedForGlobalConfiguration && AuthorizedForSiteConfiguration)
            {
                CurrentMaster.DisplaySiteSelectorPanel = true;
                if (!RequestHelper.IsPostBack())
                {
                    SiteOrGlobalSelector.SiteID = QueryHelper.GetInteger("siteId", CMSContext.CurrentSiteID);
                }
                SiteID = SiteOrGlobalSelector.SiteID;
            }
            // User is authorized for site account statuses
            else if (AuthorizedForSiteConfiguration)
            {
                SiteID = CMSContext.CurrentSiteID;
            }
            // User is authorized only for global account statuses
            else if (AuthorizedForGlobalConfiguration)
            {
                SiteID = UniSelector.US_GLOBAL_RECORD;
            }
            // User is not authorized
            else
            {
                RedirectToCMSDeskAccessDenied(ModuleEntry.CONTACTMANAGEMENT, "ReadConfiguration");
            }
        }

        // Set header actions (add button)
        string[,] actions = new string[1,8];
        actions[0, 0] = "HyperLink";
        actions[0, 1] = GetString("om.accountstatus.new");
        actions[0, 3] = AddSiteQuery("Edit.aspx", null);
        actions[0, 5] = GetImageUrl("Objects/OM_AccountStatus/add.png");
        hdrActions.Actions = actions;

        // Filter site data
        Grid.WhereCondition = SqlHelperClass.AddWhereCondition(Grid.WhereCondition, GetSiteFilter("AccountStatusSiteID"));
        Grid.OnBeforeDataReload += () => { Grid.NamedColumns["sitename"].Visible = ((SiteID < 0) && (SiteID != UniSelector.US_GLOBAL_RECORD)); };
        Grid.EditActionUrl = AddSiteQuery(Grid.EditActionUrl, null);
        Grid.ZeroRowsText = GetString("om.accountstatus.notfound");
        Grid.OnAction += new OnActionEventHandler(Grid_OnAction);
    }


    /// <summary>
    /// UniGrid action handler.
    /// </summary>
    private void Grid_OnAction(string actionName, object actionArgument)
    {
        if (actionName == "delete")
        {
            AccountStatusInfo asi = AccountStatusInfoProvider.GetAccountStatusInfo(ValidationHelper.GetInteger(actionArgument, 0));
            if (asi != null)
            {
                //  Check modify permission for given object
                if (ConfigurationHelper.AuthorizedModifyConfiguration(asi.AccountStatusSiteID, true))
                {
                    AccountStatusInfoProvider.DeleteAccountStatusInfo(asi);
                }
            }
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        // Check permissions to create new record
        if ((SiteID > 0) && !ModifySiteConfiguration)
        {
            hdrActions.Enabled = false;
        }
        else if ((SiteID <= 0) && !ModifyGlobalConfiguration)
        {
            hdrActions.Enabled = false;
        }
        // Allow new button only for particular sites or (global) site
        else if ((SiteID < 0) && (SiteID != UniSelector.US_GLOBAL_RECORD))
        {
            hdrActions.Enabled = false;
            lblWarnNew.Visible = true;
        }
    }
}