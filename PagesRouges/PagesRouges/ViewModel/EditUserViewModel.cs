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
    public class EditUserViewModel : ViewModelBase
    {
        private ObservableCollection<Service> _currentServiceList; 
        private ObservableCollection<Site> _currentSiteList;

        private User _currentUser;

        private string _previousName;
        private string _previousFirstName;
        private string _previousFix;
        private string _previousPhone;
        private string _previousEmail;

        private int _selectedNewUserServiceId;
        private int _selectedNewUserSiteId;

        private IServiceRepository serviceRepository;
        private ISiteRepository siteRepository;
        private IUserRepository userRepository;

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
        public ObservableCollection<Site> CurrentSiteList
        {
            get
            {
                return _currentSiteList;
            }
            set
            {
                _currentSiteList = value;
                OnPropertyChanged(nameof(CurrentSiteList));
            }
        }
        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        public string PreviousName
        {
            get
            {
                return _previousName;
            }
            set
            {
                _previousName = value;
                OnPropertyChanged(nameof(PreviousName));
            }
        }
        public string PreviousFirstName
        {
            get
            {
                return _previousFirstName;
            }
            set
            {
                _previousFirstName = value;
                OnPropertyChanged(nameof(PreviousFirstName));
            }
        }
        public string PreviousFix
        {
            get
            {
                return _previousFix;
            }
            set
            {
                _previousFix = value;
                OnPropertyChanged(nameof(PreviousFix));
            }
        }
        public string PreviousPhone
        {
            get
            {
                return _previousPhone;
            }
            set
            {
                _previousPhone = value;
                OnPropertyChanged(nameof(PreviousPhone));
            }
        }
        public string PreviousEmail
        {
            get
            {
                return _previousEmail;
            }
            set
            {
                _previousEmail = value;
                OnPropertyChanged(nameof(PreviousEmail));
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
        public ICommand ValidateEditCommand { get; }
        public ObservableCollection<Service> ServiceIds { get; set; } = new ObservableCollection<Service>();
        public ObservableCollection<Site> SiteIds { get; set; } = new ObservableCollection<Site>();
        public EditUserViewModel(User user)
        {
            serviceRepository = new ServiceRepository();
            siteRepository = new SiteRepository();
            userRepository = new UserRepository();

            ValidateEditCommand = new ViewModelCommand(ExecuteValidateEditCommand);

            LoadData(user);
        }
        private void LoadData(User user)
        {
            _currentUser = user;
            var serviceList = new List<Service>();
            var siteList = new List<Site>();

            serviceList = serviceRepository.GetAll().ToList();
            siteList = siteRepository.GetAll().ToList();

            CurrentServiceList = new ObservableCollection<Service>(serviceList);
            CurrentSiteList = new ObservableCollection<Site>(siteList);

            var dbServiceList = new List<Service>();
            var dbSiteList = new List<Site>();

            dbServiceList = serviceRepository.GetAll().ToList();
            dbSiteList = siteRepository.GetAll().ToList();

            ServiceIds.Clear();
            SiteIds.Clear();

            foreach(var s in dbServiceList)
            {
                ServiceIds.Add(s);
            }
            foreach (var s in dbSiteList)
            {
                SiteIds.Add(s);
            }

            PreviousName = user.Name;
            PreviousFirstName = user.FirstName;
            PreviousFix = user.FixNumber;
            PreviousPhone = user.PhoneNumber;
            PreviousEmail = user.Email;
            SelectedNewUserServiceId = user.ServiceId ?? 0;
            SelectedNewUserSiteId = user.SiteId ?? 0;

            /*
            var currentService = ServiceIds.FirstOrDefault(s => s.ServiceName == user.Service);
            SelectedNewUserServiceId = currentService?.ServiceId ?? 0;

            var currentSite = SiteIds.FirstOrDefault(s => s.SiteName == user.Site);
            SelectedNewUserSiteId = currentSite?.SiteId ?? 0;
            */
        }
        private void ExecuteValidateEditCommand(object obj)
        {
            try
            {
                
                _currentUser.Name = PreviousName.Trim();
                _currentUser.FirstName = PreviousFirstName.Trim();
                _currentUser.FixNumber = PreviousFix?.Trim();
                _currentUser.PhoneNumber = PreviousPhone?.Trim();
                _currentUser.Email = PreviousEmail?.Trim();
                _currentUser.ServiceId = SelectedNewUserServiceId;
                _currentUser.SiteId = SelectedNewUserSiteId;
                userRepository.Edit(_currentUser.Id, _currentUser);
                MessageBox.Show("Utilisateur mis à jour avec succès !");

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
