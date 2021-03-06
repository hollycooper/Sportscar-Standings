using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.OnlineMarketing;
using CMS.UIControls;
using CMS.WebAnalytics;
using CMS.ExtendedControls;

public partial class CMSModules_OnlineMarketing_Pages_Tools_AbTest_List : CMSABTestPage
{
    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        String siteName = CMSContext.CurrentSiteName;
        
        // Set disabled module info
        ucDisabledModule.SettingsKeys = "CMSAnalyticsEnabled;CMSABTestingEnabled;CMSAnalyticsTrackConversions";
        ucDisabledModule.InfoTexts.Add(GetString("WebAnalytics.Disabled") + "</br>");
        ucDisabledModule.InfoTexts.Add(GetString("abtesting.disabled") + "</br>");
        ucDisabledModule.InfoTexts.Add(GetString("webanalytics.tackconversionsdisabled"));
        ucDisabledModule.ParentPanel = pnlDisabled;

        // Prepare the actions
        string[,] actions = new string[1,6];
        actions[0, 0] = HeaderActions.TYPE_HYPERLINK;
        actions[0, 1] = ResHelper.GetString("abtesting.abtest.new");
        actions[0, 3] = ResolveUrl("new.aspx");
        actions[0, 5] = GetImageUrl("Objects/OM_AbTest/add.png");

        // Set the actions
        ICMSMasterPage master = CurrentMaster;
        master.HeaderActions.Actions = actions;

        listElem.ShowFilter = true;
    }

    #endregion
}