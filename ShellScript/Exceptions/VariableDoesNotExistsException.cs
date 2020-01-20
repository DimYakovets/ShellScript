using System;
using System.Runtime.Serialization;
                
namespace ShellScript.Exceptions
{
    [Serializable]
    internal class VariableDoesNotExistsException : Exception
    {
        public VariableDoesNotExistsException() : base($"Variable does not exists.")
        {
        }

        public VariableDoesNotExistsException(string message) : base($"Variable {message} does not exists.")
        {

        }
    }
}