using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Rendering.Composition;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tmds.DBus.Protocol;
using UsersManager.Models;

namespace UsersManager.ViewModels
{
    public partial class AddUserViewModel : ViewModelBase
    {
        public int IdView { get; set; }
        public string FirstNameView { get; set; }
        public string LastNameView { get; set; }
        public string LoginView { get; set; }
        public string EmailView { get; set; }
        public string PasswordView { get; set; }
        public int AccessLevelIndex { get; set; }
        public string NotesView { get; set; }
        
        List<string> AccessLevelList = ["Guest", "User", "Moderator", "Administrator"];
        
        
        [RelayCommand]
        public void InsertNewUser()
        {
            DataService dataService = new DataService();
            User user = new User
            {
                Id = IdView,
                FirstName = FirstNameView,
                LastName = LastNameView,
                Login= LoginView,
                Email = EmailView, 
                Password = DataService.GetMD5Hash(PasswordView),
                AccessLevel = AccessLevelList[AccessLevelIndex],
                Notes = NotesView
            };
            dataService.InsertUser(user);
        }
    }
}
