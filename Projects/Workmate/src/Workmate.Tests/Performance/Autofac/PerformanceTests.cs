using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using System.Diagnostics;

namespace Workmate.Tests.Performance.Autofac
{
  public class PerformanceTests
  {
    static class MyCustomContainer
    {
      public static MyObjectManager MyObjectManager { get; set; }
    }

    private IContainer _Container;

    public interface IMyObject
    {
      int TestProperty { get; }
    }

    public class MyObject : IMyObject
    {
      public int TestProperty { get; private set; }

      public MyObject(Random random)
      {
        this.TestProperty = random.Next();
      }
    }

    public class MyObjectManager
    {
      public IMyObject MyObject { get; set; }

      public MyObjectManager(IMyObject myObject)
      {
        MyObject = myObject;
      }
    }


    private double ResolveItems(int totalLoops)
    {
      Stopwatch sw = new Stopwatch();

      MyObjectManager myObjectManager;
      for (int i = 0; i < totalLoops; i++)
      {
        sw.Start();
        myObjectManager = _Container.Resolve<MyObjectManager>();
        sw.Stop();
        //if (i % 100000 == 0)
        //  Trace.WriteLine(myObjectManager.MyObject.TestProperty.ToString());
      }

      return (double)sw.ElapsedMilliseconds / (double)totalLoops;
    }

    private double CustomResolveItems(int totalLoops)
    {
      Stopwatch sw = new Stopwatch();

      MyObjectManager myObjectManager;
      for (int i = 0; i < totalLoops; i++)
      {
        sw.Start();
        myObjectManager = MyCustomContainer.MyObjectManager;
        sw.Stop();
      }

      return (double)sw.ElapsedMilliseconds / (double)totalLoops;
    }

    public void Test_Instanciation()
    {
      var builder = new ContainerBuilder();
      Random random = new Random();

      MyObject myObject = new MyObject(random);

      builder.RegisterInstance<MyObject>(myObject).As<IMyObject>();
      builder.RegisterInstance<MyObjectManager>(new MyObjectManager(myObject));

      _Container = builder.Build();

      MyCustomContainer.MyObjectManager = new MyObjectManager(myObject);

      Trace.WriteLine("Total ms: " + ResolveItems(100));
      Trace.WriteLine("Total ms: " + CustomResolveItems(100));

      for (int i = 0; i < 10; i++)
      {
        double a = ResolveItems(10000);
        double b = CustomResolveItems(10000);

        Trace.WriteLine("Total ms: " + a.ToString("n4") + " AND " + b.ToString("N4") + " -> " + (((a / b) - 1) * 100).ToString("N2") + "% slower");
      }
    }
  }
}
