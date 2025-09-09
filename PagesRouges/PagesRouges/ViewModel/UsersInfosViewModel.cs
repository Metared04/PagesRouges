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

        private List<Service> _servicesList;
        private List<Site> _sitesList;
        
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
        public List<Service> ServicesList
        {
            get
            {
                return _servicesList;
            }
            set
            {
                _servicesList = value;
                OnPropertyChanged(nameof(ServicesList));
            }
        }
        public List<Site> SitesList
        {
            get
            {
                return _sitesList;
            }
            set
            {
                _sitesList = value;
                OnPropertyChanged(nameof(SitesList));
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
            
            CurrentUserList = new ObservableCollection<User>(userList);
            /*
            var serviceList = new List<Service>();
            var siteList = new List<Site>();
            serviceList = serviceRepository.GetAll().ToList();
            siteList = siteRepository.GetAll().ToList();

            
            foreach (var user in userList)
            {
                user.Service = serviceList.
                user.Site = siteRepository.GetById();
            }*/
            
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
