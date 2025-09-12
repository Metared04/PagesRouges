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
    public class EditSiteViewModel : ViewModelBase
    {
        private ObservableCollection<Site> _currentSiteList;
        private Site _currentSite;
        private int _selectedNewUserSiteId;
        private string _previousSiteName;

        private ISiteRepository siteRepository;

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
        public Site CurrentSite
        {
            get
            {
                return _currentSite;
            }
            set
            {
                _currentSite = value;
                OnPropertyChanged(nameof(CurrentSite));
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
        public string PreviousSiteName
        {
            get
            {
                return _previousSiteName;
            }
            set
            {
                _previousSiteName = value;
                OnPropertyChanged(nameof(PreviousSiteName));
            }
        }
        public ICommand ValidateEditCommand { get; }
        public ObservableCollection<Site> SiteIds { get; set; } = new ObservableCollection<Site>();
        public EditSiteViewModel(Site site)
        {
            siteRepository = new SiteRepository();
            ValidateEditCommand = new ViewModelCommand(ExecuteValidateEditCommand);

            LoadData(site);
        }
        private void LoadData(Site site)
        {
            var siteList = new List<Site>();
            siteList = siteRepository.GetAll().ToList();
            CurrentSiteList = new ObservableCollection<Site>(siteList);

            var dbSiteList = new List<Site>();
            dbSiteList = siteRepository.GetAll().ToList();
            SiteIds.Clear();
            foreach (var s in dbSiteList)
            {
                SiteIds.Add(s);
            }

            SelectedNewUserSiteId = site.SiteId;
            PreviousSiteName = site.SiteName;
        }
        private void ExecuteValidateEditCommand(object obj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PreviousSiteName))
                {
                    MessageBox.Show("Le nom du site ne peut pas être vide.");
                    return;
                }
                siteRepository.Update(SelectedNewUserSiteId, PreviousSiteName);
                MessageBox.Show("Site mis à jour !");
                if (Application.Current.Windows.OfType<EditSiteView>().FirstOrDefault() is EditSiteView window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            } catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "EditSiteViewModel.ExecuteValidateEditCommand");
                MessageBox.Show($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }
    }
}
