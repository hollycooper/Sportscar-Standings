<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Process_Detail_Frameset.aspx.cs" Inherits="CMSModules_ContactManagement_Pages_Tools_PendingContacts_Process_Detail_Frameset"
    Theme="Default" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server" enableviewstate="false">
    <title>Process detail</title>
    <style type="text/css">
        body
        {
            margin: 0px;
            height: 100%;
            width: 100%;
            overflow: hidden;
        }
    </style>
</head>
<frameset border="0" rows="36,*,36" runat="server" id="rowsFrameset">
    <frame name="header" id="frameHeader" runat="server" frameborder="0" noresize="noresize" scrolling="no" />
    <frame name="content" frameborder="0" noresize="noresize" scrolling="auto" runat="server" id="frameContent" />
    <frame name="footer" id="frameFooter" runat="server" frameborder="0" noresize="noresize" scrolling="no" />
    <cms:NoFramesLiteral ID="ltlNoFrames" runat="server" />
</frameset>
</html>