using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using Workmate.Components.Entities.CMS.Articles;
using Workmate.Components.Contracts.Membership;
using Workmate.Web.Components.Security;
using System.Web.Mvc;

namespace Workmate.Web.Components.RouteHandlers.FileUpload
{
  public class FilesStatus
  {
    #region properties
    public string group { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public int size { get; set; }
    public string progress { get; set; }
    public string url { get; set; }
    public string thumbnail_url { get; set; }
    public string delete_url { get; set; }
    public string delete_type { get; set; }
    public string error { get; set; }

    public int attachmentId { get; set; }
    #endregion

    #region methods
    //private bool IsImage(string ext)
    //{
    //  return ext == ".gif" || ext == ".jpg" || ext == ".png";
    //}

    //private string EncodeFile(string fileName)
    //{
    //  return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
    //}

    //static double ConvertBytesToMegabytes(long bytes)
    //{
    //  return (bytes / 1024f) / 1024f;
    //}
    #endregion

    //private void SetValues(string fileName, int fileLength)
    //{
    //  name = fileName;
    //  type = "image/png";
    //  size = fileLength;
    //  progress = "1.0";
    //  url = "/wm/Handlers/Articles/FileUploadHandler.ashx?f=" + fileName; // TODO (Roman): implement proper path
    //  delete_url = "/wm/Handlers/Articles/FileUploadHandler.ashx?f=" + fileName;// TODO (Roman): implement proper path
    //  delete_type = "DELETE";

    //  // TODO (Roman): remove path stuff
    //  var ext = ".txt"; // Path.GetExtension(fullPath);
    //  thumbnail_url = "na.img";
    //  //var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
    //  //if (fileSize > 3 || !IsImage(ext)) 
    //  //  thumbnail_url = "/Content/img/generalFile.png";
    //  //else 
    //  //  thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath);
    //}

    public FilesStatus(ArticleAttachment attachment, string action) : this(attachment, null, action) { }
    public FilesStatus(ArticleAttachment attachment, string error, string action)
    {
      this.attachmentId = attachment.ArticleAttachmentId;
      this.delete_type = "DELETE";
      
      this.delete_url = action + "?f=" + attachment.ArticleAttachmentId; // TODO (Roman): implement proper path
      this.url = action + "?f=" + attachment.ArticleAttachmentId; // TODO (Roman): implement proper path
      this.error = error;
      this.name = attachment.FileName;
      this.progress = "1.0";
      this.size = attachment.ContentSize;
      this.type = attachment.ContentType;      
    }
  }

  public class ArticlesFileUploadHandler : AuthorizedRequestHandler
  {
    #region members
    private readonly JavaScriptSerializer _JavaScriptSerializer;
    #endregion

    #region helpers
    private static void ReturnOptions(HttpContext context)
    {
      context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
      context.Response.StatusCode = 200;
    }
    private byte[] ReadStream(Stream stream, int initialLength)
    {
      // If we've been passed an unhelpful initial length, just
      // use 32K.
      if (initialLength < 1)
      {
        initialLength = 32768;
      }

      byte[] buffer = new byte[initialLength];
      int read = 0;

      int chunk;
      while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
      {
        read += chunk;

        // If we've reached the end of our buffer, check to see if there's
        // any more information
        if (read == buffer.Length)
        {
          int nextByte = stream.ReadByte();

          // End of stream? If so, we're done
          if (nextByte == -1)
          {
            return buffer;
          }

          // Nope. Resize the buffer, put in the byte we've just
          // read, and continue
          byte[] newBuffer = new byte[buffer.Length * 2];
          Array.Copy(buffer, newBuffer, buffer.Length);
          newBuffer[read] = (byte)nextByte;
          buffer = newBuffer;
          read++;
        }
      }
      // Buffer is now too big. Shrink it.
      byte[] ret = new byte[read];
      Array.Copy(buffer, ret, read);
      return ret;
    }
    private void WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
    {
      context.Response.AddHeader("Vary", "Accept");
      try
      {
        if (context.Request["HTTP_ACCEPT"].Contains("application/json"))
          context.Response.ContentType = "application/json";
        else
          context.Response.ContentType = "text/plain";
      }
      catch
      {
        context.Response.ContentType = "text/plain";
      }

      var jsonObj = _JavaScriptSerializer.Serialize(statuses.ToArray());
      context.Response.Write(jsonObj);
    }
    #endregion

    #region delete
    private void DeleteFile(HttpContext context)
    {
      int attachmentId;
      if (int.TryParse(context.Request.QueryString.Get("f"), out attachmentId))
      {
        var report = InstanceContainer.ArticleAttachmentManager.DeleteTemporaryFile(attachmentId, this.User.UserId);
        if (report.Status != Workmate.Components.Contracts.DataRepositoryActionStatus.Success)
        {
          // TODO (Roman): errorhandling
        }
      }
    }
    #endregion

