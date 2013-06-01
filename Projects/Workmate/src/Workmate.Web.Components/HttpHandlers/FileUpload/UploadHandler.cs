using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System;
using System.Web.Routing;

namespace Workmate.Web.Components.HttpHandlers.FileUpload
{
  // TODO (roman): move all this to web components project
  // TODO (roman): remove hard coded location dependencies
  public class FilesStatus
  {
    public const string HandlerPath = "/wm/Handlers/Articles/";

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

    public FilesStatus() { }

    public FilesStatus(FileInfo fileInfo) { SetValues(fileInfo.Name, (int)fileInfo.Length, fileInfo.FullName); }

    public FilesStatus(string fileName, int fileLength, string fullPath) { SetValues(fileName, fileLength, fullPath); }

    private void SetValues(string fileName, int fileLength, string fullPath)
    {
      name = fileName;
      type = "image/png";
      size = fileLength;
      progress = "1.0";
      url = HandlerPath + "FileUploadHandler.ashx?f=" + fileName;
      delete_url = HandlerPath + "FileUploadHandler.ashx?f=" + fileName;
      delete_type = "DELETE";

      // TODO (Roman): remove path stuff
      var ext = ".txt"; // Path.GetExtension(fullPath);
      thumbnail_url = "na.img";
      //var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
      //if (fileSize > 3 || !IsImage(ext)) 
      //  thumbnail_url = "/Content/img/generalFile.png";
      //else 
      //  thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath);
    }

    private bool IsImage(string ext)
    {
      return ext == ".gif" || ext == ".jpg" || ext == ".png";
    }

    private string EncodeFile(string fileName)
    {
      return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
    }

    static double ConvertBytesToMegabytes(long bytes)
    {
      return (bytes / 1024f) / 1024f;
    }
  }

  /// <summary>
  /// Summary description for UploadHandler
  /// </summary>
  public class UploadHandler : IRouteHandler, IHttpHandler
  {
    private readonly JavaScriptSerializer js;

    private string StorageRoot = @"C:\temp\";

    public UploadHandler()
    {
      js = new JavaScriptSerializer();
      js.MaxJsonLength = 41943040;
    }

    public bool IsReusable { get { return false; } }

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return this;
    }
    public void ProcessRequest(HttpContext context)
    {
      context.Response.AddHeader("Pragma", "no-cache");
      context.Response.AddHeader("Cache-Control", "private, no-cache");

      HandleMethod(context);
    }

    // Handle request based on method
    private void HandleMethod(HttpContext context)
    {
      switch (context.Request.HttpMethod)
      {
        case "HEAD":
        case "GET":
          if (GivenFilename(context)) DeliverFile(context);
          else ListCurrentFiles(context);
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

    private static void ReturnOptions(HttpContext context)
    {
      context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS");
      context.Response.StatusCode = 200;
    }

    // Delete file from the server
    private void DeleteFile(HttpContext context)
    {
      var filePath = StorageRoot + context.Request["f"];
      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }
    }

    // Upload file to the server
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

    // Upload partial file
    private void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> statuses)
    {
      if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
      var inputStream = context.Request.Files[0].InputStream;
      var fullName = StorageRoot + Path.GetFileName(fileName);

      using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
      {
        var buffer = new byte[1024];

        var l = inputStream.Read(buffer, 0, 1024);
        while (l > 0)
        {
          fs.Write(buffer, 0, l);
          l = inputStream.Read(buffer, 0, 1024);
        }
        fs.Flush();
        fs.Close();
      }
      statuses.Add(new FilesStatus(new FileInfo(fullName)));
    }

    private byte[] ReadStrean(Stream stream, int initialLength)
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
    // Upload entire file
    private void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
    {
      for (int i = 0; i < context.Request.Files.Count; i++)
      {
        var file = context.Request.Files[i];

        var fullPath = StorageRoot + Path.GetFileName(file.FileName);

        byte[] byteArray = ReadStrean(file.InputStream, file.ContentLength);
        // TODO (Roman): store to database here

        string fullName = Path.GetFileName(file.FileName);
        statuses.Add(new FilesStatus(fullName, file.ContentLength, fullPath));
      }
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

      var jsonObj = js.Serialize(statuses.ToArray());
      context.Response.Write(jsonObj);
    }

    private static bool GivenFilename(HttpContext context)
    {
      return !string.IsNullOrEmpty(context.Request["f"]);
    }

    private void DeliverFile(HttpContext context)
    {
      var filename = context.Request["f"];
      var filePath = StorageRoot + filename;

      if (File.Exists(filePath))
      {
        context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
        context.Response.ContentType = "application/octet-stream";
        context.Response.ClearContent();
        context.Response.WriteFile(filePath);
      }
      else
        context.Response.StatusCode = 404;
    }

    private void ListCurrentFiles(HttpContext context)
    {
      var files =
          new DirectoryInfo(StorageRoot)
              .GetFiles("*", SearchOption.TopDirectoryOnly)
              .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
              .Select(f => new FilesStatus(f))
              .ToArray();

      string jsonObj = js.Serialize(files);
      context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
      context.Response.Write(jsonObj);
      context.Response.ContentType = "application/json";
    }

  }
}
