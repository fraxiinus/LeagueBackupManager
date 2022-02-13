using System;

namespace JVCS
{
    class VCSException : Exception
    {
        public VCSException(string exMsg) : base(exMsg)
        {
        }
    }
}