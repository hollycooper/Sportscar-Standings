using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using AjaxControlToolkit;

using CMS.CMSHelper;
using CMS.EventLog;
using CMS.ExtendedControls;
using CMS.GlobalHelper;
using CMS.IO;
using CMS.SettingsProvider;
using CMS.SiteProvider;
using CMS.DocumentEngine;
using CMS.UIControls;

/// <summary>
/// General Image editor class which handles displaying content.
/// </summary>
public partial class CMSAdminControls_ImageEditor_BaseImageEditor : CMSUserControl
{
    #region "Variables"

    protected string attUrl = null;
    protected string mMediaUrl = null;
    protected ImageHelper imgHelper = null;
    protected string currentFormat = null;
    protected int widthTrimValue = 0;
    protected int heightTrimValue = 0;
    protected bool userInputTrimOK = false;
    protected bool userInputResizeOK = false;
    protected int percentResizeValue = 0;
    protected int widthResizeValue = 0;
    protected int heightResizeValue = 0;
    protected ImageHelper.ImageTypeEnum imageType = ImageHelper.ImageTypeEnum.None;
    protected bool forceReload = false;
    protected bool loadingFailed = false;
    protected bool mSavingFailed = false;
    protected TreeProvider tree = null;
    protected string mGetNameResult = null;
    protected string mGetTitleResult = null;
    protected string mGetDescriptionResult = null;
    protected bool isPreview = false;
    protected TempFileInfo tempFile = null;
    private string mMainImageURL = null;
    private bool mEnabled = true;

    private bool error = false;

    #endregion


    #region "Properties"

    /// <summary>
    /// Placeholder for messages
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            if (ValidationHelper.GetBoolean(hdnShowProperties.Value, false))
            {
                return plcMessProperties;
            }

