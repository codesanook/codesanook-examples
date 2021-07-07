using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace CodeSanook.Examples.DotNetCore.Orm 
{
    public class EFDbFactory : Disposable, IDbFactory
    {
        private HttpContent _httpContext;
        private const string _contextKey = "OceanDBContext";

        public EFDbFactory()
        {
            _httpContext = HttpContext.Current;

            //fake context added for not web based bindings
            if (HttpContext.Current == null)
            {
                _httpContext = OfflineContext.Instance.GetContext();
            }
        }

        public DbEntities GetCurrentDataContext
        {
            get
            {
                var dbCommandTimeOut = int.Parse(ConfigurationManager.AppSettings["DbCommandTimeOut"]);
                if (_httpContext.Session == null)
                {
                    if (!_httpContext.Items.Contains(_contextKey))
                    {
                        var db = GetNewDataContext;
                        db.Database.CommandTimeout = dbCommandTimeOut;
                        _httpContext.Items.Add(_contextKey, db);
                    }
                    else if (_httpContext.Items[_contextKey] == null)
                    {
                        var db = GetNewDataContext;
                        db.Database.CommandTimeout = dbCommandTimeOut;
                        _httpContext.Items[_contextKey] = db;
                    }

                    return (DbEntities)_httpContext.Items[_contextKey];
                }
                else
                {
                    if (_httpContext.Session[_contextKey] == null)
                    {
                        var db = GetNewDataContext;
                        db.Database.CommandTimeout = dbCommandTimeOut;
                        _httpContext.Session[_contextKey] = db;
                    }

                    return (DbEntities)_httpContext.Session[_contextKey];
                }
            }
        }

        public DbEntities GetNewDataContext
        {
            get
            {
                return new DbEntities();
            }
        }

        protected override void DisposeCore()
        {
            // close old connection
            if (GetCurrentDataContext != null)
            {
                if (GetCurrentDataContext.Database.Connection.State == ConnectionState.Open)
                    GetCurrentDataContext.Database.Connection.Close();
            }

            // clear old session for start new
            if (_httpContext.Session != null)
                _httpContext.Session[_contextKey] = null;
            else
                _httpContext.Items[_contextKey] = null;

            // clear memory
            GC.SuppressFinalize(this);
        }

        public void ClearConnection(DbContextTransaction transaction)
        {
            if (transaction != null)
            {
                DisposeCore();
            }
        }

        public void ClearDataContext()
        {
            if (GetCurrentDataContext != null)
            {
                if (GetCurrentDataContext.Database.Connection.State == ConnectionState.Open)
                    GetCurrentDataContext.Database.Connection.Close();

                GetCurrentDataContext.Dispose();
            }
        }

        public void ClearSession()
        {
            // clear old session for start new
            if (_httpContext.Session != null)
                _httpContext.Session[_contextKey] = null;
            else
                _httpContext.Items[_contextKey] = null;
        }
    }
}
