using System;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not found.")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}