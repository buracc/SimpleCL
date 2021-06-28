using System;

namespace SimpleCL.Model.Exception
{
    public class EntityParseException : SystemException
    {
        public EntityParseException(string message) : base(message)
        {
        }
    }
}