using System.Collections.Generic;

namespace Winstanley.UnitOfWork.Interfaces
{
    public interface ISqlConnectionConfiguration
    {
        /// <summary>
        /// Gets the default connection string.
        /// </summary>
        /// <value>
        /// The default connection string.
        /// </value>
        string DefaultValue { get; }


        /// <summary>
        /// Gets the connection strings.
        /// </summary>
        /// <value>
        /// The dictionary of connection strings, accessible by their keys.
        /// </value>
        Dictionary<string, string> Values { get; }
    }
}
