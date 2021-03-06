﻿var IsCMSDesk = true;

function RefreshTree(expandNodeId, selectNodeId) {
    // Update tree
    if (parent != this) {
        parent.RefreshTree(expandNodeId, selectNodeId);
    }
    return false;
}

function SelectNode(nodeId) {
    if (parent != this) {
        parent.SelectNode(nodeId, null);
    }
}

function SetSelectedMode(mode) {
    if (parent != this) {
        parent.SetSelectedMode(mode);
    }
}

function NewDocument(parentNodeId, className) {
    if ((parentNodeId != 0) && (parent != this)) {
        parent.NewDocument(parentNodeId, className);
        parent.RefreshTree(parentNodeId, parentNodeId);
    }
}

function ConvertDocument(parentNodeId, convertDocumentId) {
    if ((parent != this) && (parentNodeId != 0) && (convertDocumentId != 0)) {
        parent.ConvertDocument(parentNodeId, convertDocumentId);
    }
}

function DeleteDocument(nodeId) {
    if ((nodeId != 0) && (parent != this)) {
        parent.DeleteDocument(nodeId);
        parent.RefreshTree(nodeId, nodeId);
    }
}

function EditDocument(nodeId) {
    if ((nodeId != 0) && (parent != this)) {
        parent.EditDocument(nodeId);
        parent.RefreshTree(nodeId, nodeId);
    }
}

function SetTabsContext(mode) {
    if (window.frames['contenteditheader'].SetTabsContext) {
        window.frames['contenteditheader'].SetTabsContext(mode);
    }
}

// Refresh tree with current node selected
function TreeRefresh() {
    if (parent != this) {
        parent.TreeRefresh();
    }
}

function CheckChanges() {
    if (window.frames['contenteditview']) {
        if (window.frames['contenteditview'].CheckChanges) {
            return window.frames['contenteditview'].CheckChanges();
        }
    }
    return true;
}

function CloseSplitMode() {
    if (parent != this) {
        parent.CloseSplitMode();
    }
}

function SplitModeRefreshFrame() {
    if (parent != this) {
        parent.SplitModeRefreshFrame();
    }
}

function PerformSplitViewRedirect(originalUrl, newCulture, successCallback, errorCallback) {
    if (parent != this) {
        parent.PerformSplitViewRedirect(originalUrl, newCulture, successCallback, errorCallback);
    }
}