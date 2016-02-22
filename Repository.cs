using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Craswell.Models;
using NHibernate;

namespace Craswell.DataAccess
{
    /// <summary>
    /// Models an persistence repository.
    /// </summary>
    /// <typeparam name="T">The type of objects held in the repository.</typeparam>
    public abstract class Repository<T>
        : Disposable, IRepository<T> where T
        : IDataModel, new()
    {
        /// <summary>
        /// The persistence layer.
        /// </summary>
        private IPersistenceLayer persistenceLayer;

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <param name="persistenceLayer">The persistence layer used by the repository.</param>
        protected Repository(IPersistenceLayer persistenceLayer)
        {
            if (persistenceLayer == null)
            {
                throw new ArgumentNullException("persistenceLayer");
            }

            this.persistenceLayer = persistenceLayer;
            this.persistenceLayer.AddAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Gets the persistence layer session factory.
        /// </summary>
        protected ISessionFactory SessionFactory
        {
            get
            {
                return this.persistenceLayer.SessionFactory;
            }
        }

        /// <summary>
        /// Persists a new instance in the repository.
        /// </summary>
        /// <param name="model">The new data model instance to make persistent.</param>
        /// <returns>The model with its identifier made to match that which was saved in the persistence store.</returns>
        public virtual T Create(T model)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                model.Id = (uint)session.Save(model);
                tx.Commit();
            }

            return model;
        }

        /// <summary>
        /// Reads a model from the repository.
        /// </summary>
        /// <param name="modelId">The identifier of the model to be read from the repository.</param>
        public virtual T Read(uint modelId)
        {
            T model;

            using (var session = this.SessionFactory.OpenSession())
            {
                model = session.Get<T>(modelId);
            }

            return model;
        }

        /// <summary>
        /// Updates the model in the repository.
        /// </summary>
        /// <param name="model">The model to be updated.</param>
        public virtual void Update(T model)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.Update(model);
                tx.Commit();
            }
        }

        /// <summary>
        /// Deletes a model from the repository.
        /// </summary>
        /// <param name="model">The model to be deleted from the repository.</param>
        public virtual void Delete(T model)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.Delete(model);
                tx.Commit();
            }
        }

        /// <summary>
        /// Disposes of managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether the instance is disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.persistenceLayer != null)
                {
                    this.persistenceLayer.Dispose();
                    this.persistenceLayer = null;
                }
            }
        }
    }
}
