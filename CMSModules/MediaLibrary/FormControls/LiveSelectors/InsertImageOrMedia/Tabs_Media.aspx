<%@ Page Language="C#" Theme="Default" AutoEventWireup="true" Inherits="CMSModules_MediaLibrary_FormControls_LiveSelectors_InsertImageOrMedia_Tabs_Media"
    CodeFile="Tabs_Media.aspx.cs" EnableEventValidation="false" %>

<%@ Register Src="~/CMSModules/MediaLibrary/Controls/Dialogs/LinkMediaSelector.ascx"
    TagName="LinkMedia" TagPrefix="cms" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server" enableviewstate="false">
    <title>Insert image or media - content</title>
    <style type="text/css">
        body
        {
            margin: 0px;
            padding: 0px;
            height: 100%;
        }
        .ImageExtraClass
        {
            position: absolute;
        }
        .ImageTooltip
        {
            border: 1px solid #ccc;
            background-color: #fff;
            padding: 3px;
            display: block;
        }
    </style>
</head>
<body class="<%=mBodyClass%>">
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="scriptManager" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="LiveSiteDialog">
        <cms:CMSUpdatePanel ID="uplContent" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <cms:LinkMedia ID="linkMedia" runat="server" IsLiveSite="true" />
            </ContentTemplate>
        </cms:CMSUpdatePanel>
    </div>
    </form>

    <script language="javascript" type="text/javascript">
        //<![CDATA[
        InitResizers();
        //]]>
    </script>

</body>
</html>
