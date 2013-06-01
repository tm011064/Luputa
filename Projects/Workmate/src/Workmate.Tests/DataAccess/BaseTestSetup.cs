using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workmate.Data;
using System.IO;
using System.Reflection;
using System.Configuration;
using NUnit.Framework;
using System.Diagnostics;

namespace Workmate.Tests.DataAccess
{
  public abstract class BaseTestSetup
  {
    protected abstract IDataStore DataStore { get; }

    protected string ApplicationPath { get; private set; }

    public BaseTestSetup()
    {
      this.ApplicationPath = new FileInfo(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", string.Empty)).DirectoryName;
      log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Combine(this.ApplicationPath, ConfigurationManager.AppSettings["log4netConfigPath"])));
    }
  }
}
