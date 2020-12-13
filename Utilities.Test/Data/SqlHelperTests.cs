using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Text;

namespace Utilities.Data.Tests
{
    [TestClass()]
    public class SqlHelperTests
    {
        private const string _connectionString = "Server=localhost;Database=TESTING;User Id=ashishoffline;Password=qwertyuiop;";
        private readonly ISqlHelper _sqlHelper;
        public SqlHelperTests()
        {
            _sqlHelper = new SqlHelper("Microsoft.Data.SqlClient", _connectionString);
        }
        [TestMethod()]
        public void Executeest()
        {
            string sqlQuery = @"INSERT INTO Sample(Name, WorkProfile)
VALUES
('Rahul Jha', 'Software Engineer');INSERT INTO Sample(Id, Name, WorkProfile)
VALUES
(10, 'Ashish Jha', 'Software Engineer');";
            var parameters = Array.Empty<SqlParameter>();

            var result = _sqlHelper.ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);

            //var result = _sqlHelper.QuerySingle(sqlQuery, CommandType.Text, parameters,
            //    reader => new Sample
            //    {
            //        Id = reader.GetInt64("Id"),
            //        Name = reader.GetString("Name"),
            //        WorkProfile = reader.GetString("WorkProfile")
            //    });
            Assert.IsNotNull(result);
        }
    }
    public class Sample
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string WorkProfile { get; set; }
    }
}