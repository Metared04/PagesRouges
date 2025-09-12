using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.Model
{
    public interface IServiceRepository
    {
        void Add(Service service);
        void Remove(int id);
        int GetRandomIdService();
        Service GetById(int id);
        IEnumerable<Service> GetAll();
    }
}
