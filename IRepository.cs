using System;
using Craswell.Models;

namespace Craswell.DataAccess
{
    /// <summary>
    /// Defines the members available to a persistence repository implementation.
    /// </summary>
    public interface IRepository<T>
        : IDisposable where T
        : IDataModel, new()
    {
        /// <summary>
        /// Persists a new instance in the repository.
        /// </summary>
        /// <param name="dataModel">The new data model instance to make persistent.</param>
        /// <returns>The model with its identifier made to match that which was saved in the persistence store.</returns>
        T Create(T dataModel);

        /// <summary>
        /// Reads a model from the repository.
        /// </summary>
        /// <param name="dataModelId">The identifier of the model to be read from the repository.</param>
        T Read(uint dataModelId);

        /// <summary>
        /// Updates the model in the repository.
        /// </summary>
        /// <param name="dataModel">The model to be updated.</param>
        void Update(T dataModel);

        /// <summary>
        /// Deletes a model from the repository.
        /// </summary>
        /// <param name="dataModel">The model to be deleted from the repository.</param>
        void Delete(T dataModel);
    }
}
