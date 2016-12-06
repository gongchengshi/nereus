using System;

namespace Nereus.Exceptions
{
   public class NereusException : Exception
   {
      public NereusException()
      {
      }

      public NereusException(string message) : base(message)
      {
      }

      public NereusException(string message, Exception innerException) : base(message, innerException) {}
   }


}