<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="CMSModules_Automation_Controls_Process_List" %>
<%@ Register Src="~/CMSAdminControls/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<%@ Register Namespace="CMS.UIControls.UniGridConfig" TagPrefix="ug" Assembly="CMS.UIControls" %>

<cms:UniGrid ID="gridState" runat="server" OrderBy="StateStatus" IsLiveSite="false" ObjectType="ma.automationstate" Columns="StateID, StateStepID, StateWorkflowID, StateStatus, StateCreated, StateUserID">
    <GridActions Parameters="StateID">
        <ug:Action Name="edit" Caption="$ma.process.manage$" Icon="Edit.png" />
        <ug:Action Name="delete" ExternalSourceName="delete" Caption="$autoMenu.RemoveState$" Icon="Delete.png" />
    </GridActions>
    <GridColumns>
        <ug:Column Source="StateWorkflowID" ExternalSourceName="processname" Caption="$Unigrid.Workflow.Columns.ProcessDisplayName$" Wrap="false" AllowSorting="false">
            <Filter Type="text" Format="StateWorkflowID IN (SELECT WorkflowID FROM CMS_Workflow WHERE {3})" Source="WorkflowDisplayName" Size="100" />
        </ug:Column>
        <ug:Column Source="StateStepID" ExternalSourceName="stepname" Caption="$Unigrid.Workflow.Columns.StepDisplayName$" Wrap="false" AllowSorting="false">
            <Filter Type="text" Format="StateStepID IN (SELECT StepID FROM CMS_WorkflowStep WHERE {3})" Source="StepDisplayName" Size="100" />
        </ug:Column>
        <ug:Column Source="StateStatus" ExternalSourceName="Status" Caption="$Unigrid.Automation.Columns.StateStatus$" Wrap="false">
            <Filter Type="custom" Path="~/CMSModules/Automation/FormControls/ProcessStatusSelector.ascx" />
        </ug:Column>
        <ug:Column Source="StateCreated" Caption="$Unigrid.Workflow.Columns.InitiatedWhen$" Wrap="false" />
        <ug:Column Source="StateUserID" Caption="$Unigrid.Workflow.Columns.InitiatedBy$" ExternalSourceName="#formattedusername|{$ma.automationstate.automatically$}" Wrap="false" />
        <ug:Column Width="100%" />
    </GridColumns>
    <GridOptions DisplayFilter="true" />
</cms:UniGrid>