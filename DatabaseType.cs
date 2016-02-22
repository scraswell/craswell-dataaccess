using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craswell.DataAccess
{
    /// <summary>
    /// Supported database types.
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Microsoft SQL Server database store.
        /// </summary>
        MSSql,

        /// <summary>
        /// MySQL database store.
        /// </summary>
        MySql,

        /// <summary>
        /// SQLite database store.
        /// </summary>
        SQLite
    }
}
