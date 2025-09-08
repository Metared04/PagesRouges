using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.Model
{
    public interface IUserRepository
    {
        void Add(User user);
        void Edit(User user, User newUser);
        void Remove(User user);
        IEnumerable<User> GetAll();
    }
}
