using PagesRouges.API;
using PagesRouges.ErrorManager;
using PagesRouges.Model;
using PagesRouges.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PagesRouges.ViewModel
{
    public class AddUserViewModel : ViewModelBase
    {
        // Champs
        private readonly RandomUserService _randomUserService;

        private string _newUserName;
        private string _newUserFirstName;
        private string _newUserFixNumber;
        private string _newUserPhoneNumber;
        private string _newUserEmail;
        private int _selectedNewUserServiceId;
        private int _selectedNewUserSiteId;

        private IUserRepository userRepository;
        private IServiceRepository serviceRepository;
        private ISiteRepository siteRepository;

        // Commandes
        public ICommand FillWithRandomDataCommand { get; }
        public ICommand AddNewUserCommand { get; }
        public ICommand CancelCommand {  get; }

        // Attributs
        public string NewUserName 
        {
            get
            {
                return _newUserName;
            }
            set
            {
                _newUserName = value;
                OnPropertyChanged(nameof(NewUserName));
            }
        }
        public string NewUserFirstName 
        { 
            get
            {
                return _newUserFirstName;
            }
            set
            {
                _newUserFirstName = value;
                OnPropertyChanged(nameof(NewUserFirstName));
            }
        }
        public string NewUserFixNumber 
        { 
            get
            {
                return _newUserFixNumber;
            }
            set
            {
                _newUserFixNumber = value;
                OnPropertyChanged(nameof(NewUserFixNumber));
            }
        }
        public string NewUserPhoneNumber 
        { 
            get
            {
                return _newUserPhoneNumber;
            }
            set
            {
                _newUserPhoneNumber = value;
                OnPropertyChanged(nameof(NewUserPhoneNumber));
            }
        }
        public string NewUserEmail 
        { 
            get
            {
                return _newUserEmail;
            } 
            set
            {
                _newUserEmail = value;
                OnPropertyChanged(nameof(NewUserEmail));
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
        public ObservableCollection<Service> ServiceIds { get; set; } = new ObservableCollection<Service>();
        public ObservableCollection<Site> SiteIds { get; set; } = new ObservableCollection<Site>();
        public AddUserViewModel()
        {
            // Initialisation des repositories
            userRepository = new UserRepository();
            serviceRepository = new ServiceRepository();
            siteRepository = new SiteRepository();

            _randomUserService = new RandomUserService();

            // Chargement des donnees
            LoadAllData();

            // Initialisation des commandes
            FillWithRandomDataCommand = new ViewModelCommand(async o => await ExecuteFillWithRandomDataCommand());
            AddNewUserCommand = new ViewModelCommand(ExecuteAddNewUserCommand);
            CancelCommand = new ViewModelCommand(ExecuteCancelCommand);
        }
        private void LoadAllData()
        {
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
        }
        private async Task ExecuteFillWithRandomDataCommand()
        {
            var random = new Random();
            var users = await _randomUserService.GetUsersAsync(1, "fr");

            if (users?.Results != null && users.Results.Any())
            {
                var u = users.Results;                
                NewUserName = u.First().Name.Last;
                NewUserFirstName = u.First().Name.First;
                NewUserFixNumber = u.First().Phone;
                NewUserPhoneNumber = u.First().Cell;
                NewUserEmail = u.First().Email;

                if (ServiceIds.Any())
                {
                    var randomServiceIndex = random.Next(0, ServiceIds.Count);
                    SelectedNewUserServiceId = ServiceIds[randomServiceIndex].ServiceId;
                }
                if (SiteIds.Any())
                {
                    var randomSiteIndex = random.Next(0, SiteIds.Count);
                    SelectedNewUserSiteId = SiteIds[randomSiteIndex].SiteId;
                }
            }
            else
            {
                MessageBox.Show("Aucun utilisateur généré.");
            }
        }
        private void ExecuteAddNewUserCommand(object obj)
        {
            if (SelectedNewUserServiceId == null)
            {
                MessageBox.Show("Aucun niveau sélectionné !");
                return;
            } else
            {
                var newUser = new User
                {
                    Name = NewUserName,
                    FirstName = NewUserFirstName,
                    FixNumber = NewUserFixNumber,
                    PhoneNumber = NewUserPhoneNumber,
                    Email = NewUserEmail,
                    ServiceId = SelectedNewUserServiceId,
                    SiteId = SelectedNewUserSiteId
                };

                try
                {
                    userRepository.Add(newUser);
                    MessageBox.Show("L'utilisateur a bien été créé.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex, "userRepository.Add");
                    MessageBox.Show($"L'utilisateur n'a pas été ajouté : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool CanExecuteAddNewUserCommand(object obj)
        {
            return (NewUserName == null && NewUserFirstName == null && NewUserFixNumber == null && NewUserPhoneNumber == null && NewUserEmail == null && SelectedNewUserServiceId == null && SelectedNewUserSiteId == null);
        }
        private void ExecuteCancelCommand(object obj)
        {

        }
    }
}
