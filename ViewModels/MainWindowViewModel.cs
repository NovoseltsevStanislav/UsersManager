using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace UsersManager.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        // public string Greeting { get; } = "Welcome to Avalonia!";

        [ObservableProperty] //Обозначение свойства в CommunityToolkit.Mvvm
        private ViewModelBase _currentPage;

        private readonly UserListViewModel _homePage = new();
        private readonly AddUserViewModel _newUserPage = new();
        private readonly EditUserViewModel _editUserPage = new();
        
        //Конструктор
        public MainWindowViewModel()
        {
            CurrentPage = _homePage;
        }

        [RelayCommand] //Обозначение метода в CommunityToolkit.Mvvm
        private void GoToHome()
        {
            CurrentPage = _homePage;
        }
        
        [RelayCommand]
        private void GoToAddUser()
        {
            CurrentPage = _newUserPage;
        }
        
        [RelayCommand]
        private void GoToEditUser()
        {
            CurrentPage = _editUserPage;
        }

    }
}
