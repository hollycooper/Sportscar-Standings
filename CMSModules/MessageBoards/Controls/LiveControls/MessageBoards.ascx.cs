using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.UIControls;

public partial class CMSModules_MessageBoards_Controls_LiveControls_MessageBoards : CMSAdminItemsControl
{
    #region "Private variables"

    private int mBoardId = 0;
    private int mGroupId = 0;
    private bool mHideWhenGroupIsNotSupplied = false;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Determines whether to hide the content of the control when GroupID is not supplied.
    /// </summary>
    public bool HideWhenGroupIsNotSupplied
    {
        get
        {
            return mHideWhenGroupIsNotSupplied;
        }
        set
        {
            mHideWhenGroupIsNotSupplied = value;
        }
    }


    /// <summary>
    /// Current board ID.
    /// </summary>
    public int BoardID
    {
        get
        {
            return mBoardId;
        }
        set
        {
            mBoardId = value;
        }
    }


    /// <summary>
    /// Current group ID.
    /// </summary>
    public int GroupID
    {
        get
        {
            return mGroupId;
        }
        set
        {
            mGroupId = value;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        #region "Security"

        RaiseOnCheckPermissions(PERMISSION_READ, this);

        boards.OnCheckPermissions += new CheckPermissionsEventHandler(boards_OnCheckPermissions);
        messages.OnCheckPermissions += new CheckPermissionsEventHandler(messages_OnCheckPermissions);

        #endregion


        if (!Visible)
        {
            EnableViewState = false;
        }

        if (StopProcessing)
        {
            messages.StopProcessing = true;
            boards.StopProcessing = true;
        }
        else
        {
            // Hide controls if the control should be hidden
            if ((GroupID == 0) && HideWhenGroupIsNotSupplied)
            {
                Visible = false;
                return;
            }

            tabElem.TabControlIdPrefix = "messageboards";

            boards.IsLiveSite = IsLiveSite;
            boards.GroupID = GroupID;
            boards.DisplayMode = DisplayMode;
            boards.HideWhenGroupIsNotSupplied = true;

            messages.IsLiveSite = IsLiveSite;
            messages.BoardID = BoardID;
            messages.GroupID = GroupID;
            messages.HideWhenGroupIsNotSupplied = true;

            // Initialize the tab control
            InitializeTabs();
        }
    }


    /// <summary>
    /// Initializes the tabs.
    /// </summary>
    private void InitializeTabs()
    {
        string[,] tabs = new string[2,4];
        tabs[0, 0] = GetString("Group_General.Boards.Messages");
        tabs[1, 0] = GetString("Group_General.Boards.Boards");

        tabElem.Tabs = tabs;
        tabElem.OnTabClicked += new EventHandler(tabElem_OnTabChanged);
    }


    protected void tabElem_OnTabChanged(object sender, EventArgs e)
    {
        int tabIndex = tabElem.SelectedTab;

        // Handle message list control setting
        tabMessages.Visible = (tabIndex == 0);
        if (tabMessages.Visible)
        {
            messages.ReloadData();
        }

        tabBoards.Visible = (tabIndex == 1);
        if (tabBoards.Visible)
        {
            boards.ReloadData();
        }
    }


    #region "Private methods"

    /// <summary>
    /// Sets the message boards section to the default state.
    /// </summary>
    private void ResetState()
    {
        tabElem.SelectedTab = 0;

        tabMessages.Visible = true;
        tabBoards.Visible = false;
    }

    #endregion


    #region "Security handlers"

    protected void messages_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        RaiseOnCheckPermissions(permissionType, sender);
    }


    protected void boards_OnCheckPermissions(string permissionType, CMSAdminControl sender)
    {
        RaiseOnCheckPermissions(permissionType, sender);
    }

    #endregion
}