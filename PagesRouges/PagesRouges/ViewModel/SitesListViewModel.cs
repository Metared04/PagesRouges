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
    public class SitesListViewModel : ViewModelBase
    {
        private ObservableCollection<Site> _currentSiteList;
        private ObservableCollection<Site> _allSites;
        private string _searchSiteText;

        private ViewModelBase _createNewSiteView;

        private ISiteRepository siteRepository;

        private ViewModelBase _siteView;

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
        public ObservableCollection<Site> AllSites
        {
            get
            {
                return _allSites;
            }
            set
            {
                _allSites = value;
                OnPropertyChanged(nameof(AllSites));
            }
        }
        public string SearchSiteText
        {
            get
            {
                return _searchSiteText;
            }
            set
            {
                _searchSiteText = value;
                OnPropertyChanged(nameof(SearchSiteText));
            }
        }
        public ViewModelBase CreateNewSiteView
        {
            get
            {
                return _createNewSiteView;
            }
            set
            {
                _createNewSiteView = value;
                OnPropertyChanged(nameof(CreateNewSiteView));
            }
        }
        public ViewModelBase SiteView
        {
            get
            {
                return _siteView;
            }
            set
            {
                _siteView = value;
                OnPropertyChanged(nameof(SiteView));
            }
        }
        public ICommand AddSiteCommand { get; }
        public ICommand DeleteSiteCommand { get; }
        public ICommand UpdateSiteCommand { get; }
        public ICommand SearchSiteNameCommand {  get; }
        public SitesListViewModel()
        {
            siteRepository = new SiteRepository();

            AddSiteCommand = new ViewModelCommand(ExecuteAddSiteCommand);
            DeleteSiteCommand = new ViewModelCommand(ExecuteDeleteSiteCommand);
            UpdateSiteCommand = new ViewModelCommand(ExecuteUpdateSiteCommand);
            SearchSiteNameCommand = new ViewModelCommand(ExecuteSearchSiteNameCommand);

            LoadData();
        }
        private void LoadData()
        {
            var siteList = new List<Site>();
            siteList = siteRepository.GetAll().ToList();
            CurrentSiteList = new ObservableCollection<Site>(siteList);
        }
        private void ExecuteAddSiteCommand(object obj)
        {
            CreateNewSiteView = new AddSiteViewModel();
            var window = new AddSiteView
            {
                DataContext = CreateNewSiteView,
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
            LoadData();
        }
        private void ExecuteDeleteSiteCommand(object obj)
        {
            var site = obj as Site;
            if (site == null) return;
            var result = MessageBox.Show($"Supprimer le site \"{site.SiteName}\" ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;
            try
            {
                siteRepository.Remove(site.SiteId);
                CurrentSiteList.Remove(site);
                MessageBox.Show("Site et utilisateurs supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erreur lors de la suppression : {ex}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorLogger.LogError(ex, "siteRepository.Remove");
                throw;
            }
        }
        private void ExecuteUpdateSiteCommand(object obj)
        {
            
        }
        private void ExecuteSearchSiteNameCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchSiteText))
            {
                CurrentSiteList = new ObservableCollection<Site>(_allSites);
            }
            else
            {
                var filtered = _allSites
                    .Where(s => s.SiteName != null &&
                                s.SiteName.ToLower().Contains(SearchSiteText.ToLower()))
                    .ToList();
                CurrentSiteList = new ObservableCollection<Site>(filtered);
            }
        }
    }
}
