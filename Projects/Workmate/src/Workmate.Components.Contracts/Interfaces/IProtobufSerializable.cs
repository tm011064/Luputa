using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Workmate.Components.Contracts
{
  public interface IProtobufSerializable
  {
    byte[] Serialize();
  }
}
