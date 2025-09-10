using Microsoft.Win32;
using PagesRouges.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Document = QuestPDF.Fluent.Document;

namespace PagesRouges.ViewModel
{
    public class UserDetailsViewModel : ViewModelBase
    {
        // Champs
        public User _user;
        private User _currentUser;
        private string _nameAndFirstName;

        // Proprietes
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(FixNumber));
                OnPropertyChanged(nameof(PhoneNumber));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Service));
                OnPropertyChanged(nameof(Site));
            }
        }

        public Guid Id => User?.Id ?? Guid.Empty;
        public string Name => User?.Name ?? string.Empty;
        public string FirstName => User?.FirstName ?? string.Empty;
        public string FixNumber => User?.FixNumber ?? string.Empty;
        public string PhoneNumber => User?.PhoneNumber ?? string.Empty;
        public string Email => User?.Email ?? string.Empty;
        public string Service => User?.Service ?? string.Empty;
        public string Site => User?.Site ?? string.Empty;

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
        public string NameAndFirstName
        {
            get
            {
                return _nameAndFirstName;
            }
            set
            {
                _nameAndFirstName = value;
                OnPropertyChanged(nameof(NameAndFirstName));
            }
        }

        // Commandes
        public ICommand ExportToPdfCommand { get; }
        public ICommand CloseCommand { get; }

        // Constructeur
        public UserDetailsViewModel(User user)
        {
            // Commandes
            ExportToPdfCommand = new ViewModelCommand(ExecuteExportToPdfCommand);
            CloseCommand = new ViewModelCommand(ExecuteCloseCommand);

            // Chargement des donnees utilisateurs
            User = user;
            NameAndFirstName = $"{user.Name} {user.FirstName}";
        }
        private void ExecuteExportToPdfCommand(object obj)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    Title = "Enregistrer le ticket en PDF",
                    FileName = $"User_{User.Id}.pdf"
                };
                if (saveFileDialog.ShowDialog() != true) return;
                QuestPDF.Settings.License = LicenseType.Community;

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(x =>
                            {
                                x.Item().Text("Pages Rouges").FontSize(20).Bold();
                                x.Item().Text($"ID: {User?.Id}").FontSize(12);
                                x.Item().Text($"Nom: {User?.Name}").FontSize(12);
                                x.Item().Text($"Prenom: {User?.FirstName}").FontSize(12);
                            });
                    });
                }).GeneratePdf(saveFileDialog.FileName);

                MessageBox.Show("PDF créé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exportation du pdf : {ex.Message}",
                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteCloseCommand(object obj)
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
