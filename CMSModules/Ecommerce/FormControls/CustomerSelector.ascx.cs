using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.CMSHelper;
using CMS.FormControls;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.UIControls;

public partial class CMSModules_Ecommerce_FormControls_CustomerSelector : FormEngineUserControl
{
    #region "Variables"

    private bool mDisplayOnlyEnabled = true;
    private int mSiteId = -1;
    private string mAdditionalItems = "";
    private bool mDisplayRegisteredCustomers = true;
    private bool mDisplayAnonymousCustomers = true;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the field value.
    /// </summary>
    public override object Value
    {
        get
        {
            return CustomerID;
        }
        set
        {
            CustomerID = ValidationHelper.GetInteger(value, 0);
        }
    }


    /// <summary>
    /// Gets or sets the Customer ID.
    /// </summary>
    public int CustomerID
    {
        get
        {
            return ValidationHelper.GetInteger(uniSelector.Value, 0);
        }
        set
        {
            uniSelector.Value = value;
        }
    }


    /// <summary>
    /// Gets or sets the enabled state of the control.
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return base.Enabled;
        }
        set
        {
            base.Enabled = value;
            if (uniSelector != null)
            {
                uniSelector.Enabled = value;
            }
        }
    }


    /// <summary>
    /// Returns ClientID of the dropdownlist.
    /// </summary>
    public override string ValueElementID
    {
        get
        {
            return uniSelector.TextBoxSelect.ClientID;
        }
    }


    /// <summary>
    /// Indicates if anonymous customers are to be displayed.
    /// </summary>
    public bool DisplayAnonymousCustomers
    {
        get
        {
            return mDisplayAnonymousCustomers;
        }
        set
        {
            mDisplayAnonymousCustomers = value;
        }
    }


    /// <summary>
    /// Indicates if registered customers are to be displayed.
    /// </summary>
    public bool DisplayRegisteredCustomers
    {
        get
        {
            return mDisplayRegisteredCustomers;
        }
        set
        {
            mDisplayRegisteredCustomers = value;
        }
    }


    /// <summary>
    /// Allows to display customers only for specified site id. Use 0 for global customers. Default value is current site id. 
    /// </summary>
    public int SiteID
    {
        get
        {
            // No site id given
            if (mSiteId == -1)
            {
                mSiteId = CMSContext.CurrentSiteID;
            }

            return mSiteId;
        }
        set
        {
            mSiteId = value;
        }
    }


    /// <summary>
    /// Allows to display only enabled items. Default value is true.
    /// </summary>
    public bool DisplayOnlyEnabled
    {
        get
        {
            return mDisplayOnlyEnabled;
        }
        set
        {
            mDisplayOnlyEnabled = value;
        }
    }


    /// <summary>
    /// Id of items which has to be displayed regardless other settings. Use ',' or ';' as separator if more ids required.
    /// </summary>
    public string AdditionalItems
    {
        get
        {
            return mAdditionalItems;
        }
        set
        {
            // Prevent from setting null value
            if (value != null)
            {
                mAdditionalItems = value.Replace(';', ',');
            }
            else
            {
                mAdditionalItems = "";
            }
        }
    }


    /// <summary>
    /// Gets the current UniSelector instance.
    /// </summary>
    public UniSelector UniSelector
    {
        get
        {
            EnsureChildControls();
            return uniSelector;
        }
    }


    /// <summary>
    /// Prefix for the resource strings which will be used for the strings of the selector.
    /// Null value of this property is ignored.
    /// </summary>
    public string ResourcePrefix
    {
        get;
        set;
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            uniSelector.StopProcessing = true;
        }
        else
        {
            InitSelector();
        }
    }


    /// <summary>
    /// Initializes the selector.
    /// </summary>
    public void InitSelector()
    {
        uniSelector.IsLiveSite = IsLiveSite;
        uniSelector.EnabledColumnName = "CustomerEnabled";
        uniSelector.IsLiveSite = IsLiveSite;

        // Set resource prefix if specified
        if (ResourcePrefix != null)
        {
            uniSelector.ResourcePrefix = ResourcePrefix;
        }

        // Initialize selector for on-line marketing
        if (ValidationHelper.GetString(GetValue("mode"), "").ToLowerCSafe() == "onlinemarketing")
        {
            UniSelector.SelectionMode = SelectionModeEnum.MultipleButton;
            UniSelector.OnItemsSelected += new EventHandler(UniSelector_OnItemsSelected);
            UniSelector.ReturnColumnName = "CustomerID";
            UniSelector.ButtonImage = GetImageUrl("Objects/ecommerce_customer/add.png");
            UniSelector.DialogLink.CssClass = "MenuItemEdit";
            UniSelector.DialogButton.CssClass = "LongButton";
            IsLiveSite = false;
            SiteID = ValidationHelper.GetInteger(GetValue("SiteID"), 0);
            UniSelector.ResourcePrefix = "om.customerselector";
        }

        string where = "";
        // Add registered customers
        if (DisplayRegisteredCustomers)
        {
            where = SqlHelperClass.AddWhereCondition(where, "CustomerUserID IN (SELECT UserID FROM CMS_UserSite WHERE SiteID = " + SiteID + ")", "OR");
        }

        // Add anonymous customers
        if (DisplayAnonymousCustomers)
        {
            where = SqlHelperClass.AddWhereCondition(where, "(CustomerSiteID = " + SiteID + ") AND (CustomerUserID IS NULL)", "OR");
        }

        // Filter out only enabled items
        if (DisplayOnlyEnabled)
        {
            where = SqlHelperClass.AddWhereCondition(where, "CustomerEnabled = 1");
        }

        // Add items which have to be on the list
        string additionalList = SqlHelperClass.GetSafeQueryString(AdditionalItems, false);
        if (!string.IsNullOrEmpty(additionalList))
        {
            where = SqlHelperClass.AddWhereCondition(where, "CustomerID IN (" + additionalList + ")", "OR");
        }

        // Selected value must be on the list
        if (CustomerID > 0)
        {
            where = SqlHelperClass.AddWhereCondition(where, "CustomerID = " + CustomerID, "OR");
        }

        uniSelector.WhereCondition = where;
    }


    protected void UniSelector_OnItemsSelected(object sender, EventArgs e)
    {
        SetValue("OnlineMarketingValue", UniSelector.Value);
        if (this != null)
        {
            RaiseOnChanged();
        }
    }
}