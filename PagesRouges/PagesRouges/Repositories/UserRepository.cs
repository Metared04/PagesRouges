using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using PagesRouges.API;
using PagesRouges.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(User user)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"Insert into [UserTable] " +
                    "(NameUser, FirstNameUser, FixNumberUser, " +
                    "PhoneNumberUser, EmailUser, UserIdService, UserIdSite) " +
                    "values (@name, @firstname, @fix, @phone, " +
                    "@mail, @idService, @idSite)";
                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@firstname", user.FirstName);
                command.Parameters.AddWithValue("@fix", user.FixNumber);
                command.Parameters.AddWithValue("@phone", user.PhoneNumber);
                command.Parameters.AddWithValue("@mail", user.Email);
                command.Parameters.AddWithValue("@idService", user.ServiceId);
                command.Parameters.AddWithValue("@idSite", user.SiteId);

                command.ExecuteNonQuery();
            }
        }

        public void Edit(User user, User newUser)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Update [UserTable] set NameUser = @name, " +
                    "FirstNameUser = @firstName, FixNumberUser = @fix, PhoneNumberUser = @phone " +
                    "EmailUser = @mail, UserIdService = @idService, UserIdSite = @idSite " +
                    "where IdUser = @id";
                command.Parameters.Add("@name", SqlDbType.Text).Value = newUser.Name;
                command.Parameters.Add("@firstName", SqlDbType.Text).Value = newUser.FirstName;
                command.Parameters.Add("@fix", SqlDbType.Text).Value = newUser.FixNumber;
                command.Parameters.Add("@phone", SqlDbType.Text).Value = newUser.PhoneNumber;
                command.Parameters.Add("@mail", SqlDbType.Text).Value = newUser.Email;
                command.Parameters.Add("@idService", SqlDbType.Int).Value = newUser.Service;
                command.Parameters.Add("@idSite", SqlDbType.Int).Value = newUser.Site;
                command.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = user.Id;
                int rowsAffected = command.ExecuteNonQuery();
            }
        }
        public void Remove(User user)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Delete from [UserTable] Where IdUser = @id";
                command.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = user.Id;
                int rowsAffected = command.ExecuteNonQuery();
            }
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
                    "u.FixNumberUser, u.PhoneNumberUser, u.EmailUser, " +
                    "s.NameService, si.NameSite from [UserTable] u " +
                    "INNER JOIN [ServiceTable] s ON u.UserIdService = s.IdService " +
                    "INNER JOIN [SiteTable] si ON u.UserIdSite = si.IdSite ";
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
                            Email = reader["EmailUser"].ToString(),
                            Service = reader["NameService"].ToString(),
                            Site = reader["NameSite"].ToString()
                        };
                        usersList.Add(user);
                    }
                }
            }

            return usersList;
        }
        public IEnumerable<User> GetAllFilteredByName(string name)
        {
            List<User> usersList = new List<User>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select u.IdUser, u.NameUser, u.FirstNameUser," +
                    "u.FixNumberUser, u.PhoneNumberUser, u.EmailUser, " +
                    "s.NameService, si.NameSite from [UserTable] u " +
                    "INNER JOIN [ServiceTable] s ON u.UserIdService = s.IdService " +
                    "INNER JOIN [SiteTable] si ON u.UserIdSite = si.IdSite " +
                    "where u.NameUser like '%'+ @name +'%'";
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
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
                            Email = reader["EmailUser"].ToString(),
                            Service = reader["NameService"].ToString(),
                            Site = reader["NameSite"].ToString()
                        };
                        usersList.Add(user);
                    }
                }
            }

            return usersList;
        }
        public IEnumerable<User> GetAllBySite(string name, int site)
        {
            List<User> usersList = new List<User>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select u.IdUser, u.NameUser, u.FirstNameUser," +
                    "u.FixNumberUser, u.PhoneNumberUser, u.EmailUser, " +
                    "s.NameService, si.NameSite from [UserTable] u " +
                    "INNER JOIN [ServiceTable] s ON u.UserIdService = s.IdService " +
                    "INNER JOIN [SiteTable] si ON u.UserIdSite = si.IdSite " +
                    "where u.UserIdSite = @site and u.NameUser like '%'+ @name +'%'";
                command.Parameters.Add("@site", SqlDbType.Int).Value = site;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
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
                            Email = reader["EmailUser"].ToString(),
                            Service = reader["NameService"].ToString(),
                            Site = reader["NameSite"].ToString()
                        };
                        usersList.Add(user);
                    }
                }
            }

            return usersList;
        }
        public IEnumerable<User> GetAllByService(string name, int service)
        {
            List<User> usersList = new List<User>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select u.IdUser, u.NameUser, u.FirstNameUser," +
                    "u.FixNumberUser, u.PhoneNumberUser, u.EmailUser, " +
                    "s.NameService, si.NameSite from [UserTable] u " +
                    "INNER JOIN [ServiceTable] s ON u.UserIdService = s.IdService " +
                    "INNER JOIN [SiteTable] si ON u.UserIdSite = si.IdSite " +
                    "where u.UserIdService = @service and u.NameUser like '%'+ @name +'%'";
                command.Parameters.Add("@service", SqlDbType.Int).Value = service;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
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
                            Email = reader["EmailUser"].ToString(),
                            Service = reader["NameService"].ToString(),
                            Site = reader["NameSite"].ToString()
                        };
                        usersList.Add(user);
                    }
                }
            }

            return usersList;
        }
        public IEnumerable<User> GetAllByServiceAndSite(string name, int service, int site)
        {
            List<User> usersList = new List<User>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select u.IdUser, u.NameUser, u.FirstNameUser," +
                    "u.FixNumberUser, u.PhoneNumberUser, u.EmailUser, " +
                    "s.NameService, si.NameSite from [UserTable] u " +
                    "INNER JOIN [ServiceTable] s ON u.UserIdService = s.IdService " +
                    "INNER JOIN [SiteTable] si ON u.UserIdSite = si.IdSite " +
                    "where u.UserIdService = @service and u.UserIdSite = @site and u.NameUser like '%'+ @name +'%'";
                command.Parameters.Add("@service", SqlDbType.Int).Value = service;
                command.Parameters.Add("@site", SqlDbType.Int).Value = site;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
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
                            Email = reader["EmailUser"].ToString(),
                            Service = reader["NameService"].ToString(),
                            Site = reader["NameSite"].ToString()
                        };
                        usersList.Add(user);
                    }
                }
            }

            return usersList;
        }
        public IEnumerable<User> GetAllFiltered(string nom, int idService, int idSite)
        {
            List<User> u = new List<User>();
            if(idService == 0 && idSite == 0){
                u = GetAllFilteredByName(nom).ToList();
            } else if(idService > 0 && idSite == 0)
            {
                u = GetAllByService(nom, idService).ToList();
            } else if(idService == 0 && idSite > 0)
            {
                u = GetAllBySite(nom, idSite).ToList();
            } else
            {
                u = GetAllByServiceAndSite(nom, idService, idSite).ToList();
            }
            return u;
        }
    }
}
