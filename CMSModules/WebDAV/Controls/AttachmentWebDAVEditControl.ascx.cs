﻿using System;

using CMS.DocumentEngine;
using CMS.UIControls;
using CMS.SettingsProvider;

/// <summary>
/// Attachment WebDAV control.
/// </summary>
public partial class CMSModules_WebDAV_Controls_AttachmentWebDAVEditControl : WebDAVEditControl
{
    #region "Constructors"

    /// <summary>
    /// Creates instance.
    /// </summary>
    public CMSModules_WebDAV_Controls_AttachmentWebDAVEditControl()
    {
        mControlType = WebDAVControlTypeEnum.Attachment;
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Gets the attachment URL.
    /// </summary>
    protected override string GetUrl()
    {
        string nodeAliasPath = NodeAliasPath;

        // Get group sub node alias path
        if (GroupNode != null)
        {
            if (nodeAliasPath.StartsWithCSafe(GroupNode.NodeAliasPath, true))
            {
                nodeAliasPath = nodeAliasPath.Remove(0, GroupNode.NodeAliasPath.Length);
            }
        }

        return AttachmentURLProvider.GetAttachmentWebDAVUrl(SiteName, nodeAliasPath, NodeCultureCode, AttachmentFieldName, FileName, GroupName);
    }

    #endregion
}