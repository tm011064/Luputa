using Workmate.Components.Entities.CMS;
using System;
using Workmate.Components.Contracts.CMS;
namespace Workmate.Components.Entities.CMS.MessageBoards
{
  public class MessageBoard
  {
    private CMSSection _CMSSection;
    internal CMSSection CMSSection { get { return _CMSSection; } }

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

    public int MessageBoardId
    {
      get { return _CMSSection.CMSSectionId; }
      set { _CMSSection.CMSSectionId = value; }
    }

    public MessageBoard(int applicationId, string name, bool isActive, bool isModerated)
    {
      _CMSSection = new CMSSection(applicationId, name, isActive, isModerated, CMSSectionType.MessageBoard);
    }
    internal MessageBoard(CMSSection record)
    {
      _CMSSection = record;
    }
  }
}
