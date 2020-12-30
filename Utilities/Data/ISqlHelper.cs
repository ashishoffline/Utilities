using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Data
{
    /// <summary>
    /// Generic ADO.NET Implementation for Database access.
    /// </summary>
    public interface ISqlHelper
    {
        /// <summary>
        /// Executes the SqlQuery and returns an <see cref="System.Data.DataSet"/>.
        /// </summary>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <returns>returns <see cref="System.Data.DataSet"/> containing <see cref="System.Data.DataTable"/> for every ResultSet.</returns>
        DataSet Query(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel);

        /// <summary>
        /// This is the asynchronous version of <see cref="Query(string, CommandType, IEnumerable{DbParameter}, IsolationLevel)"/>. Executes the SqlQuery and returns an <see cref="System.Data.DataSet"/>.
        /// </summary>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <param name="cancellationToken">An optional token to cancel the asynchronous operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<DataSet> QueryAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the SqlQuery and returns the data of first result set returned by the query.
        /// It uses <see cref="System.Linq.Expressions.Expression"/> to build the mappingFunc <see cref="Func{DbDataReader, T}"/> It does CaseInsenstivie Name comparison.
        /// And this <see cref="Func{DbDataReader, T}"/> is cached, So It is as fast as manually written <see cref="Func{DbDataReader, T}"/>.
        /// But you must not use This method If parameter Value is hardcoded in the Query directly and parameter value changes everytime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <returns>returns <see cref="System.Collections.Generic.IList{T}"/> containing the data from the First ResultSet of the Sql.</returns>
        IList<T> Query<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Executes the SqlQuery and returns the data of first result set returned by the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="mappingFunc"></param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <returns>returns <see cref="System.Collections.Generic.IList{T}"/> containing the data from the First ResultSet of the Sql.</returns>
        IList<T> Query<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// This is the asynchronous version of <see cref="Query{T}(string, CommandType, IEnumerable{DbParameter}, Func{DbDataReader, T}, IsolationLevel)"/>. Executes the SqlQuery and returns the data of first result set returned by the query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="mappingFunc"></param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <param name="cancellationToken">An optional token to cancel the asynchronous operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<IList<T>> QueryAsync<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the SqlQuery and returns the first row in the result set returned by the query.All other rows are ignored.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="mappingFunc"></param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <returns>returns <see cref="{T}"/> which contains first rows data.</returns>
        T QuerySingle<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// This is the asynchronous version of <see cref="QuerySingle{T}(string, CommandType, IEnumerable{DbParameter}, Func{DbDataReader, T}, IsolationLevel)"/>. Executes the SqlQuery and returns the first row in the result set returned by the query.All other rows are ignored.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="mappingFunc"></param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <param name="cancellationToken">An optional token to cancel the asynchronous operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<T> QuerySingleAsync<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <returns>The first column of the first row in the result set.</returns>
        object ExecuteScalar(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// This is the asynchronous version of <see cref="ExecuteScalar(string, CommandType, IEnumerable{DbParameter}, IsolationLevel)"/>.Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored.
        /// </summary>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="isolationLevel">An optional one of the enumeration values that specifies the isolation level for the transaction to use. The default value is <see cref="IsolationLevel.ReadCommitted"/>.</param>
        /// <param name="cancellationToken">An optional token to cancel the asynchronous operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<object> ExecuteScalarAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes the SqlQuery against the Database and returns the number of rows affected.
        /// </summary>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters);

        /// <summary>
        /// This is the asynchronous version of <see cref="ExecuteNonQuery(string, CommandType, IEnumerable{DbParameter})"/>. Executes the SqlQuery against the Database and returns the number of rows affected.
        /// </summary>
        /// <param name="sqlQuery">SqlQuery Text or StoredProcedure Name.</param>
        /// <param name="commandType">One of the enumeration values that specifies how a command string is interpreted.</param>
        /// <param name="dbParameters">The parameters of the SQL statement or stored procedure.</param>
        /// <param name="cancellationToken">An optional token to cancel the asynchronous operation. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<int> ExecuteNonQueryAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, CancellationToken cancellationToken = default);

    }
}
