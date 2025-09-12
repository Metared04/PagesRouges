using PagesRouges.ErrorManager;
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
using System.Timers;

namespace PagesRouges.ViewModel
{
    public class UsersInfosViewModel : ViewModelBase
    {
        // Champs
        private ObservableCollection<User> _currentUserList;
        private int _selectedNewUserServiceId;
        private int _selectedNewUserSiteId;
        private string _searchText = "";

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
        public int SelectedNewUserServiceId
        {
            get
            {
                return _selectedNewUserServiceId;
            }
            set
            {
                _selectedNewUserServiceId = value;
                OnPropertyChanged(nameof(SelectedNewUserServiceId));
            }
        }
        public int SelectedNewUserSiteId
        {
            get
            {
                return _selectedNewUserSiteId;
            }
            set
            {
                _selectedNewUserSiteId = value;
                OnPropertyChanged(nameof(SelectedNewUserSiteId));
            }
        }
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
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
        public ICommand DeleteUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand SearchFilteredUserCommand { get; }
        public ICommand RefreshUserListCommand { get; }
        public ICommand GetExceptionCommand { get; }

        public ObservableCollection<Service> ServiceIds { get; set; } = new ObservableCollection<Service>();
        public ObservableCollection<Site> SiteIds { get; set; } = new ObservableCollection<Site>();

        // Constructeur
        public UsersInfosViewModel()
        {
            userRepository = new UserRepository();
            siteRepository = new SiteRepository();
            serviceRepository = new ServiceRepository();

            // Initialisation des commandes
            ShowUserInfosCommand = new ViewModelCommand(ExecuteShowUserInfosCommand);
            SearchFilteredUserCommand = new ViewModelCommand(ExecuteSearchFilteredUserCommand);
            RefreshUserListCommand = new ViewModelCommand(ExecuteRefreshUserListCommand);
            GetExceptionCommand = new ViewModelCommand(ExecuteGetExceptionCommand);

            LoadData();
        }
        private void LoadData()
        {
            var userList = new List<User>();
            userList = userRepository.GetAll().ToList();            
            
            CurrentUserList = new ObservableCollection<User>(userList);

            var dbServicesList = new List<Service>();
            var dbSiteList = new List<Site>();

            dbServicesList = serviceRepository.GetAll().ToList();
            dbSiteList = siteRepository.GetAll().ToList();

            ServiceIds.Clear();
            SiteIds.Clear();

            foreach (var service in dbServicesList)
            {
                ServiceIds.Add(service);
            }
            foreach (var site in dbSiteList)
            {
                SiteIds.Add(site);
            }
            SelectedNewUserServiceId = 0;
            SelectedNewUserSiteId = 0;
            SearchText = "";
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
        private void ExecuteSearchFilteredUserCommand(object obj)
        {
            try
            {
                var userList = new List<User>();
                userList = userRepository.GetAllFiltered(SearchText, SelectedNewUserServiceId, SelectedNewUserSiteId).ToList();
                CurrentUserList = new ObservableCollection<User>(userList);
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var filteredByText = userList.Where(u =>
                        (!string.IsNullOrEmpty(u.Name) && u.Name.ToLower().Contains(SearchText.ToLower())) ||
                        (!string.IsNullOrEmpty(u.FirstName) && u.FirstName.ToLower().Contains(SearchText.ToLower()))
                    ).ToList();

                    CurrentUserList = new ObservableCollection<User>(filteredByText);
                }
                else
                {
                    CurrentUserList = new ObservableCollection<User>(userList);
                }
            } catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "UserRepository.GetAllFiltered");
                throw;
            }
        }
        private void ExecuteRefreshUserListCommand(object obj)
        {
            LoadData();
        }
        private void ExecuteGetExceptionCommand(object obj)
        {
            MessageBox.Show("Exception generer.");
            ErrorLogger.LogError("Je genere une exception xD", "UsersInfosViewModel.ExecuteGetExceptionCommand");
        }        
    }
}
