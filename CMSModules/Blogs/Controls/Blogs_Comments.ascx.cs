using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

using CMS.Blogs;
using CMS.CMSHelper;
using CMS.ExtendedControls;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.DocumentEngine;
using CMS.UIControls;

public partial class CMSModules_Blogs_Controls_Blogs_Comments : CMSAdminControl
{
    #region "Variables"

    protected CurrentUserInfo currentUser = null;
    private bool mShowFilter = true;
    private string mIsApproved = "no";
    private string mIsSpam = "all";
    private string mOrderBy = "CommentDate asc";
    private string mItemsPerPage = string.Empty;
    private string mBlogName = "##myblogs##";
    private string mSiteName = CMSContext.CurrentSiteName;

    #endregion


    #region "Properties"

    /// <summary>
    /// Indicates whether shown comments are approved.
    /// </summary>
    public string IsApproved
    {
        get
        {
            return mIsApproved;
        }
        set
        {
            mIsApproved = value;
        }
    }


    /// <summary>
    /// Blog name to filter.
    /// </summary>
    public string BlogName
    {
        get
        {
            return mBlogName;
        }
        set
        {
            mBlogName = value;
        }
    }


    /// <summary>
    /// Order by for grid.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return mOrderBy;
        }

        set
        {
            mOrderBy = value;
        }
    }


    /// <summary>
    /// Items per page.
    /// </summary>
    public string ItemsPerPage
    {
        get
        {
            return mItemsPerPage;
        }
        set
        {
            mItemsPerPage = value;
        }
    }


    /// <summary>
    /// Indicates whether show spam marked comments.
    /// </summary>
    public string IsSpam
    {
        get
        {
            return mIsSpam;
        }
        set
        {
            mIsSpam = value;
        }
    }


    /// <summary>
    /// Site name of blogs.
    /// </summary>
    public string SiteName
    {
        get
        {
            return mSiteName;
        }
        set
        {
            mSiteName = value;
        }
    }


    /// <summary>
    /// If false don't show filter.
    /// </summary>
    public bool ShowFilter
    {
        get
        {
            return mShowFilter;
        }
        set
        {
            mShowFilter = value;
        }
    }


    /// <summary>
    /// Gets the filter query string.
    /// </summary>
    public string FilterQueryString
    {
        get
        {
            return "&approved=" + IsApproved +
                   "&isspam=" + IsSpam;
        }
    }


    /// <summary>
    /// Creates condition for DB query (if filter set via web part).
    /// </summary>    
    private String CommentWhereCondition
    {
        get
        {
            string where = String.Empty;

            // Is approved
            switch (IsApproved.ToLowerCSafe())
            {
                case "yes":
                    where = SqlHelperClass.AddWhereCondition(where, "CommentApproved = 1");
                    break;

                case "no":
                    where = SqlHelperClass.AddWhereCondition(where, "(CommentApproved = 0 OR CommentApproved IS NULL )");
                    break;
            }

            // Is spam
            switch (IsSpam.ToLowerCSafe())
            {
                case "yes":
                    where = SqlHelperClass.AddWhereCondition(where, "CommentIsSpam = 1");
                    break;

                case "no":
                    where = SqlHelperClass.AddWhereCondition(where, "(CommentIsSpam = 0 OR CommentIsSpam IS NULL )");
                    break;
            }
            return where;
        }
    }


    /// <summary>
    /// Gets the Blog part of the WHERE condition.
    /// </summary>
    public string BlogWhereCondition
    {
        get
        {
            string blogWhere = string.Empty;


            // Blogs dropdownlist
            switch (BlogName.ToLowerCSafe())
            {
                case "##all##":
                    // If current user isn't Global admin or user with 'Manage' permissions for blogs
                    if (!currentUser.IsAuthorizedPerResource("cms.blog", "Manage"))
                    {
                        blogWhere = "(NodeOwner=" + currentUser.UserID +
                                    " OR (';' + BlogModerators + ';' LIKE N'%;" + SqlHelperClass.GetSafeQueryString(currentUser.UserName, false) + ";%'))";
                    }
                    break;

                case "##myblogs##":
                    blogWhere = "NodeOwner = " + currentUser.UserID;
                    break;

                default:
                    blogWhere = "BlogID = " + BlogName;
                    break;
            }

            return blogWhere;
        }
    }


    /// <summary>
    /// Indicates if controls is in MyDesk section.
    /// </summary>
    public bool IsInMydesk
    {
        get
        {
            return filterElem.IsInMydesk;
        }
        set
        {
            filterElem.IsInMydesk = value;
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            gridComments.Visible = false;
            return;
        }

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "PostBackRererence", ScriptHelper.GetScript("function postBack () {" + ControlsHelper.GetPostBackEventReference(this, String.Empty) + ";}"));

        ScriptHelper.RegisterDialogScript(Page);
        gridComments.OnAction += gridComments_OnAction;
        gridComments.OnExternalDataBound += gridComments_OnExternalDataBound;
        gridComments.OnDataReload += gridComments_OnDataReload;
        gridComments.HideControlForZeroRows = true;
        gridComments.ShowActionsMenu = true;
        gridComments.Columns = "CommentID, CommentDate, CommentApproved, CommentUserName, CommentText, CommentIsSpam";

        // Get all possible column names.
        BlogCommentInfo bci = new BlogCommentInfo();
        gridComments.AllColumns = SqlHelperClass.MergeColumns(bci.ColumnNames);

        btnAction.Text = GetString("General.Ok");
        btnAction.OnClientClick = "return MassConfirm('" + drpAction.ClientID + "'," + ScriptHelper.GetString(GetString("MyBlogs.Comments.ConfirmDelete")) + ");";

        currentUser = CMSContext.CurrentUser;

        // Load action dropdown
        if (!URLHelper.IsPostback())
        {
            // Actions dropdown
            drpAction.Items.Add(new ListItem(GetString("General.SelectAction"), string.Empty));
            drpAction.Items.Add(new ListItem(GetString("General.Approve"), "approve"));
            drpAction.Items.Add(new ListItem(GetString("General.Reject"), "reject"));
            drpAction.Items.Add(new ListItem(GetString("MyBlogs.Comments.IsSpam"), "spam"));
            drpAction.Items.Add(new ListItem(GetString("MyBlogs.Comments.IsNoSpam"), "nospam"));
            drpAction.Items.Add(new ListItem(GetString("General.Delete"), "delete"));
        }

        if (!ShowFilter)
        {
            filterElem.Visible = false;
            gridComments.OrderBy = OrderBy;
        }

        if (!RequestHelper.IsPostBack() && !string.IsNullOrEmpty(ItemsPerPage))
        {
            gridComments.Pager.DefaultPageSize = ValidationHelper.GetInteger(ItemsPerPage, -1);
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Hide actions dropdown and button if no data
        pnlActions.Visible = !DataHelper.DataSourceIsEmpty(gridComments.GridView.DataSource);
    }


    protected DataSet gridComments_OnDataReload(string completeWhere, string currentOrder, int currentTopN, string columns, int currentOffset, int currentPageSize, ref int totalRecords)
    {
        string whereCondition = String.Empty;
        string blogWhereCondition = String.Empty;
        if (ShowFilter)
        {
            whereCondition = filterElem.CommentWhereCondition;
            blogWhereCondition = filterElem.BlogWhereCondition;
        }
        else
        {
            whereCondition = CommentWhereCondition;
            blogWhereCondition = BlogWhereCondition;
        }

        // All sites  
        if (SiteName == "-1")
        {
            SiteName = TreeProvider.ALL_SITES;
        }

        // Set the current site if the siteName is not defined
        if (String.IsNullOrEmpty(SiteName))
        {
            SiteName = CMSContext.CurrentSiteName;
        }
        return BlogCommentInfoProvider.GetComments(0, null, whereCondition, blogWhereCondition, columns, currentTopN, currentOrder, currentOffset, currentPageSize, ref totalRecords, SiteName);
    }


    protected void gridComments_OnAction(string actionName, object actionArgument)
    {
        int commentId = ValidationHelper.GetInteger(actionArgument, 0);
        switch (actionName.ToLowerCSafe())
        {
            case "delete":
                // Delete specified comment
                BlogCommentInfoProvider.DeleteBlogCommentInfo(commentId);
                break;

            case "approve":
                BlogCommentInfo bci = BlogCommentInfoProvider.GetBlogCommentInfo(commentId);
                if (bci != null)
                {
                    if (bci.CommentApproved)
                    {
                        // Set comment as 'rejected'
                        bci.CommentApproved = false;
                        bci.CommentApprovedByUserID = 0;
                    }
                    else
                    {
                        // Set comment as 'approved'
                        bci.CommentApproved = true;
                        bci.CommentApprovedByUserID = currentUser.UserID;
                    }
                    BlogCommentInfoProvider.SetBlogCommentInfo(bci);
                }
                break;

            case "edit":
                // JavaScript
                break;
        }
    }


    protected object gridComments_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "commentusername":
                return HTMLHelper.HTMLEncode(Convert.ToString(parameter));

            case "commenttext":
                string text = Convert.ToString(parameter);
                if (text.Length > 50)
                {
                    text = text.Substring(0, 50) + "...";
                }
                return HTMLHelper.HTMLEncode(text);

            case "commentapproved":
                return UniGridFunctions.ColoredSpanYesNo(parameter);

            case "commentisspam":

                return UniGridFunctions.ColoredSpanYesNoReversed(parameter);

            case "approve":
                bool approve = ValidationHelper.GetBoolean(((DataRowView)((GridViewRow)parameter).DataItem).Row["CommentApproved"], false);
                if (!approve)
                {
                    ImageButton button = ((ImageButton)sender);
                    button.ImageUrl = GetImageUrl("Design/Controls/UniGrid/Actions/Approve.png");
                    button.ToolTip = GetString("general.approve");
                }
                else
                {
                    ImageButton button = ((ImageButton)sender);
                    button.ImageUrl = GetImageUrl("Design/Controls/UniGrid/Actions/Reject.png");
                    button.ToolTip = GetString("general.reject");
                }
                break;

            case "edit":
                string commentId = ((DataRowView)((GridViewRow)parameter).DataItem).Row["CommentID"].ToString();

                ImageButton editButton = ((ImageButton)sender);

                // Get filter query string
                string queryCondition = ShowFilter ? filterElem.FilterQueryString : FilterQueryString;

                // If no display use postback for refresh
                string usePostback = String.Empty;
                if (!ShowFilter)
                {
                    usePostback = "&usepostback=true";
                }

                editButton.OnClientClick = "modalDialog('" + ResolveUrl("~/CMSModules/Blogs/Controls/Comment_Edit.aspx") + "?commentID=" + commentId + queryCondition + usePostback + "', 'CommentEdit', 500, 440); return false;";
                break;
        }

        return parameter;
    }


    /// <summary>
    /// Handle mass actions.
    /// </summary>
    protected void btnAction_Click(object sender, EventArgs e)
    {
        if (drpAction.SelectedValue != "SELECT")
        {
            List<string> list = gridComments.SelectedItems;
            if (list.Count > 0)
            {
                foreach (string commnentId in list)
                {
                    BlogCommentInfo bci = BlogCommentInfoProvider.GetBlogCommentInfo(Convert.ToInt32(commnentId));
                    switch (drpAction.SelectedValue.ToLowerCSafe())
                    {
                        case "delete":
                            // Delete specified comment
                            BlogCommentInfoProvider.DeleteBlogCommentInfo(bci);
                            break;

                        case "approve":
                            if (!bci.CommentApproved)
                            {
                                // Set comment as 'approved'
                                bci.CommentApproved = true;
                                bci.CommentApprovedByUserID = CMSContext.CurrentUser.UserID;
                                BlogCommentInfoProvider.SetBlogCommentInfo(bci);
                            }
                            break;

                        case "reject":
                            if (bci.CommentApproved)
                            {
                                // Set comment as 'reject'
                                bci.CommentApproved = false;
                                bci.CommentApprovedByUserID = 0;
                                BlogCommentInfoProvider.SetBlogCommentInfo(bci);
                            }
                            break;

                        case "spam":
                            if (!bci.CommentIsSpam)
                            {
                                bci.CommentIsSpam = true;
                                BlogCommentInfoProvider.SetBlogCommentInfo(bci);
                            }
                            break;

                        case "nospam":
                            if (bci.CommentIsSpam)
                            {
                                bci.CommentIsSpam = false;
                                BlogCommentInfoProvider.SetBlogCommentInfo(bci);
                            }
                            break;
                    }
                }
            }
        }

        // Clear selection
        gridComments.ResetSelection();

        ReloadData();
    }


    /// <summary>
    /// Load data according to filter settings.
    /// </summary>
    public override void ReloadData()
    {
        gridComments.FilterIsSet = true;
        gridComments.ReloadData();
        base.ReloadData();
    }

    #endregion
}