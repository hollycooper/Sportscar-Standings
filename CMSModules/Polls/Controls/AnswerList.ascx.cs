using System;
using System.Data;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.FormEngine;
using CMS.GlobalHelper;
using CMS.Polls;
using CMS.SettingsProvider;
using CMS.SiteProvider;
using CMS.UIControls;

public partial class CMSModules_Polls_Controls_AnswerList : CMSAdminListControl
{
    #region "Variables"

    private bool mAllowEdit = true;
    private bool bizFormsAvailable = false;

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets and sets Poll ID.
    /// </summary>
    public int PollId
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState[ClientID + "PollID"], 0);
        }
        set
        {
            ViewState[ClientID + "PollID"] = value;
        }
    }


    /// <summary>
    /// Indicates if DelayedReload for Unigrid should be used.
    /// </summary>
    public bool DelayedReload
    {
        get
        {
            return uniGrid.DelayedReload;
        }
        set
        {
            uniGrid.DelayedReload = value;
        }
    }


    /// <summary>
    /// Indicates if move/edit actions should be allowed
    /// </summary>
    public bool AllowEdit
    {
        get
        {
            return mAllowEdit;
        }
        set
        {
            mAllowEdit = value;
        }
    }


    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check if parent object exists
        if ((PollId > 0) && !IsLiveSite)
        {
            CMSPage.EditedObject = PollInfoProvider.GetPollInfo(PollId);
        }

        ScriptHelper.RegisterDialogScript(Page);

        uniGrid.IsLiveSite = IsLiveSite;
        uniGrid.OnAction += new OnActionEventHandler(uniGrid_OnAction);
        uniGrid.GridView.AllowSorting = false;
        uniGrid.WhereCondition = "AnswerPollID=" + PollId;
        uniGrid.ZeroRowsText = GetString("general.nodatafound");
        uniGrid.OnExternalDataBound += UniGrid_OnExternalDataBound;
        uniGrid.OnBeforeDataReload += UniGrid_OnBeforeDataReload;

        if (!AllowEdit)
        {    
            uniGrid.ShowObjectMenu = false;
        }

        bizFormsAvailable = ModuleEntry.IsModuleLoaded(ModuleEntry.BIZFORM) && ResourceSiteInfoProvider.IsResourceOnSite(ModuleEntry.BIZFORM, CMSContext.CurrentSiteName);
    }


    /// <summary>
    /// Handles the UniGrid's OnAction event.
    /// </summary>
    /// <param name="actionName">Name of item (button) that throws event</param>
    /// <param name="actionArgument">ID (value of Primary key) of corresponding data row</param>
    protected void uniGrid_OnAction(string actionName, object actionArgument)
    {
        if (actionName == "edit")
        {
            SelectedItemID = Convert.ToInt32(actionArgument);
            RaiseOnEdit();
        }
        else if (actionName == "delete")
        {
            if (!AllowEdit)
            {
                return;
            }

            // Delete PollAnswerInfo object from database
            PollAnswerInfoProvider.DeletePollAnswerInfo(Convert.ToInt32(actionArgument));
            ReloadData(true);
        }
        else if (actionName == "moveup")
        {
            if (!AllowEdit)
            {
                return;
            }

            // Move the answer up in order
            PollAnswerInfoProvider.MoveAnswerUp(PollId, Convert.ToInt32(actionArgument));
            ReloadData(true);
        }
        else if (actionName == "movedown")
        {
            if (!AllowEdit)
            {
                return;
            }

            // Move the answer down in order
            PollAnswerInfoProvider.MoveAnswerDown(PollId, Convert.ToInt32(actionArgument));
            ReloadData(true);
        }
    }


    /// <summary>
    /// Forces unigrid to reload data.
    /// </summary>
    public override void ReloadData(bool forceReload)
    {
        uniGrid.WhereCondition = "AnswerPollID=" + PollId;

        if (forceReload)
        {
            uniGrid.WhereClause = null;
            uniGrid.ResetFilter();
        }

        uniGrid.ReloadData();
    }


    private object UniGrid_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "edit":
            case "moveup":
            case "movedown":
            case "delete":
                (sender as ImageButton).Visible = AllowEdit;
                return sender;
            case "answerenabled":
                return UniGridFunctions.ColoredSpanYesNo(parameter);
            case "answerisopenended":
                return String.IsNullOrEmpty(ValidationHelper.GetString(parameter, string.Empty)) ? GetString("polls.AnswerTypeStandard") : GetString("polls.AnswerTypeOpenEnded");
            case "answerform":
                if (sender is ImageButton)
                {
                    ImageButton imageButton = sender as ImageButton;
                    GridViewRow gvr = parameter as GridViewRow;

                    if (!CMSContext.CurrentUser.IsAuthorizedPerResource("CMS.Form", "ReadData"))
                    {
                        imageButton.Visible = false;
                    }
                    else if (gvr != null)
                    {
                        DataRowView drv = gvr.DataItem as DataRowView;
                        if (drv != null)
                        {
                            string formName = ValidationHelper.GetString(drv["AnswerForm"], null);
                            if (String.IsNullOrEmpty(formName))
                            {
                                imageButton.Visible = false;
                            }
                            else
                            {
                                BizFormInfo bfi = BizFormInfoProvider.GetBizFormInfo(formName, CMSContext.CurrentSiteID);
                                if ((bfi != null) && bizFormsAvailable)
                                {
                                    imageButton.OnClientClick = "modalDialog('" + ResolveUrl("~/CMSModules/Polls/Tools/Polls_Answer_Results.aspx") + "?formid=" + bfi.FormID + "&dialogmode=1', 'AnswerForm', '1000', '700'); return false;";
                                }
                                else
                                {
                                    imageButton.Visible = false;
                                }
                            }
                        }
                    }
                }
                return sender;
        }
        return parameter;
    }


    protected void UniGrid_OnBeforeDataReload()
    {
        PollInfo pi = PollInfoProvider.GetPollInfo(PollId);
        uniGrid.GridView.Columns[4].Visible = (pi != null) && (pi.PollSiteID > 0) && (pi.PollGroupID == 0) && bizFormsAvailable;
    }

    #endregion
}