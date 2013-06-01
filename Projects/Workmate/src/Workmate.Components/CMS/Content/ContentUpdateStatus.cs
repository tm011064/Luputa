using System;
namespace Workmate.Components.CMS.Content
{
	public enum ContentUpdateStatus
	{
		Success,
		ValidationFailed = -1001,
		SectionNotFound = -1,
		UnknownError = -1099,
		InvalidHtml = -2000
	}
}
