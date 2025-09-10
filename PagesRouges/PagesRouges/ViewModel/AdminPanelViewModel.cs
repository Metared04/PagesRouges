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
    public class AdminPanelViewModel : ViewModelBase
    {
        // Champs
        private ViewModelBase _currentChildChildView;

        
        private ISiteRepository siteRepository;
        private IServiceRepository serviceRepository;
        
        public ViewModelBase CurrentChildChildView
        {
            get
            {
                return _currentChildChildView;
            }
            set
            {
                _currentChildChildView = value;
                OnPropertyChanged(nameof(CurrentChildChildView));
            }
        }
        

        // Commandes
        public ICommand ShowUsersListCommand { get; }
        public ICommand ShowServicesListCommand { get; }
        public ICommand ShowSitesListCommand { get; }
        

        // Constructeur
        public AdminPanelViewModel()
        {
            
            siteRepository = new SiteRepository();
            serviceRepository = new ServiceRepository();

            // Initialisation des commandes
            ShowUsersListCommand = new ViewModelCommand(ExecuteShowUsersListCommand);
            ShowServicesListCommand = new ViewModelCommand(ExecuteShowServicesListCommand);
            ShowSitesListCommand = new ViewModelCommand(ExecuteShowSitesListCommand);

            //View par default
            ExecuteShowUsersListCommand(null);

        }
        
        private void ExecuteShowUsersListCommand(object obj)
        {
            CurrentChildChildView = new UserListViewModel();
        }
        private void ExecuteShowServicesListCommand(object obj)
        {
            CurrentChildChildView = new ServicesListViewModel();
        }
        private void ExecuteShowSitesListCommand(object obj)
        {
            CurrentChildChildView = new SitesListViewModel();
        }
        
    }
}
