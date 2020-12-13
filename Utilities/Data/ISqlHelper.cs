using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Utilities.Data
{
    public interface ISqlHelper
    {
        DataSet Query(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task<DataSet> QueryAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        IList<T> Query<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task<IList<T>> QueryAsync<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        T QuerySingle<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task<T> QuerySingleAsync<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        object ExecuteScalar(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task<object> ExecuteScalarAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        int ExecuteNonQuery(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters);
        Task<int> ExecuteNonQueryAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters);

    }
}
