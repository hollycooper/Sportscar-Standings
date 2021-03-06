﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using CMS.CMSHelper;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.UIControls;

[RegisterTitle("com.ui.productscategories")]
[Security(Resource = "CMS.Ecommerce", UIElements = "Products.Categories")]
public partial class CMSModules_Ecommerce_Pages_Tools_Products_Product_Edit_Categories : CMSProductPropertiesPage
{
    #region "Variables"

    protected bool hasModifyPermission = true;
    protected CurrentUserInfo currentUser;

    #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        // Get current user
        currentUser = CMSContext.CurrentUser;

        // Enable split mode
        EnableSplitMode = true;

        // Mark selected tab
        UIContext.ProductTab = ProductTabEnum.Categories;
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        // UI settings
        categoriesElem.DisplaySavedMessage = false;
        categoriesElem.OnAfterSave += categoriesElem_OnAfterSave;
        categoriesElem.UniSelector.OnSelectionChanged += categoriesElem_OnSelectionChanged;

        if (Node != null)
        {
            // Check read permissions
            if (currentUser.IsAuthorizedPerDocument(Node, NodePermissionsEnum.Read) == AuthorizationResultEnum.Denied)
            {
                RedirectToAccessDenied(String.Format(GetString("cmsdesk.notauthorizedtoreaddocument"), Node.NodeAliasPath));
            }

            // Check modify permissions
            else if (currentUser.IsAuthorizedPerDocument(Node, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Denied)
            {
                hasModifyPermission = false;

                // Disable selector
                categoriesElem.Enabled = false;

                DocumentManager.DocumentInfo = String.Format(GetString("cmsdesk.notauthorizedtoeditdocument"), Node.NodeAliasPath);
            }

            // Display all global categories in administration UI
            categoriesElem.UserID = currentUser.UserID;
            categoriesElem.DocumentID = Node.DocumentID;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Display 'The changes were saved' message
        if (QueryHelper.GetBoolean("saved", false))
        {
            ShowChangesSaved();
        }
    }

    #endregion


    #region "Handlers"

    private void categoriesElem_OnAfterSave()
    {
        if (hasModifyPermission)
        {
            // Log the synchronization
            DocumentSynchronizationHelper.LogDocumentChange(Node, TaskTypeEnum.UpdateDocument, Tree);
        }

        ShowChangesSaved();

        // Refresh frame in split mode
        if (CMSContext.DisplaySplitMode && (CultureHelper.GetOriginalPreferredCulture() != Node.DocumentCulture))
        {
            AddScript("SplitModeRefreshFrame();");
        }
    }


    private void categoriesElem_OnSelectionChanged(object sender, EventArgs e)
    {
        if (hasModifyPermission)
        {
            categoriesElem.Save();
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Adds the script to the output request window.
    /// </summary>
    /// <param name="script">Script to add</param>
    public override void AddScript(string script)
    {
        ltlScript.Text += ScriptHelper.GetScript(script);
    }

    #endregion
}
