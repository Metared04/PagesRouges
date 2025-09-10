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

        private IServiceRepository serviceRepository;

        private ViewModelBase _createNewServiceView;

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
        public ICommand AddServiceCommand { get; }
        public ICommand DeleteServiceCommand { get; }
        public ICommand UpdateServiceCommand { get; }
        public ServicesListViewModel()
        {
            serviceRepository = new ServiceRepository();

            AddServiceCommand = new ViewModelCommand(ExecuteAddServiceCommand);
            DeleteServiceCommand = new ViewModelCommand(ExecuteDeleteServiceCommand);
            UpdateServiceCommand = new ViewModelCommand(ExecuteUpdateServiceCommand);

            LoadData();
        }
        private void LoadData()
        {
            var serviceList = new List<Service>();
            serviceList = serviceRepository.GetAll().ToList();
            CurrentServiceList = new ObservableCollection<Service>(serviceList);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression : {ex}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteUpdateServiceCommand(object obj)
        {
            //throw new NotImplementedException();
        }
    }
}
