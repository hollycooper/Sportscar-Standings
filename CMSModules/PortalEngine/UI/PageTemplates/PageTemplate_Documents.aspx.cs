using System;
using System.Data;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.PortalEngine;
using CMS.SettingsProvider;
using CMS.SiteProvider;
using CMS.DocumentEngine;
using CMS.UIControls;

public partial class CMSModules_PortalEngine_UI_PageTemplates_PageTemplate_Documents : CMSEditTemplatePage
{
    #region "Private variables"

    private UserInfo currentUser = null;

    #endregion


    #region "Page events"

    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        currentUser = CMSContext.CurrentUser;

        // Get current page template ID
        if (PageTemplate != null)
        {
            if (PageTemplate.IsReusable)
            {
                filterDocuments.LoadSites = true;
            }
            else
            {
                filterDocuments.SitesPlaceHolder.Visible = false;
            }
        }
        docElem.UniGrid.OnBeforeDataReload += new OnBeforeDataReload(UniGrid_OnBeforeDataReload);
        docElem.UniGrid.OnAfterDataReload += new OnAfterDataReload(UniGrid_OnAfterDataReload);

        string site = null;

        // Site selector is available for global admin
        if (currentUser.IsGlobalAdministrator)
        {
            site = filterDocuments.SelectedSite;
        }
        else
        {
            site = CMSContext.CurrentSiteName;
        }

        // For ad-hoc templates use all sites
        if (!PageTemplate.IsReusable)
        {
            site = TreeProvider.ALL_SITES;
        }
        docElem.SiteName = site;
    }

    #endregion


    #region "Grid events"

    protected void UniGrid_OnBeforeDataReload()
    {
        // Generate where condition
        string where = String.Format("DocumentPageTemplateID = {0} OR NodeTemplateID = {0} OR NodeWireframeTemplateID = {0}", PageTemplateID);

        where = SqlHelperClass.AddWhereCondition(where, filterDocuments.WhereCondition);
        docElem.UniGrid.WhereCondition = SqlHelperClass.AddWhereCondition(docElem.UniGrid.WhereCondition, where);
    }


    protected void UniGrid_OnAfterDataReload()
    {
        // Filter data by permissions
        DataSet ds = docElem.UniGrid.GridView.DataSource as DataSet;
        ds = TreeSecurityProvider.FilterDataSetByPermissions(ds, NodePermissionsEnum.Read, currentUser);
        plcFilter.Visible = docElem.UniGrid.DisplayExternalFilter(filterDocuments.FilterIsSet);
    }

    #endregion
}