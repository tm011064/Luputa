IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
ALTER TABLE [dbo].[cms_ContentRatings]  WITH CHECK ADD  CONSTRAINT [FK_cms_ContentRatings_cms_Contents] FOREIGN KEY([ContentId])
REFERENCES [cms_Contents] ([ContentId])
ON DELETE CASCADE
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cms_ContentRatings_cms_Contents]') AND parent_object_id = OBJECT_ID(N'[dbo].[cms_ContentRatings]'))
ALTER TABLE [dbo].[cms_ContentRatings] CHECK CONSTRAINT [FK_cms_ContentRatings_cms_Contents]
GO