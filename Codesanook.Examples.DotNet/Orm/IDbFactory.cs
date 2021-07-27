using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Codesanook.Examples.DotNet.Orm 
{
    public interface IDbFactory : IDisposable
    {
        EFDbCotext GetCurrentDataContext { get; }
        /// <summary>
        /// Get OceanEntities by new() (this method will be used directly when working with asynchronous operation)
        /// </summary>
        EFDbCotext GetNewDataContext { get; }
        void ClearConnection(IDbContextTransaction transaction);
        void ClearDataContext();
        void ClearSession();
    }
}
