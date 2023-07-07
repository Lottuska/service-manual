using System;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException() : base("Error occurred in the database.")
        {
        }

        public DatabaseException(string message) : base(message)
        {
        }
    }
}