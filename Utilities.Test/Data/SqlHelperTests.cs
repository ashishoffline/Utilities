using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
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
        public void QueryTest()
        {
            string sqlQuery = @"SELECT * FROM Sample;";
            var parameters = Array.Empty<DbParameter>();

            Stopwatch stopWatch = new Stopwatch();
            IList<Sample> result = null;
            stopWatch.Start();
            for (int i = 0; i < 10; i++)
            {
                result = _sqlHelper.Query<Sample>(sqlQuery, CommandType.Text, parameters);

                //result = _sqlHelper.Query(sqlQuery, CommandType.Text, parameters,
                //reader => new Sample
                //{
                //    Id = reader.GetInt64(0),
                //    Name = reader.GetString(1),
                //    WorkProfile = reader.GetString(2),
                //    Salary = reader.GetInt64(3),
                //    Incentive = reader.GetDouble(4),
                //    Guid = reader.GetGuid(5),
                //    LastUpdatedDate = reader.GetDateTime(6)
                //});
            }
            stopWatch.Stop();
            
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            string sqlQuery = @"INSERT INTO Sample(Name, WorkProfile, Salary, Incentive, Guid, LastUpdatedDate) VALUES (@Name, @WorkProfile, @Salary, @Incentive, @Guid, @LastUpdatedDate);";
            for (int i = 0; i < 10000; i++)
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name",$"Name - {i}"),
                    new SqlParameter("@WorkProfile",$"Work - {i}"),
                    new SqlParameter("@Salary",i*1000),
                    new SqlParameter("@Incentive",i/(double)1000.00),
                    new SqlParameter("@Guid",Guid.NewGuid()),
                    new SqlParameter("@LastUpdatedDate",DateTime.Now),
                };
                _sqlHelper.ExecuteNonQuery(sqlQuery, CommandType.Text, parameters);
            }
            Assert.IsTrue(true);
        }
    }
    public class Sample
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string WorkProfile { get; set; }
        public long Salary { get; set; }
        public double Incentive { get; set; }
        public Guid Guid { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}