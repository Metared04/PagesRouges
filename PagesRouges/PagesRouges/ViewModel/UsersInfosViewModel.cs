using PagesRouges.Model;
using PagesRouges.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PagesRouges.ViewModel
{
    public class UsersInfosViewModel : ViewModelBase
    {
        // Champs
        private ObservableCollection<User> _currentUserList;

        private IUserRepository userRepository;
        private ISiteRepository siteRepository;
        private IServiceRepository serviceRepository;
        
        public ObservableCollection<User> CurrentUserList
        {
            get
            {
                return _currentUserList;
            }
            set
            {
                _currentUserList = value;
                OnPropertyChanged(nameof(CurrentUserList));
            }
        }

        // Commandes
        public ICommand ShowUserInfosCommand { get; }

        // Constructeur
        public UsersInfosViewModel()
        {
            userRepository = new UserRepository();
            siteRepository = new SiteRepository();
            serviceRepository = new ServiceRepository();

            LoadData();

        }
        private void LoadData()
        {
            var userList = new List<User>();
            userList = userRepository.GetAll().ToList();
            /*
            foreach (var user in userList)
            {
                user.IdService = serviceRepository.GetById(user.Service);
                user.IdSite = siteRepository.GetById(user.Site);
            }
            */
            CurrentUserList = new ObservableCollection<User>(userList);
        }
    }
}
