using System;

namespace SimpleCL.Models.Exceptions
{
    public class EntityParseException : SystemException
    {
        private const string ErrorMsg = "Couldn't parse entity with id: ";
        public EntityParseException(uint id) : base(ErrorMsg + id)
        {
        }
    }
}