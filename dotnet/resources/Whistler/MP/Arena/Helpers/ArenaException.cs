using System;

namespace Whistler.MP.Arena.Helpers
{
    public class ArenaException : Exception
    {
        public ArenaException(string message) : base(message)
        { }
    }
}