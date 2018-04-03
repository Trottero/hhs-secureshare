using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureShare.Webapp.Extensions
{
    public class FaceAuthenticationException : Exception
    {
        public FaceAuthenticationException(string message) : base(message)
        {
        }
    }
}
