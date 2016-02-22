using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Craswell.Models;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;

namespace Craswell.DataAccess
{
    /// <summary>
    /// Models the persistence layer.
    /// </summary>
    public class PersistenceLayer : Disposable, IPersistenceLayer
    {
        /// <summary>
        /// The connection string used to access the data store.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The database type.
        /// </summary>
        private readonly DatabaseType dbType;

        /// <summary>
        /// The assemblies containing persistent models.
        /// </summary>
        private List<Assembly> modelAssemblies = new List<Assembly>();

        /// <summary>
        /// The session factory.
        /// </summary>
        private ISessionFactory sessionFactory;

        /// <summary>
        /// The NHibernate configuration.
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// Initializes a new instance of the PersistenceLayer class.
        /// </summary>
        /// <param name="connectionString">The connection string used to access the data store.</param>
        public PersistenceLayer(string connectionString, DatabaseType dbType)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            this.connectionString = connectionString;
            this.dbType = dbType;
        }

        /// <summary>
        /// Gets the session factory.
        /// </summary>
        public ISessionFactory SessionFactory
        {
            get
            {
                if (this.sessionFactory == null)
                {
                    this.CreateSessionFactory();
                }

                return this.sessionFactory;
            }
        }

        /// <summary>
        /// Adds an assembly to the list of assemblies from which mapping
        /// files should be read.
        /// </summary>
        /// <param name="modelAssembly">The model assembly to be added.</param>
        public void AddAssembly(Assembly modelAssembly)
        {
            if (modelAssembly == null)
            {
                throw new ArgumentNullException("modelAssembly");
            }

            this.modelAssemblies.Add(modelAssembly);
            this.ConfigureDatastore();
            this.ConfigureAssemblies();
            this.CreateSessionFactory();
        }

        /// <summary>
        /// Disposes of managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether the instance is disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.sessionFactory != null)
                {
                    this.sessionFactory.Dispose();
                    this.sessionFactory = null;
                }
            }
        }

        /// <summary>
        /// Configures the data store.
        /// </summary>
        private void ConfigureDatastore()
        {
            this.configuration = new Configuration();

            this.configuration = this.configuration.DataBaseIntegration(dbi =>
            {
                dbi.ConnectionString = this.connectionString;
                switch (this.dbType)
                {
                    case DatabaseType.MySql:
                        dbi.Dialect<MySQL55InnoDBDialect>();
                        break;

                    case DatabaseType.SQLite:
                        dbi.Dialect<SQLiteDialect>();
                        break;

                    default:
                        dbi.Dialect<MsSql2008Dialect>();
                        break;
                }
            });
        }

        /// <summary>
        /// Configures the persistence layer.
        /// </summary>
        private void ConfigureAssemblies()
        {
            foreach (var assembly in this.modelAssemblies)
            {
                this.configuration = this.configuration.AddAssembly(assembly);
            }
        }

        /// <summary>
        /// Creates the session factory.
        /// </summary>
        private void CreateSessionFactory()
        {
            if (this.modelAssemblies.Count < 1)
            {
                throw new PersistenceLayerException("There have been no assemblies added from which mappings should be read.");
            }

            if (this.configuration == null)
            {
                this.ConfigureDatastore();
                this.ConfigureAssemblies();
            }

            this.sessionFactory = this.configuration.BuildSessionFactory();
        }
    }
}
