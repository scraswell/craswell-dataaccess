using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Craswell.DataAccess
{
    /// <summary>
    /// Exceptions thrown by the persistence layer.
    /// </summary>
    [Serializable]
    public class PersistenceLayerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the PersistenceLayerException class.
        /// </summary>
        public PersistenceLayerException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PersistenceLayerException class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public PersistenceLayerException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PersistenceLayerException class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PersistenceLayerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PersistenceLayerException class.
        /// </summary>
        /// <param name="serializationInfo">The serialization information.</param>
        /// <param name="streamingContext">The streaming context information.</param>
        protected PersistenceLayerException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
