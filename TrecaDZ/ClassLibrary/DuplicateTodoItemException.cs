using System;
using System.Runtime.Serialization;

namespace ClassLibrary
{

    internal class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException()
        {
        }

        public DuplicateTodoItemException(string message) : base(message)
        {
        }
    }
}