using PagesRouges.Model;
using PagesRouges.Repositories;
using PagesRouges.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private ViewModelBase _userView;
        
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
        public ViewModelBase UserView
        {
            get
            {
                return _userView;
            }
            set
            {
                _userView = value;
                OnPropertyChanged(nameof(UserView));
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

            // Initialisation des commandes
            ShowUserInfosCommand = new ViewModelCommand(ExecuteShowUserInfosCommand);

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
        private void ExecuteShowUserInfosCommand(object obj)
        {
            var user = obj as User;
            UserView = new UserDetailsViewModel(user);
            var window = new UserDetailsView
            {
                DataContext = UserView,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
        }
    }
}
