using Milestone_CST350.DataServices;
using Milestone_CST350.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone_CST350.Services
{
    public class UserService
    {
        DBConnection database = new DBConnection();
        UserDAO service = new UserDAO();

        public int RegisterUser(User model)
        {
            var dbConnection = database.DbConnection();

            return service.RegisterUser(model, dbConnection);
        }

        public bool DeleteUser(int id)
        {
            var dbConnection = database.DbConnection();
            return service.DeleteUser(id, dbConnection);
        }

        public User FindById(int id)
        {
            var dbConnection = database.DbConnection();
            return service.FindById(id, dbConnection);
        }

        public int FindByUsernameAndPassword(string username, string password)
        {
            var dbConnection = database.DbConnection();

            // Return if found
            return service.FindByUsernameAndPassword(username, password, dbConnection);
        }

        public bool UpdateUser(User user)
        {
            var dbConnection = database.DbConnection();
            return service.UpdateUser(user, dbConnection);
        }
    }
}
