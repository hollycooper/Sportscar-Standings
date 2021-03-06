﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using CMS.PortalControls;
using CMS.UIControls;
using CMS.Chat;
using CMS.GlobalHelper;
using CMS.CMSHelper;
using CMS.Controls;
using CMS.DataEngine;
using CMS.SettingsProvider;
using System.Web.Script.Serialization;

public partial class CMSWebParts_Chat_ChatOnlineUsers : CMSAbstractWebPart 
{
    #region "Variables"

    bool mInviteMode = false;
    string mGroupName = "";
    bool mIsSupport = false;

    #endregion


    #region Properties

    /// <summary>
    /// Gets or sets OnlineUserTransformationName property. 
    /// </summary>
    public string OnlineUserTransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(this.GetValue("OnlineUserTransformationName"), ChatHelper.TransformationOnlineUsers);
        }
        set
        {
            this.SetValue("OnlineUserTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ChatMessageTransformationName property.
    /// </summary>
    public string ChatMessageTransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(this.GetValue("ChatMessageTransformationName"), ChatHelper.TransformationRoomMessages);
        }
        set
        {
            this.SetValue("ChatMessageTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ChatErrorTransformationName property.
    /// </summary>
    public string ChatErrorTransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(this.GetValue("ChatErrorTransformationName"), ChatHelper.TransformationErrors);
        }
        set
        {
            this.SetValue("ChatErrorTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ChatErrorDeleteAllButtonTransformationName property.
    /// </summary>
    public string ChatErrorDeleteAllButtonTransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(this.GetValue("ChatErrorDeleteAllButtonTransformationName"), ChatHelper.TransformationErrorsDeleteAll);
        }
        set
        {
            this.SetValue("ChatErrorDeleteAllButtonTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets ChatRoomUserTransformationName property.
    /// </summary>
    public string ChatRoomUserTransformationName
    {
        get
        {
            return DataHelper.GetNotEmpty(this.GetValue("ChatRoomUserTransformationName"), ChatHelper.TransformationRoomUsers);
        }
        set
        {
            this.SetValue("ChatRoomUserTransformationName", value);
        }
    }


    /// <summary>
    /// Gets or sets EnablePaging property.
    /// </summary>
    public bool EnablePaging
    {
        get
        {
            return ValidationHelper.GetBoolean(this.GetValue("EnablePaging"), false);
        }
        set
        {
            this.SetValue("EnablePaging", value);
        }
    }


    /// <summary>
    /// Gets or sets EnableFiltering property.
    /// </summary>
    public bool EnableFiltering
    {
        get
        {
            return ValidationHelper.GetBoolean(this.GetValue("EnableFiltering"), false);
        }
        set
        {
            this.SetValue("EnableFiltering", value);
        }
    }


    /// <summary>
    /// Gets or sets ShowFilterItems property.
    /// </summary>
    public int ShowFilterItems
    {
        get
        {
            return ValidationHelper.GetInteger(this.GetValue("ShowFilterItems"), -1);
        }
        set
        {
            this.SetValue("ShowFilterItems", value);
        }
    }


    /// <summary>
    /// Gets or sets PagingItems property.
    /// </summary>
    public int PagingItems
    {
        get
        {
            return ValidationHelper.GetInteger(this.GetValue("PagingItems"), -1);
        }
        set
        {
            this.SetValue("PagingItems", value);
        }
    }


    /// <summary>
    /// Indicates if webpart is in invite mode
    /// That means that clicking users in list will invite them to current room.
    /// </summary>
    public bool InviteMode
    {
        get
        {
            return mInviteMode;
        }
        set
        {
            mInviteMode = value;
        }
    }


    /// <summary>
    /// Gets or sets GroupName property (only used when invite mode is set). 
    /// </summary>
    public string GroupName
    {
        get
        {
            return mGroupName;
        }
        set
        {
            mGroupName = value;
        }
    }


    /// <summary>
    /// Indicates if this webpart is in support chat window.
    /// </summary>
    public bool IsSupport
    {
        get
        {
            return mIsSupport;
        }
        set
        {
            mIsSupport = value;
        }
    }


    public string InnerContainerName { get; set; }
    public string InnerContainerTitle { get; set; }

    #endregion


    #region "Page Events"

    protected void Page_Prerender(object sender, EventArgs e)
    {
        if (!IsVisible)
        {
            return;
        }
        ChatHelper.MakeWebpartEnvelope("ChatWebpartEnvelope ChatWebpartEnvelopeOnlineUsers", this, InnerContainerTitle, InnerContainerName);
        if (IsSupport)
        {
            ChatHelper.RegisterStylesheet(Page, true);
        }
        else
        {
            ChatHelper.RegisterStylesheet(Page);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsVisible)
        {
            pnlOnlineUsersWP.Visible = false;
            return;
        }

        // Registration to chat webservice
        AbstractCMSPage cmsPage = Page as AbstractCMSPage;
        if (cmsPage != null)
        {
            ChatHelper.RegisterChatAJAXProxy(cmsPage);
        }

        // Script references insertion
        ChatHelper.RegisterChatManager(Page);
        ScriptHelper.RegisterScriptFile(Page, "jquery/jquery-tmpl.js");
        ScriptHelper.RegisterScriptFile(Page, "~/CMSModules/Chat/CMSPages/Scripts/ListPaging.js");
        ScriptHelper.RegisterScriptFile(Page, "~/CMSWebParts/Chat/ChatOnlineUsers_files/ChatOnlineUsers.js");

        
        if (EnableFiltering)
        {
            pnlChatOnlineUsersFiltering.Visible = true;
        }
        if (InviteMode)
        {
            pnlChatOnlineUsersInvite.Visible = true;
        }

        ScriptHelper.RegisterStartupScript(Page, typeof(string), "OnlineUsers_" + ClientID, BuildStartupScript(), true);
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Builds startup script.
    /// </summary>
    private string BuildStartupScript()
    {
        int id = ChatPopupWindowSettingsHelper.Store(ChatMessageTransformationName, ChatRoomUserTransformationName, ChatErrorTransformationName, ChatErrorDeleteAllButtonTransformationName);            

        JavaScriptSerializer sr = new JavaScriptSerializer();
        string json = sr.Serialize(
            new
            {
                onlineUserTemplate = ChatHelper.GetWebpartTransformation(OnlineUserTransformationName,"chat.error.transformation.users.onlineuser"), 
                chatRoomWindowUrl = ChatHelper.GetChatRoomWindowURL(), 
                clientID = ClientID, 
                GUID = id, 
                contentClientID = pnlChatOnlineUsers.ClientID, 
                inviteMode = InviteMode,
                pnlFilterClientID = pnlChatOnlineUsersFiltering.ClientID,
                pnlPagingClientID = pnlChatOnlineUsersPaging.ClientID,
                pagingItems = PagingItems > 0 ? PagingItems : ChatHelper.WPPagingItems,
                pagingEnabled = EnablePaging,
                btnFilter = btnChatOnlineUsersFilter.ClientID,
                txtFilter = txtChatOnlineUsersFilter.ClientID, 
                filterEnabled = EnableFiltering,
                pnlInfo = pnlChatOnlineUsersInfo.ClientID, 
                resStrNoFound = ResHelper.GetString("chat.onlineusers.notfound"), 
                resStrResults = ResHelper.GetString("chat.onlineusers.results"),
                loadingDiv = ChatHelper.GetWebpartLoadingDiv("ChatOnlineUsersWPLoading", "chat.wploading.onlineusers"), 
                filterCount = ShowFilterItems >= 0 ? ShowFilterItems : ChatHelper.WPShowFilterLimit, 
                envelopeID = "envelope_" + ClientID,
                resStrNoOneInviteMode = ResHelper.GetString("chat.onlineusers.invitemodenousers"),
                groupID = GroupName,
                invitePanel = pnlChatOnlineUsersInvite.ClientID
            }
        );
        return String.Format("InitChatOnlineUsersWebpart({0});", json);    
    }

    #endregion

}
