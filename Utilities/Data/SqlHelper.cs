using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Data
{
    public class SqlHelper : ISqlHelper
    {
        private static readonly ConcurrentDictionary<Tuple<string, Type>, object> cachedMappingFunc = new ConcurrentDictionary<Tuple<string, Type>, object>();

        #region ErrorMessage
        private const string Empty = "It is either null, Empty or WhiteSpace";
        #endregion

        private readonly string _connectionString;
        private readonly DbProviderFactory _dbProviderFactory;


        #region Constructors
        /// <summary>
        /// Create Instance of the SqlHelper.
        /// </summary>
        /// <param name="providerInvariantName">DbProvider InvariantName.Example -
        /// Microsoft.Data.SqlClient for SQL Server, 
        /// Npgsql for PostgreSQL</param>
        /// <param name="connectionString">ConnectionString as per the Database standard.</param>
        public SqlHelper(string providerInvariantName, string connectionString)
        {
            _dbProviderFactory = GetDbProviderFactory(providerInvariantName) ?? throw new ArgumentException("Either DbProvider InvariantName is wrong or Assembly/Package is not Installed.", nameof(providerInvariantName));
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), Empty);
            }
            else
            {
                _connectionString = connectionString;
            }
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
            {
                throw new ArgumentNullException(nameof(connectionString), Empty);
            }
            else
            {
                _connectionString = connectionString;
            }
        }
        #endregion

        #region Get Everything as DataSet
        public DataSet Query(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
        public async Task<DataSet> QueryAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
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

        #region GetData and map Using Expression API
        public IList<T> Query<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            #region Using Expression Api
                            Tuple<string, Type> identity = Tuple.Create(sqlQuery, typeof(T));
                            if (!cachedMappingFunc.TryGetValue(identity, out object serializer))
                            {
                                serializer = GetMapFunc<T>(reader);
                                cachedMappingFunc[identity] = serializer;
                            }
                            Func<DbDataReader, T> mapFunc = (Func<DbDataReader, T>)serializer;
                            #endregion

                            while (reader.Read())
                            {
                                result.Add(mapFunc(reader));
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

        private static Func<DbDataReader, T> GetMapFunc<T>(DbDataReader dataReader)
        {
            // declare Expression List
            List<Expression> exps = new List<Expression>();

            // Creating a parameter expression.
            ParameterExpression paramExp = Expression.Parameter(typeof(DbDataReader), "dataReader");

            // Creating an expression to hold a local variable.
            ParameterExpression targetExp = Expression.Variable(typeof(T), "obj");

            // Add expression tree assignment to the exp list
            exps.Add(Expression.Assign(targetExp, Expression.New(targetExp.Type)));

            //does int based lookup -> Not clear what is the use of this
            PropertyInfo indexerInfo = typeof(DbDataReader).GetProperty("Item", new[] { typeof(int) });

            // grab a collection of column names and Corresponding Index Ordinal from our data reader
            var columnNames = Enumerable.Range(0, dataReader.FieldCount).Select(i => new { Index = i, Name = dataReader.GetName(i) });

            // Get all the Public,Instance properties of Class(T)
            IEnumerable<PropertyInfo> propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetSetMethod() != null);

            // loop through all the column of DbDataReader and map to Class Property using ColumnName & Property name ignoring Case
            foreach (var column in columnNames)
            {
                foreach (PropertyInfo property in propertyInfos)
                {
                    if (string.Equals(property.Name, column.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        // build expression tree to map the column to to a Property of Class (T)
                        ConstantExpression columnIndexExp = Expression.Constant(column.Index, typeof(int));

                        IndexExpression propertyExp = Expression.MakeIndex(paramExp, indexerInfo, new[] { columnIndexExp });

                        ParameterExpression cellValueExp = Expression.Variable(typeof(object), "cellValue");

                        ConditionalExpression convertExp = Expression.Condition(
                            Expression.Equal(cellValueExp, Expression.Constant(DBNull.Value)),
                            Expression.Default(property.PropertyType),
                            Expression.Convert(cellValueExp, property.PropertyType));

                        BlockExpression cellValueReadExp = Expression.Block(new[] { cellValueExp }, Expression.Assign(cellValueExp, propertyExp), convertExp);

                        BinaryExpression bindExp = Expression.Assign(Expression.Property(targetExp, property), cellValueReadExp);

                        // Add to Expression List
                        exps.Add(bindExp);
                        break;
                    }
                }
            }

            // add the originating map to our expression list
            exps.Add(targetExp);

            // Compile the map
            return Expression.Lambda<Func<DbDataReader, T>>(Expression.Block(new[] { targetExp }, exps), paramExp).Compile();
        }
        #endregion

        #region Get MultipleRow from First ResultSet
        public IList<T> Query<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
        public async Task<IList<T>> QueryAsync<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleResult, cancellationToken))
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
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
        public async Task<T> QuerySingleAsync<T>(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, Func<DbDataReader, T> mappingFunc, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        using (DbDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken))
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
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
        public async Task<object> ExecuteScalarAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

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
                        dbConnection.Open();
                        command.Transaction = dbConnection.BeginTransaction(isolationLevel);
                        result = await command.ExecuteScalarAsync(cancellationToken);
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
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
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
        public async Task<int> ExecuteNonQueryAsync(string sqlQuery, CommandType commandType, IEnumerable<DbParameter> dbParameters, CancellationToken cancellationToken = default)
        {
            int recordCount;
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentNullException(nameof(sqlQuery), Empty);
            }

            if (dbParameters == null)
            {
                throw new ArgumentNullException(nameof(dbParameters));
            }

            using (DbConnection dbConnection = CreateDbConnection())
            {
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = sqlQuery;
                        command.CommandType = commandType;
                        command.Parameters.AddRange((Array)dbParameters);
                        command.Transaction = dbConnection.BeginTransaction();
                        recordCount = await command.ExecuteNonQueryAsync(cancellationToken);
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
            {
                return dbProviderFactory;
            }

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
