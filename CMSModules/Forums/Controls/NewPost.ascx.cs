using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;

using CMS.CMSHelper;
using CMS.ExtendedControls;
using CMS.Forums;
using CMS.GlobalHelper;
using CMS.PortalEngine;
using CMS.SettingsProvider;
using CMS.SiteProvider;
using CMS.WebAnalytics;

public partial class CMSModules_Forums_Controls_NewPost : ForumViewer
{
    #region "Private variables"

    private bool mUseHTMLEditor = true;
    private bool mDisplaySubjectForReply = false;
    private bool mRequiresEmail = false;
    private bool mRequiresEmailOnlyForPublic = false;
    private bool mAllowSubscription = true;
    private bool mAllowSignature = false;
    private bool mUseExternalPreview = false;
    private bool? mEnableSubscription = null;

    private const int POST_USERNAME_LENGTH = 200;
    private const int POST_SUBJECT_LENGTH = 450;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Indicates if control is used on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            plcMess.IsLiveSite = value;
            base.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Gets the user nickname if is available or username.
    /// </summary>
    public string UserName
    {
        get
        {
            if (!String.IsNullOrEmpty(CMSContext.CurrentUser.UserNickName.Trim()))
            {
                return CMSContext.CurrentUser.UserNickName.Trim();
            }

            return Functions.GetFormattedUserName(CMSContext.CurrentUser.UserName, IsLiveSite);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the post form uses html editor or text area.
    /// </summary>
    public bool UseHTMLEditor
    {
        get
        {
            return mUseHTMLEditor;
        }
        set
        {
            mUseHTMLEditor = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether subject field is displayed for thread replies.
    /// </summary>
    public bool DisplaySubjectForReply
    {
        get
        {
            return mDisplaySubjectForReply;
        }
        set
        {
            mDisplaySubjectForReply = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the email is required.
    /// </summary>
    public bool RequiresEmail
    {
        get
        {
            return mRequiresEmail;
        }
        set
        {
            mRequiresEmail = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the email is required only for public user.
    /// </summary>
    public bool RequiresEmailOnlyForPublic
    {
        get
        {
            return mRequiresEmailOnlyForPublic;
        }
        set
        {
            mRequiresEmailOnlyForPublic = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether subscription to current post is allowed.
    /// </summary>
    public bool AllowSubscription
    {
        get
        {
            return mAllowSubscription;
        }
        set
        {
            mAllowSubscription = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether the signature for current post is allowed.
    /// </summary>
    public bool AllowSignature
    {
        get
        {
            return mAllowSignature;
        }
        set
        {
            mAllowSignature = value;
        }
    }


    /// <summary>
    /// Gets or sets value that indicates whether preview is handled by external source.
    /// </summary>
    public bool UseExternalPreview
    {
        get
        {
            return mUseExternalPreview;
        }
        set
        {
            mUseExternalPreview = value;
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether subscription is enabled.
    /// </summary>
    public new bool EnableSubscription
    {
        get
        {
            if (!mEnableSubscription.HasValue)
            {
                // Find nearest forum viewer control to get enables subscription value
                ForumViewer fv = (ForumViewer)ControlsHelper.GetParentControl(this, typeof(ForumViewer));
                if (fv != null)
                {
                    mEnableSubscription = fv.EnableSubscription;
                }
                else
                {
                    mEnableSubscription = true;
                }
            }

            return mEnableSubscription.Value;
        }
        set
        {
            mEnableSubscription = value;
        }
    }

    #endregion


    #region "Events"

    /// <summary>
    /// Occurs when a preview is required.
    /// </summary>
    public event EventHandler OnPreview;

    /// <summary>
    /// Occurs when a post is saved and requires moderation.
    /// </summary>
    public event EventHandler OnModerationRequired;

    #endregion


    /// <summary>
    /// OnLoad override.
    /// </summary>
    /// <param name="e">Event args</param>
    protected override void OnLoad(EventArgs e)
    {
        CopyValuesFromParent(this);
        Initialize();
        base.OnLoad(e);
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ForumInfo fi = ForumContext.CurrentForum;
        if (!fi.ForumHTMLEditor)
        {
            // WAI validation
            lblText.AssociatedControlClientID = ucBBEditor.TextArea.ClientID;
        }
    }


    /// <summary>
    /// Sets post text with dependence on current setting, use HTML or text area.
    /// </summary>
    /// <param name="text">Text to set</param>
    private void SetPostText(string text)
    {
        // Set HTML editor or textarea
        if (ForumContext.CurrentForum.ForumHTMLEditor)
        {
            htmlTemplateBody.ResolvedValue = text;
        }
        else
        {
            ucBBEditor.Text = text;
        }
    }


    /// <summary>
    /// Initialize resources, holders, controls etc.
    /// </summary>
    protected void Initialize()
    {
        #region "Captcha"

        if ((ForumContext.CurrentForum.ForumUseCAPTCHA) && (ForumContext.CurrentState != ForumStateEnum.EditPost))
        {
            // Do not generate security every time
            SecurityCode1.AlwaysGenerate = false;
            SecurityCode1.KeepCodeAutomatically = false;
        }
        else
        {
            plcCaptcha.Visible = false;
        }

        #endregion


        #region "Settings of HTML editor"

        // Set HTML editor properties
        htmlTemplateBody.AutoDetectLanguage = false;
        htmlTemplateBody.DefaultLanguage = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        htmlTemplateBody.EditorAreaCSS = "";
        htmlTemplateBody.ToolbarSet = "Forum";
        htmlTemplateBody.DisableObjectResizing = true; // Disable image resizing
        htmlTemplateBody.RemovePlugin("contextmenu"); // Disable context menu
        htmlTemplateBody.IsLiveSite = IsLiveSite;
        htmlTemplateBody.MediaDialogConfig.UseFullURL = true;
        htmlTemplateBody.LinkDialogConfig.UseFullURL = true;

        #endregion


        #region "Resource strings"

        // Resources
        rfvSubject.ErrorMessage = GetString("Forums_WebInterface_ForumNewPost.subjectErrorMsg");
        lblText.Text = GetString("Forums_WebInterface_ForumNewPost.text");
        rfvText.ErrorMessage = GetString("Forums_WebInterface_ForumNewPost.textErrorMsg");
        rfvUserName.ErrorMessage = GetString("Forums_WebInterface_ForumNewPost.usernameErrorMsg");
        btnOk.Text = GetString("general.ok");
        btnCancel.Text = GetString("general.cancel");
        btnPreview.Text = GetString("Forums_WebInterface_ForumNewPost.Preview");
        lblSubscribe.Text = GetString("Forums_WebInterface_ForumNewPost.Subscription");
        lblSignature.Text = GetString("Forums_WebInterface_ForumNewPost.Signature");
        rfvEmail.ErrorMessage = GetString("Forums_WebInterface_ForumNewPost.emailErrorMsg");
        lblCaptcha.Text = GetString("Forums_WebInterface_ForumNewPost.captcha");
        lblAttachFile.Text = GetString("For.NewPost.Attach");
        lblNickName.Text = GetString("Forums_WebInterface_ForumNewPost.NickName");

        // Regular expression to validate email (e-mail is not required)
        rfvEmail.ValidationExpression = @"^([\w0-9_\-\+]+(\.[\w0-9_\-\+]+)*@[\w0-9_-]+(\.[\w0-9_-]+)+)*$";

        // WAI validation
        lblCaptcha.AssociatedControlClientID = SecurityCode1.InputClientID;

        #endregion


        #region "Controls visibility"

        ForumInfo fi = ForumContext.CurrentForum;

        // Hide or display html editor/ text area
        if (fi.ForumHTMLEditor)
        {
            ucBBEditor.Visible = false;
            rfvText.Enabled = false;

            // Define customizable shortcuts
            Hashtable keystrokes = new Hashtable()
                                       {
                                           { "link", "CKEDITOR.CTRL + 76 /*L*/" },
                                           { "bold", "CKEDITOR.CTRL + 66 /*B*/" },
                                           { "italic", "CKEDITOR.CTRL + 73 /*I*/" },
                                           { "underline", "CKEDITOR.CTRL + 85 /*U*/" }
                                       };

            if (!fi.ForumEnableURL)
            {
                htmlTemplateBody.RemoveButton("InsertUrl");
                if (!fi.ForumEnableAdvancedURL)
                {
                    // Remove the keyborad shortcut for the link insertion
                    keystrokes.Remove("link");
                }
            }
            if (!fi.ForumEnableAdvancedURL)
            {
                htmlTemplateBody.RemoveButton("InsertLink");
            }
            if (!fi.ForumEnableImage)
            {
                htmlTemplateBody.RemoveButton("InsertImage");
            }
            if (!fi.ForumEnableAdvancedImage)
            {
                htmlTemplateBody.RemoveButton("InsertImageOrMedia");
            }
            if (!fi.ForumEnableQuote)
            {
                htmlTemplateBody.RemoveButton("InsertQuote");
            }
            if (!fi.ForumEnableFontBold)
            {
                htmlTemplateBody.RemoveButton("Bold");
                keystrokes.Remove("bold");
            }
            if (!fi.ForumEnableFontItalics)
            {
                htmlTemplateBody.RemoveButton("Italic");
                keystrokes.Remove("italic");
            }
            if (!fi.ForumEnableFontUnderline)
            {
                htmlTemplateBody.RemoveButton("Underline");
                keystrokes.Remove("underline");
            }
            if (!fi.ForumEnableFontStrike)
            {
                htmlTemplateBody.RemoveButton("Strike");
            }
            if (!fi.ForumEnableFontColor)
            {
                htmlTemplateBody.RemoveButton("TextColor");
                htmlTemplateBody.RemoveButton("BGColor");
            }

            // Generate keystrokes string for the CK Editor
            StringBuilder sb = new StringBuilder("[ [ CKEDITOR.ALT + 121 /*F10*/, 'toolbarFocus' ], [ CKEDITOR.ALT + 122 /*F11*/, 'elementsPathFocus' ], [ CKEDITOR.CTRL + 90 /*Z*/, 'undo' ], [ CKEDITOR.CTRL + 89 /*Y*/, 'redo' ], [ CKEDITOR.CTRL + CKEDITOR.SHIFT + 90 /*Z*/, 'redo' ], [ CKEDITOR.ALT + ( CKEDITOR.env.ie || CKEDITOR.env.webkit ? 189 : 109 ) /*-*/, 'toolbarCollapse' ], [ CKEDITOR.ALT + 48 /*0*/, 'a11yHelp' ]");
            string format = ", [ {0}, '{1}' ]";

            foreach (DictionaryEntry entry in keystrokes)
            {
                sb.Append(String.Format(format, entry.Value, entry.Key));
            }

            sb.Append("]");
            htmlTemplateBody.Keystrokes = sb.ToString();
        }
        else
        {
            ucBBEditor.IsLiveSite = IsLiveSite;
            ucBBEditor.ShowImage = fi.ForumEnableImage;
            ucBBEditor.ShowQuote = fi.ForumEnableQuote;
            ucBBEditor.ShowURL = fi.ForumEnableURL;
            ucBBEditor.ShowBold = fi.ForumEnableFontBold;
            ucBBEditor.ShowItalic = fi.ForumEnableFontItalics;
            ucBBEditor.ShowUnderline = fi.ForumEnableFontUnderline;
            ucBBEditor.ShowStrike = fi.ForumEnableFontStrike;
            ucBBEditor.ShowColor = fi.ForumEnableFontColor;
            ucBBEditor.ShowCode = fi.ForumEnableCodeSnippet;
            ucBBEditor.ShowAdvancedImage = fi.ForumEnableAdvancedImage;
            ucBBEditor.ShowAdvancedURL = fi.ForumEnableAdvancedURL;
            htmlTemplateBody.Visible = false;
        }

        if ((fi.ForumModerated) && (!ForumContext.UserIsModerator(fi.ForumID, CommunityGroupID)))
        {
            ShowInformation(GetString("forums.requiremoderation"));
        }

        if ((CMSContext.CurrentUser.IsPublic()) || (!CheckPermission("AttachFiles", fi.AllowAttachFiles, fi.ForumGroupID, fi.ForumID)) || (fi.ForumModerated && !ForumContext.UserIsModerator(fi.ForumID, CommunityGroupID)))
        {
            plcAttachFile.Visible = false;
        }

        // If user can choose thread type and this is not reply, show the options
        if ((fi.ForumType == 0) && (ForumContext.CurrentReplyThread == null))
        {
            // Only thread can be set
            if ((ForumContext.CurrentState != ForumStateEnum.EditPost) || (ForumContext.CurrentPost.PostLevel == 0))
            {
                plcThreadType.Visible = true;
            }
        }

        // Hide or display subscription checkbox with dependence 
        // on allow subscription property value and security
        if ((!AllowSubscription) || (!CheckPermission("Subscribe", fi.AllowSubscribe, fi.ForumGroupID, fi.ForumID)))
        {
            SubscribeHolder.Visible = false;
        }

        // Display signature if is allowed
        if (!AllowSignature)
        {
            plcSignature.Visible = false;
        }

        bool newThread = (ForumContext.CurrentReplyThread == null);

        // Display username textbox if is change name allowed or label with user name
        if (fi.ForumAllowChangeName || CMSContext.CurrentUser.IsPublic() || ((ForumContext.CurrentForum != null) && (ForumContext.UserIsModerator(ForumContext.CurrentForum.ForumID, ForumContext.CommunityGroupID))))
        {
            if (!RequestHelper.IsPostBack())
            {
                // Do not show 'public' for unauthenticated user
                if (!CMSContext.CurrentUser.IsPublic())
                {
                    txtUserName.Text = UserName;
                }
            }
            plcNickName.Visible = false;
        }
        else
        {
            if (ForumContext.CurrentMode != ForumMode.Edit)
            {
                lblNickNameValue.Text = HTMLHelper.HTMLEncode(UserName);
            }
            else
            {
                lblNickNameValue.Text = HTMLHelper.HTMLEncode(ForumContext.CurrentPost.PostUserName);
            }
            plcUserName.Visible = false;
        }

        // Prefill user email and reset the security code
        if (!RequestHelper.IsPostBack())
        {
            txtEmail.Text = CMSContext.CurrentUser.Email;
            SecurityCode1.GenerateNew();
        }

        if (ForumContext.CurrentReplyThread != null)
        {
            string replyPrefix = GetString("forums.replyprefix");
            if (!ForumContext.CurrentReplyThread.PostSubject.StartsWithCSafe(replyPrefix))
            {
                txtSubject.Text = replyPrefix + ForumContext.CurrentReplyThread.PostSubject;
                txtSubject.Text = TextHelper.LimitLength(txtSubject.Text, POST_SUBJECT_LENGTH, "");
            }
            else
            {
                txtSubject.Text = ForumContext.CurrentReplyThread.PostSubject;
            }
            txtSubject.Text = txtSubject.Text;


            // New post - check max level for subscribcribtion
            if (ForumContext.CurrentReplyThread.PostLevel >= ForumPostInfoProvider.MaxPostLevel - 1)
            {
                SubscribeHolder.Visible = false;
            }
        }
        // Edit post - check max level for subscribcribtion
        else if ((ForumContext.CurrentPost != null) && (ForumContext.CurrentPost.PostLevel >= ForumPostInfoProvider.MaxPostLevel))
        {
            SubscribeHolder.Visible = false;
        }


        // Hide subscription if not enabled
        if (!EnableSubscription)
        {
            SubscribeHolder.Visible = false;
        }

        #endregion


        #region "Post Data"

        if (!RequestHelper.IsPostBack())
        {
            // Check whether current state is edit
            if (ForumContext.CurrentState == ForumStateEnum.EditPost)
            {
                txtEmail.Text = ForumContext.CurrentPost.PostUserMail;
                txtSignature.Text = ForumContext.CurrentPost.PostUserSignature;
                txtSubject.Text = ForumContext.CurrentPost.PostSubject;
                txtUserName.Text = ForumContext.CurrentPost.PostUserName;

                SetPostText(ForumContext.CurrentPost.PostText);


                radTypeDiscussion.Checked = true;

                if (ForumContext.CurrentPost.PostType == 1)
                {
                    radTypeQuestion.Checked = true;
                }
            }
            else if ((ForumContext.CurrentMode == ForumMode.Quote) && (ForumContext.CurrentReplyThread != null))
            {
                // Indicates whether wysiwyg editor is used
                bool isHtml = ForumContext.CurrentForum.ForumHTMLEditor;
                // Keeps post user name
                string userName = ForumContext.CurrentReplyThread.PostUserName;

                // Encode username for wysiwyg editor
                if (isHtml)
                {
                    userName = HTMLHelper.HTMLEncode(userName);
                }

                SetPostText(DiscussionMacroHelper.GetQuote(userName, ForumContext.CurrentReplyThread.PostText));

                // Set new line after
                if (isHtml)
                {
                    htmlTemplateBody.ResolvedValue += "<br /><br />";
                }
                else
                {
                    ucBBEditor.Text += "\n";
                }
            }
        }

        #endregion
    }


    /// <summary>
    /// OK click hadler.
    /// </summary>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        #region "Security"

        // Check whether forum exists
        if (ForumContext.CurrentForum == null)
        {
            return;
        }

        // Check security
        bool securityCheck = true;
        switch (ForumContext.CurrentState)
        {
            case ForumStateEnum.NewThread:
                securityCheck = IsAvailable(ForumContext.CurrentForum, ForumActionType.NewThread);
                break;

            case ForumStateEnum.ReplyToPost:
                securityCheck = IsAvailable(ForumContext.CurrentForum, ForumActionType.Reply);
                break;

            case ForumStateEnum.EditPost:
                securityCheck = ForumContext.CurrentPost != null && IsAvailable(ForumContext.CurrentPost, ForumActionType.Edit);
                break;
        }

        if (!securityCheck)
        {
            ShowError(GetString("ForumNewPost.PermissionDenied"));
            return;
        }


        #region "Captcha"

        // Check security code if is required
        if ((ForumContext.CurrentForum.ForumUseCAPTCHA) && (!SecurityCode1.IsValid()) && (ForumContext.CurrentState != ForumStateEnum.EditPost))
        {
            ShowError(GetString("ForumNewPost.InvalidCaptcha"));
            return;
        }

        #endregion



        #region "Email field"

        // Create instance of validator
        Validator validator = new Validator();

        // Check whether email is valid
        string result = validator.IsEmail(txtEmail.Text, rfvEmail.ErrorMessage).Result;

        // Check whether email is present with correct format if email is required
        // or when subscribtion to current post is checked
        if ((ForumContext.CurrentForum.ForumRequireEmail || chkSubscribe.Checked) && (!String.IsNullOrEmpty(result)))
        {
            ShowError(result);
            return;
        }

        // Check if email is added if is in correct format
        if ((txtEmail.Text.Trim() != "") && (!String.IsNullOrEmpty(result)))
        {
            ShowError(rfvEmail.ErrorMessage);
            return;
        }

        #endregion


        #region "Subject"

        // Check whether subject is filled 
        if (txtSubject.Text.Trim() == "")
        {
            ShowError(rfvSubject.ErrorMessage);
            return;
        }

        #endregion


        #region "Text"

        validator = new Validator();

        // Check post text in HTML editor or text area
        if (!ForumContext.CurrentForum.ForumHTMLEditor)
        {
            // Check whether post text is added in text area
            if ((result = validator.NotEmpty(DiscussionMacroHelper.RemoveTags(ucBBEditor.Text), rfvText.ErrorMessage).Result) != "")
            {
                ShowError(result);
                return;
            }
        }
        else
        {
            // Check whether post text is added in HTML editor
            if ((result = validator.NotEmpty(htmlTemplateBody.ResolvedValue, rfvText.ErrorMessage).Result) != "")
            {
                ShowError(result);
                return;
            }
        }

        #endregion


        #region "User name"

        // Check whether user name is filled if user name field is visible
        if (ForumContext.CurrentForum.ForumAllowChangeName || CMSContext.CurrentUser.IsPublic() || ((ForumContext.CurrentForum != null) && (ForumContext.UserIsModerator(ForumContext.CurrentForum.ForumID, ForumContext.CommunityGroupID))))
        {
            validator = new Validator();

            if (!String.IsNullOrEmpty(result = validator.NotEmpty(txtUserName.Text, rfvUserName.ErrorMessage).Result))
            {
                ShowError(result);
                return;
            }
        }

        #endregion


        #endregion


        #region "Forum post properties"

        bool newPost = false;

        // Current forum info object
        ForumInfo fi = ForumContext.CurrentForum;

        // Forum post info object
        ForumPostInfo fp = null;

        // Get forum post info with dependence on current state
        if (ForumContext.CurrentState == ForumStateEnum.EditPost)
        {
            // Get existing object
            fp = ForumContext.CurrentPost;
            fp.PostLastEdit = DateTime.Now;
        }
        else
        {
            // Create new forum post info object
            fp = new ForumPostInfo();
            newPost = true;
        }


        #region "Ad-hoc forum"

        if (IsAdHocForum && (ForumContext.CurrentForum.ForumID == 0))
        {
            if (CMSContext.CurrentDocument == null)
            {
                ShowError(GetString("forums.documentdoesnotexist"));
                return;
            }

            fi.ForumGroupID = ForumGroupInfoProvider.GetAdHocGroupInfo(SiteID).GroupID;
            fi.ForumName = "AdHoc-" + Guid.NewGuid();
            fi.ForumDisplayName = TextHelper.LimitLength(CMSContext.CurrentDocument.GetDocumentName(), POST_USERNAME_LENGTH, String.Empty);
            fi.ForumOpen = true;
            fi.ForumModerated = false;
            fi.ForumAccess = 040000;
            fi.ForumThreads = 0;
            fi.ForumPosts = 0;
            fi.ForumLogActivity = LogActivity;
            ForumInfoProvider.SetForumInfo(fi);

            ForumContext.CurrentForum.ForumID = fi.ForumID;
            ForumContext.ForumID = fi.ForumID;
            ForumID = fi.ForumID;
        }

        #endregion


        // Post forum
        fp.PostForumID = ForumContext.CurrentForum.ForumID;
        // Get forum post info with dependence on current state
        if (ForumContext.CurrentState != ForumStateEnum.EditPost)
        {
            // Post time
            fp.PostTime = DateTime.Now;
            // User IP address
            fp.PostInfo.IPAddress = HTTPHelper.UserHostAddress;
            // User agent
            fp.PostInfo.Agent = Request.UserAgent;
            // Post user id
            if (!CMSContext.CurrentUser.IsPublic())
            {
                fp.PostUserID = CMSContext.CurrentUser.UserID;
            }

            // Post signature
            fp.PostUserSignature = txtSignature.Text;
        }

        // Post subject
        fp.PostSubject = txtSubject.Text;
        // Post user email
        fp.PostUserMail = txtEmail.Text;


        // Post type
        int forumType = ForumContext.CurrentForum.ForumType;
        if (forumType == 0)
        {
            if (ForumContext.CurrentReplyThread == null)
            {
                // New thread - use type which user chosen
                fp.PostType = (radTypeDiscussion.Checked ? 0 : 1);
            }
            else
            {
                // Reply - use parent type
                fp.PostType = ForumContext.CurrentReplyThread.PostType;
            }
        }
        else
        {
            // Fixed type - use the forum setting
            fp.PostType = forumType - 1;
        }

        bool newThread = (ForumContext.CurrentReplyThread == null);

        // Set username if change name is allowed
        if (fi.ForumAllowChangeName || CMSContext.CurrentUser.IsPublic() || ForumContext.UserIsModerator(fp.PostForumID, ForumContext.CommunityGroupID))
        {
            fp.PostUserName = TextHelper.LimitLength(txtUserName.Text, POST_USERNAME_LENGTH, "");
        }
        else
        {
            // Get forum post info with dependence on current state
            if (ForumContext.CurrentState != ForumStateEnum.EditPost)
            {
                fp.PostUserName = UserName;
            }
        }

        // Post parent id -> reply to
        if (ForumContext.CurrentReplyThread != null)
        {
            fp.PostParentID = ForumContext.CurrentReplyThread.PostId;

            // Check max relative level
            if ((MaxRelativeLevel > -1) && (ForumContext.CurrentReplyThread.PostLevel >= MaxRelativeLevel))
            {
                ShowError(GetString("Forums.MaxRelativeLevelError"));
                return;
            }
        }

        // Get post text from HTML editor if is enabled
        fp.PostText = ForumContext.CurrentForum.ForumHTMLEditor ? htmlTemplateBody.ResolvedValue : ucBBEditor.Text;

        // Approve post if forum is not moderated
        if (newPost)
        {
            if (!ForumContext.CurrentForum.ForumModerated)
            {
                fp.PostApproved = true;
            }
            else
            {
                if (ForumContext.UserIsModerator(fp.PostForumID, CommunityGroupID))
                {
                    fp.PostApproved = true;
                    fp.PostApprovedByUserID = CMSContext.CurrentUser.UserID;
                }
            }
        }

        // If signature is enabled then
        if (EnableSignature)
        {
            fp.PostUserSignature = CMSContext.CurrentUser.UserSignature;
        }

        #endregion


        if (!BadWordInfoProvider.CanUseBadWords(CMSContext.CurrentUser, CMSContext.CurrentSiteName))
        {
            // Prepare columns to check
            Dictionary<string, int> columns = new Dictionary<string, int>();
            columns.Add("PostText", 0);
            columns.Add("PostSubject", 450);
            columns.Add("PostUserSignature", 0);
            columns.Add("PostUserName", 200);

            // Perform bad words check
            string badMessage = BadWordsHelper.CheckBadWords(fp, columns, "PostApproved", "PostApprovedByUserID", fp.PostText, CMSContext.CurrentUser.UserID, () => { return ValidatePost(fp); });

            if (String.IsNullOrEmpty(badMessage))
            {
                if (!ValidatePost(fp))
                {
                    badMessage = GetString("ForumNewPost.EmptyBadWord");
                }
            }

            if (!String.IsNullOrEmpty(badMessage))
            {
                ShowError(badMessage);
                return;
            }
        }



        // Flood protection
        if (FloodProtectionHelper.CheckFlooding(CMSContext.CurrentSiteName, CMSContext.CurrentUser))
        {
            ShowError(GetString("General.FloodProtection"));
            return;
        }

        // Check banned ip
        if (!BannedIPInfoProvider.IsAllowed(CMSContext.CurrentSiteName, BanControlEnum.AllNonComplete))
        {
            ShowError(GetString("General.BannedIP"));
            return;
        }

        string baseUrl = ForumContext.CurrentForum.ForumBaseUrl;
        if (String.IsNullOrEmpty(baseUrl))
        {
            baseUrl = FriendlyBaseURL;
        }

        string unsubscriptionUrl = ForumContext.CurrentForum.ForumUnsubscriptionUrl;
        if (String.IsNullOrEmpty(unsubscriptionUrl))
        {
            unsubscriptionUrl = UnsubscriptionURL;
        }

        // USe parent post id for new post
        int subscibePostId = newPost ? fp.PostParentID : fp.PostId;

        // Check subscriptions
        if ((chkSubscribe.Checked) && (!String.IsNullOrEmpty(txtEmail.Text)) && (ForumSubscriptionInfoProvider.IsSubscribed(txtEmail.Text.Trim(), fp.PostForumID, subscibePostId)))
        {
            // Post of the forum is already subscribed to this email -> show an error
            chkSubscribe.Checked = false;
            ShowError(GetString("Forums.EmailAlreadySubscribed"));
            return;
        }

        // Save post object
        ForumPostInfoProvider.SetForumPostInfo(fp, baseUrl, unsubscriptionUrl);
        LogPostActivity(fp, fi);


        #region "Subscription"

        // If subscribe is checked create new subscription to the current post
        if ((chkSubscribe.Checked) && (!ForumSubscriptionInfoProvider.IsSubscribed(fp.PostUserMail, fp.PostForumID, fp.PostId)))
        {
            // Create new subscription info object
            ForumSubscriptionInfo fsi = new ForumSubscriptionInfo();
            // Set info properties
            fsi.SubscriptionForumID = fp.PostForumID;
            fsi.SubscriptionEmail = fp.PostUserMail;
            fsi.SubscriptionPostID = fp.PostId;
            fsi.SubscriptionUserID = fp.PostUserID;
            fsi.SubscriptionGUID = Guid.NewGuid();

            // Save subscription
            ForumSubscriptionInfoProvider.Subscribe(fsi, DateTime.Now, true, true);

            if (fsi.SubscriptionApproved)
            {
                LogSubscriptionActivity(fsi, fi);
            }
        }

        #endregion


        // Generate new captcha code
        SecurityCode1.GenerateNew();


        if ((!fp.PostApproved) && (!ForumContext.UserIsModerator(fp.PostForumID, CommunityGroupID)))
        {
            if (OnModerationRequired != null)
            {
                OnModerationRequired(this, null);
            }
        }

        // Keep current user info
        CurrentUserInfo currentUser = CMSContext.CurrentUser;

        if (currentUser.IsAuthenticated() && chkAttachFile.Checked && (currentUser.IsGlobalAdministrator || ForumContext.CurrentForum.AllowAttachFiles != SecurityAccessEnum.Nobody))
        {
            // Redirect to the post attachments
            URLHelper.Redirect(GetURL(fp, ForumActionType.Attachment));
        }
        else
        {
            if (!StopProcessing)
            {
                // Redirect back to the forum or forum thread
                URLHelper.Redirect(ClearURL());
            }
        }
    }


    /// <summary>
    /// Preview button click handler.
    /// </summary>
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        // If external preview is enabled fire OnPreview event
        if (UseExternalPreview)
        {
            #region "Forum properties"

            // Get forum post info with dependence on current state
            ForumPostInfo fp = ForumContext.CurrentState == ForumStateEnum.EditPost ? ForumContext.CurrentPost : new ForumPostInfo();

            // Post forum
            fp.PostForumID = ForumContext.CurrentForum.ForumID;
            // Post time
            fp.PostTime = DateTime.Now;
            // Post subject
            fp.PostSubject = txtSubject.Text;
            // Post user email
            fp.PostUserMail = txtEmail.Text;
            // Post signature
            fp.PostUserSignature = txtSignature.Text;

            // Post user id
            if (!CMSContext.CurrentUser.IsPublic())
            {
                fp.PostUserID = CMSContext.CurrentUser.UserID;
            }

            // Post user name
            if (ForumContext.CurrentForum.ForumAllowChangeName || CMSContext.CurrentUser.IsPublic() || (ForumContext.UserIsModerator(ForumContext.CurrentForum.ForumID, ForumContext.CommunityGroupID)))
            {
                fp.PostUserName = TextHelper.LimitLength(txtUserName.Text, POST_USERNAME_LENGTH, "");
            }
            else
            {
                fp.PostUserName = UserName;
            }

            // Post parent id -> reply to
            if (ForumContext.CurrentReplyThread != null)
            {
                fp.PostParentID = ForumContext.CurrentReplyThread.PostId;
            }

            // Get post text from HTML editor if is enabled
            fp.PostText = ForumContext.CurrentForum.ForumHTMLEditor ? htmlTemplateBody.ResolvedValue : ucBBEditor.Text;

            #endregion


            if (OnPreview != null)
            {
                OnPreview(fp, null);
            }
        }
        else
        {
            pnlReplyPost.Visible = true;
            lblSubjectPreview.Text = txtSubject.Text;

            // Get forum text from HTML editor or text area
            if (ForumContext.CurrentForum.ForumHTMLEditor)
            {
                lblTextPreview.Text = htmlTemplateBody.ResolvedValue;
            }
            else
            {
                lblTextPreview.Text = HTMLHelper.EnsureLineEnding(HTMLHelper.HTMLEncode(ucBBEditor.Text), "<br />");
            }
        }
    }


    /// <summary>
    /// Cancel button click handler.
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        URLHelper.Redirect(ClearURL());
    }


    private static bool ValidatePost(ForumPostInfo fpi)
    {
        if ((fpi.PostSubject == null) || (fpi.PostText == null) || (fpi.PostUserName == null))
        {
            return false;
        }

        return ((fpi.PostSubject.Trim() != "") && (fpi.PostText.Trim() != "") && (fpi.PostUserName.Trim() != ""));
    }


    /// <summary>
    /// Logs "subscription" activity.
    /// </summary>
    /// <param name="fsi">Forum subscription</param>
    /// <param name="fi">Forum object</param>
    private void LogSubscriptionActivity(ForumSubscriptionInfo fsi, ForumInfo fi)
    {
        if ((fi == null) || !fi.ForumLogActivity)
        {
            return;
        }

        Activity activity =new ActivitySubscriptionForumPost(fi, fsi, CMSContext.CurrentDocument, CMSContext.ActivityEnvironmentVariables);
        activity.Log();
    }


    /// <summary>
    /// Logs "post" activity.
    /// </summary>
    /// <param name="fp">Forum post object</param>
    /// <param name="fi">Forum info</param>
    private void LogPostActivity(ForumPostInfo fp, ForumInfo fi)
    {
        Activity forumPost = new ActivityForumPost(fp, fi, CMSContext.CurrentDocument, fi.ForumLogActivity, CMSContext.ActivityEnvironmentVariables);
        forumPost.Log();
    }
}