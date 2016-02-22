using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Craswell.Models;
using Craswell.Models.IdentityManagement;
using NHibernate.Criterion;

namespace Craswell.DataAccess
{
    /// <summary>
    /// Models the identity repository.
    /// </summary>
    public class IdentityRepository : EncryptedRepository<Identity>
    {
        /// <summary>
        /// Initializes a new instance of the IdentityRepository class.
        /// </summary>
        /// <param name="encryptionPassphrase">The encryption passphrase.</param>
        /// <param name="persistenceLayer">The persistence layer.</param>
        public IdentityRepository(
            string encryptionPassphrase,
            IPersistenceLayer persistenceLayer)
            : base(encryptionPassphrase, persistenceLayer)
        {
        }

        /// <summary>
        /// Encrypts the identity model.
        /// </summary>
        /// <param name="model">The identity model to be encrypted.</param>
        /// <returns>The identity model with its members encrypted.</returns>
        protected override Identity Encrypt(Identity model)
        {
            model.AssociatedResource = this.EncryptText(model.AssociatedResource);
            model.Description = this.EncryptText(model.Description);
            model.Notes = this.EncryptText(model.Notes);
            model.Password = this.EncryptText(model.Password);
            model.Title = this.EncryptText(model.Title);
            model.Username = this.EncryptText(model.Username);

            return model;
        }

        /// <summary>
        /// Decrypts the identity model.
        /// </summary>
        /// <param name="model">The identity model to be decrypted.</param>
        /// <returns>The identity model with its members decrypted.</returns>
        protected override Identity Decrypt(Identity model)
        {
            model.AssociatedResource = this.DecryptText(model.AssociatedResource);
            model.Description = this.DecryptText(model.Description);
            model.Notes = this.DecryptText(model.Notes);
            model.Password = this.DecryptText(model.Password);
            model.Title = this.DecryptText(model.Title);
            model.Username = this.DecryptText(model.Username);

            return model;
        }
    }
}
