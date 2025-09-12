using Azure;
using PagesRouges.API;
using PagesRouges.ErrorManager;
using PagesRouges.Model;
using PagesRouges.Repositories;
using PagesRouges.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PagesRouges.ViewModel
{
    public class UserListViewModel : ViewModelBase
    {
        private readonly RandomUserService _randomUserService;

        private ObservableCollection<User> _currentUserList;
        private ViewModelBase _createNewUserView;
        private ViewModelBase _createRandomNewUsersView;

        private IUserRepository userRepository;
        private IServiceRepository serviceRepository;
        private ISiteRepository siteRepository;
    
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
        private ViewModelBase CreateNewUserView
        {
            get
            {
                return _createNewUserView;
            }
            set
            {
                _createNewUserView = value;
                OnPropertyChanged(nameof(CreateNewUserView));
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

        public ICommand ShowUserInfosCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand UpdateUserInfosCommand { get; }
        public ICommand SearchNameCommand { get; }
        public ICommand AddUserCommand { get; }

        public UserListViewModel()
        {
            _randomUserService = new RandomUserService();

            userRepository = new UserRepository();
            serviceRepository = new ServiceRepository();
            siteRepository = new SiteRepository();

            ShowUserInfosCommand = new ViewModelCommand(ExecuteShowUserInfosCommand);
            DeleteUserCommand = new ViewModelCommand(ExecuteDeleteUserCommand);
            UpdateUserInfosCommand = new ViewModelCommand(ExecuteUpdateUserInfosCommand);
            SearchNameCommand = new ViewModelCommand(ExecuteSearchNameCommand);
            AddUserCommand = new ViewModelCommand(ExecuteAddUserCommand);

            LoadData();
        }
        private void LoadData()
        {
            var userList = new List<User>();
            userList = userRepository.GetAll().ToList();
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
            LoadData();
        }
        private void ExecuteDeleteUserCommand(object obj)
        {
            var user = obj as User;
            if (user == null) return;
            var result = MessageBox.Show($"Supprimer l'utilisateur \"{user.Name}\" ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;
            try
            {
                userRepository.Remove(user);
                CurrentUserList.Remove(user);
                MessageBox.Show("Utilisateur supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erreur lors de la suppression : {ex}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorLogger.LogError(ex, "UserRepository.Remove");
                throw;
            }
        }
        private void ExecuteUpdateUserInfosCommand(object obj)
        {

        }
        private void ExecuteSearchNameCommand(object obj)
        {

        }
        private void ExecuteAddUserCommand(object obj)
        {
            CreateNewUserView = new AddUserViewModel();
            var window = new AddUserView
            {
                DataContext = CreateNewUserView,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
            LoadData();
        }
        
    }
}
