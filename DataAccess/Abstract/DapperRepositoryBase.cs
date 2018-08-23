using Dapper;
using DataAccess.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataAccess.Abstract
{
    public abstract class DapperRepositoryBase : IRepositoryBase
    {
        protected IDbConnection _connection = null;

        protected List<T> Query<T>(string storedProcName)
        {
            return Query<T>(storedProcName, null);
        }

        protected List<T> Query<T>(string storedProcName, object parameters)
        {
            return _connection.Query<T>(storedProcName, parameters, commandType: CommandType.StoredProcedure).ToList();
        }

        protected void Execute(string storedProcName)
        {
            Execute(storedProcName, null);
        }

        protected void Execute(string storedProcName, object parameters)
        {
            _connection.Execute(storedProcName, parameters, commandType: CommandType.StoredProcedure);
        }

        protected void Execute(string storedProcName, object parameters, IDbTransaction transaction)
        {
            _connection.Execute(storedProcName, parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
        }

        protected object ExecuteScalar(string storedProcName)
        {
            return ExecuteScalar(storedProcName, null);
        }

        protected object ExecuteScalar(string storedProcName, object parameters)
        {
            return _connection.ExecuteScalar(storedProcName, parameters, commandType: CommandType.StoredProcedure);
        }

        protected List<T> QueryTableView<T>(string query)
        {
            return _connection.Query<T>(query).ToList();
        }

        #region IDisposable
        public bool CanDispose { get; set; }

        public void Dispose(bool force)
        {
            this.CanDispose = true;
            Dispose();
        }

        public void Dispose()
        {
            if (_connection != null && this.CanDispose)
                _connection.Dispose();
        }
        #endregion
    }
}