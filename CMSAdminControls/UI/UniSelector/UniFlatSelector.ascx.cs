using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using CMS.Controls;
using CMS.ExtendedControls;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.UIControls;

public partial class CMSAdminControls_UI_UniSelector_UniFlatSelector : UniFlatSelector, IPostBackEventHandler, ICallbackEventHandler
{
    #region "Variables"

    // Repeater
    private string mOrderBy = string.Empty;
    private string mWhereCondition = string.Empty;
    private int mSelectTopN = 0;
    private string mQueryName = string.Empty;
    private string mSelectedColumns = string.Empty;

    // Pager 
    private int mPageSize = 10;
    private UniPagerMode mPagingMode = UniPagerMode.Querystring;
    private string mQueryStringKey = "";
    private int mGroupSize = 10;
    private bool mDisplayFirstLastAutomatically = false;
    private bool mDisplayPreviousNextAutomatically = false;
    private bool mHidePagerForSinglePage = false;
    private int mMaxPages = 200;

    // Other
    private string callBackHandlerFunction = string.Empty;
    private string selectItemFunction = string.Empty;

    private bool searchExecuted = false;

    #endregion


    #region "Repeater properties"

    /// <summary>
    /// Gets or sets the order by clause.
    /// </summary>
    public override string OrderBy
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(mOrderBy, repItems.OrderBy), repItems.OrderBy);
        }
        set
        {
            mOrderBy = value;
            repItems.OrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets the where condition.
    /// </summary>
    public override string WhereCondition
    {
        get
        {
            return DataHelper.GetNotEmpty(mWhereCondition, repItems.WhereCondition);
        }
        set
        {
            mWhereCondition = value;
            repItems.WhereCondition = value;
        }
    }


    /// <summary>
    /// Gets or sets the number which indicates how many documents should be displayed.
    /// </summary>
    public override int SelectTopN
    {
        get
        {
            return ValidationHelper.GetInteger(mSelectTopN, repItems.TopN);
        }
        set
        {
            repItems.TopN = value;
            mSelectTopN = value;
        }
    }


    /// <summary>
    /// Gets or sets the query name.
    /// </summary>
    public override string QueryName
    {
        get
        {
            return DataHelper.GetNotEmpty(mQueryName, repItems.QueryName);
        }
        set
        {
            mQueryName = value;
            repItems.QueryName = value;
        }
    }


    /// <summary>
    /// Gets or sets the selected columns.
    /// </summary>
    public override string SelectedColumns
    {
        get
        {
            return DataHelper.GetNotEmpty(mSelectedColumns, repItems.SelectedColumns);
        }
        set
        {
            mSelectedColumns = value;
            repItems.SelectedColumns = value;
        }
    }

    #endregion


    #region "Unipager properties"

    /// <summary>
    /// Gets or sets page size.
    /// </summary>
    public override int PageSize
    {
        get
        {
            return ValidationHelper.GetInteger(mPageSize, pgrItems.PageSize);
        }
        set
        {
            mPageSize = value;
            pgrItems.PageSize = value;
        }
    }


    /// <summary>
    /// Gets or sets search option.
    /// </summary>
    public override UniPagerMode PagingMode
    {
        get
        {
            return mPagingMode;
        }
        set
        {
            mPagingMode = value;
            pgrItems.PagerMode = value;
        }
    }


    /// <summary>
    /// Gets or sets query string key.
    /// </summary>
    public override string QueryStringKey
    {
        get
        {
            return mQueryStringKey;
        }
        set
        {
            mQueryStringKey = value;
            pgrItems.QueryStringKey = value;
        }
    }


    /// <summary>
    /// Gets or sets group size.
    /// </summary>
    public override int GroupSize
    {
        get
        {
            return mGroupSize;
        }
        set
        {
            mGroupSize = value;
            pgrItems.GroupSize = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether first and last item template are displayed dynamically based on current view.
    /// </summary>
    public override bool DisplayFirstLastAutomatically
    {
        get
        {
            return mDisplayFirstLastAutomatically;
        }
        set
        {
            mDisplayFirstLastAutomatically = value;
            pgrItems.DisplayFirstLastAutomatically = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether first and last item template are displayed dynamically based on current view.
    /// </summary>
    public override bool DisplayPreviousNextAutomatically
    {
        get
        {
            return mDisplayPreviousNextAutomatically;
        }
        set
        {
            mDisplayPreviousNextAutomatically = value;
            pgrItems.DisplayPreviousNextAutomatically = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether pager should be hidden for single page.
    /// </summary>
    public override bool HidePagerForSinglePage
    {
        get
        {
            return mHidePagerForSinglePage;
        }
        set
        {
            mHidePagerForSinglePage = value;
            pgrItems.HidePagerForSinglePage = value;
        }
    }


    /// <summary>
    /// Gets or sets the pager max pages.
    /// </summary>
    public override int MaxPages
    {
        get
        {
            return mMaxPages;
        }
        set
        {
            mMaxPages = value;
            pgrItems.MaxPages = value;
        }
    }

    #endregion


    #region "Other properties"

    /// <summary>
    /// Gets or sets the value that indicates whether first item should be selected
    /// </summary>
    public bool PreSelectFirstItem
    {
        get;
        set;
    }


    /// <summary>
    /// Gets the additional placeholder
    /// </summary>
    public PlaceHolder AdditionalPlaceHolder
    {
        get
        {
            return plcAdditional;
        }
    }


    /// <summary>
    /// Gets or sets selected item.
    /// </summary>
    public override string SelectedItem
    {
        get
        {
            return hdnSelectedItem.Value;
        }
        set
        {
            hdnSelectedItem.Value = value;
        }
    }


    /// <summary>
    /// Gets the client ID of hidden field with selected item value.
    /// </summary>
    public string SelectedItemFieldId
    {
        get
        {
            return hdnSelectedItem.ClientID;
        }
    }


    /// <summary>
    /// Gets or sets error text.
    /// </summary>
    public string ErrorText
    {
        get
        {
            return lblError.Text;
        }

        set
        {
            lblError.Text = value;
        }
    }


    /// <summary>
    /// Gets the search check box. The check box behavior is to be defined in the upper control.
    /// </summary>
    public CheckBox SearchCheckBox
    {
        get
        {
            return chkSearch;
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Page load.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event argumets</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (StopProcessing)
        {
            return;
        }

        // Set default selected value to javascript-side variable
        if (!RequestHelper.IsPostBack())
        {
            ScriptHelper.RegisterStartupScript(this, typeof(string), "FirstSelectorValue", ScriptHelper.GetScript("selectedValue = '" + SelectedItem + "'; selectedSkipDialog = false; selectedFlatItem = $j('.FlatSelectedItem'); selectedItemName = selectedFlatItem.find('.SelectorFlatText').text().trim();"));
        }

        ScriptHelper.RegisterJQuery(Page);
        ScriptHelper.RegisterFlatResize(Page);

        // Ensure that the number of the displayed items has always a maximum set
        int itemsCount = ValidationHelper.GetInteger(hdnItemsCount.Value, 0);
        if (itemsCount < 1)
        {
            itemsCount = 1;
        }

        // Set the page size. This value is set by a javascript function according the window size.
        PageSize = itemsCount;


        // Generate unique name for generic select function
        selectItemFunction = ClientID + "_SelectItem";
        callBackHandlerFunction = ClientID + "_Handler";

        // Custom function name
        if (!String.IsNullOrEmpty(JavascriptFunction))
        {
            selectItemFunction = JavascriptFunction;
        }

        ScriptHelper.RegisterStartupScript(this, typeof(string), "SelfValue", ScriptHelper.GetScript("function UniFlat_GetSelectedValue(){ objItem = document.getElementById('" + hdnSelectedItem.ClientID + "'); if (objItem != null) { return objItem.value; } return ''; }"));

        repItems.ItemTemplate = ItemTemplate;
        repItems.AlternatingItemTemplate = AlternatingItemTemplate;
        repItems.FooterTemplate = FooterTemplate;
        repItems.HeaderTemplate = HeaderTemplate;
        repItems.SeparatorTemplate = SeparatorTemplate;

        // Disable caching (cannot guarantee valid cache item)
        repItems.CacheMinutes = 0;

        lblSearch.Text = GetString(SearchLabelResourceString);

        // Create and register delayed search
        string delayedSearchScript =
            @"var timer = null ;             

            function SetupSearch()
            {                         
                var txtSearch = $j('#" + txtSearch.ClientID + @"');
                if (txtSearch) {
                    txtSearch.keyup(StartTimeout);
                }
            }
          
            function StartTimeout()
            {                                
                clearTimeout(timer);                
                timer = setTimeout('OnTimeout()', 700);                
            }

            function OnTimeout()
            {                                         
                " + ControlsHelper.GetPostBackEventReference(btnUpdate, ClientID + "_search") + @"                        
            }  
            
            SetupSearch();
            ";

        ScriptHelper.RegisterStartupScript(this, typeof(string), "DelayedSearch", ScriptHelper.GetScript(delayedSearchScript));
        btnUpdate.Click += new EventHandler(btnUpdate_Click);

        // Create and register select item script
        if (String.IsNullOrEmpty(JavascriptFunction))
        {
            string selectItemScript = (!String.IsNullOrEmpty(CustomSelectItemFunction)) ? CustomSelectItemFunction :
                                                                                                                       "function " + selectItemFunction + @"(value, sender)
            {           
                // Remove old selection    
                selectedFlatItem = $j('div .FlatSelectedItem');  
                if (selectedFlatItem != null)
                {
                    selectedFlatItem.removeClass('FlatSelectedItem');
                    selectedFlatItem.addClass('FlatItem');
                }
                
                // Add new selection
                selectedFlatItem = $j(sender);
                selectedFlatItem.removeClass('FlatItem');
                selectedFlatItem.addClass('FlatSelectedItem');
                
                selectedValue = value;
                selectedSkipDialog = (sender.getAttribute('data-skipdialog') == 1);
                selectedItemName = selectedFlatItem.find('div .SelectorFlatText').text().trim();
                $j('#" + SelectedItemFieldId + "').val(value);\n" +
                                                                                                                       "var callBackArgument = value;\n"
                                                                                                                       + Page.ClientScript.GetCallbackEventReference(this, "callBackArgument", callBackHandlerFunction, callBackHandlerFunction) + "}";

            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), selectItemFunction, ScriptHelper.GetScript(selectItemScript));
        }

        // Create and register callback handler script 
        string callBackHandlerScript = (!String.IsNullOrEmpty(CustomCallBackHandlerFunction)) ? CustomCallBackHandlerFunction :
                                                                                                                                  "function " + callBackHandlerFunction + @"(content, context) 
            {   
                var pnlDescription = $j('div .SelectorFlatDescription'); 
                if (pnlDescription != null) {
                    pnlDescription.html(content);
                }
            } 
            ";

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), callBackHandlerFunction, ScriptHelper.GetScript(callBackHandlerScript));

        // If select function is defined, hook double click on it
        if (!String.IsNullOrEmpty(SelectFunction))
        {
            // Create and register script for double click
            string doubleclickScript = (!String.IsNullOrEmpty(CustomDoubleClickFunction)) ? CustomDoubleClickFunction :
                                                                                                                          @"// Set selected item in tree" +
                                                                                                                          "\n$j(\"#" + pnlRepeater.ClientID + " div[id^='fi_']\").each(function() {" +
                                                                                                                          @"var jThis = $j(this);
                jThis.bind('dblclick', function(){ "
                                                                                                                          + SelectFunction + @"(jThis.attr('id').substring(3), this.getAttribute('data-skipdialog')==1) ;    
                });
            });            
            ";

            ScriptHelper.RegisterStartupScript(pnlUpdate, typeof(string), "DoubleClick", ScriptHelper.GetScript(doubleclickScript));
        }

        if (!RequestHelper.IsPostBack())
        {
            // Add a reload script to the page which will update the page size (items count) according to the window size.
            RegisterRefreshPageSizeScript(false);
        }
    }


    /// <summary>
    /// Add a reload script to the page which will update the page size (items count) according to the window size.
    /// </summary>
    /// <param name="forceResize">Indicates whether to invoke resizing of the page before calculating the items count</param>
    public void RegisterRefreshPageSizeScript(bool forceResize)
    {
        // This method needs the "FlatResize.js" script to be registered
        // Add a reload script to the page which will update the page size (items count) according to the window size
        StringBuilder script = new StringBuilder();
        script.Append("$j(function() {");

        // Invoke resizing of the page before calculating the items count
        if (forceResize)
        {
            script.Append(@"
                // Get items
                getItems(true);
                // Initial resize
                resizeareainternal();
                // get the total count of items which can fit into the page
                uniFlatItemsCount = getItemsCount();");
        }

        script.Append(@"
            var hdnItemsCount = document.getElementById('" + hdnItemsCount.ClientID + @"');
            if (hdnItemsCount != null) { hdnItemsCount.value = uniFlatItemsCount; } " +
                      ControlsHelper.GetPostBackEventReference(btnUpdate, string.Empty) + ";" +
                      "})");

        ScriptHelper.RegisterStartupScript(this, typeof(string), "GetItemsCount", ScriptHelper.GetScript(script.ToString()));
    }


    /// <summary>
    /// On PreRender.
    /// </summary>    
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (StopProcessing)
        {
            return;
        }
        else if (!string.IsNullOrEmpty(ErrorText))
        {
            pnlLabel.Visible = true;
            lblError.Visible = true;
            return;
        }

        // Search condition
        string searchText = SearchText;
        if (!RequestHelper.IsPostBack())
        {
            searchText = QueryHelper.GetString("searchtext", SearchText);
        }

        txtSearch.Text = searchText;

        // Add search condition
        if (!string.IsNullOrEmpty(searchText))
        {
            // Avoid SQL injection
            searchText = SqlHelperClass.GetSafeQueryString(searchText, false);

            string[] columns = SearchColumn.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string columnsWhere = string.Empty;

            foreach (string column in columns)
            {
                columnsWhere = SqlHelperClass.AddWhereCondition(columnsWhere, column + " LIKE N'%" + searchText + "%'", "OR");
            }

            repItems.WhereCondition = SqlHelperClass.AddWhereCondition(repItems.WhereCondition, columnsWhere);
        }

        // Handle TOP N for first load
        if (repItems.TopN == 0)
        {
            int currentPage = pgrItems.CurrentPage;

            // Minimum is first group
            int currentGroup = currentPage / pgrItems.GroupSize + ((currentPage % pgrItems.GroupSize > 0) ? 1 : 0);
            int topN = currentGroup * pgrItems.GroupSize * pgrItems.PageSize + pgrItems.PageSize;
            repItems.TopN = topN;
        }


        // Clear selected item value if remembering is disabled (category change, paginng, ...)
        if ((!RememberSelectedItem) && !String.IsNullOrEmpty(SelectedItem))
        {
            SelectedItem = string.Empty;

            // Get updated description
            string description = RaiseOnItemSelected(SelectedItem);

            // Clear client side values and update description
            ScriptHelper.RegisterStartupScript(this, typeof(string), "ClearSelectedItem",
                                               ScriptHelper.GetScript(
                                                   "selectedFlatItem = null;" +
                                                   "selectedValue = null;" +
                                                   "selectedItemName = null;" +
                                                   callBackHandlerFunction + "('" + description + "');"
                                                   ));
        }

        // Different no records messages based on search
        if (String.IsNullOrEmpty(SearchText))
        {
            lblNoRecords.Text = GetString(NoRecordsMessage);
        }
        else
        {
            lblNoRecords.Text = GetString(NoRecordsSearchMessage);
        }

        repItems.ReloadData(true);

        // Show no results found
        if (!repItems.HasData())
        {
            pnlLabel.Visible = true;
            lblNoRecords.Visible = true;
        }

        // Set focus on the search text box when required
        if (!searchExecuted && UseStartUpFocus)
        {
            RegisterFocusScript();
        }
    }

    #endregion


    #region "Event handling"

    /// <summary>
    /// Button search click.
    /// </summary>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchText = txtSearch.Text;
        RaiseOnSearch();
    }

    bool firstItem = true;

    /// <summary>
    /// Repeater item databound.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Event argumets</param>
    protected void repItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.DataItem != null)
        {
            DataRow row = ((DataRowView)e.Item.DataItem).Row;

            if (PreSelectFirstItem && firstItem)
            {
                SelectedItem = Convert.ToString(row[ValueColumn]);
                //hdnSelectedItem.EnableViewState = true;
            }
            string value = ValidationHelper.GetString(row[ValueColumn], "");
            string cssClass = ((value == SelectedItem)) ? "FlatSelectedItem" : "FlatItem";

            firstItem = false;

            string skipPropertiesDialogAttribute = string.Empty;

            // Add the attribute indicating that the item (web part, widget...) should be inserted without the property dialog
            if (!string.IsNullOrEmpty(SkipPropertiesDialogColumn) && ValidationHelper.GetBoolean(row[SkipPropertiesDialogColumn], false))
            {
                skipPropertiesDialogAttribute = "data-skipdialog=\"1\"";
            }

            // Add javascript function
            string link = string.Empty;

            link = selectItemFunction + "(" + ScriptHelper.GetString(value) + ", this )";


            // Add postback event reference
            if (UsePostback)
            {
                if (!string.IsNullOrEmpty(link))
                {
                    link += ";";
                }

                link += ControlsHelper.GetPostBackEventReference(this, HTMLHelper.HTMLEncode(value));
            }

            // Add envelope
            e.Item.Controls.AddAt(0, new LiteralControl("<div id=\"fi_" + value + "\" class=\"" + cssClass + "\" onclick=\"" + link + ";\"" + skipPropertiesDialogAttribute + " >"));
            e.Item.Controls.Add(new LiteralControl("</div>"));
        }
    }


    /// <summary>
    /// Updates content update panel.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event argumets</param>
    private void btnUpdate_Click(object sender, EventArgs e)
    {
        SearchText = txtSearch.Text;
        RaiseOnSearch();
        searchExecuted = true;
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Reloads data.
    /// </summary>
    public void ReloadData()
    {
        if (!StopProcessing)
        {
            repItems.ReloadData(true);
        }
    }


    /// <summary>
    /// Returns url to flat item image.
    /// </summary>
    /// <param name="metafileGuid">Meta file guid. If value is empty not available image is used</param>    
    public string GetFlatImageUrl(object metafileGuid)
    {
        if (String.IsNullOrEmpty(ValidationHelper.GetString(metafileGuid, "")))
        {
            return HTMLHelper.HTMLEncode(ResolveUrl(NotAvailableImageUrl));
        }
        else
        {
            return HTMLHelper.HTMLEncode(ResolveUrl("~/CMSPages/GetMetaFile.aspx") + "?maxsidesize=" + ImageMaxSideSize + "&fileguid=" + metafileGuid);
        }
    }


    /// <summary>
    /// Clears searched text.
    /// </summary>
    public override void ClearSearchText()
    {
        SearchText = String.Empty;
        txtSearch.Text = String.Empty;
    }


    /// <summary>
    /// Reset pager to the first page.
    /// </summary>
    public override void ResetPager()
    {
        pgrItems.CurrentPage = 1;
    }


    /// <summary>
    /// Clears search condition and resets pager to first page.
    /// </summary>
    public override void ResetToDefault()
    {
        ClearSearchText();
        ResetPager();
        if (ControlsHelper.IsInUpdatePanel(this))
        {
            ControlsHelper.UpdateCurrentPanel(this);
        }
    }


    private void RegisterFocusScript()
    {
        string txtSearchFocus =
            @"function Focus()
            {                
                var textbox = document.getElementById('" + txtSearch.ClientID + @"') ;            
                if (textbox != null)
                {
                    textbox.focus();                    
                }
            }            
            setTimeout('Focus()', 100);";
        ScriptHelper.RegisterStartupScript(this, typeof(string), "TextboxFocus", ScriptHelper.GetScript(txtSearchFocus));
    }

    #endregion


    #region IPostBackEventHandler Members

    public void RaisePostBackEvent(string eventArgument)
    {
        SelectedItem = eventArgument;
        RaiseOnItemSelected(eventArgument);
    }

    #endregion


    #region ICallbackEventHandler Members

    private string callbackArgument = string.Empty;

    public string GetCallbackResult()
    {
        SelectedItem = callbackArgument;
        return RaiseOnItemSelected(SelectedItem);
    }


    public void RaiseCallbackEvent(string eventArgument)
    {
        callbackArgument = eventArgument;
    }

    #endregion
}