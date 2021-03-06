using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.GlobalHelper;
using CMS.Polls;
using CMS.UIControls;
using CMS.ExtendedControls.ActionsConfig;

public partial class CMSModules_Groups_Tools_Polls_Polls_Edit_Answer_Edit : CMSGroupPollsPage
{
    protected int pollId = 0;
    protected int answerId = 0;
    protected int groupId = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        // Get AnswerID and PollID from querystring
        pollId = QueryHelper.GetInteger("pollId", 0);
        answerId = QueryHelper.GetInteger("answerId", 0);
        groupId = QueryHelper.GetInteger("groupId", 0);

        string currentPollAnswer = GetString("Polls_Answer_Edit.NewItemCaption");

        // Initialize AnswerEdit control
        if (QueryHelper.GetInteger("saved", 0) == 1)
        {
            AnswerEdit.Saved = true;
        }
        AnswerEdit.ItemID = answerId;
        AnswerEdit.PollId = pollId;
        AnswerEdit.OnSaved += new EventHandler(AnswerEdit_OnSaved);
        AnswerEdit.OnCheckPermissions += new CMSAdminControl.CheckPermissionsEventHandler(AnswerEdit_OnCheckPermissions);

        if (answerId > 0)
        {
            CurrentMaster.Title.HelpTopicName = "answer_edit";
            PollAnswerInfo pollAnswerObj = PollAnswerInfoProvider.GetPollAnswerInfo(answerId);
            EditedObject = pollAnswerObj;
            if (pollAnswerObj != null)
            {
                // Check that poll belongs to the specified group
                if ((pollAnswerObj.AnswerPollID > 0) && (groupId > 0))
                {
                    PollInfo poll = PollInfoProvider.GetPollInfo(pollAnswerObj.AnswerPollID);

                    // Answer not found or doesn't belong to specified group
                    if ((poll == null) || (poll.PollGroupID != groupId))
                    {
                        RedirectToAccessDenied(GetString("community.group.pollnotassigned"));
                    }
                }

                // Set control
                currentPollAnswer = GetString("Polls_Answer_Edit.AnswerLabel") + " " + pollAnswerObj.AnswerOrder.ToString();
                pollId = pollAnswerObj.AnswerPollID;
            }
        }
        else
        {
            CurrentMaster.Title.HelpTopicName = "new_answer";
        }

        // Validate
        EditedObject = PollInfoProvider.GetPollInfo(pollId);

        // Initializes page title control		
        string[,] breadcrumbs = new string[2,3];
        breadcrumbs[0, 0] = GetString("Polls_Answer_Edit.ItemListLink");
        breadcrumbs[0, 1] = "~/CMSModules/Groups/Tools/Polls/Polls_Edit_Answer_List.aspx?pollId=" + pollId + "&groupId=" + groupId;
        breadcrumbs[0, 2] = "";
        breadcrumbs[1, 0] = currentPollAnswer;
        breadcrumbs[1, 1] = "";
        breadcrumbs[1, 2] = "";
        CurrentMaster.Title.Breadcrumbs = breadcrumbs;

        // New item link
        HeaderAction add = new HeaderAction()
        {
            ControlType = HeaderActionTypeEnum.Hyperlink,
            Text = GetString("Polls_Answer_List.NewItemCaption"),
            RedirectUrl = ResolveUrl("Polls_Edit_Answer_Edit.aspx?pollId=" + pollId.ToString() + "&groupId=" + groupId),
            ImageUrl = GetImageUrl("Objects/Polls_PollAnswer/add.png")
        };
        CurrentMaster.HeaderActions.AddAction(add);
    }


    private void AnswerEdit_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        // Check 'Manage' permission
        PollInfo pi = PollInfoProvider.GetPollInfo(AnswerEdit.PollId);
        int groupId = 0;

        if (pi != null)
        {
            groupId = pi.PollGroupID;
        }

        // Check permissions
        CheckPermissions(groupId, CMSAdminControl.PERMISSION_MANAGE);
    }


    /// <summary>
    /// AnswerEdit event handler.
    /// </summary>
    private void AnswerEdit_OnSaved(object sender, EventArgs e)
    {
        URLHelper.Redirect("Polls_Edit_Answer_Edit.aspx?answerId=" + AnswerEdit.ItemID.ToString() + "&groupId=" + groupId + "&saved=1");
    }
}