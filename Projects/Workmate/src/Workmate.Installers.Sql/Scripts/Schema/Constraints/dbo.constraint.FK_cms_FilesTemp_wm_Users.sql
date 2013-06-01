IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
ALTER TABLE [dbo].[cms_FilesTemp]  WITH CHECK ADD  CONSTRAINT [FK_cms_FilesTemp_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_FilesTemp_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_FilesTemp]'))
ALTER TABLE [dbo].[cms_FilesTemp] CHECK CONSTRAINT [FK_cms_FilesTemp_wm_Users]
GO