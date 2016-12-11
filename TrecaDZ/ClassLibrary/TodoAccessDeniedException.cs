using System;
using System.Runtime.Serialization;

namespace ClassLibrary
{
 
    internal class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException()
        {
        }

        public TodoAccessDeniedException(string message) : base(message)
        {
        }
    }
}