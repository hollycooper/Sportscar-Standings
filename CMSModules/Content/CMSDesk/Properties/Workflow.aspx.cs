using System;

using CMS.CMSHelper;
using CMS.GlobalHelper;
using CMS.UIControls;
using CMS.WorkflowEngine;
using CMS.ExtendedControls;
using CMS.DocumentEngine;

[RegisterTitle("content.ui.propertiesworkflow")]
public partial class CMSModules_Content_CMSDesk_Properties_Workflow : CMSPropertiesPage
{
    #region "Variables"

    private WorkflowInfo workflow = null;

    #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!CMSContext.CurrentUser.IsAuthorizedPerUIElement("CMS.Content", "Properties.Workflow"))
        {
            RedirectToCMSDeskUIElementAccessDenied("CMS.Content", "Properties.Workflow");
        }

        DocumentManager.OnCheckPermissions += DocumentManager_OnCheckPermissions;


        // Disable confirm changes checking
        DocumentManager.RegisterSaveChangesScript = false;

        // Init node
        workflowElem.Node = Node;

        workflow = DocumentManager.Workflow;
        if (workflow != null)
        {
            menuElem.OnClientStepChanged = ClientScript.GetPostBackEventReference(pnlUp, null);

            // Backward compatibility - Display Archive button for all steps
            menuElem.ForceArchive = workflow.IsBasic;
        }

        // Enable split mode
        EnableSplitMode = true;

        UIContext.PropertyTab = PropertyTabEnum.Workflow;
    }


    protected void DocumentManager_OnCheckPermissions(object sender, SimpleDocumentManagerEventArgs e)
    {
        e.CheckDefault = false;
        e.ErrorMessage = String.Format(GetString("cmsdesk.notauthorizedtoeditdocument"), e.Node.NodeAliasPath);
        e.IsValid = (CurrentUser.IsAuthorizedPerDocument(e.Node, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Allowed) || DocumentManager.WorkflowManager.CanUserManageWorkflow(CurrentUser, Node.NodeSiteName);
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        pnlContainer.Enabled = !DocumentManager.ProcessingAction;
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (workflow != null)
        {
            // Backward compatibility
            if (workflow.WorkflowAutoPublishChanges)
            {
                string message = DocumentManager.GetDocumentInfo(true);
                if (!string.IsNullOrEmpty(message))
                {
                    message += "<br />";
                }
                message += GetString("WorfklowProperties.AutoPublishChanges");
                DocumentManager.DocumentInfo = message;
            }
        }

        // Register the scripts
        if (!DocumentManager.RefreshActionContent)
        {
            ScriptHelper.RegisterProgress(Page);
        }
    }

    #endregion
}