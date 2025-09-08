using Microsoft.Data.SqlClient;
using PagesRouges.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.Repositories
{
    public class ServiceRepository : RepositoryBase, IServiceRepository
    {
        public Service GetById(int id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from [ServiceTable] where IdService = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Service
                        {
                            ServiceId = (int)reader["IdService"],
                            ServiceName = reader["NameService"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}
