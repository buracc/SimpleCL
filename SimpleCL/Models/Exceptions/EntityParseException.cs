using System;

namespace SimpleCL.Models.Exceptions
{
    public class EntityParseException : SystemException
    {
        public EntityParseException(string message) : base(message)
        {
        }
    }
}