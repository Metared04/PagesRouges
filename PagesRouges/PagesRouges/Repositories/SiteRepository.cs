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
