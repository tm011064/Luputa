using Workmate.Components.Contracts.CMS;

namespace Workmate.Components.Entities.CMS.PrivateMessages
{
  public class Inbox
  {
    private CMSSection _CMSSection;
    internal CMSSection CMSSection { get { return _CMSSection; } }

    public int InboxId
    {
      get { return _CMSSection.CMSSectionId; }
      set { _CMSSection.CMSSectionId = value; }
    }

    #region standard properties
    public string Name
    {
      get { return _CMSSection.Name; }
      set { _CMSSection.Name = value; }
    }
    public string Description
    {
      get { return _CMSSection.Description; }
      set { _CMSSection.Description = value; }
    }
    public bool IsActive
    {
      get { return _CMSSection.IsActive; }
      set { _CMSSection.IsActive = value; }
    }
    public bool IsModerated
    {
      get { return _CMSSection.IsModerated; }
      set { _CMSSection.IsModerated = value; }
    }
    #endregion

    public Inbox(int applicationId, string name, bool isActive)
    {
      _CMSSection = new CMSSection(applicationId, name, isActive, false, CMSSectionType.PrivateMessageInbox);
    }
    public Inbox(CMSSection record)
    {
      _CMSSection = record;
    }
  }
}
