using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Workmate.Web.Components.Validation;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Contracts.CMS.Articles;
using Workmate.Web.Components;
using Workmate.Web.Components.Application;

namespace Workmate.Web.UI.Models.Articles
{
  public class CreateArticleModel
  {
    [Required]
    [StringLength(256, MinimumLength=3)]
    public string Subject { get; set; }

    [Required]
    [AllowHtml]
    public string Body { get; set; }

    public string Language { get; set; }

    public SelectList LanguageSelectList { get; set; }

    public List<string> Categories { get; set; }

    public CreateArticleModel(int totalCategories, SelectList languageSelectList)
    {
      this.Categories = new List<string>();
      for (int i = 0; i < totalCategories; i++)
        this.Categories.Add(null);

      this.LanguageSelectList = languageSelectList;
    }

    public CreateArticleModel() { }
  }
}