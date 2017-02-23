using System;

namespace Experion.Marina.Common
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class)]
    public class LoggerAttribute : Attribute
    {
        public readonly string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public LoggerAttribute(string name)
        {
            Name = name;
        }
    }
}
