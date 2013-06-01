IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
ALTER TABLE [dbo].[cms_ContentRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentRatings_wm_Users] FOREIGN KEY([UserId])
REFERENCES [wm_Users] ([UserId])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_wm_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
ALTER TABLE [dbo].[cms_ContentRatings] CHECK CONSTRAINT [FK_cms_ContentRatings_wm_Users]
GO