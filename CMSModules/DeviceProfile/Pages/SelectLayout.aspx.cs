﻿using System;
using System.Text;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.PortalEngine;
using CMS.SettingsProvider;
using CMS.UIControls;

public partial class CMSModules_DeviceProfile_Pages_SelectLayout : CMSDeviceProfilesModalPage
{

    #region "Variables"

    private LayoutInfo mSourceLayout = null;
    private LayoutInfo mTargetLayout = null;

    #endregion


    #region "Properties"

    protected LayoutInfo SourceLayout
    {
        get
        {
            if (mSourceLayout == null)
            {
                int sourceLayoutId = ValidationHelper.GetInteger(URLHelper.GetQueryValue(URLHelper.CurrentURL, "sourceLayoutId"), 0);
                mSourceLayout = LayoutInfoProvider.GetLayoutInfo(sourceLayoutId);
            }
            return mSourceLayout;
        }
    }


    protected LayoutInfo TargetLayout
    {
        get
        {
            if (mTargetLayout == null)
            {
                int targetLayoutId = (int)(ViewState["TargetLayoutId"] ?? 0);
                mTargetLayout = LayoutInfoProvider.GetLayoutInfo(targetLayoutId);
                if (mTargetLayout == null)
                {
                    targetLayoutId = ValidationHelper.GetInteger(URLHelper.GetQueryValue(URLHelper.CurrentURL, "targetLayoutId"), 0);
                    mTargetLayout = LayoutInfoProvider.GetLayoutInfo(targetLayoutId);
                }
            }
            return mTargetLayout;
        }
        set
        {
            mTargetLayout = value;
            ViewState["TargetLayoutId"] = mTargetLayout.LayoutId;
        }
    }


    protected bool DisplayOnlyCompatibleLayouts
    {
        get
        {
            return RequestHelper.IsPostBack() ? ValidationHelper.GetBoolean(Request.Form["chkDisplayOnlyCompatibleLayouts"], false) : true;
        }
    }


    protected bool StopProcessing { get; set; }

    #endregion


    #region "Life-cycle methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ExecuteAction(Initialize);
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ExecuteAction(EnsureTargetLayoutDescription);
    }


    private string LayoutSelector_OnItemSelected(string value)
    {
        int targetLayoutId = ValidationHelper.GetInteger(value, 0);
        TargetLayout = LayoutInfoProvider.GetLayoutInfo(targetLayoutId);

        return ExecuteFunction(() => GetTargetLayoutDescription());
    }

    #endregion


    #region "Private methods"

    private void Initialize()
    {
        // Check whether the current user is allowed to use this dialog
        if (!CMSContext.CurrentUser.IsGlobalAdministrator)
        {
            StopProcessing = true;
            return;
        }

        // Validate parameters
        QueryHelper.ValidateHash("hash");

        CurrentMaster.Title.TitleText = HTMLHelper.HTMLEncode(String.Format(GetString("device_profile.layoutmapping.dialogtitle"), ResHelper.LocalizeString(SourceLayout.LayoutDisplayName)));
        CurrentMaster.Title.TitleImage = GetImageUrl("Objects/CMS_Layout/object.png");
        InitializeSelector();
    }


    private void InitializeSelector()
    {
        LayoutSelector.QueryName = "cms.layout.selectall";
        LayoutSelector.ValueColumn = "LayoutID";
        LayoutSelector.SearchLabelResourceString = "layouts.layoutname";
        LayoutSelector.SearchColumn = "LayoutDisplayName";
        LayoutSelector.SelectedColumns = "LayoutCodeName, LayoutThumbnailGUID, LayoutDisplayName, LayoutID";
        LayoutSelector.OrderBy = "LayoutDisplayName";
        LayoutSelector.NotAvailableImageUrl = GetImageUrl("Objects/CMS_Layout/notavailable.png");
        LayoutSelector.NoRecordsMessage = "layouts.norecordsincategory";
        LayoutSelector.NoRecordsSearchMessage = "layouts.norecords";
        if (DisplayOnlyCompatibleLayouts && (SourceLayout.LayoutZoneCount > 0))
        {
            LayoutSelector.WhereCondition = String.Format("(LayoutZoneCount IS null OR LayoutZoneCount = {0:D})", SourceLayout.LayoutZoneCount);
        }
        LayoutSelector.SelectFunction = "Client_SetTargetLayout";
        LayoutSelector.OnItemSelected += new UniFlatSelector.ItemSelectedEventHandler(LayoutSelector_OnItemSelected);
    }


    private void EnsureTargetLayoutDescription()
    {
        if (String.IsNullOrEmpty(LayoutDescriptionLiteral.Text))
        {
            LayoutDescriptionLiteral.Text = GetTargetLayoutDescription();
        }
    }


    private string GetTargetLayoutDescription()
    {
        StringBuilder response = new StringBuilder();
        if (TargetLayout != null)
        {
            response.AppendFormat("<div class=\"ItemName\">{0}</div>", HTMLHelper.HTMLEncode(ResHelper.LocalizeString(TargetLayout.LayoutDisplayName)));
            if (!String.IsNullOrEmpty(TargetLayout.LayoutDescription))
            {
                response.AppendFormat("<div class=\"Description\">{0}</div>", HTMLHelper.HTMLEncode(ResHelper.LocalizeString(TargetLayout.LayoutDescription)));
            }
            else
            {
                response.AppendFormat("<div class=\"Description DimText\">{0}</div>", HTMLHelper.HTMLEncode(GetString("device_profile.layoutmapping.nodescriptionavailable")));
            }
        }
        else
        {
            response.AppendFormat("<div class=\"ItemName\">{0}</div>", HTMLHelper.HTMLEncode(GetString("device_profile.layoutmapping.targetlayout")));
            response.AppendFormat("<div class=\"Description DimText\">{0}</div>", HTMLHelper.HTMLEncode(GetString("device_profile.layoutmapping.selecttargetlayout")));
        }
        response.AppendFormat("<input type='hidden' id='SourceLayoutId' value='{0:D}' />", SourceLayout.LayoutId);
        response.AppendFormat("<input type='hidden' id='TargetLayoutId' value='{0:D}' />", TargetLayout != null ? TargetLayout.LayoutId : 0);

        return response.ToString();
    }


    private void ExecuteAction(Action action)
    {
        if (!StopProcessing)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                string text = GetString("general.exception");
                if (SettingsKeyProvider.DevelopmentMode)
                {
                    ShowError(text, null, exception.Message ?? exception.ToString());
                }
                else
                {
                    ShowError(text);
                }
                StopProcessing = true;
            }
        }
    }


    private T ExecuteFunction<T>(Func<T> function)
    {
        if (!StopProcessing)
        {
            try
            {
                return function.Invoke();
            }
            catch (Exception exception)
            {
                string text = GetString("general.exception");
                if (SettingsKeyProvider.DevelopmentMode)
                {
                    ShowError(text, null, exception.Message ?? exception.ToString());
                }
                else
                {
                    ShowError(text);
                }
                StopProcessing = true;
            }
        }

        return default(T);
    }

    #endregion

}