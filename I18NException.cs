using System;

namespace I18N
{
    public class I18NException : Exception
    {
        public I18NException(string message) : base(message)
        {
        }

        public I18NException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public I18NException()
        {
        }
    }
}