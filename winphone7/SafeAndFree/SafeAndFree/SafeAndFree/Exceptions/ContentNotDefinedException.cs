using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SafeAndFree.Exceptions
{
    /// <summary>
    /// This exception is meant to be thrown when an object
    /// needs to have a reference to a ContentManager object,
    /// but does not have one by the time the ContentManager is
    /// needed.
    /// </summary>
    class ContentNotDefinedException : Exception
    {
        /// <summary>
        /// Constructor to report the default message.
        /// </summary>
        public ContentNotDefinedException() : base("The ContentManager object has not been defined.")
        {
        }

        /// <summary>
        /// Constructor to report the given message.
        /// </summary>
        /// <param name="message">The message to report.</param>
        public ContentNotDefinedException(string message) : base(message)
        {
        }
    }
}
