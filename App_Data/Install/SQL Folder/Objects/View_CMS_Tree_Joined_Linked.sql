SET ANSI_PADDING ON
GO
CREATE VIEW [View_CMS_Tree_Joined_Linked] WITH SCHEMABINDING AS 
SELECT S.SiteName, C.ClassName, C.ClassDisplayName, T.[NodeID], T.[NodeAliasPath], T.[NodeName], T.[NodeAlias], T.[NodeClassID], T.[NodeParentID], T.[NodeLevel], T.[NodeACLID], T.[NodeSiteID], T.[NodeGUID], T.[NodeOrder], T.[IsSecuredNode], T.[NodeCacheMinutes], T.[NodeSKUID], T.[NodeDocType], T.[NodeHeadTags], T.[NodeBodyElementAttributes], T.[NodeInheritPageLevels], T.[NodeChildNodesCount], T.[RequiresSSL], T.[NodeLinkedNodeID], T.[NodeOwner], T.[NodeCustomData], T.[NodeGroupID], T.[NodeLinkedNodeSiteID], T.[NodeTemplateID], T.[NodeWireframeTemplateID], T.[NodeWireframeComment], T.[NodeTemplateForAllCultures], T.[NodeInheritPageTemplate], T.[NodeWireframeInheritPageLevels], T.[NodeAllowCacheInFileSystem], D.[DocumentID], D.[DocumentName], D.[DocumentNamePath], D.[DocumentModifiedWhen], D.[DocumentModifiedByUserID], D.[DocumentForeignKeyValue], D.[DocumentCreatedByUserID], D.[DocumentCreatedWhen], D.[DocumentCheckedOutByUserID], D.[DocumentCheckedOutWhen], D.[DocumentCheckedOutVersionHistoryID], D.[DocumentPublishedVersionHistoryID], D.[DocumentWorkflowStepID], D.[DocumentPublishFrom], D.[DocumentPublishTo], D.[DocumentUrlPath], D.[DocumentCulture], D.[DocumentNodeID], D.[DocumentPageTitle], D.[DocumentPageKeyWords], D.[DocumentPageDescription], D.[DocumentShowInSiteMap], D.[DocumentMenuItemHideInNavigation], D.[DocumentMenuCaption], D.[DocumentMenuStyle], D.[DocumentMenuItemImage], D.[DocumentMenuItemLeftImage], D.[DocumentMenuItemRightImage], D.[DocumentPageTemplateID], D.[DocumentMenuJavascript], D.[DocumentMenuRedirectUrl], D.[DocumentUseNamePathForUrlPath], D.[DocumentStylesheetID], D.[DocumentContent], D.[DocumentMenuClass], D.[DocumentMenuStyleOver], D.[DocumentMenuClassOver], D.[DocumentMenuItemImageOver], D.[DocumentMenuItemLeftImageOver], D.[DocumentMenuItemRightImageOver], D.[DocumentMenuStyleHighlighted], D.[DocumentMenuClassHighlighted], D.[DocumentMenuItemImageHighlighted], D.[DocumentMenuItemLeftImageHighlighted], D.[DocumentMenuItemRightImageHighlighted], D.[DocumentMenuItemInactive], D.[DocumentCustomData], D.[DocumentExtensions], D.[DocumentCampaign], D.[DocumentTags], D.[DocumentTagGroupID], D.[DocumentWildcardRule], D.[DocumentWebParts], D.[DocumentRatingValue], D.[DocumentRatings], D.[DocumentPriority], D.[DocumentType], D.[DocumentLastPublished], D.[DocumentUseCustomExtensions], D.[DocumentGroupWebParts], D.[DocumentCheckedOutAutomatically], D.[DocumentTrackConversionName], D.[DocumentConversionValue], D.[DocumentSearchExcluded], D.[DocumentLastVersionName], D.[DocumentLastVersionNumber], D.[DocumentIsArchived], D.[DocumentLastVersionType], D.[DocumentLastVersionMenuRedirectUrl], D.[DocumentHash], D.[DocumentLogVisitActivity], D.[DocumentGUID], D.[DocumentWorkflowCycleGUID], D.[DocumentSitemapSettings], D.[DocumentIsWaitingForTranslation], D.[DocumentSKUName], D.[DocumentSKUDescription], D.[DocumentSKUShortDescription], D.[DocumentWorkflowActionStatus]
FROM dbo.CMS_Tree T INNER JOIN dbo.CMS_Document D ON T.NodeLinkedNodeID = D.DocumentNodeID INNER JOIN dbo.CMS_Site S ON T.NodeSiteID = S.SiteID INNER JOIN dbo.CMS_Class C ON T.NodeClassID = C.ClassID
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE CLUSTERED INDEX [IX_View_CMS_Tree_Joined_Linked_NodeSiteID_DocumentCulture_NodeID]
	ON [View_CMS_Tree_Joined_Linked] ([NodeSiteID], [DocumentCulture], [NodeID])
	
GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_View_CMS_Tree_Joined_Linked_ClassName_NodeSiteID_DocumentForeignKeyValue_DocumentCulture]
	ON [View_CMS_Tree_Joined_Linked] ([ClassName], [NodeSiteID], [DocumentForeignKeyValue], [DocumentCulture])
	
GO
SET ANSI_PADDING OFF
GO
