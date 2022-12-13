using Milestone_CST350.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.DataServices
{
    public class GameDAO
    {

        public List<Board> GetAllGames(SqlConnection dbConnection)
        {
            List<Board> games = new List<Board>();
            Board gameBoard = new Board();

            string sqlStatement = "Select * from dbo.Game";

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            try
            {
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        gameBoard = new Board
                        {
                            Id = (int)reader[0],
                            DateTime = (string)reader[1],
                            UserId = (int)reader[2],
                            GameData = (string)reader[3]
                        };
                        games.Add(gameBoard);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return games;
        }

        public int SaveGame(int userId, Board gameBoard, SqlConnection dbConnection)
        {
            int res = -1;
            string sqlStatement = "Insert into dbo.Game (DateTime, UserId, GameData) VALUES (@DateTime, @UserId, @GameData)";


            /*SqlCommand command = new SqlCommand(sqlStatement, dbConnection);
            command.Parameters.AddWithValue("@DateTime", gameBoard.DateTime);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@GameData", Newtonsoft.Json.JsonConvert.SerializeObject(gameBoard));*/

            try
            {
                using (SqlCommand command = new SqlCommand(sqlStatement, dbConnection))
                {
                    command.Parameters.Add("@DateTime", System.Data.SqlDbType.Text).Value = gameBoard.DateTime;
                    //command.Parameters.AddWithValue("@DateTime", '%' + gameBoard.DateTime + '%');
                    command.Parameters.AddWithValue("@UserId", userId);
                    //command.Parameters.AddWithValue("@GameData", '%' + gameBoard.GameData + '%');
                    command.Parameters.Add("@GameData", System.Data.SqlDbType.Text).Value = gameBoard.GameData;

                    dbConnection.Open();
                    int reader = command.ExecuteNonQuery();

                    if (reader > 0)
                    {
                        res = 1;
                        dbConnection.Close();
                    }                                     
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return res;
        }

        public Board FindGameById(int Id, SqlConnection dbConnection)
        {
            Board gameBoard = new Board();

            string sqlStatement = "Select * from dbo.Game where Id = @Id";

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            // Prepared statement
            command.Parameters.AddWithValue("@Id", Id);

            try
            {
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        gameBoard = new Board
                        {
                            Id = (int)reader[0],
                            DateTime = (string)reader[1],
                            UserId = (int)reader[2],
                            GameData = (string)reader[3]
                        };
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return gameBoard;
        }


        public bool DeleteGame(int gameId, SqlConnection dbConnection)
        {
            // Will return true if deletion completed
            bool success = false;

            string sqlStatement = "DELETE from dbo.Game WHERE Id = @Id";

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            // Prepared Statement
            command.Parameters.AddWithValue("@Id", gameId);

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

            // Return if User deleted
            return success;
        }

        public List<Board> GetAllUserGames(int userId, SqlConnection dbConnection)
        {
            List<Board> games = new List<Board>();
            Board gameBoard = new Board();

            string sqlStatement = "Select * from dbo.Game where UserId = @userId";

            SqlCommand command = new SqlCommand(sqlStatement, dbConnection);

            // Prepared statement
            command.Parameters.AddWithValue("@userId", userId);

            try
            {
                dbConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read()) 
                    {
                        gameBoard = new Board
                        {
                            Id = (int)reader[0],
                            DateTime = (string)reader[1],
                            UserId = (int)reader[2],
                            GameData = (string)reader[3]
                        };
                        games.Add(gameBoard);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return games;
        }

        public bool UpdateGame(int gameId, Board gameBoard, SqlConnection dbConnection)
        {
            var result = false;

            string sqlStatement = "Update dbo.Game set DateTime = @DateTime, GameData = @GameData WHERE Id = @Id";

            try
            {
                using (SqlCommand command = new SqlCommand(sqlStatement, dbConnection))
                {
                    command.Parameters.Add("@DateTime", System.Data.SqlDbType.Text).Value = gameBoard.DateTime;
                    //command.Parameters.AddWithValue("@DateTime", '%' + gameBoard.DateTime + '%');
                    command.Parameters.AddWithValue("@Id", gameId);
                    //command.Parameters.AddWithValue("@GameData", '%' + gameBoard.GameData + '%');
                    command.Parameters.Add("@GameData", System.Data.SqlDbType.Text).Value = gameBoard.GameData;

                    dbConnection.Open();
                    int reader = command.ExecuteNonQuery();

                    if (reader > 0)
                    {
                        result = true;
                    }

                    dbConnection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
}
