using PagesRouges.ErrorManager;
using PagesRouges.Model;
using PagesRouges.Repositories;
using PagesRouges.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PagesRouges.ViewModel
{
    public class ServicesListViewModel : ViewModelBase
    {
        private ObservableCollection<Service> _currentServiceList;

        private int _selectedNewUserServiceId;

        private IServiceRepository serviceRepository;

        private ViewModelBase _createNewServiceView;
        private ViewModelBase _serviceEditorView;

        private List<Service> _servicesList;

        public ObservableCollection<Service> CurrentServiceList
        {
            get
            {
                return _currentServiceList;
            }
            set
            {
                _currentServiceList = value;
                OnPropertyChanged(nameof(CurrentServiceList));
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
        public ViewModelBase CreateNewServiceView
        {
            get
            {
                return _createNewServiceView;
            }
            set
            {
                _createNewServiceView = value;
                OnPropertyChanged(nameof(CreateNewServiceView));
            }
        }
        public ViewModelBase ServiceEditorView
        {
            get
            {
                return _serviceEditorView;
            }
            set
            {
                _serviceEditorView = value;
                OnPropertyChanged(nameof(ServiceEditorView));
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
        public ICommand AddServiceCommand { get; }
        public ICommand DeleteServiceCommand { get; }
        public ICommand UpdateServiceCommand { get; }
        public ICommand RefreshServiceListCommand { get; }
        public ICommand SearchServiceCommand { get; }
        public ObservableCollection<Service> ServiceIds { get; set; } = new ObservableCollection<Service>();
        public ServicesListViewModel()
        {
            serviceRepository = new ServiceRepository();

            AddServiceCommand = new ViewModelCommand(ExecuteAddServiceCommand);
            DeleteServiceCommand = new ViewModelCommand(ExecuteDeleteServiceCommand);
            UpdateServiceCommand = new ViewModelCommand(ExecuteUpdateServiceCommand);
            RefreshServiceListCommand = new ViewModelCommand(ExecuteRefreshServiceListCommand);
            SearchServiceCommand = new ViewModelCommand(ExecuteSearchServiceCommand);

            LoadData();
        }
        private void LoadData()
        {
            var serviceList = new List<Service>();
            serviceList = serviceRepository.GetAll().ToList();
            CurrentServiceList = new ObservableCollection<Service>(serviceList);

            var dbServicesList = new List<Service>();
            dbServicesList = serviceRepository.GetAll().ToList();
            ServiceIds.Clear();
            foreach (var service in dbServicesList)
            {
                ServiceIds.Add(service);
            }
            SelectedNewUserServiceId = 0;
        }
        private void ExecuteAddServiceCommand(object obj)
        {
            CreateNewServiceView = new AddServiceViewModel();
            var window = new AddServiceView
            {
                DataContext = CreateNewServiceView,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
            LoadData();
        }
        private void ExecuteDeleteServiceCommand(object obj)
        {
            var service = obj as Service;
            if (service == null) return;
            var result = MessageBox.Show($"Supprimer le service \"{service.ServiceName}\" ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;
            try
            {
                serviceRepository.Remove(service.ServiceId);
                CurrentServiceList.Remove(service);
                MessageBox.Show("Service et utilisateurs supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "serviceRepository.Remove");
                MessageBox.Show($"Le service n'a pas été supprimé : {ex}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteUpdateServiceCommand(object obj)
        {
            var service = obj as Service;
            ServiceEditorView = new EditServiceViewModel(service);
            var window = new EditServiceView
            {
                DataContext = ServiceEditorView,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
            LoadData();
        }
        private void ExecuteRefreshServiceListCommand(object obj)
        {
            LoadData();
        }
        private void ExecuteSearchServiceCommand(object obj)
        {
            try
            {
                var serviceList = new List<Service>();
                serviceList = serviceRepository.GetAllById(SelectedNewUserServiceId).ToList();
                CurrentServiceList = new ObservableCollection<Service>(serviceList);
            } catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "serviceRepository.GetAllById");
                MessageBox.Show($"La recherche n'a pas été effectué : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
