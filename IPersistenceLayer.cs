using System;
using System.Reflection;

using NHibernate;

namespace Craswell.DataAccess
{
    /// <summary>
    /// Defines the members available to persistence layer implementations.
    /// </summary>
    public interface IPersistenceLayer : IDisposable
    {
        /// <summary>
        /// Gets the session factory.
        /// </summary>
        ISessionFactory SessionFactory { get; }

        /// <summary>
        /// Adds an assembly from which NHibernate mappings should be loaded.
        /// </summary>
        /// <param name="assembly">The model assembly.</param>
        void AddAssembly(Assembly assembly);
    }
}
