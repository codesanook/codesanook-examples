using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSanook.Examples.CSharp.Orm 
{
    public interface IDbFactory : IDisposable
    {
        OceanEntities GetCurrentDataContext { get; }
        /// <summary>
        /// Get OceanEntities by new() (this method will be used directly when working with asynchronous operation)
        /// </summary>
        OceanEntities GetNewDataContext { get; }
        void ClearConnection(DbContextTransaction transaction);
        void ClearDataContext();
        void ClearSession();
    }
}
