﻿using System;
using System.Web.UI;

using CMS.ExtendedControls;
using CMS.GlobalHelper;
using CMS.UIControls;

public partial class CMSFormControls_Selectors_SelectFileOrFolder_Content : CMSModalPage
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        CheckGlobalAdministratorOrHash();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptHelper.RegisterJQuery(Page);
        CMSDialogHelper.RegisterDialogHelper(Page);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "InitResizers", "$j(InitResizers());", true);

        fileSystem.InitFromQueryString();
    }
}