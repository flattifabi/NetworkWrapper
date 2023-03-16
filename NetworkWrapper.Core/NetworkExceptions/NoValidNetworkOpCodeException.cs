using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkWrapper.Core.NetworkExceptions
{
    [Serializable]
    public class NoValidNetworkOpCodeException : Exception
    {
        public NoValidNetworkOpCodeException() { }
        public NoValidNetworkOpCodeException(string message) : base(message) { }
        public NoValidNetworkOpCodeException(string message, Exception inner) : base(message, inner) { }
    }
}
