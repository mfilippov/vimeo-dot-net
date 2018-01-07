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
    /// Parameter dictionary
    /// </summary>
    public class ParameterDictionary : Dictionary<string, string>, IParameterProvider
    {
        /// <summary>
        /// ParameterDictionary is always considered valid, so no error is ever returned.
        /// </summary>
        /// <returns>null</returns>
        public string ValidationError()
        {
            return null;
        }

        /// <summary>
        /// Provides universal interface to retrieve parameter values.
        /// </summary>
        /// <returns>Returns all parameters as name/value pairs.</returns>
        public IDictionary<string, string> GetParameterValues()
        {
            return this;
        }
    }
}