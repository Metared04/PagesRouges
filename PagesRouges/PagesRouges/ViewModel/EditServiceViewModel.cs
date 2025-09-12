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
    public class EditServiceViewModel : ViewModelBase
    {
        private ObservableCollection<Service> _currentServiceList;
        private Service _currentService;
        private int _selectedNewUserServiceId;
        private string _previousServiceName;

        private IServiceRepository serviceRepository;
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
        public Service CurrentService
        {
            get
            {
                return _currentService;
            }
            set
            {
                _currentService = value;
                OnPropertyChanged(nameof(CurrentService));
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
        public string PreviousServiceName
        {
            get
            {
                return _previousServiceName;
            }
            set
            {
                _previousServiceName = value;
                OnPropertyChanged(nameof(PreviousServiceName));
            }
        }
        public ICommand ValidateEditCommand { get; }
        public ObservableCollection<Service> ServiceIds { get; set; } = new ObservableCollection<Service>();
        public EditServiceViewModel(Service service)
        {
            serviceRepository = new ServiceRepository();
            ValidateEditCommand = new ViewModelCommand(ExecuteValidateEditCommand);

            LoadData(service);
        }
        private void LoadData(Service service)
        {
            var serviceList = new List<Service>();
            serviceList = serviceRepository.GetAll().ToList();
            CurrentServiceList = new ObservableCollection<Service>(serviceList);

            var dbServiceList = new List<Service>();
            dbServiceList = serviceRepository.GetAll().ToList();
            ServiceIds.Clear();
            foreach (var s in dbServiceList)
            {
                ServiceIds.Add(s);
            }

            SelectedNewUserServiceId = service.ServiceId;
            PreviousServiceName = service.ServiceName;
        }
        private void ExecuteValidateEditCommand(object obj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PreviousServiceName))
                {
                    MessageBox.Show("Le nom du site ne peut pas être vide.");
                    return;
                }
                serviceRepository.Update(SelectedNewUserServiceId, PreviousServiceName);
                MessageBox.Show("Service mis à jour !");
                if (Application.Current.Windows.OfType<EditSiteView>().FirstOrDefault() is EditSiteView window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "EditSiteViewModel.ExecuteValidateEditCommand");
                MessageBox.Show($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }
    }
}
