using System;

namespace SimpleCL.Models.Exceptions
{
    public class SroClientLaunchException : SystemException
    {
        public SroClientLaunchException(string message) : base(message)
        {
        }
    }
}