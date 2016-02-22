using System;
using Craswell.Encryption;
using Craswell.Models;

namespace Craswell.DataAccess
{
    /// <summary>
    /// Models an encrypted persistence repository.
    /// </summary>
    /// <typeparam name="T">The type of objects held in the repository.</typeparam>
    public abstract class EncryptedRepository<T>
        : Repository<T> where T
        : IDataModel, new()
    {
        /// <summary>
        /// The encryption passphrase.
        /// </summary>
        private readonly string encryptionPassphrase;

        /// <summary>
        /// The AES encryption tool.
        /// </summary>
        private AesEncryptionTool aesEncryptionTool = new AesEncryptionTool();

        /// <summary>
        /// Initializes a new instance of the EncryptedRepository class.
        /// </summary>
        /// <param name="encryptionPassphrase">The encryption passphrase.</param>
        /// <param name="persistenceLayer">The persistence layer.</param>
        public EncryptedRepository(
            string encryptionPassphrase,
            IPersistenceLayer persistenceLayer)
            : base(persistenceLayer)
        {
            if (string.IsNullOrEmpty(encryptionPassphrase))
            {
                throw new ArgumentNullException("encryptionPassphrase");
            }

            this.encryptionPassphrase = encryptionPassphrase;
        }

        /// <summary>
        /// Persists a new instance in the repository.
        /// </summary>
        /// <param name="model">The new data model instance to make persistent.</param>
        /// <returns>The model with its identifier made to match that which was saved in the persistence store.</returns>
        public override T Create(T model)
        {
            model = this.Encrypt(model);
            model = base.Create(model);

            return this.Decrypt(model);
        }

        /// <summary>
        /// Reads a model from the repository.
        /// </summary>
        /// <param name="modelId">The identifier of the model to be read from the repository.</param>
        public override T Read(uint modelId)
        {
            T model = base.Read(modelId);

            return this.Decrypt(model);
        }

        /// <summary>
        /// Updates the model in the repository.
        /// </summary>
        /// <param name="model">The model to be updated.</param>
        public override void Update(T model)
        {
            model = this.Encrypt(model);

            base.Update(model);
        }

        /// <summary>
        /// Deletes a model from the repository.
        /// </summary>
        /// <param name="model">The model to be deleted from the repository.</param>
        public override void Delete(T model)
        {
            model = this.Encrypt(model);

            base.Delete(model);
        }

        /// <summary>
        /// Encrypts a string of text.
        /// </summary>
        /// <param name="text">The text to be encrypted.</param>
        /// <returns>The resulting ciphertext.</returns>
        protected virtual string EncryptText(string text)
        {
            return this.aesEncryptionTool
                .EncryptText(text, this.encryptionPassphrase);
        }

        /// <summary>
        /// Encrypts a string of text.
        /// </summary>
        /// <param name="text">The text to be encrypted.</param>
        /// <returns>The resulting ciphertext.</returns>
        protected virtual string DecryptText(string text)
        {
            return this.aesEncryptionTool
                .DecryptText(text, this.encryptionPassphrase);
        }

        /// <summary>
        /// Encrypts the members of a model for storage.
        /// </summary>
        /// <param name="model">The model for which members should be encrypted.</param>
        /// <returns>The model with its members encrypted.</returns>
        protected abstract T Encrypt(T model);

        /// <summary>
        /// Decrypts the members of a model for storage.
        /// </summary>
        /// <param name="model">The model for which members should be decrypted.</param>
        /// <returns>The model with its members decrypted.</returns>
        protected abstract T Decrypt(T model);
    }
}
