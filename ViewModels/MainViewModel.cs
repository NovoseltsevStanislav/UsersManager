using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UsersManager.Models;
namespace UsersManager.ViewModels;
public partial class MainViewModel : ViewModelBase
{
    //Поле с индексом выбранного пользователя
    [ObservableProperty] 
    private int _userIndex;
    //Поля статуса активности кнопок
    [ObservableProperty]
    private Boolean _dbOpenedStatus = false;
    [ObservableProperty]
    private Boolean _selectUserStatus = false;
    //Поле с коллекцией пользователей (Users) с тремя заранее заготовленными пользователями
    [ObservableProperty]
    private Collection<User> _users = new(new []
    {
        new User(){
            Id = 1,
            FirstName = "Stanislav", 
            LastName = "Novoseltsev", 
            Login = "Stassnov", 
            Password ="123", 
            Email = "stanislav@gmail.com",
            AccessLevel = "Administrator",
            Notes = "First note"
        },
        new User(){
            Id = 2,
            FirstName = "Denise", 
            LastName = "Daniels", 
            Login = "denise.daniels", 
            Password ="ghosts", 
            Email = "denise.daniels@example.com",
            AccessLevel = "User",
            Notes = "Second note"
        },
        new User(){
            Id = 3,
            FirstName = "Nicholas", 
            LastName = "Jenkins", 
            Login = "nicholas.jenkins", 
            Password ="othello", 
            Email = "nicholas.jenkins@example.com",
            AccessLevel = "Moderator",
            Notes = "Third note"
        },
    });
    //Поля для вкладки добавления нового пользователя
    public int IdView { get; set; }
    public string FirstNameView { get; set; }
    public string LastNameView { get; set; }
    public string LoginView { get; set; }
    public string EmailView { get; set; }
    public string PasswordView { get; set; }
    public int AccessLevelIndex { get; set; }
    public string NotesView { get; set; }
    List<string> AccessLevelList = ["Guest", "User", "Moderator", "Administrator"];
    //Конструктор
    public MainViewModel()
    {
        // DataService dataService = new DataService();
        // IList<User> usersList = dataService.GetUserList();
        // IdView = usersList.First().Id;
        // FirstNameView = usersList.First().FirstName;
        // LastNameView = usersList.First().LastName;
        // LoginView = usersList.First().Login;
        // EmailView = usersList.First().Email;
        // PasswordView = usersList.First().Password;
        // AccessLevelIndex = AccessLevelList.IndexOf(usersList.First().AccessLevel);
        // NotesView = usersList.First().Notes;

    }

    [RelayCommand]
    private void Testing()
    {
        Console.WriteLine(UserIndex);
    }
    //Метод добавления нового пользователя
    [RelayCommand]
    private void InsertNewUser()
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
            LoadUsersList();
    }
    //Метод обновления данных пользователя
    [RelayCommand]
    private void UpdateUserData()
    {
        DataService dataService = new DataService();
        IList<User> usersList = dataService.GetUserList();
        IdView = usersList[UserIndex].Id;
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
        dataService.UpdateUser(user);
        LoadUsersList();
    }
    //Метод добавления нового пользователя
    [RelayCommand]
    private void DeleteUser()
    {
        DataService dataService = new DataService();
        IList<User> usersList = dataService.GetUserList();
        if (UserIndex >= 0)
        {
            dataService.DeleteUser(usersList[UserIndex].Id.ToString());
            LoadUsersList();
        }
        else
        {
            return;
        }
    }
    
    //Метод подгрузки данных из БД в коллекцию
    private void LoadUsersList()
    {
        DataService dataService = new DataService();
        IList<User> usersList = dataService.GetUserList();
        Users = new ObservableCollection<User>(usersList);
    }
    //Метод вызываемый по кнопке в интерфейсе, запускающий функцию открытия диалогового окна
    //Также перезаписывает глобальную переменную пути к файлу и статус на 'True'
    [RelayCommand]
    private async Task OpenFile(CancellationToken token)
    {
        var file = await DoOpenFilePickerAsync();
        if (file is null) return;
        var filePath = file.TryGetLocalPath();
        OpenedFile.DbPath = filePath;
        DbOpenedStatus = true;
        LoadUsersList();
        Console.WriteLine(OpenedFile.DbPath);
    }
    //Метод открытия диалогового окна выбора файла с фильтром по типу файла
    private async Task<IStorageFile?> DoOpenFilePickerAsync()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            desktop.MainWindow?.StorageProvider is not { } provider)
            throw new NullReferenceException("Missing StorageProvider instance.");
        var liteDbType = new FilePickerFileType("LiteDB Files")
        {
            Patterns = new[] { "*.db" },
            AppleUniformTypeIdentifiers = new[] { "*.db" },
            MimeTypes = new[] { "DB/*" }
        };
        var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select LiteDB File",
            FileTypeFilter = new[] {liteDbType},
            AllowMultiple = false
        });
        return files?.Count >= 1 ? files[0] : null;
    }
}