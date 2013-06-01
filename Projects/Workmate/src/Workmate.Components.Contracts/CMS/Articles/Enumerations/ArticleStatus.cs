using System;
namespace Workmate.Components.Contracts.CMS.Articles
{
	public enum ArticleStatus : byte
	{
		Published = 1,
		Unpublished = 2,
		Unapproved = 3,
		Deleted = 4
	}
}
