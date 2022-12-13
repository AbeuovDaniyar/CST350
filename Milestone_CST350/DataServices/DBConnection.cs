using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.DataServices
{
    public class DBConnection
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=milestone;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public SqlConnection DbConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            if (connection != null)
            {
                return connection;
            }
            else
            {
                return null;
            }
        }
    }
}
