using System;
using System.Collections.Generic;
using System.Linq;
using Winstanley.UnitOfWork.Interfaces;

namespace Winstanley.UnitOfWork
{
    public class SqlConnectionConfiguration : ISqlConnectionConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionConfiguration"/> class.
        /// </summary>
        /// <param name="value">The connection string.</param>
        /// <exception cref="System.ArgumentException">A database connection string has not been provided - value</exception>
        public SqlConnectionConfiguration(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("A database connection string has not been provided", nameof(value));

            DefaultValue = value;
            Values.Add("Default", value);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionConfiguration"/> class.
        /// </summary>
        /// <param name="values">The connection strings.</param>
        /// <exception cref="System.ArgumentException">
        /// A database connection string has not been provided - values
        /// or
        /// A database connection string has not been provided - values
        /// </exception>
        public SqlConnectionConfiguration(Dictionary<string, string> values)
        {
            if (!values.Any())
                throw new ArgumentException("A database connection string has not been provided", nameof(values));

            if (string.IsNullOrWhiteSpace(values.First().Value))
                throw new ArgumentException("A database connection string has not been provided", nameof(values));

            DefaultValue = values.First().Value;
            Values = values;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionConfiguration"/> class.
        /// </summary>
        /// <param name="defaultValue">The default connection string.</param>
        /// <param name="values">The connection strings.</param>
        /// <exception cref="System.ArgumentException">
        /// A database connection string has not been provided - defaultValue
        /// or
        /// A database connection string has not been provided - values
        /// or
        /// A database connection string has not been provided - values
        /// </exception>
        public SqlConnectionConfiguration(string defaultValue, Dictionary<string, string> values)
        {
            if (string.IsNullOrWhiteSpace(defaultValue))
                throw new ArgumentException("A database connection string has not been provided", nameof(defaultValue));

            if (!values.Any())
                throw new ArgumentException("A database connection string has not been provided", nameof(values));

            if (string.IsNullOrWhiteSpace(values.First().Value))
                throw new ArgumentException("A database connection string has not been provided", nameof(values));

            Values = values;
            DefaultValue = defaultValue;
        }


        /// <summary>
        /// Gets the default connection string.
        /// </summary>
        /// <value>
        /// The default connection string.
        /// </value>
        public string DefaultValue { get; }


        /// <summary>
        /// Gets the connection strings.
        /// </summary>
        /// <value>
        /// The dictionary of connection strings, accessible by their keys.
        /// </value>
        public Dictionary<string, string> Values { get; } = new();
    }
}