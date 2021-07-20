using System;

namespace SimpleCL.Models.Exceptions
{
    public class SroClientNotFoundException : SystemException
    {
        public SroClientNotFoundException(string message) : base(message)
        {
        }
    }
}