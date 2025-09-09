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
        public IEnumerable<Service> GetAll()
        {
            List<Service> serviceList = new List<Service>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from ServiceTable";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var service = new Service()
                        {
                            ServiceId = reader.GetInt32(0),
                            ServiceName = reader.GetString(1),
                        };
                        serviceList.Add(service);
                    }
                }
            }
            return serviceList;
        }

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
