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
        public void Add(Service service)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"Insert into [ServiceTable] " +
                    "(NameService) values (@name)";
                command.Parameters.AddWithValue("@name", service.ServiceName);

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
                command.CommandText = "Delete from [ServiceTable] Where IdService = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                int rowsAffected = command.ExecuteNonQuery();
            }
        }
        public int GetRandomIdService()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select top 1 IdService " +
                    "from ServiceTable order by NEWID()";
                object result = command.ExecuteScalar();
                return (result != null) ? Convert.ToInt32(result) : -1;
            }
        }
        public IEnumerable<Service> GetAll()
        {
            List<Service> serviceList = new List<Service>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from ServiceTable order by IdService";

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
        public IEnumerable<Service> GetAllById(int id)
        {
            List<Service> servicesList = new List<Service>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from ServiceTable where IdService = @id";
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var service = new Service
                        {
                            ServiceId = (int)reader["IdService"],
                            ServiceName = reader["NameService"].ToString()
                        }; 
                        servicesList.Add(service);
                    }
                }
            }

            return servicesList;
        }
    }
}
