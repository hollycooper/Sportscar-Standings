﻿using System;
using System.Text;
using System.Web;
using System.Web.UI;

using CMS.CMSHelper;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.ExtendedControls;
using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.UIControls;

public partial class CMSModules_Content_Controls_CMSDeskTree : CMSUserControl, IPostBackEventHandler
{
    #region "Properties"

    /// <summary>
    /// Requested action.
    /// </summary>
    protected string Action
    {
        get
        {
            return ValidationHelper.GetString(Request.Form["hdnAction"], string.Empty);
        }
    }


    /// <summary>
    /// Action parameter 1.
    /// </summary>
    protected string Param1
    {
        get
        {
            return ValidationHelper.GetString(Request.Form["hdnParam1"], string.Empty);
        }
    }


    /// <summary>
    /// Action parameter 2.
    /// </summary>
    protected string Param2
    {
        get
        {
            return ValidationHelper.GetString(Request.Form["hdnParam2"], string.Empty);
        }
    }


    /// <summary>
    /// Scrollbar position.
    /// </summary>
    protected int ScrollPosition
    {
        get
        {
            return ValidationHelper.GetInteger(Request.Form["hdnScroll"], 0);
        }
    }


    /// <summary>
    /// Node to be expanded.
    /// </summary>
    private int NodeID
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("NodeID"), 0);
        }
        set
        {
            SetValue("NodeID", value);
        }
    }


    /// <summary>
    /// Culture of the tree.
    /// </summary>
    private string Culture
    {
        get
        {
            return ValidationHelper.GetString(GetValue("Culture"), string.Empty);
        }
        set
        {
            SetValue("Culture", value);
        }
    }


    /// <summary>
    /// Node to be expanded.
    /// </summary>
    private int ExpandNodeID
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ExpandNodeID"), 0);
        }
        set
        {
            SetValue("ExpandNodeID", value);
        }
    }


    /// <summary>
    /// Determines whether the tree is used for browsing products.
    /// </summary>
    public bool IsProductTree
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("IsProductTree"), false);
        }
        set
        {
            SetValue("IsProductTree", value);
        }
    }


    /// <summary>
    /// Indicates global objects are allowed on current site besides site-specific ones. Type of the objects depends on settings key name 
    /// set in GlobalObjectsKeyName property.
    /// </summary>
    public bool AllowGlobalObjects
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowGlobalObjects"), false);
        }
        set
        {
            SetValue("AllowGlobalObjects", value);
        }
    }


    /// <summary>
    /// Indicates if products will be displayed in products sections tree. This setting has no effect when not using sections tree.
    /// </summary>
    public bool DisplayProductsInSectionsTree
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayProductsInSectionsTree"), false);
        }
        set
        {
            SetValue("DisplayProductsInSectionsTree", value);
        }
    }


    /// <summary>
    /// Indicates whether it is possible to create products without document data.
    /// </summary>
    public bool AllowProductsWithoutDocuments
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowProductsWithoutDocuments"), false);
        }
        set
        {
            SetValue("AllowProductsWithoutDocuments", value);
        }
    }


    /// <summary>
    /// Determines from which node should tree display nodes.
    /// </summary>
    public string StartingAliasPath
    {
        get
        {
            return ValidationHelper.GetString(GetValue("StartingAliasPath"), CMSContext.CurrentUser.UserStartingAliasPath);
        }
        set
        {
            SetValue("StartingAliasPath", value);
        }
    }


    /// <summary>
    /// Javascript called before selecting a node.
    /// </summary>
    public string BeforeSelectNodeScript
    {
        get
        {
            return ValidationHelper.GetString(GetValue("BeforeSelectNodeScript"), "");
        }
        set
        {
            SetValue("BeforeSelectNodeScript", value);
        }
    }

    #endregion


    #region "Page events"

    protected override void OnInit(EventArgs e)
    {
        if (!Page.IsCallback && RequestHelper.IsPostBack())
        {
            NodeID = ValidationHelper.GetInteger(Param1, 0);
            if (Action.ToLowerCSafe() == "refresh")
            {
                ExpandNodeID = ValidationHelper.GetInteger(Param2, 0);
            }
        }

        // Set up the tree
        if ((NodeID > 0) && !RequestHelper.IsPostBack())
        {
            treeElem.SelectedNodeID = NodeID;
            // Setup the current node script
            AddScript(string.Format("currentNodeId = {0};", NodeID));
        }

        treeElem.ExpandNodeID = ExpandNodeID;
        treeElem.Culture = Culture;
        treeElem.NodeTextTemplate = string.Format("{0}<span class=\"ContentTreeItem\" onclick=\"{2} SelectNode(##NODEID##, this); return false;\">##ICON##<span class=\"Name\">##NODENAME##</span></span>{1}", ContextMenuContainer.GetStartTag("nodeMenu", "##NODEID##", false, false), ContextMenuContainer.GetEndTag(false), BeforeSelectNodeScript);
        treeElem.SelectedNodeTextTemplate = string.Format("{0}<span id=\"treeSelectedNode\" class=\"ContentTreeSelectedItem\" onclick=\"{2} SelectNode(##NODEID##, this); return false;\">##ICON##<span class=\"Name\">##NODENAME##</span></span>{1}", ContextMenuContainer.GetStartTag("nodeMenu", "##NODEID##", false, false), ContextMenuContainer.GetEndTag(false), BeforeSelectNodeScript);
        treeElem.MaxTreeNodeText = string.Format("<span class=\"ContentTreeItem\" onclick=\"Listing(##PARENTNODEID##, this); return false;\"><span class=\"Name\" style=\"font-style: italic;\">{0}</span></span>", GetString("ContentTree.SeeListing"));
        treeElem.SelectPublishedData = false;

        if (IsProductTree)
        {
            menuCont.MenuControlPath = "~/CMSModules/Ecommerce/Controls/UI/ProductSectionsContextMenu.ascx";
            treeElem.RootNodeCreated += treeElem_RootNodeCreated;

            // Filter only product section nodes
            string where = "NodeClassID IN (SELECT ClassID FROM CMS_Class WHERE ClassIsProductSection = 1)";

            // Check if products are to be displayed in tree
            if (DisplayProductsInSectionsTree)
            {
                // Include product documents
                where = SqlHelperClass.AddWhereCondition(where, "(NodeSKUID IS NOT NULL)", "OR");
            }

            treeElem.MapProvider.WhereCondition = SqlHelperClass.AddWhereCondition(treeElem.MapProvider.WhereCondition, where);
        }
        else
        {
            menuCont.MenuControlPath = "~/CMSModules/Content/Controls/TreeContextMenu.ascx";
        }

        if (!string.IsNullOrEmpty(StartingAliasPath))
        {
            treeElem.Path = StartingAliasPath;
        }
        base.OnInit(e);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsCallback)
        {
            // Register the dialog script
            ScriptHelper.RegisterJQuery(Page);
            ScriptHelper.RegisterDialogScript(Page);
            
            string script =
                @"
function InitTreeParams(action, param1, param2)
{
    var elm = jQuery('#" + pnlTreeArea.ClientID + @"');
    jQuery('#hdnScroll').val(elm.scrollTop());
    jQuery('#hdnAction').val(action); 
    jQuery('#hdnParam1').val(param1); 
    jQuery('#hdnParam2').val(param2);
}

function ProcessRequest(action, param1, param2)
{
    InitTreeParams(action, param1, param2);
" + Page.ClientScript.GetPostBackEventReference(this, null) + ";" +
@"}
";

            iconRefresh.OnClientClick = "TreeRefresh();";

            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "processRequestScript", ScriptHelper.GetScript(script));
            AddScript("var currentNode = document.getElementById('treeSelectedNode');");
        }
    }


    protected override void Render(HtmlTextWriter writer)
    {
        // Render control as a div with ID (to ensure IPostBackEventHandler works in UpdatePanel)
        writer.WriteBeginTag("div");
        writer.WriteAttribute("id", ClientID);
        writer.Write(HtmlTextWriter.TagRightChar);
        base.Render(writer);
        writer.WriteEndTag("div");
    }

    #endregion


    #region "IPostBackEventHandler members"

    public void RaisePostBackEvent(string eventArgument)
    {
        CurrentUserInfo currentUser = CMSContext.CurrentUser;

        // Current Node ID
        int nodeId = ValidationHelper.GetInteger(Param1, 0);

        TreeProvider tree = new TreeProvider(currentUser);
        EventLogProvider log = new EventLogProvider();

        string documentName = string.Empty;
        string action = Action.ToLowerCSafe();
        string siteName = CMSContext.CurrentSiteName;

        // Process the request
        switch (action)
        {
            case "refresh":
                treeElem.SelectedNodeID = nodeId;
                AddScript("currentNodeId = " + nodeId + ";");
                break;

            case "moveup":
            case "movedown":
            case "movetop":
            case "movebottom":
                // Move the document up (document order)
                try
                {
                    if (nodeId == 0)
                    {
                        AddAlert(GetString("ContentRequest.ErrorMissingSource"));
                        return;
                    }

                    // Get document to move
                    TreeNode node = tree.SelectSingleNode(nodeId);

                    // Check the permissions for document
                    if (currentUser.IsAuthorizedPerDocument(node, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Allowed)
                    {
                        // Root of products tree can not be moved
                        if (!IsProductTree || (node.NodeAliasPath.CompareToCSafe(StartingAliasPath, true) != 0))
                        {
                            switch (action)
                            {
                                case "moveup":
                                    node = tree.MoveNodeUp(nodeId);
                                    break;

                                case "movedown":
                                    node = tree.MoveNodeDown(nodeId);
                                    break;

                                case "movetop":
                                    node = tree.SelectSingleNode(nodeId);
                                    tree.SetNodeOrder(nodeId, DocumentOrderEnum.First);
                                    break;

                                case "movebottom":
                                    node = tree.SelectSingleNode(nodeId);
                                    tree.SetNodeOrder(nodeId, DocumentOrderEnum.Last);
                                    break;
                            }

                            if (node != null)
                            {
                                // Log the synchronization tasks for the entire tree level
                                if (SettingsKeyProvider.GetBoolValue(siteName + ".CMSStagingLogChanges"))
                                {
                                    // Log the synchronization tasks for the entire tree level
                                    DocumentSynchronizationHelper.LogDocumentChangeOrder(siteName, node.NodeAliasPath, tree);
                                }

                                // Select the document in the tree
                                documentName = node.GetDocumentName();

                                treeElem.ExpandNodeID = node.NodeParentID;
                                treeElem.SelectedNodeID = node.NodeID;
                                AddScript("currentNodeId = " + node.NodeID + ";");
                            }
                            else
                            {
                                AddAlert(GetString("ContentRequest.MoveFailed"));
                            }
                        }
                    }
                    else
                    {
                        // Select the document in the tree
                        treeElem.SelectedNodeID = nodeId;

                        AddAlert(GetString("ContentRequest.MoveDenied"));
                    }
                }
                catch (Exception ex)
                {
                    log.LogEvent(EventLogProvider.EVENT_TYPE_ERROR, DateTime.Now, "Content", "MOVE", currentUser.UserID, currentUser.UserName, nodeId, documentName, HTTPHelper.UserHostAddress, EventLogProvider.GetExceptionLogMessage(ex), CMSContext.CurrentSite.SiteID, HTTPHelper.GetAbsoluteUri());
                    AddAlert(GetString("ContentRequest.MoveFailed") + " : " + ex.Message);
                }
                break;

            case "setculture":
                // Set the preferred culture code
                try
                {
                    // Set the culture code
                    string language = ValidationHelper.GetString(Param2, string.Empty);
                    if (!string.IsNullOrEmpty(language))
                    {
                        CMSContext.PreferredCultureCode = language;
                        treeElem.Culture = language;
                    }
                    // Refresh the document
                    if (nodeId > 0)
                    {
                        treeElem.SelectedNodeID = nodeId;

                        AddScript("SelectNode(" + nodeId + ");");
                    }
                }
                catch (Exception ex)
                {
                    log.LogEvent(EventLogProvider.EVENT_TYPE_ERROR, DateTime.Now, "Content", "SETCULTURE", currentUser.UserID, currentUser.UserName, nodeId, documentName, HTTPHelper.UserHostAddress, EventLogProvider.GetExceptionLogMessage(ex), CMSContext.CurrentSite.SiteID, HTTPHelper.GetAbsoluteUri());
                    AddAlert(GetString("ContentRequest.ErrorChangeLanguage"));
                }
                break;

            case "setdevice":
                // Set the device profile
                try
                {
                    // Set the device name
                    string deviceName = ValidationHelper.GetString(Param2, string.Empty);
                    CMSContext.CurrentDeviceProfileName = deviceName;

                    // Refresh the document
                    if (nodeId > 0)
                    {
                        treeElem.SelectedNodeID = nodeId;

                        AddScript("SelectNode(" + nodeId + ");");
                    }
                }
                catch (Exception ex)
                {
                    log.LogEvent(EventLogProvider.EVENT_TYPE_ERROR, DateTime.Now, "Content", "SETDEVICE", currentUser.UserID, currentUser.UserName, nodeId, documentName, HTTPHelper.UserHostAddress, EventLogProvider.GetExceptionLogMessage(ex), CMSContext.CurrentSite.SiteID, HTTPHelper.GetAbsoluteUri());
                    AddAlert(GetString("ContentRequest.ErrorChangeLanguage"));
                }
                break;

            // Sorting
            case "sortalphaasc":
            case "sortalphadesc":
            case "sortdateasc":
            case "sortdatedesc":
                // Set the preferred culture code
                try
                {
                    // Get document to sort
                    TreeNode node = tree.SelectSingleNode(nodeId);

                    // Check the permissions for document
                    if ((currentUser.IsAuthorizedPerDocument(node, NodePermissionsEnum.Modify) == AuthorizationResultEnum.Allowed)
                        && (currentUser.IsAuthorizedPerDocument(node, NodePermissionsEnum.ExploreTree) == AuthorizationResultEnum.Allowed))
                    {
                        switch (action)
                        {
                            case "sortalphaasc":
                                tree.OrderNodesAlphabetically(nodeId, true);
                                break;

                            case "sortalphadesc":
                                tree.OrderNodesAlphabetically(nodeId, false);
                                break;

                            case "sortdateasc":
                                tree.OrderNodesByDate(nodeId, true);
                                break;

                            case "sortdatedesc":
                                tree.OrderNodesByDate(nodeId, false);
                                break;
                        }

                        // Log the synchronization tasks for the entire tree level
                        if (SettingsKeyProvider.GetBoolValue(siteName + ".CMSStagingLogChanges"))
                        {
                            // Log the synchronization tasks for the entire tree level
                            string fakeAlias = node.NodeAliasPath.TrimEnd('/') + "/child";
                            DocumentSynchronizationHelper.LogDocumentChangeOrder(siteName, fakeAlias, tree);
                        }
                    }
                    else
                    {
                        AddAlert(GetString("ContentRequest.SortDenied"));
                    }

                    // Refresh the tree
                    if (nodeId > 0)
                    {
                        treeElem.ExpandNodeID = nodeId;
                        treeElem.SelectedNodeID = nodeId;
                        if (IsProductTree)
                        {
                            AddScript("window.frames['contentview'].location.replace(window.frames['contentview'].location);");
                        }
                        else
                        {
                            AddScript("SelectNode(" + nodeId + ");");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogEvent(EventLogProvider.EVENT_TYPE_ERROR, DateTime.Now, "Content", "SORT", currentUser.UserID, currentUser.UserName, nodeId, documentName, HTTPHelper.UserHostAddress, EventLogProvider.GetExceptionLogMessage(ex), CMSContext.CurrentSite.SiteID, HTTPHelper.GetAbsoluteUri());
                    AddAlert(GetString("ContentRequest.ErrorSort"));
                }
                break;
        }

        // Maintain scrollbar position
        string script =
@"
SetSelectedNodeId(currentNodeId);
MaintainScroll('" + nodeId + @"','" + pnlTreeArea.ClientID + @"', " + ScrollPosition + @");
HideAllContextMenus();
";

        ScriptHelper.RegisterStartupScript(Page, typeof(string), "MaintainScrollbar", script, true);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Adds the alert message to the output request window.
    /// </summary>
    /// <param name="message">Message to display</param>
    private void AddAlert(string message)
    {
        ScriptHelper.RegisterStartupScript(this, typeof(string), message.GetHashCode().ToString(), ScriptHelper.GetAlertScript(message));
    }


    /// <summary>
    /// Adds the script to the output request window.
    /// </summary>
    /// <param name="script">Script to add</param>
    public void AddScript(string script)
    {
        ScriptHelper.RegisterStartupScript(this, typeof(string), script.GetHashCode().ToString(), ScriptHelper.GetScript(script));
    }


    protected void treeElem_RootNodeCreated(object sender, EventArgs e)
    {
        // Add "Unassigned products" node when global products or products without document allowed
        if (AllowGlobalObjects || AllowProductsWithoutDocuments)
        {
            UITreeView treeControl = sender as UITreeView;
            if (treeControl != null)
            {
                System.Web.UI.WebControls.TreeNode newNode = new System.Web.UI.WebControls.TreeNode();
                string imageUrl = GetImageUrl("Objects/Ecommerce_SKU/list.png");
                string tooltip = GetString("com.productstree.unassignedtooltip");

                // Set the base data
                newNode.Value = "-1";
                newNode.NavigateUrl = "javascript:void(0);";

                // Prepare icon
                StringBuilder sb = new StringBuilder();
                sb.Append("<img src=\"", imageUrl, "\" alt=\"\" style=\"border:0px;vertical-align:middle;\" onclick=\"return false;\"", HttpUtility.HtmlEncode(tooltip), " class=\"", "Image20", "\" />");
                string imageTag = sb.ToString();

                // Prepare name
                string nodeName = HttpUtility.HtmlEncode(GetString("com.productstree.unassigned"));
                string nodeNameJava = ScriptHelper.GetString(nodeName);

                newNode.Text = treeElem.NodeTextTemplate.Replace("##NODEID##", "-1").Replace("##NODENAMEJAVA##", nodeNameJava).Replace("##NODENAME##", nodeName).Replace("##ICON##", imageTag);

                const int childNodesCount = 0;
                newNode.Text = newNode.Text.Replace("##NODECHILDNODESCOUNT##", childNodesCount.ToString());

                // Insert node to the top of tree
                treeControl.Nodes.AddAt(0, newNode);
            }
        }
    }

    #endregion
}
