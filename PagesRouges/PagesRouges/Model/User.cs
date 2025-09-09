using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string FixNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Service { get; set; }
        public Service IdService { get; set; }
        public string Site { get; set; }
        public Site IdSite { get; set; }
    }
}
