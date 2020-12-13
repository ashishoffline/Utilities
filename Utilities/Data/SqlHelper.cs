using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace Utilities.Data
{
    public class SqlHelper : ISqlHelper
    {
        #region ErrorMessage
        private const string Empty = "It is either null, Empty or WhiteSpace";
        #endregion

        private readonly string _connectionString;
        private readonly DbProviderFactory _dbProviderFactory;
        /// <summary>
        /// Create Instance of the SqlHelper.
        /// </summary>
        /// <param name="providerInvariantName">DbProvider InvariantName.Example -
        /// Microsoft.Data.SqlClient for SQL Server
        /// Npgsql for PostgreSQL</param>
        /// <param name="connectionString">ConnectionString as per the Database standard.</param>
        public SqlHelper(string providerInvariantName, string connectionString)
        {
            _dbProviderFactory = GetDbProviderFactory(providerInvariantName) ?? throw new ArgumentException("Either DbProvider InvariantName is wrong or Assembly/Package is not Installed.", nameof(providerInvariantName));
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), Empty);
            else
                _connectionString = connectionString;
        }
        /// <summary>
        /// Create Instance of the SqlHelper.
        /// </summary>
        /// <param name="dbProviderFactory">Database specific DbProviderFactory Instance</param>
        /// <param name="connectionString">ConnectionString as per the Database standard.</param>
        public SqlHelper(DbProviderFactory dbProviderFactory, string connectionString)
        {
            _dbProviderFactory = dbProviderFactory ?? throw new ArgumentNullException(nameof(dbProviderFactory));
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), Empty);
            else
                _connectionString = connectionString;
        }
        #region Get Everything as DataSet
        public DataSet Query(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));
            DataSet result = new DataSet();
            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = command.ExecuteReader())
                        {
                            while (!reader.IsClosed)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.BeginLoadData();
                                dataTable.Load(reader);
                                dataTable.EndLoadData();
                                result.Tables.Add(dataTable);
                            }
                        }
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return result;
        }
        public async Task<DataSet> QueryAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));
            DataSet result = new DataSet();
            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (!reader.IsClosed)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.BeginLoadData();
                                dataTable.Load(reader);
                                dataTable.EndLoadData();
                                result.Tables.Add(dataTable);
                            }
                        }
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return result;
        } 
        #endregion

        #region Get MultipleRow from First ResultSet
        public IList<T> Query<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));
            IList<T> result = new List<T>();
            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            while (reader.Read())
                            {
                                result.Add(mappingFunc(reader));
                            }
                        }
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return result;
        }
        public async Task<IList<T>> QueryAsync<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));
            IList<T> result = new List<T>();
            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult))
                        {
                            while (reader.Read())
                            {
                                result.Add(mappingFunc(reader));
                            }
                        }
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return result;
        } 
        #endregion

        #region Get SingleRow
        public T QuerySingle<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));
            T result = default(T);
            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                result = mappingFunc(reader);
                            }
                        }
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return result;
        }
        public async Task<T> QuerySingleAsync<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));
            T result = default(T);
            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                result = mappingFunc(reader);
                            }
                        }
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return result;
        } 
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));
            object result = default;
            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        result = command.ExecuteScalar();
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return result;
        }
        public async Task<object> ExecuteScalarAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));
            object result = default;
            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        result = await command.ExecuteScalarAsync();
                        command.Transaction.Commit();
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return result;
        } 
        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters)
        {
            int recordCount;
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));

            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction();
                        recordCount = command.ExecuteNonQuery();
                        if (recordCount != -1)
                        {
                            command.Transaction.Commit();
                        }
                        else
                        {
                            command.Transaction.Rollback();
                        }
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return recordCount;
        }
        public async Task<int> ExecuteNonQueryAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters)
        {
            int recordCount;
            if (string.IsNullOrWhiteSpace(sqlQuery))
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            if (dbParameters == null)
                throw new ArgumentNullException(nameof(dbParameters));

            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.CommandTimeout = 120;
                        command.Transaction = dbConnection.BeginTransaction();
                        recordCount = await command.ExecuteNonQueryAsync();
                        if (recordCount != -1)
                        {
                            command.Transaction.Commit();
                        }
                        else
                        {
                            command.Transaction.Rollback();
                        }
                    }
                    catch (Exception)
                    {
                        command.Transaction?.Rollback();
                        throw;
                    }
                    finally
                    {
                        dbConnection?.Close();
                    }
                }
            }
            return recordCount;
        } 
        #endregion

        #region Private Methods
        private DbConnection CreateDbConnection()
        {
            DbConnection dbConnection = null;
            if (_dbProviderFactory != null)
            {
                dbConnection = _dbProviderFactory.CreateConnection();
            }

            if (dbConnection == null)
            {
                throw new InvalidOperationException("Failed to instantiate DB connection. Note: this is likely an issue with the provider, not the network connection.");
            }
            dbConnection.ConnectionString = _connectionString;
            try
            {
                dbConnection?.Open();
            }
            catch (Exception ex)
            {
                ex.Data["connectionString"] = _connectionString;
                throw;
            }
            finally
            {
                dbConnection?.Close();
            }
            return dbConnection;
        }
        private DbProviderFactory GetDbProviderFactory(string providerInvariantName)
        {
            bool providerExist = DbProviderFactories.TryGetFactory(providerInvariantName, out DbProviderFactory dbProviderFactory);
            if (providerExist)
                return dbProviderFactory;

            Assembly assembly = Assembly.Load(providerInvariantName);
            if (assembly != null)
            {
                Type type = null;
                foreach (var item in assembly.GetTypes())
                {
                    if (item.BaseType == typeof(DbProviderFactory) && item.Name.EndsWith("Factory"))
                    {
                        type = item;
                        break;
                    }
                }
                if (type != null)
                {
                    FieldInfo instanceField = type.GetField("Instance");
                    if (instanceField != null)
                    {
                        var instance = instanceField.GetValue(null);
                        if (instance != null && instance is DbProviderFactory)
                        {
                            return instance as DbProviderFactory;
                        }
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
