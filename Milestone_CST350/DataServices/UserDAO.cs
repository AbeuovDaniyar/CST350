using Milestone_CST350.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.DataServices
{
    public class UserDAO
    {
        internal int RegisterUser(User user, SqlConnection dbConnection)
        {
            string sqlStatement = "Insert into dbo.Users (FirstName, LastName, UserName, Password) " +
                                  "VALUES (@FirstName, @LastName, @UserName, @Password)";

            int res = -1;

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);
            command.Parameters.Add("@FirstName", System.Data.SqlDbType.NChar, 20).Value = user.FirstName;
            command.Parameters.Add("@LastName", System.Data.SqlDbType.NChar, 20).Value = user.LastName;
            command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar, 50).Value = user.UserName;
            command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 50).Value = user.Password;

            try
            {
                dbConnection.Open();
                int reader = command.ExecuteNonQuery();

                if (reader > 0)
                {
                    res = 1;
                    dbConnection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return res;
        }

        public User FindById(int id, SqlConnection dbConnection)
        {
            User user = new User();

            string sqlStatement = "Select * from dbo.Users where Id = @id";

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            // Prepared statement
            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

            try
            {
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.Id = (int)reader[0];
                        user.FirstName = (string)reader[1];
                        user.LastName = (string)reader[2];
                        user.UserName = (string)reader[3];
                        user.Password = (string)reader[4];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return user;
        }

        public int FindByUsernameAndPassword(string username, string password, SqlConnection dbConnection)
        {
            string sqlStatement = "Select * from dbo.Users where username LIKE @UserName AND password LIKE @Password";

            // Will return true or false if found
            int res = -1;

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            // Prepared statement
            command.Parameters.AddWithValue("@UserName", '%' + username + '%');
            command.Parameters.AddWithValue("@Password", '%' + password + '%');

            try
            {
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        res = (int)reader[0];
                    }
                    dbConnection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Return if found
            return res;
        }

        public bool DeleteUser(int id, SqlConnection dbConnection)
        {
            // Will return true if deletion completed
            bool success = false;

            string sqlStatement = "DELETE from dbo.Users WHERE Id = @Id";

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            // Prepared Statement
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

            try
            {
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.RecordsAffected < 0)
                {
                    dbConnection.Close();
                    success = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Return if User deleted
            return success;
        }

        public bool UpdateUser(User user, SqlConnection dbConnection)
        {
            var success = false;

            string sqlStatement = "Update dbo.Users set FirstName = @FirstName, LastName = @LastName, UserName = @UserName, Password = @Password WHERE Id = @Id";

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            // Prepared Statement
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = user.Id;
            command.Parameters.Add("@FirstName", System.Data.SqlDbType.NChar, 20).Value = user.FirstName;
            command.Parameters.Add("@LastName", System.Data.SqlDbType.NChar, 20).Value = user.LastName;
            command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar, 50).Value = user.UserName;
            command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 50).Value = user.Password;

            try
            {
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.RecordsAffected < 0)
                {
                    dbConnection.Close();
                    success = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return success;
        }
    }
}