    #region uploads
    private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
    {
      IUserBasic user = context.User.Identity as IUserBasic;
      HttpPostedFile httpPostedFile;
      for (int i = 0; i < context.Request.Files.Count; i++)
      {
        httpPostedFile = context.Request.Files[i];

        ArticleAttachment articleAttachment = new ArticleAttachment(this.RequestContextData.ApplicationThemeInfo.ApplicationId, user);

        articleAttachment.Content = ReadStream(httpPostedFile.InputStream, httpPostedFile.ContentLength);
        articleAttachment.ContentSize = httpPostedFile.ContentLength;
        articleAttachment.ContentType = httpPostedFile.ContentType;
        articleAttachment.FileName = httpPostedFile.FileName;

        UrlHelper urlHelper = new UrlHelper(context.Request.RequestContext);
        string action = urlHelper.RouteUrl(MagicStrings.FormatRouteName(
          this.RequestContextData.ApplicationThemeInfo.ApplicationGroup, "Articles_FileUploadHandler"));
        
        var report = InstanceContainer.ArticleAttachmentManager.CreateTemporaryFile(articleAttachment);
        if (report.Status != Workmate.Components.Contracts.DataRepositoryActionStatus.Success)
        {
          // TODO (Roman): errorhandling
          statuses.Add(new FilesStatus(articleAttachment, report.Message, action));
        }
        else
          statuses.Add(new FilesStatus(articleAttachment, action));
      }
    }
    private void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> statuses)
    {
      // TODO (Roman): do we need this?

      //if (context.Request.Files.Count != 1) 
      //  throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");

      //IUserBasic user = context.User.Identity as IUserBasic;
      //Stream inputStream = context.Request.Files[0].InputStream;

      //ArticleAttachment articleAttachment = new ArticleAttachment(user);
      
      //HttpPostedFile httpPostedFile = context.Request.Files[0];
      //articleAttachment.Content = ReadStream(httpPostedFile.InputStream, httpPostedFile.ContentLength);
      //articleAttachment.ContentSize = httpPostedFile.ContentLength;
      //articleAttachment.ContentType = httpPostedFile.ContentType;
      //articleAttachment.FileName = httpPostedFile.FileName;

      ////using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
      ////{
      ////  var buffer = new byte[1024];

      ////  var l = inputStream.Read(buffer, 0, 1024);
      ////  while (l > 0)
      ////  {
      ////    fs.Write(buffer, 0, l);
      ////    l = inputStream.Read(buffer, 0, 1024);
      ////  }
      ////  fs.Flush();
      ////  fs.Close();
      ////}
      //statuses.Add(new FilesStatus(articleAttachment, null, null));

      throw new NotImplementedException();
    }

    private void UploadFile(HttpContext context)
    {
      var statuses = new List<FilesStatus>();
      var headers = context.Request.Headers;

      if (string.IsNullOrEmpty(headers["X-File-Name"]))
      {
        UploadWholeFile(context, statuses);
      }
      else
      {
        UploadPartialFile(headers["X-File-Name"], context, statuses);
      }

      WriteJsonIframeSafe(context, statuses);
    }
    #endregion

    #region responses
    private void DeliverFile(HttpContext context)
    {
      //var filename = context.Request["f"];
      //var filePath = StorageRoot + filename;

      //if (File.Exists(filePath))
      //{
      //  context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
      //  context.Response.ContentType = "application/octet-stream";
      //  context.Response.ClearContent();
      //  context.Response.WriteFile(filePath);
      //}
      //else
      //  context.Response.StatusCode = 404;

      throw new NotImplementedException();
    }
    private void ListCurrentFiles(HttpContext context)
    {
      //var files =
      //    new DirectoryInfo(StorageRoot)
      //        .GetFiles("*", SearchOption.TopDirectoryOnly)
      //        .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
      //        .Select(f => new FilesStatus(f))
      //        .ToArray();

      //string jsonObj = js.Serialize(files);
      //context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
      //context.Response.Write(jsonObj);
      //context.Response.ContentType = "application/json";

      throw new NotImplementedException();
    }
    #endregion

    // Handle request based on method
    private void HandleMethod(HttpContext context)
    {
      switch (context.Request.HttpMethod)
      {
        case "HEAD":
        case "GET":
          if (!string.IsNullOrEmpty(context.Request["f"]))
            DeliverFile(context);
          else
            ListCurrentFiles(context);
          break;

        case "POST":
        case "PUT":
          UploadFile(context);
          break;

        case "DELETE":
          DeleteFile(context);
          break;

        case "OPTIONS":
          ReturnOptions(context);
          break;

        default:
          context.Response.ClearHeaders();
          context.Response.StatusCode = 405;
          break;
      }
    }

    #region IHttpHandler Members

    public override void ProcessAuthorizedRequest(HttpContext context)
    {
      context.Response.AddHeader("Pragma", "no-cache");
      context.Response.AddHeader("Cache-Control", "private, no-cache");

      HandleMethod(context);
    }

    #endregion

    #region constructors
    public ArticlesFileUploadHandler()
    {
      _JavaScriptSerializer = new JavaScriptSerializer();
      _JavaScriptSerializer.MaxJsonLength = 41943040;
    }
    #endregion
  }
}
