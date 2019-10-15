using System;
using UnityEngine;

namespace FizzlePuzzle.Extension
{
    internal class FizzleException : UnityException
    {
        internal FizzleException(string message) : base(message)
        {
        }

        internal FizzleException(string message, Exception cause) : base(message + ", " + cause.Message + cause.StackTrace, cause)
        {
        }

        internal FizzleException(Exception cause) : base(cause.Message, cause)
        {
        }
    }
}
