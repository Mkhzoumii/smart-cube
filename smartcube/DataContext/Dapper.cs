using Dapper; // لتفعيل Dapper
using Microsoft.Data.SqlClient; // لإضافة SqlClient
using System.Data;
using System.Collections.Generic;

namespace smartcube.DataContext
{
    public class DataContextDapper
    {
        private readonly IConfiguration _config;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        // التأكد من أن القيمة التي تم إرجاعها يمكن أن تكون null
        public T? ExecuteScalar<T>(string sql, object? parameters = null) where T : class
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return parameters == null
                    ? dbConnection.ExecuteScalar<T>(sql) // إرجاع null إذا لم يتم العثور على قيمة
                    : dbConnection.ExecuteScalar<T>(sql, parameters); // إرجاع null إذا لم يتم العثور على قيمة
            }
        }

        // التأكد من أن القيمة التي تم إرجاعها يمكن أن تكون null
        public IEnumerable<T>? LoadData<T>(string sql, object? parameters = null) where T : class
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return parameters == null
                    ? dbConnection.Query<T>(sql) // إرجاع null إذا لم يتم العثور على قيمة
                    : dbConnection.Query<T>(sql, parameters); // إرجاع null إذا لم يتم العثور على قيمة
            }
        }

        // التأكد من أن القيمة التي تم إرجاعها يمكن أن تكون null
        public T? LoadDataSingle<T>(string sql, object? parameters = null) where T : class
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return dbConnection.QuerySingleOrDefault<T>(sql, parameters); // إرجاع null إذا لم يتم العثور على قيمة
            }
        }

        public bool ExecuteSql(string sql)
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return dbConnection.Execute(sql) > 0;
            }
        }

        public int ExecuteSqlRowCount(string sql)
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                return dbConnection.Execute(sql);
            }
        }

        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            using (SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                dbConnection.Open();
                SqlCommand commandWithParams = new SqlCommand(sql, dbConnection);
                foreach (var param in parameters)
                {
                    commandWithParams.Parameters.Add(param);
                }
                int rowsAffected = commandWithParams.ExecuteNonQuery();
                dbConnection.Close();
                return rowsAffected > 0;
            }
        }
    }
}
