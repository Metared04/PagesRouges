using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using PagesRouges.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.Repositories
{
    public class SiteRepository : RepositoryBase, ISiteRepository
    {
        public void Add(Site site)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"Insert into [SiteTable] " +
                    "(NameSite) values (@name)";
                command.Parameters.AddWithValue("@name", site.SiteName);

                command.ExecuteNonQuery();
            }
        }
        public void Remove(int id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Delete from [SiteTable] Where IdSite = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                int rowsAffected = command.ExecuteNonQuery();
            }
        }
        public IEnumerable<Site> GetAll()
        {
            List<Site> siteList = new List<Site>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from SiteTable order by IdSite";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var site = new Site()
                        {
                            SiteId = reader.GetInt32(0),
                            SiteName = reader.GetString(1),
                        };
                        siteList.Add(site);
                    }
                }
            }
            return siteList;
        }

        public Site GetById(int id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from [SiteTable] where IdSite = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Site
                        {
                            SiteId = (int)reader["IdSite"],
                            SiteName = reader["NameSite"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}
