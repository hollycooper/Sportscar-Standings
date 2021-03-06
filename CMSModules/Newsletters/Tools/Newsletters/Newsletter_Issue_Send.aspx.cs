using System;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.Newsletter;
using CMS.UIControls;
using CMS.ExtendedControls.ActionsConfig;
using CMS.SettingsProvider;
using CMS.ExtendedControls;

public partial class CMSModules_Newsletters_Tools_Newsletters_Newsletter_Issue_Send : CMSNewsletterNewslettersPage
{
    #region "Variables"

    protected int issueId = 0;
    NewsletterInfo newsletter;

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Get issue ID from querystring
        issueId = QueryHelper.GetInteger("issueid", 0);

        // Get newsletter issue and check its existence
        IssueInfo issue = IssueInfoProvider.GetIssueInfo(issueId);
        EditedObject = issue;

        string infoMessage = null;
        bool isABTest = issue.IssueIsABTest;
        bool sendingIssueAllowed = false;

        sendElem.Visible = !isABTest;
        sendVariant.Visible = isABTest;

        if (!isABTest)
        {
            sendElem.IssueID = issueId;
            sendElem.NewsletterID = issue.IssueNewsletterID;
            bool isSent = (issue.IssueMailoutTime != DateTimeHelper.ZERO_TIME) && (issue.IssueMailoutTime < DateTime.Now);
            infoMessage = (isSent ? GetString("Newsletter_Issue_Header.AlreadySent") : GetString("Newsletter_Issue_Header.NotSentYet"));

            // If resending is disabled check that the issue has 'Idle' status
            newsletter = NewsletterInfoProvider.GetNewsletterInfo(issue.IssueNewsletterID);
            if (newsletter != null)
            {
                sendingIssueAllowed = newsletter.NewsletterEnableResending || issue.IssueStatus == IssueStatusEnum.Idle;
            }
        }
        else
        {
            sendVariant.StopProcessing = (issueId <= 0);
            sendVariant.IssueID = issueId;
            sendVariant.OnChanged -= new EventHandler(sendVariant_OnChanged);
            sendVariant.OnChanged += new EventHandler(sendVariant_OnChanged);
            sendVariant.ReloadData(!RequestHelper.IsPostBack());
            infoMessage = sendVariant.InfoMessage;
            sendingIssueAllowed = sendVariant.SendingAllowed;
        }

        // Display additional information
        if (!String.IsNullOrEmpty(infoMessage))
        {
            ShowInformationInternal(infoMessage);
        }

        InitHeaderActions(isABTest && (issue.IssueStatus != IssueStatusEnum.Finished), sendingIssueAllowed);
        
        string scriptBlock = @"function RefreshPage() {{ document.location.replace(document.location); }}";
        ScriptHelper.RegisterClientScriptBlock(this, GetType(), "Actions", scriptBlock, true);

        if (!RequestHelper.IsPostBack() && (QueryHelper.GetInteger("sent", 0) == 1))
        {
            ShowConfirmation(GetString("Newsletter_Send.SuccessfullySent"));
        }
    }


    protected void sendVariant_OnChanged(object sender, EventArgs e)
    {
        ShowInformationInternal(sendVariant.InfoMessage);
    }


    /// <summary>
    /// Sends an issue.
    /// </summary>
    protected void Send()
    {
        // Check permission
        if (!CMSContext.CurrentUser.IsAuthorizedPerResource("cms.newsletter", "authorissues"))
        {
            RedirectToCMSDeskAccessDenied("cms.newsletter", "authorissues");
        }

        string errMessage = null;
        if (sendElem.Visible)
        {
            if (!sendElem.SendIssue())
            {
                errMessage = sendElem.ErrorMessage;
            }
            else
            {
                if ((newsletter != null) && !newsletter.NewsletterEnableResending)
                {
                    // Redirect to the issue list page
                    ScriptHelper.RegisterStartupScript(this, typeof(string), "Newsletter_Issue_Send", "parent.location='" + ResolveUrl("~/CMSModules/Newsletters/Tools/Newsletters/Newsletter_Issue_List.aspx?newsletterid=" + newsletter.NewsletterID) + "';", true);
                }
            }
        }
        else if (sendVariant.Visible)
        {
            if (!sendVariant.SendIssue())
            {
                errMessage = sendVariant.ErrorMessage;
            }
        }

        if (String.IsNullOrEmpty(errMessage))
        {
            string url = URLHelper.AddParameterToUrl(URLHelper.CurrentURL, "sent", "1");
            URLHelper.Redirect(url);
        }
        else
        {
            ShowError(errMessage);
        }
    }


    /// <summary>
    /// Saves current issue variant setting.
    /// </summary>
    protected void Save()
    {
        // Check permission
        if (!CMSContext.CurrentUser.IsAuthorizedPerResource("cms.newsletter", "authorissues"))
        {
            RedirectToCMSDeskAccessDenied("cms.newsletter", "authorissues");
        }

        if (sendVariant.SaveIssue())
        {
            if (!String.IsNullOrEmpty(sendVariant.InfoMessage))
            {
                ShowInformationInternal(sendVariant.InfoMessage);
            }

            ShowChangesSaved();
        }
        else
        {
            ShowError(sendVariant.ErrorMessage);
        }
    }


    /// <summary>
    /// Initializes header action control.
    /// </summary>
    /// <param name="isActiveABTest">Indicates if the issue is A/B test and its status is not 'Finished'</param>
    /// <param name="allowSendingIssue">Indicates if sending is allowed</param>
    private void InitHeaderActions(bool isActiveABTest, bool allowSendingIssue)
    {
        HeaderActions hdrActions = CurrentMaster.HeaderActions;
        hdrActions.ActionsList.Clear();

        // Init save button
        if (isActiveABTest)
        {
            hdrActions.ActionsList.Add(new SaveAction(this));
        }

        // Init send button
        if (allowSendingIssue)
        {
            hdrActions.ActionsList.Add(new HeaderAction()
            {
                ControlType = HeaderActionTypeEnum.LinkButton,
                CommandName = ComponentEvents.SUBMIT,
                Text = GetString("newsletterissue_send.send"),
                Tooltip = GetString("newsletterissue_send.send"),
                ImageUrl = GetImageUrl("CMSModules/CMS_Newsletter/send.png"),
                Enabled = true
            });
        }

        hdrActions.ActionPerformed += new CommandEventHandler(hdrActions_ActionPerformed);
        hdrActions.ReloadData();

        CurrentMaster.DisplayActionsPanel = true;
    }


    /// <summary>
    /// Shows user friendly message in ordinary label.
    /// </summary>
    /// <param name="message">Message to be shown</param>
    private void ShowInformationInternal(string message)
    {
        lblInfo.Text = message;
        lblInfo.Visible = true;
    }


    protected void hdrActions_ActionPerformed(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case ComponentEvents.SAVE:
                Save();
                break;
            case ComponentEvents.SUBMIT:
                Send();
                break;
        }
    }

    #endregion
}