namespace Workmate.Components.Contracts.CMS.MessageBoards
{
	public enum MessageStatus : byte
	{
		Unknown = 1,
		Approved = 2,
		Spam = 3,
		LikelySpam = 4,
		NeedsApproval = 5,
		LikelyAbusive = 6,
		Abusive = 7
	}
}
