using System;
using System.Web.UI.WebControls;

using CMS.CMSHelper;
using CMS.FormControls;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.ExtendedControls;
using CMS.PortalEngine;

/// <summary>
/// This form control is used in layout UI to edit page layouts code.
/// </summary>
public partial class CMSFormControls_Layouts_PageLayoutCode : FormEngineUserControl
{
    #region "Properties"

    /// <summary>
    /// Gets or sets Page layout code.
    /// </summary>
    public override object Value
    {
        get
        {
            return tbLayoutCode.Text;
        }
        set
        {
            tbLayoutCode.Text = ValidationHelper.GetString(value, "");
        }
    }


    /// <summary>
    /// Name of the column with layout code.
    /// </summary>
    public string CodeColumn
    {
        get
        {
            return ValidationHelper.GetString(GetValue("CodeColumn"), "");
        }
        set
        {
            SetValue("CodeColumn", value);
        }
    }


    /// <summary>
    /// Name of the column with layout type.
    /// </summary>
    public string TypeColumn
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TypeColumn"), "");
        }
        set
        {
            SetValue("TypeColumn", value);
        }
    }


    /// <summary>
    /// Returns ExtendedArea object for code editing.
    /// </summary>
    public ExtendedTextArea Editor
    {
        get
        {
            return tbLayoutCode.Editor;
        }
    }


    /// <summary>
    /// Enables or disables the control
    /// </summary>
    public override bool Enabled
    {
        get
        {
            return drpType.Enabled;
        }
        set
        {
            drpElements.Enabled = value;
            btn.Enabled = value;
            drpType.Enabled = value;
            tbLayoutCode.ReadOnly = !value;
        }
    }


    /// <summary>
    /// Determines whether the code is in the fullscreen mode.
    /// </summary>
    public bool FullscreenMode
    {
        get;
        set;
    }

    #endregion


    #region "Page events

    public override object[,] GetOtherValues()
    {
        object[,] values = new object[2, 2];
        values[0, 0] = CodeColumn;
        values[0, 1] = tbLayoutCode.Text;
        values[1, 0] = TypeColumn;
        values[1, 1] = (drpType.SelectedValue == null ? TransformationTypeEnum.Ascx.ToString() : drpType.SelectedValue.ToLowerCSafe());
        return values;
    }


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        string lang = DataHelper.GetNotEmpty(SettingsHelper.AppSettings["CMSProgrammingLanguage"], "C#");
        ltlDirectives.Text = "&lt;%@ Control Language=\"" + lang + "\" ClassName=\"Simple\" Inherits=\"CMS.PortalControls.CMSAbstractLayout\" %&gt;<br />&lt;%@ Register Assembly=\"CMS.PortalControls\" Namespace=\"CMS.PortalControls\" TagPrefix=\"cms\" %&gt;";

        if (drpType.Items.Count == 0)
        {
            drpType.Items.Add(new ListItem(GetString("TransformationType.Ascx"), TransformationTypeEnum.Ascx.ToString()));
            drpType.Items.Add(new ListItem(GetString("TransformationType.Html"), TransformationTypeEnum.Html.ToString()));
        }
    }


    /// <summary>
    /// Ensures insert zone element items
    /// </summary>
    protected void InitZoneElements()
    {

        drpElements.Items.Clear();
        // Ascx
        if (drpType.SelectedIndex == 0)
        {
            drpElements.Items.Add(new ListItem(GetString("PageLayout.ConditionalElement"), "cl"));
            drpElements.Items.Add(new ListItem(GetString("PageLayout.DeviceElement"), "dl"));
            drpElements.Items.Add(new ListItem(GetString("PageLayout.ZoneElement"), "wpz"));
            drpElements.SelectedIndex = 2;
        }
        // Html
        else
        {
            drpElements.Items.Add(new ListItem(GetString("PageLayout.ZoneElement"), "wpzhtml"));
        }

        btn.Text = GetString("dialogs.actions.insert");

    }


    protected void InitZoneElementsScript()
    {
        // Insert element script
        string script = @"
function InsertLayoutElement()
{
    var type = document.getElementById('" + drpElements.ClientID + @"').value;
    var cedit = " + tbLayoutCode.Editor.EditorID + @";
    var elem = '<cms:CMSWebPartZone ZoneID=""#"" runat=""server"" />';
    var idDefault = 'ZoneA'; 
    
    switch(type)
    {
        case 'wpzhtml':
            elem = '{^WebPartZone|(id)#^}';
            break;
        
        case 'cl':
            elem = '<cms:CMSConditionalLayout runat=""server"" ID=""#"" ></cms:CMSConditionalLayout>';
            idDefault = 'ConditionLayout';
            break;    

        case 'dl':
            elem = '<cms:CMSDeviceLayout runat=""server"" ID=""#"" VisibleForDeviceProfiles="""" ></cms:CMSDeviceLayout>';
            idDefault = 'DeviceLayout';
            break;    
    }
    
    cedit.replaceSelection(elem.replace('#',idDefault)); 
    cedit.focus();
}
";
        ScriptHelper.RegisterStartupScript(this, typeof(string), "LayoutCodeInsertElement", ScriptHelper.GetScript(script));
        btn.OnClientClick = "InsertLayoutElement();return false;";
    }


    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (!RequestHelper.IsPostBack())
        {
            string type = ValidationHelper.GetString(this.Form.GetFieldValue(TypeColumn), "ascx").ToLowerCSafe();
            if (type == "html")
            {
                drpType.SelectedIndex = 1;
            }
            else
            {
                drpType.SelectedIndex = 0;
            }
            tbLayoutCode.Text = ValidationHelper.GetString(this.Form.GetFieldValue(CodeColumn), "");
        }

        if (FullscreenMode)
        {
            tbLayoutCode.TopOffset = 40;
        }

        InitZoneElements();
    }


    /// <summary>
    /// Display info message
    /// </summary>
    public void ShowMessage()
    {
        string type = ValidationHelper.GetString(this.Form.GetFieldValue(TypeColumn), "ascx").ToLowerCSafe();
        if ((type == "ascx") && !CMSContext.CurrentUser.IsAuthorizedPerResource("cms.design", "editcode"))
        {
            // Display info message for active UI form
            if (Visible)
            {
                ShowWarning(GetString("design.editcodemissing"), null, null);
            }
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        bool isAscx = (drpType.SelectedValue.ToLowerCSafe() == "ascx");

        if (Enabled)
        {
            Enabled = !isAscx || CMSContext.CurrentUser.IsAuthorizedPerResource("cms.design", "editcode");
            drpType.Enabled = true;
        }

        // Setup the information and code type
        if (isAscx)
        {
            tbLayoutCode.Editor.Language = LanguageEnum.ASPNET;
            tbLayoutCode.UseAutoComplete = false;
        }
        else
        {
            tbLayoutCode.Editor.Language = LanguageEnum.HTMLMixed;
            tbLayoutCode.UseAutoComplete = true;
        }

        plcDirectives.Visible = isAscx;

        InitZoneElementsScript();
    }


    protected void drpType_selectedIndexChanged(object sender, EventArgs ea)
    {
        // If not ascx authorized only change to HTML is allowed
        if (!CMSContext.CurrentUser.IsAuthorizedPerResource("cms.design", "editcode"))
        {
            drpType.SelectedIndex = 1;
            return;
        }

        ShowMessage();
    }

    #endregion
}