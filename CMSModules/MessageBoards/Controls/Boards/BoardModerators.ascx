<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="CMSModules_MessageBoards_Controls_Boards_BoardModerators" CodeFile="BoardModerators.ascx.cs" %>
<%@ Register Src="~/CMSModules/Membership/FormControls/Users/securityAddUsers.ascx" TagName="SelectUser" TagPrefix="cms" %>

<cms:MessagesPlaceHolder ID="plcMess" runat="server" />
<asp:CheckBox ID="chkBoardModerated" runat="server" CssClass="CheckBoxMovedLeft"
    OnCheckedChanged="chkBoardModerated_CheckedChanged" AutoPostBack="true" />
<br />
<br />
<asp:Label ID="lblModerators" runat="server" EnableViewState="false" CssClass="BoldInfoLabel" />
<cms:SelectUser ID="userSelector" runat="server" />
