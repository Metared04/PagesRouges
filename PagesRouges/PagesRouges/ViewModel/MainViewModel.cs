using PagesRouges.Model;
using PagesRouges.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PagesRouges.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        // Champs
        private ViewModelBase _currentChildView;
        private Visibility _isLoginVisible = Visibility.Collapsed;
        private string _storedPassword = "mdp";


        // Proprietes
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {

                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }
        public Visibility IsLoginVisible
        {
            get
            {
                return _isLoginVisible;
            }
            set
            {
                _isLoginVisible = value;
                OnPropertyChanged(nameof(IsLoginVisible));
            }
        }

        // Commandes
        public ICommand ShowUsersListCommand { get; }
        public ICommand OpenLoginCommand { get; }
        public ICommand CheckPasswordCommand { get; }

        // Constructeur
        public MainViewModel()
        {
            // Initialisation des commandes
            ShowUsersListCommand = new ViewModelCommand(ExecuteShowUsersListCommand);
            OpenLoginCommand = new ViewModelCommand(ExecuteOpenLoginCommand);
            CheckPasswordCommand = new ViewModelCommand(ExecuteCheckPasswordCommand);

            ExecuteShowUsersListCommand(null);
        }
        private void ExecuteShowUsersListCommand(object obj)
        {
            CurrentChildView = new UsersInfosViewModel();
        }
        private void ExecuteOpenLoginCommand(object obj)
        {
            IsLoginVisible = Visibility.Visible; 
        }
        private void ExecuteCheckPasswordCommand(object obj)
        {
            var passwordBox = obj as PasswordBox;
            string inputPassword = passwordBox?.Password ?? string.Empty;
            MessageBox.Show($"Mdp = {inputPassword}");

            if (inputPassword == _storedPassword)
            {
                IsLoginVisible = Visibility.Collapsed;
                CurrentChildView = new AdminPanelViewModel();
            }
            else
            {
                MessageBox.Show("Mot de passe incorrect !");
            }
        }
    }
}