            return base.MessagesPlaceHolder;
        }
    }


    /// <summary>
    /// Gets or sets the maximal version count (999 by default).
    /// </summary>
    public int MaxVersionsCount
    {
        get
        {
            if (ViewState["MaxVersionsCount"] == null)
            {
                ViewState["MaxVersionsCount"] = ValidationHelper.GetInteger(SettingsHelper.AppSettings["CMSImageEditorMaxVersionsCount"], 999);
            }
            return ValidationHelper.GetInteger(ViewState["MaxVersionsCount"], 999);
        }
        set
        {
            ViewState["MaxVersionsCount"] = value;
        }
    }


    /// <summary>
    /// Gets or sets the number of oldest version.
    /// </summary>
    public int OldestVersion
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["OldestVersion"], 0);
        }
        set
        {
            ViewState["OldestVersion"] = value;
        }
    }


    /// <summary>
    /// Gets or sets the current version of image being modified.
    /// </summary>
    public int CurrentVersion
    {
        get
        {
            return ValidationHelper.GetInteger(ViewState["CurrentVersion"], 0);
        }
        set
        {
            ViewState["CurrentVersion"] = value;
        }
    }


    /// <summary>
    /// Gets the GUID of the instance of the ImageEditor.
    /// </summary>
    public Guid InstanceGUID
    {
        get
        {
            Guid instanceGuid = ValidationHelper.GetGuid(ViewState["InstanceGUID"], Guid.Empty);
            if (instanceGuid == Guid.Empty)
            {
                instanceGuid = Guid.NewGuid();
                ViewState["InstanceGUID"] = instanceGuid;
            }
            return instanceGuid;
        }
    }


    /// <summary>
    /// Attachment URL.
    /// </summary>
    public string AttUrl
    {
        get
        {
            return attUrl;
        }
        set
        {
            attUrl = value;
        }
    }


    /// <summary>
    /// Media file URL.
    /// </summary>
    public string MediaUrl
    {
        get
        {
            return mMediaUrl;
        }
        set
        {
            mMediaUrl = value;
        }
    }


    /// <summary>
    /// Image type.
    /// </summary>
    public ImageHelper.ImageTypeEnum ImageType
    {
        get
        {
            return imageType;
        }
        set
        {
            imageType = value;
        }
    }


    /// <summary>
    /// Tree of current file.
    /// </summary>
    public TreeProvider Tree
    {
        get
        {
            return tree;
        }
        set
        {
            tree = value;
        }
    }


    /// <summary>
    /// Image helper instance.
    /// </summary>
    public ImageHelper ImgHelper
    {
        get
        {
            return imgHelper;
        }
        set
        {
            imgHelper = value;
        }
    }


    /// <summary>
    /// Indicates if loading failed.
    /// </summary>
    public bool LoadingFailed
    {
        get
        {
            return loadingFailed;
        }
        set
        {
            loadingFailed = value;
        }
    }


    /// <summary>
    /// Indicates if saving failed.
    /// </summary>
    public bool SavingFailed
    {
        get
        {
            return mSavingFailed;
        }
        set
        {
            mSavingFailed = value;
        }
    }


    /// <summary>
    /// Loading failed label.
    /// </summary>
    public LocalizedLabel LblLoadFailed
    {
        get
        {
            return lblLoadFailed;
        }
        set
        {
            lblLoadFailed = value;
        }
    }


    /// <summary>
    /// File name text box.
    /// </summary>
    public CMSTextBox TxtFileName
    {
        get
        {
            return txtFileName;
        }
        set
        {
            txtFileName = value;
        }
    }


    /// <summary>
    /// Extension label.
    /// </summary>
    public LocalizedLabel LblExtensionValue
    {
        get
        {
            return lblExtensionValue;
        }
        set
        {
            lblExtensionValue = value;
        }
    }


    /// <summary>
    /// Extension label.
    /// </summary>
    public LocalizedLabel LblImageSizeValue
    {
        get
        {
            return lblImageSizeValue;
        }
        set
        {
            lblImageSizeValue = value;
        }
    }


    /// <summary>
    /// Width label.
    /// </summary>
    public LocalizedLabel LblWidthValue
    {
        get
        {
            return lblWidthValue;
        }
        set
        {
            lblWidthValue = value;
        }
    }


    /// <summary>
    /// Height label.
    /// </summary>
    public LocalizedLabel LblHeightValue
    {
        get
        {
            return lblHeightValue;
        }
        set
        {
            lblHeightValue = value;
        }
    }

    /// <summary>
    /// Current format of image.
    /// </summary>
    public string CurrentFormat
    {
        get
        {
            return currentFormat;
        }
        set
        {
            currentFormat = value;
        }
    }


    /// <summary>
    /// Script literal.
    /// </summary>
    public Literal LtlScript
    {
        get
        {
            return ltlScript;
        }
        set
        {
            ltlScript = value;
        }
    }


    /// <summary>
    /// Result for renaming file.
    /// </summary>
    public string GetNameResult
    {
        get
        {
            return mGetNameResult;
        }
        set
        {
            mGetNameResult = value;
        }
    }


    /// <summary>
    /// Result of file title.
    /// </summary>
    public string GetTitleResult
    {
        get
        {
            return mGetTitleResult;
        }
        set
        {
            mGetTitleResult = value;
        }
    }


    /// <summary>
    /// Result of file description.
    /// </summary>
    public string GetDescriptionResult
    {
        get
        {
            return mGetDescriptionResult;
        }
        set
        {
            mGetDescriptionResult = value;
        }
    }


    /// <summary>
    /// Indicates if loaded image is thumbnail.
    /// </summary>
    public bool IsPreview
    {
        get
        {
            return isPreview;
        }
        set
        {
            isPreview = value;
        }
    }


    /// <summary>
    /// Main URL used for image if undo/redo functionality is not posible.
    /// </summary>
    public string MainImageURL
    {
        get
        {
            return mMainImageURL;
        }
        set
        {
            mMainImageURL = value;
        }
    }


    /// <summary>
    /// Indicates if control is enabled.
    /// </summary>
    public bool Enabled
    {
        get
        {
            return mEnabled;
        }
        set
        {
            mEnabled = value;
        }
    }

    #endregion


    #region "Private properties"

    /// <summary>
    /// Indicates if versioning is enabled.
    /// </summary>
    public bool VersioningEnabled
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["VersioningEnabled"], true);
        }
        set
        {
            ViewState["VersioningEnabled"] = value;
        }
    }


    /// <summary>
    /// Indicates if versioning is checked.
    /// </summary>
    public bool VersioningChecked
    {
        get
        {
            return ValidationHelper.GetBoolean(ViewState["VersioningChecked"], false);
        }
        set
        {
            ViewState["VersioningChecked"] = value;
        }
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Loads image type from querystring.
    /// </summary>
    public delegate void OnLoadImageType();

    public event OnLoadImageType LoadImageType;


    /// <summary>
    /// Loads image URL.
    /// </summary>
    public delegate void OnLoadImageUrl();

    public event OnLoadImageUrl LoadImageUrl;


    /// <summary>
    /// Initializes common properties used for processing image.
    /// </summary>
    public delegate void OnInitializeProperties();

    public event OnInitializeProperties InitializeProperties;


    /// <summary>
    /// Initialize labels according to current image type.
    /// </summary>
    public delegate void OnInitializeLabels(bool reloadName);

    public event OnInitializeLabels InitializeLabels;


    /// <summary>
    /// Saves modified image data.
    /// </summary>
    /// <param name="name">Image name</param>
    /// <param name="extension">Image extension</param>
    /// <param name="mimetype">Image mimetype</param>
    /// <param name="title">Image title</param>
    /// <param name="description">Image description</param>
    /// <param name="binary">Image binary data</param>
    /// <param name="width">Image width</param>
    /// <param name="height">Image height</param>
    public delegate void OnSaveImage(string name, string extension, string mimetype, string title, string description, byte[] binary, int width, int height);

    public event OnSaveImage SaveImage;


    /// <summary>
    /// Returns image name, title and description according to image type.
    /// </summary>
    /// <returns>Image name, title and description</returns>
    public delegate void OnGetMetaData();

    public event OnGetMetaData GetMetaData;

    #endregion


    #region "Methods"

    /// <summary>
    /// Shows the specified error message, optionally with a tooltip text.
    /// </summary>
    /// <param name="text">Error message text</param>
    /// <param name="description">Additional description</param>
    /// <param name="tooltipText">Tooltip text</param>
    public override void ShowError(string text, string description, string tooltipText)
    {
        error = true;
        base.ShowError(text, description, tooltipText);
    }


    protected override void OnInit(EventArgs e)
    {
        ControlsHelper.EnsureScriptManager(Page);

        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterTrimScript();

        if (LoadImageType != null)
        {
            LoadImageType();
        }

        ltlScript.Text = "";

        if (!URLHelper.IsPostback())
        {
            // Display image if available data
            if (imageType != ImageHelper.ImageTypeEnum.None)
            {
                if (InitializeProperties != null)
                {
                    InitializeProperties();
                }

                if (!loadingFailed)
                {
                    currentFormat = imgHelper.ImageFormatToString();
                }
            }

            // Create first version on first load (original)
            if (ImgHelper != null)
            {
                CreateVersion(ImgHelper.SourceData);
            }
        }
        else
        {
            // Load current version to edit
            tempFile = TempFileInfoProvider.GetTempFileInfo(InstanceGUID, CurrentVersion);
            if (tempFile != null)
            {
                tempFile.Generalized.EnsureBinaryData();
                ImgHelper = new ImageHelper(tempFile.FileBinary);
                currentFormat = imgHelper.ImageFormatToString();
            }

            if (!IsUndoRedoPossible() && (InitializeProperties != null))
            {
                InitializeProperties();

                if (!loadingFailed)
                {
                    currentFormat = imgHelper.ImageFormatToString();
                }
            }
        }

        InitializeStrings(!RequestHelper.IsPostBack());
        InitializeFields();

        if (!URLHelper.IsPostback())
        {
            // Initialize labels depending on image type in parent control
            if (InitializeLabels != null)
            {
                InitializeLabels(true);
            }
        }

        // Show tab 'Properties'
        if (ValidationHelper.GetBoolean(hdnShowProperties.Value, false))
        {
            ScriptHelper.RegisterStartupScript(this, typeof(string), "ShowProperties", ScriptHelper.GetScript("ShowProperties(true, '" + hdnShowProperties.ClientID + "');"));
        }

        if (!IsPreview)
        {
            // Enable or disable meta data editor
            metaDataEditor.Enabled = Enabled;
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Register tooltip script
        ScriptHelper.RegisterTooltip(Page);
        // Register wopener script
        ScriptHelper.RegisterWOpenerScript(Page);
        // Register only numbers script
        ScriptHelper.RegisterOnlyNumbersScript(Page);
        // Register script file
        ScriptHelper.RegisterScriptFile(Page, ResolveUrl("~/CMSAdminControls/ImageEditor/BaseImageEditor.js"));
        // Initialize scripts
        StringBuilder sb = new StringBuilder();
        if (IsUndoEnabled() || IsRedoEnabled())
        {
            // Add close confirmation
            sb.AppendFormat("window.discardChangesConfirmation = {0};\n", ScriptHelper.GetString(GetString("imageeditor.discardchangesconfirmation")));
        }
        sb.AppendLine("window.skipCloseConfirm = false;");
        sb.AppendLine("InitializeEditor();");
        ScriptHelper.RegisterStartupScript(this, GetType(), "initialize", ScriptHelper.GetScript(sb.ToString()));
        // Postback handling script
        ScriptHelper.RegisterOnSubmitStatement(this, typeof(string), "skipConfirm", "window.skipCloseConfirm = true;");

        if (imageType == ImageHelper.ImageTypeEnum.PhysicalFile)
        {
            metaDataEditor.Visible = false;
        }

        if (imageType != ImageHelper.ImageTypeEnum.None)
        {
            if (!loadingFailed)
            {
                InitFileInfo();

                string url = URLHelper.ResolveUrl("~/CMSAdminControls/ImageEditor/ImageEditorInnerPage.aspx?editorguid=" + InstanceGUID + "&versionnumber=" + CurrentVersion);
                if (!IsUndoRedoPossible())
                {
                    if (LoadImageUrl != null)
                    {
                        LoadImageUrl();
                    }

                    if (!String.IsNullOrEmpty(AttUrl))
                    {
                        url = URLHelper.AddParameterToUrl(url, "imgurl", Server.UrlEncode(ResolveUrl(AttUrl)));
                    }
                    else if (!String.IsNullOrEmpty(MediaUrl))
                    {
                        url = URLHelper.AddParameterToUrl(url, "imgurl", Server.UrlEncode(ResolveUrl(MediaUrl)));
                    }
                }

                if (String.IsNullOrEmpty(url))
                {
                    LoadingFailed = true;
                }
                else
                {
                    if (tempFile != null)
                    {
                        url = URLHelper.AddParameterToUrl(url, "imgwidth", tempFile.FileImageWidth.ToString());
                        url = URLHelper.AddParameterToUrl(url, "imgheight", tempFile.FileImageHeight.ToString());
                    }

                    url = URLHelper.AddParameterToUrl(url, "hash", QueryHelper.GetHash(url));

                    // Load image to frame
                    HtmlControl frameImg = (HtmlControl)FindControl("frameImg");
                    frameImg.Attributes["src"] = url;

                    // Load image to this page
                    imgMain.ImageUrl = url;

                    lblActualFormat.Text = currentFormat;

                    PopulateConversionDropdownlist();
                }
            }
        }

        // Toggle actions
        ToogleActions(Enabled & !loadingFailed);

        for (int i = 0; i < ajaxAccordion.Panes.Count; i++)
        {
            if (i == ajaxAccordion.SelectedIndex)
            {
                ajaxAccordion.Panes[i].HeaderCssClass = ajaxAccordion.HeaderSelectedCssClass;
            }
            else
            {
                ajaxAccordion.Panes[i].HeaderCssClass = ajaxAccordion.HeaderCssClass;
            }

            AccordionPane pane = ajaxAccordion.Panes[i];

            if (pane.ID == "pnlAccordion6")
            {
                pane.HeaderContainer.Attributes.Add("onclick", "javascript:ShowProperties(true, '" + hdnShowProperties.ClientID + "');");
            }
            else
            {
                pane.HeaderContainer.Attributes.Add("onclick", "javascript:ShowProperties(false, '" + hdnShowProperties.ClientID + "');");
            }
        }
    }


    /// <summary>
    /// Sets correct image size to the javascript.
    /// </summary>
    private void SetScriptProperties(bool reload)
    {
        string script;

        // Set image size at the moment of Page_Load
        if (!reload)
        {
            script = "var ImageWidth = " + imgHelper.ImageWidth + "; var ImageHeight = " + imgHelper.ImageHeight + ";";
            ScriptHelper.RegisterStartupScript(Page, typeof(string), "CorrectSize", ScriptHelper.GetScript(script));
        }
        // Set new image size at the moment of button click event
        else
        {
            script = "ImageWidth = " + imgHelper.ImageWidth + "; ImageHeight = " + imgHelper.ImageHeight + ";";
            ScriptHelper.RegisterStartupScript(Page, typeof(string), "NewSize", ScriptHelper.GetScript(script));
        }
    }


    /// <summary>
    /// Registeres client script for trim buttons.
    /// </summary>
    private void RegisterResizeScript()
    {
        // Script for locking aspect ratio - width
        string script = "function ChangedWidth() {\n" +
                        "var width = document.getElementById('" + txtResizeWidth.ClientID + "');\n" +
                        "if ((width.value > 0) && document.getElementById('" + chkMaintainRatio.ClientID + "').checked)\n" +
                        "{\n" +
                        "var newValue = Math.round((width.value / ImageWidth) * ImageHeight);\n" +
                        "if (newValue > 9999) newValue = 9999;\n" +
                        "document.getElementById('" + txtResizeHeight.ClientID + "').value = newValue;\n" +
                        "}\n" +
                        "return false;\n" +
                        "}\n";

        ScriptHelper.RegisterClientScriptBlock(base.Page, typeof(string), "ChangedWidth", ScriptHelper.GetScript(script));

        // Script for locking aspect ratio - height
        script = "function ChangedHeight() {\n" +
                 "var height = document.getElementById('" + txtResizeHeight.ClientID + "');\n" +
                 "if ((height.value > 0) && document.getElementById('" + chkMaintainRatio.ClientID + "').checked)\n" +
                 "{\n" +
                 "var newValue = Math.round((height.value / ImageHeight) * ImageWidth);\n" +
                 "if (newValue > 9999) newValue = 9999;\n" +
                 "document.getElementById('" + txtResizeWidth.ClientID + "').value = newValue;\n" +
                 "}\n" +
                 "return false;\n" +
                 "}\n";

        ScriptHelper.RegisterClientScriptBlock(base.Page, typeof(string), "ChangedHeight", ScriptHelper.GetScript(script));

        // Script for locking aspect ratio - height
        script = "function ChangedPercent() {\n" +
                 "var percent = document.getElementById('" + txtResizePercent.ClientID + "');\n" +
                 "if (percent.value > 0)\n" +
                 "{\n" +
                 "var newWidth = Math.round((percent.value * ImageWidth)/100);\n" +
                 "var newHeight = Math.round((percent.value * ImageHeight)/100);\n" +
                 "var newRatio = 1;\n" +
                 "if (newWidth > newHeight)\n" +
                 "{\n" +
                 "if (newWidth > 9999) \n" +
                 "{\n" +
                 "newRatio = (Math.floor((9999/newWidth)*100)/100); newWidth = newRatio * newWidth; newHeight = newRatio * newHeight; percent.value = percent.value * newRatio;\n" +
                 "}\n" +
                 "} else { \n" +
                 "if (newHeight > 9999) \n" +
                 "{\n" +
                 "newRatio = (Math.floor((9999/newHeight)*100)/100); newWidth = newRatio * newWidth; newHeight = newRatio * newHeight; percent.value = percent.value * newRatio;\n" +
                 "}\n" +
                 "}\n" +
                 "document.getElementById('" + txtResizeWidth.ClientID + "').value = newWidth;\n" +
                 "document.getElementById('" + txtResizeHeight.ClientID + "').value = newHeight;\n" +
                 "}\n" +
                 "return false;\n" +
                 "}\n";

        ScriptHelper.RegisterClientScriptBlock(base.Page, typeof(string), "ChangedPercent", ScriptHelper.GetScript(script));
    }


    /// <summary>
    /// Script to disable/enable convert quality for JPG conversions
    /// </summary>
    private void RegisterDropDownScript()
    {
        string script = "function ChangedConvert() {\n" +
                        "var convert = document.getElementById('" + drpConvert.ClientID + "');\n" +
                        "if (convert.value == 'jpg') {document.getElementById('" + txtQuality.ClientID + "').disabled = false;}\n" +
                        "else {document.getElementById('" + txtQuality.ClientID + "').disabled = true;}\n" +
                        "return false;\n" +
                        "}\n";

        ScriptHelper.RegisterClientScriptBlock(base.Page, typeof(string), "ChangedConvert", ScriptHelper.GetScript(script));
    }


    /// <summary>
    /// Registeres client script for trim buttons.
    /// </summary>
    private void RegisterTrimScript()
    {
        ScriptHelper.RegisterJQuery(Page);

        string script =
            "function ApplyTrim() {\n" + ControlsHelper.GetPostBackEventReference(btnCrop, "") + "}\n";

        ScriptHelper.RegisterClientScriptBlock(base.Page, typeof(string), "ApplyTrim", ScriptHelper.GetScript(script));
    }


    /// <summary>
    /// Checks user input when resizing image.
    /// </summary>
    private void CheckResizeFields()
    {
        if (imgHelper != null)
        {
            percentResizeValue = ValidationHelper.GetInteger(txtResizePercent.Text, 0);
            widthResizeValue = ValidationHelper.GetInteger(txtResizeWidth.Text, 0);
            heightResizeValue = ValidationHelper.GetInteger(txtResizeHeight.Text, 0);

            // Check input for percent value
            if (radByPercentage.Checked)
            {
                if ((percentResizeValue <= 0))
                {
                    userInputResizeOK = false;
                    lblValidationFailedResize.Visible = true;
                }
                else
                {
                    // Make sure that width and height are corresponding to percent value
                    txtResizePercentChanged(null, null);
                    userInputResizeOK = true;
                }
            }
            // Check input for absolute values
            else if (radByAbsolute.Checked)
            {
                // If user input is less then 1 then display error message
                if ((widthResizeValue <= 0) || (heightResizeValue <= 0) || (widthResizeValue > 9999) || (heightResizeValue > 9999))
                {
                    userInputResizeOK = false;
                    lblValidationFailedResize.Visible = true;
                }
                else
                {
                    userInputResizeOK = true;
                }
            }
        }
    }


    /// <summary>
    /// Forces auto updating width and height fields when changing percent value of image size.
    /// </summary>
    protected void txtResizePercentChanged(object sender, EventArgs e)
    {
        int percent = ValidationHelper.GetInteger(txtResizePercent.Text, 0);

        // Modify width and height only if percent value is within limits
        if (percent == 0)
        {
            txtResizePercent.Text = "100";
            percent = 100;
        }

        // Compute width and height according to % value
        int convertWidth = widthResizeValue = (int)((Convert.ToDouble(txtResizePercent.Text) / (Double)100) * (Convert.ToDouble(imgHelper.ImageWidth)));
        int convertHeight = heightResizeValue = (int)((Convert.ToDouble(txtResizePercent.Text) / (Double)100) * (Convert.ToDouble(imgHelper.ImageHeight)));

        txtResizeWidth.Text = convertWidth.ToString();
        txtResizeHeight.Text = convertHeight.ToString();
    }


    /// <summary>
    /// Initializes all strings on the page.
    /// </summary>
    protected void InitializeStrings(bool reloadName)
    {
        // Initialize strings that are not dependent on any value
        btnRotate90Left.AlternateText = GetString("img.rotate90left");
        btnRotate90Right.AlternateText = GetString("img.rotate90right");
        btnFlipHorizontal.AlternateText = GetString("img.fliphorizontal");
        btnFlipVertical.AlternateText = GetString("img.flipvertical");
        btnConvert.Text = GetString("general.ok");
        btnGrayscale.AlternateText = GetString("img.grayscale");
        radByPercentage.AutoPostBack = true;
        radByPercentage.ResourceString = "img.resizebypercent";
        radByAbsolute.ResourceString = "img.resizebyabsolute";
        chkMaintainRatio.ResourceString = "img.maintainratio";
        btnRotate90Left.ImageUrl = GetImageUrl("Design/Controls/ImageEditor/rotate90left.png");
        btnRotate90Right.ImageUrl = GetImageUrl("Design/Controls/ImageEditor/rotate90right.png");
        btnFlipHorizontal.ImageUrl = GetImageUrl("Design/Controls/ImageEditor/fliphorizontal.png");
        btnFlipVertical.ImageUrl = GetImageUrl("Design/Controls/ImageEditor/flipvertical.png");
        btnGrayscale.ImageUrl = GetImageUrl("Design/Controls/ImageEditor/grayscale.png");
        btnChangeMetaData.Text = GetString("general.ok");
        lblQualityFailed.Visible = false;
        lblValidationFailedResize.Visible = false;
        lblLoadFailed.Visible = lblLoadFailed.Visible || loadingFailed || SavingFailed;

        // Initialize strings that are depending on loaded image
        if (imgHelper != null)
        {
            if ((!RequestHelper.IsPostBack()) || (forceReload))
            {
                txtResizePercentChanged(null, null);
            }

            imgMain.Width = new Unit(imgHelper.ImageWidth, UnitType.Pixel);
            imgMain.Height = new Unit(imgHelper.ImageHeight, UnitType.Pixel);

            // Register client scripts 
            RegisterResizeScript();
            SetScriptProperties(forceReload);
            RegisterDropDownScript();
            txtResizePercent.Attributes.Add("onchange", "ChangedPercent(); return false;");
            txtResizeWidth.Attributes.Add("onchange", "ChangedWidth(); return false;");
            txtResizeHeight.Attributes.Add("onchange", "ChangedHeight(); return false;");
            chkMaintainRatio.Attributes.Add("onchange", "ChangedWidth(); return false;");
            drpConvert.Attributes.Add("onchange", "ChangedConvert(); return false;");
        }
    }


    /// <summary>
    /// Loads fields that are changed by Postbacks.
    /// </summary>
    protected void InitializeFields()
    {
        // Initialize properties that are dependent on other form values
        if (radByPercentage.Checked)
        {
            txtResizePercent.Enabled = true;
        }
        else
        {
            txtResizePercent.Enabled = false;

            txtResizePercent.Attributes["disabled"] = "disabled";
        }

        if (radByAbsolute.Checked)
        {
            chkMaintainRatio.Enabled = true;
            txtResizeWidth.Enabled = true;
            txtResizeHeight.Enabled = true;
        }
        else
        {
            chkMaintainRatio.Enabled = false;
            txtResizeWidth.Enabled = false;
            txtResizeHeight.Enabled = false;

            chkMaintainRatio.InputAttributes["disabled"] = "disabled";
            txtResizeWidth.Attributes["disabled"] = "disabled";
            txtResizeHeight.Attributes["disabled"] = "disabled";
        }

        if (drpConvert.SelectedValue == "jpg")
        {
            txtQuality.Enabled = true;
        }
        else
        {
            txtQuality.Enabled = false;
        }

        txtCropWidth.Attributes["onKeyDown"] = "return OnlyNumbers(event);";
        txtResizeWidth.Attributes["onKeyDown"] = "return OnlyNumbers(event);";
        txtResizeHeight.Attributes["onKeyDown"] = "return OnlyNumbers(event);";
        txtQuality.Attributes["onKeyDown"] = "return OnlyNumbers(event);";
        txtCropHeight.Attributes["onKeyDown"] = "return OnlyNumbers(event);";
        txtCropX.Attributes["onKeyDown"] = "return OnlyNumbers(event);";
        txtCropY.Attributes["onKeyDown"] = "return OnlyNumbers(event);";

        // Disable for preview image
        if (IsPreview)
        {
            txtFileName.Enabled = false;
            metaDataEditor.Enabled = false;
            btnChangeMetaData.Enabled = false;
        }
    }


    /// <summary>
    /// Populates dropdown list with available image formats.
    /// </summary>
    protected void PopulateConversionDropdownlist()
    {
        string selected = drpConvert.SelectedValue;

        drpConvert.Items.Clear();
        if (currentFormat != null)
        {
            foreach (string item in Enum.GetNames(typeof(ImageHelper.SupportedTypesEnum)))
            {
                // If current image type is not equal with one from enumeration then add it to drop-down list
                // If current image type is jpg then add it to drop-down list so that you can change image quality
                if ((item.CompareToCSafe(currentFormat.ToLowerCSafe()) != 0) || (item.CompareToCSafe("jpg", true) == 0))
                {
                    ListItem lstItem = new ListItem(item);
                    lstItem.Selected = (item == selected);
                    drpConvert.Items.Add(lstItem);
                }
            }
        }
    }


    /// <summary>
    /// Propagates all changed image properties to other fields.
    /// </summary>
    public void PropagateChanges(bool reloadName)
    {
        forceReload = true;
        if (InitializeProperties != null)
        {
            InitializeProperties();
        }
        InitializeStrings(reloadName);
        InitializeFields();
        forceReload = false;
    }


    /// <summary>
    /// Button btnGrayscale event handler.
    /// </summary>
    protected void btnGrayscaleClick(object sender, EventArgs e)
    {
        if (imgHelper != null)
        {
            // Grayscale attachment or metafile
            CreateVersion(imgHelper.GetGrayscaledImageData());
            PropagateChanges(false);
        }
    }


    /// <summary>
    /// Button btnRotate90Left event handler.
    /// </summary>
    protected void btnRotate90LeftClick(object sender, EventArgs e)
    {
        if (imgHelper != null)
        {
            CreateVersion(imgHelper.GetRotatedImageData(ImageHelper.ImageRotationEnum.Rotate270));
            PropagateChanges(false);
        }
    }


    /// <summary>
    /// Button btnRotate90Right  event handler.
    /// </summary>
    protected void btnRotate90RightClick(object sender, EventArgs e)
    {
        if (imgHelper != null)
        {
            CreateVersion(imgHelper.GetRotatedImageData(ImageHelper.ImageRotationEnum.Rotate90));
            PropagateChanges(false);
        }
    }


    /// <summary>
    /// Button FlipHorizontal  event handler.
    /// </summary>
    protected void btnFlipHorizontalClick(object sender, EventArgs e)
    {
        if (imgHelper != null)
        {
            CreateVersion(imgHelper.GetFlippedImageData(ImageHelper.ImageFlipEnum.FlipHorizontal));
            PropagateChanges(false);
        }
    }


    /// <summary>
    /// Button FlipVertical event handler.
    /// </summary>
    protected void btnFlipVerticalClick(object sender, EventArgs e)
    {
        if (imgHelper != null)
        {
            CreateVersion(imgHelper.GetFlippedImageData(ImageHelper.ImageFlipEnum.FlipVertical));
            PropagateChanges(false);
        }
    }


    /// <summary>
    /// Button Convert event handler.
    /// </summary>
    protected void btnConvertClick(object sender, EventArgs e)
    {
        if (imgHelper != null)
        {
            int qualityValue = ValidationHelper.GetInteger(txtQuality.Text, 0);
            if ((qualityValue <= 0) || (qualityValue > 100))
            {
                lblQualityFailed.Visible = true;
                txtQuality.Text = "100";
                txtResizePercentChanged(null, null);
                return;
            }
            else
            {
                lblQualityFailed.Visible = false;
            }

            // Create strings to change image name, title, description and type
            if (GetMetaData != null)
            {
                GetMetaData();
            }
            string name = GetNameResult;
            string extension = "";
            string mimetype = "";

            // Initialize new image name string
            currentFormat = drpConvert.SelectedValue;

            // Initialize rest of the image type string
            extension = "." + currentFormat;
            name += extension;
            mimetype = "image/";

            if (currentFormat == "jpg")
            {
                mimetype += "jpeg";
            }
            else
            {
                mimetype += currentFormat;
            }

            // Convert image
            if (currentFormat == "jpg")
            {
                if (!lblQualityFailed.Visible)
                {
                    CreateVersion(imgHelper.GetConvertedImageData(ImageHelper.StringToImageFormat(currentFormat), qualityValue));
                }
            }
            else
            {
                CreateVersion(imgHelper.GetConvertedImageData(ImageHelper.StringToImageFormat(currentFormat), 100));
            }

            // Update dropdown list with new conversion values
            lblActualFormat.Text = currentFormat;
            PopulateConversionDropdownlist();
            PropagateChanges(false);

            // Reset textbox values
            if (currentFormat == "jpg")
            {
                txtQuality.Enabled = true;
            }
            else
            {
                txtQuality.Enabled = false;
            }
            txtQuality.Text = "100";
        }
    }


    /// <summary>
    /// Resize button event handler.
    /// </summary>
    protected void btnResizeClick(object sender, EventArgs e)
    {
        if (imgHelper != null)
        {
            // Check user input
            CheckResizeFields();
            //  If validation passed then resize image
            if (userInputResizeOK)
            {
                if (SaveImage != null)
                {
                    try
                    {
                        CreateVersion(imgHelper.GetResizedImageData(widthResizeValue, heightResizeValue));
                    }
                    catch (Exception ex)
                    {
                        LblLoadFailed.Visible = true;
                        LblLoadFailed.ResourceString = "img.errors.processing";
                        ScriptHelper.AppendTooltip(LblLoadFailed, ex.Message, "help");
                        EventLogProvider.LogException("Image editor", "RESIZEIMAGE", ex);
                        LoadingFailed = true;
                    }
                }
                PropagateChanges(false);

                txtResizePercentChanged(null, null);
            }
            // Otherwise reset input textboxes
            else
            {
                txtResizePercentChanged(null, null);
            }
        }
    }


    protected void btnCropClick(object sender, EventArgs e)
    {
        int x = ValidationHelper.GetInteger(txtCropX.Text, 0);
        int y = ValidationHelper.GetInteger(txtCropY.Text, 0);
        int w = ValidationHelper.GetInteger(txtCropWidth.Text, 0);
        int h = ValidationHelper.GetInteger(txtCropHeight.Text, 0);

        if ((w > 0) && (h > 0))
        {
            CreateVersion(imgHelper.GetTrimmedImageData(w, h, x, y));
            ClearCropInfo();
            PropagateChanges(false);
        }
        else
        {
            lblCropError.ResourceString = "img.errors.cropsize";
            lblCropError.Visible = true;
        }
    }


    /// <summary>
    /// Change width and height textbox autopostbacks when keeping aspect ratio.
    /// </summary>
    protected void chkMaintainRatioChanged(object sender, EventArgs e)
    {
        if (chkMaintainRatio.Checked)
        {
            txtResizeHeight.AutoPostBack = true;
            txtResizeWidth.AutoPostBack = true;
        }
        else
        {
            txtResizeHeight.AutoPostBack = false;
            txtResizeWidth.AutoPostBack = false;
        }
    }


    /// <summary>
    /// Change meta data (name, title and description).
    /// </summary>
    protected void btnChangeMetaDataClick(object sender, EventArgs e)
    {
        // Check that user input is valid string
        Validator validator = new Validator();
        string fileName = ValidationHelper.GetSafeFileName(txtFileName.Text.Trim());
        string result = validator.IsFileName(fileName, GetString("img.errors.filename")).Result;
        if (string.IsNullOrEmpty(result))
        {
            string newName = URLHelper.GetSafeFileName(fileName, CMSContext.CurrentSiteName);
            result = validator.NotEmpty(newName, GetString("img.errors.filename")).Result;

            // Check that user input is smaller then DB field
            if (newName.Length <= 245)
            {
                string title = metaDataEditor.ObjectTitle;
                string description = metaDataEditor.ObjectDescription;

                CreateVersion(imgHelper.SourceData, newName, title, description);

                if (LoadingFailed)
                {
                    ShowError(GetString(LblLoadFailed.ResourceString));
                }
                else
                {
                    ShowChangesSaved();
                }
            }
            else
            {
                ShowError(GetString("img.errors.filetoolong"));
            }
        }

        if (!string.IsNullOrEmpty(result))
        {
            ShowError(GetString("img.errors.filename"));
        }
    }


    private void ToogleActions(bool enable)
    {
        btnConvert.Enabled = enable;
        btnFlipHorizontal.Enabled = enable;
        btnFlipVertical.Enabled = enable;
        btnGrayscale.Enabled = enable;
        btnResize.Enabled = enable;
        btnRotate90Left.Enabled = enable;
        btnRotate90Right.Enabled = enable;
        btnCrop.Enabled = enable;
        btnCropReset.Enabled = enable;

        lblRotate90Left.Enabled = enable;
        lblRotate90Right.Enabled = enable;
        lblFlipHorizontal.Enabled = enable;
        lblFlipVertical.Enabled = enable;
        lblBtnGrayscale.Enabled = enable;

        txtCropWidth.Enabled = enable;
        txtCropHeight.Enabled = enable;
        txtCropX.Enabled = enable;
        txtCropY.Enabled = enable;
        txtResizePercent.Enabled = enable;
        txtResizeWidth.Enabled = enable;
        txtResizeHeight.Enabled = enable;

        drpConvert.Enabled = enable;
        chkMaintainRatio.Enabled = enable;
        chkCropLock.Enabled = enable;
        radByAbsolute.Enabled = enable;
        radByPercentage.Enabled = enable;

        if (!enable)
        {
            // Clear textboxes if editor is disabled
            txtResizeWidth.Text = String.Empty;
            txtResizeHeight.Text = String.Empty;
            txtResizePercent.Text = String.Empty;
            txtQuality.Text = String.Empty;
            txtCropWidth.Text = String.Empty;
            txtCropHeight.Text = String.Empty;
            txtCropX.Text = String.Empty;
            txtCropY.Text = String.Empty;
        }

        // Skip in case of preview image
        if (!IsPreview)
        {
            btnChangeMetaData.Enabled = enable;
            txtFileName.Enabled = enable;
            metaDataEditor.Enabled = enable;
        }
    }

    #endregion


    #region "Undo redo functionality"

    /// <summary>
    /// Returns true if the files are stored only in DB or user has disk read/write permissions. Otherwise false.
    /// </summary>
    public bool IsUndoRedoPossible()
    {
        if (ViewState["IsUndoRedoPossible"] == null)
        {
            bool storeFilesInFileSystem = TempFileInfoProvider.StoreFilesInFileSystem(CMSContext.CurrentSiteName);
            bool storeFilesInDatabase = TempFileInfoProvider.StoreFilesInDatabase(CMSContext.CurrentSiteName);

            if (storeFilesInDatabase)
            {
                ViewState["IsUndoRedoPossible"] = true;
            }
            else
            {
                if (storeFilesInFileSystem)
                {
                    string dir = TempFileInfoProvider.GetTempFilesFolderPath(TempFileInfoProvider.IMAGE_EDITOR_FOLDER, InstanceGUID);
                    if (DirectoryHelper.CheckPermissions(dir))
                    {
                        ViewState["IsUndoRedoPossible"] = true;
                    }
                    else
                    {
                        ViewState["IsUndoRedoPossible"] = false;
                    }
                }
                else
                {
                    ViewState["IsUndoRedoPossible"] = false;
                }
            }
        }

        return ValidationHelper.GetBoolean(ViewState["IsUndoRedoPossible"], true);
    }


    /// <summary>
    /// Returns true if there is a previous version of the file which is being modified.
    /// </summary>
    public bool IsUndoEnabled()
    {
        return IsUndoRedoPossible() && (CurrentVersion > 1);
    }


    /// <summary>
    /// Returns true if there is a next version of the file which is being modified.
    /// </summary>
    public bool IsRedoEnabled()
    {
        return IsUndoRedoPossible() && (CurrentVersion < OldestVersion);
    }


    /// <summary>
    /// Processes the undo action.
    /// </summary>
    public void ProcessUndo()
    {
        CurrentVersion--;
        tempFile = TempFileInfoProvider.GetTempFileInfo(InstanceGUID, CurrentVersion);
        InitFileInfo();
        PopulateConversionDropdownlist();
        PropagateChanges(false);
    }


    /// <summary>
    /// Processes the redo action.
    /// </summary>
    public void ProcessRedo()
    {
        CurrentVersion++;
        tempFile = TempFileInfoProvider.GetTempFileInfo(InstanceGUID, CurrentVersion);
        InitFileInfo();
        PopulateConversionDropdownlist();
        PropagateChanges(false);
    }


    /// <summary>
    /// Creates new version of image.
    /// </summary>
    /// <param name="data">Image data</param>
    public void CreateVersion(byte[] data)
    {
        string filename = null;
        string title = null;
        string description = null;

        // Get metadata from info object
        if (tempFile == null)
        {
            if (GetMetaData != null)
            {
                GetMetaData();
                filename = GetNameResult;
                title = GetTitleResult;
                description = GetDescriptionResult;
            }
        }
        else
        {
            filename = txtFileName.Text.Trim();
            title = metaDataEditor.ObjectTitle;
            description = metaDataEditor.ObjectDescription;
        }

        CreateVersion(data, filename, title, description);
    }


    /// <summary>
    /// Creates new version of image.
    /// </summary>
    /// <param name="data">Image data</param>
    /// <param name="name">File name</param>
    /// <param name="title">File title</param>
    /// <param name="description">File description</param>
    public void CreateVersion(byte[] data, string name, string title, string description)
    {
        if (IsUndoRedoPossible())
        {
            // If current 
            if ((CurrentVersion) >= MaxVersionsCount)
            {
                LblLoadFailed.Visible = true;
                LblLoadFailed.ResourceString = "img.errors.maxversion";
                LoadingFailed = true;
            }
            else if (ImgHelper != null)
            {
                try
                {
                    // Set the imagehelper to new data
                    ImgHelper = new ImageHelper(data);
                    CurrentVersion++;

                    // Save new file version
                    TempFileInfo tfi = new TempFileInfo();
                    tfi.FileDirectory = "ImageEditor";
                    tfi.FileBinary = data;
                    tfi.FileParentGUID = InstanceGUID;
                    tfi.FileExtension = "." + currentFormat;
                    tfi.FileName = name;
                    tfi.FileTitle = title;
                    tfi.FileDescription = description;
                    tfi.FileNumber = CurrentVersion;
                    tfi.FileMimeType = "image/" + (currentFormat == "jpg" ? "jpeg" : currentFormat);
                    tfi.FileImageWidth = ImgHelper.ImageWidth;
                    tfi.FileImageHeight = ImgHelper.ImageHeight;
                    tfi.FileSize = ImgHelper.SourceData.Length;
                    tfi.FileSiteName = CMSContext.CurrentSiteName;

                    tempFile = tfi;

                    SetScriptProperties(true);

                    // Delete all next temporary files before creating new
                    TempFileInfoProvider.DeleteTempFiles(TempFileInfoProvider.IMAGE_EDITOR_FOLDER, InstanceGUID, CurrentVersion - 1, MaxVersionsCount);

                    // Save new temporary file
                    TempFileInfoProvider.SetTempFileInfo(tfi);

                    // Oldest version is allways current if creating new version
                    OldestVersion = CurrentVersion;
                }
                catch (Exception ex)
                {
                    LblLoadFailed.Visible = true;
                    LblLoadFailed.ResourceString = "img.errors.processing";
                    ScriptHelper.AppendTooltip(LblLoadFailed, ex.Message, "help");
                    EventLogProvider.LogException("Image editor", "LOAD", ex);
                    LoadingFailed = true;
                }
            }
        }
        else
        {
            if (SaveImage != null)
            {
                string extension = "." + currentFormat;
                string mimetype = "image/" + (currentFormat == "jpg" ? "jpeg" : currentFormat);

                ImgHelper.LoadImage(data);

                SaveImage(name, extension, mimetype, title, description, data, ImgHelper.ImageWidth, ImgHelper.ImageHeight);
            }
        }
    }


    /// <summary>
    /// Saves current version of image and discards all other versions.
    /// </summary>
    public void SaveCurrentVersion()
    {
        SaveCurrentVersion(false);
    }


    /// <summary>
    /// Saves current version of image and discards all other versions.
    /// </summary>
    /// <param name="includeExtensionInName">If true file name is returned with extension</param>
    public void SaveCurrentVersion(bool includeExtensionInName)
    {
        if (imgHelper != null)
        {
            TempFileInfo tfi = TempFileInfoProvider.GetTempFileInfo(InstanceGUID, CurrentVersion);
            if (tfi != null)
            {
                if (SaveImage != null)
                {
                    tfi.Generalized.EnsureBinaryData();
                    SaveImage((includeExtensionInName ? tfi.FileName + "." + tfi.FileExtension.TrimStart('.') : tfi.FileName), tfi.FileExtension, tfi.FileMimeType, tfi.FileTitle, tfi.FileDescription, tfi.FileBinary, tfi.FileImageWidth, tfi.FileImageHeight);
                }
            }
        }
        if (!SavingFailed)
        {
            // Delete all versions
            TempFileInfoProvider.DeleteTempFiles(TempFileInfoProvider.IMAGE_EDITOR_FOLDER, InstanceGUID);
        }
    }


    /// <summary>
    /// Initializes file info labels.
    /// </summary>
    private void InitFileInfo()
    {
        if ((tempFile != null) && !error && !lblCropError.Visible)
        {
            currentFormat = tempFile.FileExtension.TrimStart('.');
            LblImageSizeValue.Text = DataHelper.GetSizeString(tempFile.FileSize);
            lblActualFormat.Text = currentFormat;
            LblExtensionValue.Text = tempFile.FileExtension;
            TxtFileName.Text = tempFile.FileName;
            metaDataEditor.ObjectTitle = tempFile.FileTitle;
            metaDataEditor.ObjectDescription = tempFile.FileDescription;
            LblWidthValue.Text = tempFile.FileImageWidth.ToString();
            LblHeightValue.Text = tempFile.FileImageHeight.ToString();
            txtResizeWidth.Text = LblWidthValue.Text;
            txtResizeHeight.Text = LblHeightValue.Text;
            txtResizePercent.Text = "100";
            txtQuality.Text = "100";
            ClearCropInfo();
        }
    }


    /// <summary>
    /// Clears crop information.
    /// </summary>
    private void ClearCropInfo()
    {
        // Reset coordinates and dimension
        txtCropX.Text = "0";
        txtCropY.Text = "0";
        txtCropWidth.Text = "0";
        txtCropHeight.Text = "0";
        chkCropLock.Checked = false;
    }


    /// <summary>
    /// Sets info object to metadata editor.
    /// </summary>
    /// <param name="infoObject">Info object</param>
    public void SetMetaDataInfoObject(GeneralizedInfo infoObject)
    {
        metaDataEditor.InfoObject = infoObject;
    }


    /// <summary>
    /// Sets title and description.
    /// </summary>
    /// <param name="title">Title</param>
    /// <param name="description">Description</param>
    public void SetTitleAndDescription(string title, string description)
    {
        metaDataEditor.ObjectTitle = title;
        metaDataEditor.ObjectDescription = description;
    }

    #endregion
}