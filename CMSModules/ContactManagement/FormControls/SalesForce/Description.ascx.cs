﻿using System;

using CMS.CMSHelper;
using CMS.ExtendedControls;
using CMS.FormControls;
using CMS.GlobalHelper;
using CMS.LicenseProvider;
using CMS.OnlineMarketing;
using CMS.SettingsProvider;

/// <summary>
/// Displays the description macro setting, and allows the user to edit it.
/// </summary>
public partial class CMSModules_ContactManagement_FormControls_SalesForce_Description : FormEngineUserControl
{

    #region "Public properties"

    /// <summary>
    /// Gets or sets the description macro.
    /// </summary>
    public override object Value
    {
        get
        {
            return DescriptionMacroEditor.Text;
        }
        set
        {
            DescriptionMacroEditor.Text = value as string;
        }
    }

    #endregion

    #region "Life-cycle methods"

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ContactInfo contact = new ContactInfo();
        ContextResolver resolver = CMSContext.CurrentResolver.CreateContextChild();
        resolver.SetNamedSourceData("Contact", contact);
        DescriptionMacroEditor.Resolver = resolver;
        DescriptionMacroEditor.Editor.Language = LanguageEnum.Text;
    }

    #endregion

}