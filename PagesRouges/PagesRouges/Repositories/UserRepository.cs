using Microsoft.Data.SqlClient;
using PagesRouges.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Edit(User user, User newUser)
        {
            throw new NotImplementedException();
        }
        public void Remove(User user)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<User> GetAll()
        {
            List<User> usersList = new List<User>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select u.IdUser, u.NameUser, u.FirstNameUser," +
                    "u.FixNumberUser, u.PhoneNumberUser, u.Email, " +
                    "s.ServiceName, si.SiteName from [UserTable] u " +
                    "INNER JOIN [ServiceTable] s ON u.UserIdService = s.ServiceId " +
                    "INNER JOIN [SiteTable] si ON u.UserIdSite = si.SiteId";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            Id = (Guid)reader["IdUser"],
                            Name = reader["NameUser"].ToString(),
                            FirstName = reader["FirstNameUser"].ToString(),
                            FixNumber = reader["FixNumberUser"].ToString(),
                            PhoneNumber = reader["PhoneNumberUser"].ToString(),
                            Email = reader["Email"].ToString(),
                            Service = reader["ServiceName"].ToString(),
                            Site = reader["SiteName"].ToString()
                        };
                        usersList.Add(user);
                    }
                }
            }

            return usersList;
        }
    }
}
