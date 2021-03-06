using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.SiteProvider;
using CMS.UIControls;

public partial class CMSModules_SmartSearch_SearchIndex_Content_Edit : SiteManagerPage
{
    private string itemType = QueryHelper.GetString("itemtype", SearchIndexSettingsInfo.TYPE_ALLOWED);
    private int indexId = QueryHelper.GetInteger("indexid", 0);
    private Guid itemGuid = QueryHelper.GetGuid("guid", Guid.Empty);
    private string indexType = PredefinedObjectType.DOCUMENT;


    protected void Page_Load(object sender, EventArgs e)
    {
        // Get search index info object
        SearchIndexInfo sii = SearchIndexInfoProvider.GetSearchIndexInfo(indexId);
        if (sii != null)
        {
            // If doesn't exist select default type
            indexType = sii.IndexType;
        }

        ContentEdit.Visible = false;
        forumEdit.Visible = false;
        customTableEdit.Visible = false;

        ContentEdit.StopProcessing = true;
        forumEdit.StopProcessing = true;
        customTableEdit.StopProcessing = true;

        // Set tabs according to index type
        switch (indexType)
        {
            case PredefinedObjectType.DOCUMENT:
            case SearchHelper.DOCUMENTS_CRAWLER_INDEX:

                // Document index
                ContentEdit.ItemID = indexId;
                ContentEdit.ItemGUID = itemGuid;
                ContentEdit.Visible = true;
                ContentEdit.StopProcessing = false;
                break;

            case PredefinedObjectType.FORUM:

                // Forum index type
                forumEdit.ItemID = indexId;
                forumEdit.ItemGUID = itemGuid;
                forumEdit.Visible = true;
                forumEdit.StopProcessing = false;
                break;

            case SettingsObjectType.CUSTOMTABLE:

                // Custom table index
                customTableEdit.ItemID = indexId;
                customTableEdit.ItemGUID = itemGuid;
                customTableEdit.Visible = true;
                customTableEdit.StopProcessing = false;
                break;
        }

        ContentEdit.ItemType = itemType;
        forumEdit.ItemType = itemType;
    }


    protected override void OnPreRender(EventArgs e)
    {
        // Set item type
        switch (indexType)
        {
            case PredefinedObjectType.DOCUMENT:
                itemType = ContentEdit.ItemType;
                break;
            case PredefinedObjectType.FORUM:
                itemType = forumEdit.ItemType;
                break;
        }

        // Set help according item type
        if (itemType == SearchIndexSettingsInfo.TYPE_ALLOWED)
        {
            switch (indexType)
            {
                case PredefinedObjectType.DOCUMENT:
                case SearchHelper.DOCUMENTS_CRAWLER_INDEX:
                    CurrentMaster.Title.HelpTopicName = "searchindex_allowed_content";
                    break;
                case PredefinedObjectType.FORUM:
                    CurrentMaster.Title.HelpTopicName = "searchindex_allowed_forum";
                    break;
                case SettingsObjectType.CUSTOMTABLE:
                    CurrentMaster.Title.HelpTopicName = "searchindex_customtable_edit";
                    break;
            }
        }
        else
        {
            // Excluded help topics
            switch (indexType)
            {
                case PredefinedObjectType.DOCUMENT:
                case SearchHelper.DOCUMENTS_CRAWLER_INDEX:
                    CurrentMaster.Title.HelpTopicName = "searchindex_excluded_content";
                    break;
                case PredefinedObjectType.FORUM:
                    CurrentMaster.Title.HelpTopicName = "searchindex_excluded_forum";
                    break;
            }
        }

        // Init breadcrumbs - new item breadcrumb
        string[,] breadcrumbs = new string[2,3];
        breadcrumbs[0, 0] = GetString("srch.index.itemlist");
        breadcrumbs[0, 1] = "~/CMSModules/SmartSearch/SearchIndex_Content_List.aspx?indexid=" + indexId;
        breadcrumbs[0, 2] = "";

        if (itemGuid != Guid.Empty)
        {
            breadcrumbs[1, 0] = GetString("srch.index.currentitem");
        }
        else
        {
            // Allowed or Excluded item
            if (itemType == SearchIndexSettingsInfo.TYPE_ALLOWED)
            {
                breadcrumbs[1, 0] = GetString("srch.index.newtitemallowed");
            }
            else
            {
                breadcrumbs[1, 0] = GetString("srch.index.newtitemexcluded");
            }
        }

        breadcrumbs[1, 1] = "";
        breadcrumbs[1, 2] = "";

        CurrentMaster.Title.Breadcrumbs = breadcrumbs;
    }
}