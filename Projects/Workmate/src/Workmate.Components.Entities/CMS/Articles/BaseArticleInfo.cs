using CommonTools.Extensions;
using Workmate.Components.Entities.CMS;
using System;
using System.Xml.Linq;
namespace Workmate.Components.Entities.CMS.Articles
{
  public class BaseArticleInfo
  {
    public virtual int ArticleId { get; private set; }
    public virtual int ArticleGroupThreadId { get; private set; }
    public virtual int ArticleGroupId { get; private set; }
    public virtual string FriendlyName { get; private set; }
    public virtual string WidgetHeadLine { get; private set; }
    public virtual string WidgetLead { get; private set; }
    public virtual DateTime DateCreatedUtc { get; private set; }
    public virtual int TotalComments { get; private set; }

    public BaseArticleInfo(
      int articleId
      , int articleGroupThreadId
      , int articleGroupId
      , string friendlyName
      , DateTime dateCreatedUtc
      , int totalComments
      , XElement extraInfo)
    {
      this.ArticleId = articleId;
      this.ArticleGroupId = articleGroupId;
      this.ArticleGroupThreadId = articleGroupThreadId;
      this.FriendlyName = friendlyName;
      this.DateCreatedUtc = dateCreatedUtc;
      this.TotalComments = totalComments;
      XElement container = extraInfo ?? new XElement("i");
      this.WidgetHeadLine = container.ParseXElementNode("h", string.Empty);
      this.WidgetLead = container.ParseXElementNode("l", string.Empty);
    }
  }
}
