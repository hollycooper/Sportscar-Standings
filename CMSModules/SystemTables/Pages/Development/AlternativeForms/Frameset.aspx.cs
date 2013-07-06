using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.GlobalHelper;
using CMS.UIControls;

public partial class CMSModules_SystemTables_Pages_Development_AlternativeForms_Frameset : SiteManagerPage
{
    protected int classId = 0;
    protected int altFormId = 0;
    protected int saved = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        classId = QueryHelper.GetInteger("classid", 0);
        altFormId = QueryHelper.GetInteger("altformid", 0);
        saved = QueryHelper.GetInteger("saved", 0);
    }
}