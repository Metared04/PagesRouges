using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.Model
{
    public interface ISiteRepository
    {
        Site GetById(int id);
        IEnumerable<Site> GetAll();
    }
}
