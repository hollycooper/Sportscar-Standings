using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.UIControls;

public partial class CMSModules_ImportExport_SiteManager_Site_New : CMSImportExportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Initialize PageTitle
        string[,] pageTitleTabs = new string[2, 3];
        pageTitleTabs[0, 0] = GetString("general.sites");
        pageTitleTabs[0, 1] = "~/CMSSiteManager/Sites/site_list.aspx";
        pageTitleTabs[0, 2] = "cmsdesktop";
        pageTitleTabs[1, 0] = GetString("Site_Edit.NewSite");
        pageTitleTabs[1, 1] = string.Empty;

        ptNewSite.Breadcrumbs = pageTitleTabs;
        ptNewSite.TitleText = GetString("Site_Edit.NewSite");
        ptNewSite.TitleImage = GetImageUrl("Objects/CMS_Site/new.png");
    }
}