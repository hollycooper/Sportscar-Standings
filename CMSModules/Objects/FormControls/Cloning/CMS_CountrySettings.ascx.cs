using System;
using System.Data;
using System.Collections;

using CMS.FormControls;
using CMS.SettingsProvider;
using CMS.GlobalHelper;
using CMS.SiteProvider;
using CMS.UIControls;

public partial class CMSModules_Objects_FormControls_Cloning_CMS_CountrySettings : CloneSettingsControl
{
    #region "Properties"

    /// <summary>
    /// Gets properties hashtable.
    /// </summary>
    public override Hashtable CustomParameters
    {
        get
        {
            return GetProperties();
        }
    }

    #endregion


    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!RequestHelper.IsPostBack())
        {
            CountryInfo country = (CountryInfo) InfoToClone;
            txtTwoLetterCode.Text = country.CountryTwoLetterCode;
            txtThreeLetterCode.Text = country.CountryThreeLetterCode;
        }
    }


    /// <summary>
    /// Returns true if custom settings are valid against given clone setting.
    /// </summary>
    /// <param name="settings">Clone settings</param>
    public override bool IsValid(CloneSettings settings)
    {
        // Check the uniqueness of 2-letter and 3-letter codes
        string where = "CountryTwoLetterCode = '" + SqlHelperClass.GetSafeQueryString(txtTwoLetterCode.Text) + "' OR CountryThreeLetterCode = '" + SqlHelperClass.GetSafeQueryString(txtThreeLetterCode.Text) + "'";

        DataSet ds = CountryInfoProvider.GetCountries(where, null);
        if (!DataHelper.DataSourceIsEmpty(ds))
        {
            ShowError(GetString("clonning.settings.country.uniquecodes"));
            return false;
        }

        return true;
    }


    /// <summary>
    /// Returns properties hashtable.
    /// </summary>
    private Hashtable GetProperties()
    {
        Hashtable result = new Hashtable();
        result[SiteObjectType.COUNTRY + ".twolettercode"] = txtTwoLetterCode.Text;
        result[SiteObjectType.COUNTRY + ".threelettercode"] = txtThreeLetterCode.Text;
        return result;
    }

    #endregion
}