using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PagesRouges.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        // Champs
        private ViewModelBase _currentChildView;

        // Proprietes
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {

                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        // Commandes
        public ICommand ShowUsersListCommand { get; }

        // Constructeur
        public MainViewModel()
        {
            // Initialisation des commandes
            ShowUsersListCommand = new ViewModelCommand(ExecuteShowUsersListCommand);

            ExecuteShowUsersListCommand(null);
        }
        private void ExecuteShowUsersListCommand(object obj)
        {
            CurrentChildView = new UsersInfosViewModel();
        }
    }
}
