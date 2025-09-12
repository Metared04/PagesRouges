using PagesRouges.ErrorManager;
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
    public class AddSiteViewModel : ViewModelBase
    {
        // Champs
        private string _newSiteName;

        private ISiteRepository siteRepository;

        // Commandes
        public ICommand CreateNewSiteCommand { get; }
        public ICommand CloseWindowCommand { get; }

        // Attributs
        public string NewSiteName
        {
            get
            {
                return _newSiteName;
            }
            set
            {
                _newSiteName = value;
                OnPropertyChanged(nameof(NewSiteName));
            }
        }
        public AddSiteViewModel()
        {
            // Initialisation des repositories
            siteRepository = new SiteRepository();

            // Initialisation des commandes
            CreateNewSiteCommand = new ViewModelCommand(ExecuteCreateNewSiteCommand);
            CloseWindowCommand = new ViewModelCommand(ExecuteCloseWindowCommand);
        }

        private void ExecuteCreateNewSiteCommand(object obj)
        {
            if(NewSiteName == null)
            {
                MessageBox.Show("Entrez le nom de la ville !");
                return;
            } 
            else
            {
                var newSite = new Site
                {
                    SiteName = NewSiteName
                };

                try
                {
                    siteRepository.Add(newSite);
                    MessageBox.Show("Le site a bien été créé.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Erreur lors de la création du site : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    ErrorLogger.LogError(ex, "UserRepository.Add");
                    throw;
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
