using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UsersManager.Models;
using System.Linq;

namespace UsersManager.ViewModels
{
    public static class OpenedFile
    {
        public static string DbPath;
    }
    
    public partial class UserListViewModel: ViewModelBase
    {
        [ObservableProperty] private string? _fileText;
        [ObservableProperty] private string? _filePath;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<User> _usersbd;
        public ObservableCollection<User> UsersBD
        {
            get => _usersbd;
            set
            {
                _usersbd = value;
                OnPropertyChanged(nameof(UsersBD));
            }
        }
        
        [RelayCommand]
        public void LoadDataGrid()
        {
            
            var dataService = new DataService();
            var UserList = dataService.GetUserList();
            _usersbd = new ObservableCollection<User>(UserList);
        }
        
        [RelayCommand]
        private async Task OpenFile(CancellationToken token)
        {
            var file = await DoOpenFilePickerAsync();
            if (file is null) return;
            //(Чтение данных из открытого файла)
            // await using var readStream = await file.OpenReadAsync();
            // using var reader = new StreamReader(readStream);
            // FileText = await reader.ReadToEndAsync(token);
            FilePath = file.TryGetLocalPath();
            OpenedFile.DbPath = FilePath;
            LoadDataGrid();
            
            Console.WriteLine(OpenedFile.DbPath);
        }
        
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
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }

    public class UserListVMInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
