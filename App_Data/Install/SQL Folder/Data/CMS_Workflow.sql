SET IDENTITY_INSERT [CMS_Workflow] ON
INSERT INTO [CMS_Workflow] ([WorkflowID], [WorkflowDisplayName], [WorkflowName], [WorkflowGUID], [WorkflowLastModified], [WorkflowAutoPublishChanges], [WorkflowUseCheckinCheckout], [WorkflowType], [WorkflowSendEmails], [WorkflowSendApproveEmails], [WorkflowSendRejectEmails], [WorkflowSendPublishEmails], [WorkflowSendArchiveEmails], [WorkflowApprovedTemplateName], [WorkflowRejectedTemplateName], [WorkflowPublishedTemplateName], [WorkflowArchivedTemplateName], [WorkflowSendReadyForApprovalEmails], [WorkflowReadyForApprovalTemplateName], [WorkflowNotificationTemplateName], [WorkflowAllowedObjects]) VALUES (1, N'Default workflow', N'default', '331fd346-2a1b-4a63-bb26-474e75a14807', '20120528 16:38:12', NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT INTO [CMS_Workflow] ([WorkflowID], [WorkflowDisplayName], [WorkflowName], [WorkflowGUID], [WorkflowLastModified], [WorkflowAutoPublishChanges], [WorkflowUseCheckinCheckout], [WorkflowType], [WorkflowSendEmails], [WorkflowSendApproveEmails], [WorkflowSendRejectEmails], [WorkflowSendPublishEmails], [WorkflowSendArchiveEmails], [WorkflowApprovedTemplateName], [WorkflowRejectedTemplateName], [WorkflowPublishedTemplateName], [WorkflowArchivedTemplateName], [WorkflowSendReadyForApprovalEmails], [WorkflowReadyForApprovalTemplateName], [WorkflowNotificationTemplateName], [WorkflowAllowedObjects]) VALUES (212, N'Translation - Default language version', N'Translation_workflow', '64b14868-7767-4ed6-bb5e-7c8b8465b60c', '20120709 08:58:18', 0, NULL, 1, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N';##DOCUMENTS##;')
INSERT INTO [CMS_Workflow] ([WorkflowID], [WorkflowDisplayName], [WorkflowName], [WorkflowGUID], [WorkflowLastModified], [WorkflowAutoPublishChanges], [WorkflowUseCheckinCheckout], [WorkflowType], [WorkflowSendEmails], [WorkflowSendApproveEmails], [WorkflowSendRejectEmails], [WorkflowSendPublishEmails], [WorkflowSendArchiveEmails], [WorkflowApprovedTemplateName], [WorkflowRejectedTemplateName], [WorkflowPublishedTemplateName], [WorkflowArchivedTemplateName], [WorkflowSendReadyForApprovalEmails], [WorkflowReadyForApprovalTemplateName], [WorkflowNotificationTemplateName], [WorkflowAllowedObjects]) VALUES (211, N'Translation - Import to other language versions', N'Import_translation_workflow', 'dec28292-de79-4a52-881c-734d1ce80341', '20120615 07:32:26', 0, NULL, 1, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N';##DOCUMENTS##;')
INSERT INTO [CMS_Workflow] ([WorkflowID], [WorkflowDisplayName], [WorkflowName], [WorkflowGUID], [WorkflowLastModified], [WorkflowAutoPublishChanges], [WorkflowUseCheckinCheckout], [WorkflowType], [WorkflowSendEmails], [WorkflowSendApproveEmails], [WorkflowSendRejectEmails], [WorkflowSendPublishEmails], [WorkflowSendArchiveEmails], [WorkflowApprovedTemplateName], [WorkflowRejectedTemplateName], [WorkflowPublishedTemplateName], [WorkflowArchivedTemplateName], [WorkflowSendReadyForApprovalEmails], [WorkflowReadyForApprovalTemplateName], [WorkflowNotificationTemplateName], [WorkflowAllowedObjects]) VALUES (46, N'Versioning without workflow', N'versioningWithoutWorkflow', '38bbee58-ba57-438b-a3db-9b0c153315c6', '20110113 19:14:24', 1, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [CMS_Workflow] OFF