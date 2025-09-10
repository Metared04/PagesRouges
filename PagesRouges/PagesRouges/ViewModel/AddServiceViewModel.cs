using PagesRouges.Model;
using PagesRouges.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PagesRouges.ViewModel
{
    public class AddServiceViewModel : ViewModelBase
    {
        // Champs
        private string _newServiceName;

        private IServiceRepository serviceRepository;

        // Commandes
        public ICommand CreateNewServiceCommand { get; }
        public ICommand CloseWindowCommand { get; }

        // Attributs
        public string NewServiceName
        {
            get
            {
                return _newServiceName;
            }
            set
            {
                _newServiceName = value;
                OnPropertyChanged(nameof(NewServiceName));
            }
        }
        public AddServiceViewModel()
        {
            // Initialisation des repositories
            serviceRepository = new ServiceRepository();

            // Initialisation des commandes
            CreateNewServiceCommand = new ViewModelCommand(ExecuteCreateNewServiceCommand);
            CloseWindowCommand = new ViewModelCommand(ExecuteCloseWindowCommand);
        }

        private void ExecuteCreateNewServiceCommand(object obj)
        {
            if (NewServiceName == null)
            {
                MessageBox.Show("Entrez le nom du service !");
                return;
            }
            else
            {
                var newService = new Service
                {
                    ServiceName = NewServiceName
                };

                try
                {
                    serviceRepository.Add(newService);
                    MessageBox.Show("Le service a bien été créé.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la création du site : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ExecuteCloseWindowCommand(object obj)
        {
            if (obj is Window w)
            {
                w.Close();
                return;
            }

            var window = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.DataContext == this);
            window?.Close();
        }
    }
}
