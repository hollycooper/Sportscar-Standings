using System;
using System.Web;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.UIControls;

[RegisterTitle("content.ui.masterpage")]
public partial class CMSModules_Content_CMSDesk_MasterPage_PageEdit : CMSContentPage
{
    #region "Variables"

    private CurrentUserInfo user = null;

    #endregion


    #region "Methods"

    protected override void CreateChildControls()
    {
        // Enable split mode
        EnableSplitMode = true;

        user = CMSContext.CurrentUser;
        if (Node != null)
        {
            CMSContext.EditedObject = Node;
            ucHierarchy.PreviewObjectName = Node.NodeAliasPath;
            ucHierarchy.AddContentParameter(new UILayoutValue("PreviewObject", Node));
            ucHierarchy.DefaultAliasPath = Node.NodeAliasPath;
            ucHierarchy.IgnoreSessionValues = true;
        }

        base.CreateChildControls();
    }

    protected override void OnInit(EventArgs e)
    {
        // Check UIProfile
        if (!user.IsAuthorizedPerUIElement("CMS.Content", "MasterPage"))
        {
            RedirectToCMSDeskUIElementAccessDenied("CMS.Content", "MasterPage");
        }

        // Check "Design" permission
        if (!user.IsAuthorizedPerResource("CMS.Design", "Design"))
        {
            RedirectToAccessDenied("CMS.Design", "Design");
        }

        ucHierarchy.RegisterEnvelopeClientID();
        base.OnInit(e);
    }

    #endregion
}