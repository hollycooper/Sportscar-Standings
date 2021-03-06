using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.GlobalHelper;
using CMS.UIControls;

public partial class CMSModules_DocumentTypes_Pages_AlternativeForms_AlternativeForms_Layout : SiteManagerPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CurrentMaster.PanelContent.CssClass = string.Empty;

        layoutElem.FormType = CMSModules_AdminControls_Controls_Class_Layout.FORMTYPE_DOCUMENT;
        layoutElem.ObjectID = QueryHelper.GetInteger("altformid", 0);
        layoutElem.IsAlternative = true;
    }
}