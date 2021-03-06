using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.UIControls;

public partial class CMSModules_Forums_Tools_Groups_Group_List : CMSForumsPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        forumGroupList.OnAction += new CommandEventHandler(forumGroupList_OnAction);

        InitializeMasterPage();

        // Do not display Groups forums
        forumGroupList.WhereCondition = "GroupGroupID IS NULL";
    }


    protected void forumGroupList_OnAction(object sender, CommandEventArgs e)
    {
        switch (e.CommandName.ToLowerCSafe())
        {
            case "edit":
                URLHelper.Redirect("Group_Frameset.aspx?groupId=" + Convert.ToString(e.CommandArgument));
                break;

            case "delete":
            case "up":
            case "down":
                forumGroupList.ReloadData();
                break;
        }
    }


    /// <summary>
    /// Initializes Master Page.
    /// </summary>
    protected void InitializeMasterPage()
    {
        // Set title and help
        Title = "Group list";
        CurrentMaster.Title.HelpTopicName = "forum_list";
        CurrentMaster.Title.HelpName = "helpTopic";

        // Set title label
        CurrentMaster.Title.TitleText = GetString("Group_List.HeaderCaption");
        CurrentMaster.Title.TitleImage = GetImageUrl("Objects/Forums_ForumGroup/object.png");

        // Set actions
        string[,] actions = new string[1,8];
        actions[0, 0] = "HyperLink";
        actions[0, 1] = GetString("Group_List.NewItemCaption");
        actions[0, 3] = ResolveUrl("Group_New.aspx");
        actions[0, 5] = GetImageUrl("Objects/Forums_ForumGroup/add.png");

        CurrentMaster.HeaderActions.Actions = actions;
    }
}