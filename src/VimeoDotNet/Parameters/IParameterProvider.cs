using System.Collections.Generic;

namespace VimeoDotNet.Parameters
{
    /// <summary>
    /// IParameterProvider
    /// </summary>
    public interface IParameterProvider
    {
        /// <summary>
        /// Performs validation and returns a description of the first error encountered.
        /// </summary>
        /// <returns>Description of first error, or null if none found.</returns>
        string ValidationError();

        /// <summary>
        /// Provides universal interface to retrieve parameter values.
        /// </summary>
        /// <returns>Returns all parameters as name/value pairs.</returns>
        IDictionary<string, string> GetParameterValues();
    }

    /// <summary>
    /// Class ParameterDictionary.
    /// Implements the <see cref="System.Collections.Generic.Dictionary{System.String, System.String}" />
    /// Implements the <see cref="VimeoDotNet.Parameters.IParameterProvider" />
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Dictionary{System.String, System.String}" />
    /// <seealso cref="VimeoDotNet.Parameters.IParameterProvider" />
    public class ParameterDictionary : Dictionary<string, string>, IParameterProvider
    {
        /// <inheritdoc />
        public string ValidationError()
        {
            return null;
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetParameterValues()
        {
            return this;
        }
    }
}