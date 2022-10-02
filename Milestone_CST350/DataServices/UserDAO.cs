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
        internal bool RegisterUser(User user, SqlConnection dbConnection)
        {
            string sqlStatement = "Insert into dbo.Users (firstname, lastname, sex, age, state, email, username, password) " +
                                  "VALUES (@FirstName, @LastName, @Sex, @Age, @State, @Email, @UserName, @Password)";

            bool success = false;

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);
            //command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = null;
            command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 50).Value = user.FirstName;
            command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 50).Value = user.LastName;
            command.Parameters.Add("@Sex", System.Data.SqlDbType.NChar, 10).Value = user.Sex;
            command.Parameters.Add("@Age", System.Data.SqlDbType.Int).Value = user.Age;
            command.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50).Value = user.State;
            command.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 50).Value = user.Email;
            command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar, 50).Value = user.UserName;
            command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar, 50).Value = user.Password;

            try
            {
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.RecordsAffected > 0)
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
                        user.Sex = (string)reader[3];
                        user.Age = (int)reader[4];
                        user.State = (string)reader[5];
                        user.Email = (string)reader[6];
                        user.UserName = (string)reader[7];
                        user.Password = (string)reader[8];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return user;
        }

        public bool FindByUsernameAndPassword(string username, string password, SqlConnection dbConnection)
        {
            string sqlStatement = "Select * from dbo.Users where username LIKE @UserName AND password LIKE @Password";

            // Will return true or false if found
            bool success = false;

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
                    dbConnection.Close();
                    success = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Return if found
            return success;
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

            string sqlStatement = "Update dbo.Users set FIRSTNAME = @FirstName, LASTNAME = @LastName, SEX = @Sex, AGE = @Age, STATE = @State, EMAIL = @Email, USERNAME = @UserName, PASSWORD = @Password WHERE Id = @Id";

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            // Prepared Statement
            command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = user.Id;
            command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 50).Value = user.FirstName;
            command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 50).Value = user.LastName;
            command.Parameters.Add("@Sex", System.Data.SqlDbType.NChar, 10).Value = user.Sex;
            command.Parameters.Add("@Sex", System.Data.SqlDbType.Int).Value = user.Age;
            command.Parameters.Add("@State", System.Data.SqlDbType.NVarChar, 50).Value = user.State;
            command.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 50).Value = user.Email;
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
