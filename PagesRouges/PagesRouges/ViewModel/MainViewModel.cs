using PagesRouges.Model;
using PagesRouges.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PagesRouges.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        // Champs
        private ViewModelBase _currentChildView;
        private bool _isAdminPanelVisible;
        private bool _isLoginVisible;
        

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
        public bool IsAdminPanelVisible
        {
            get
            {
                return _isAdminPanelVisible;
            }
            set
            {
                _isAdminPanelVisible = value;
                OnPropertyChanged(nameof(IsAdminPanelVisible));
            }
        }
        public bool IsLoginVisible
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
        public ICommand ValidatePasswordCommand { get; }

        // Constructeur
        public MainViewModel()
        {
            // Initialisation des commandes
            ShowUsersListCommand = new ViewModelCommand(ExecuteShowUsersListCommand);
            
            //ValidatePasswordCommand = new ViewModelCommand(ExecuteValidatePasswordCommand);

            ExecuteShowUsersListCommand(null);
        }
        private void ExecuteShowUsersListCommand(object obj)
        {
            CurrentChildView = new UsersInfosViewModel();
        }
        
        private void ExecuteValidatePassword(object obj)
        {
            string password = obj as string;

            if (password == "MonMotDePasseSecret")
            {
                IsLoginVisible = false;
                IsAdminPanelVisible = true;
            }
            else
            {
                MessageBox.Show("Mot de passe incorrect !");
            }
        }
    }
}
