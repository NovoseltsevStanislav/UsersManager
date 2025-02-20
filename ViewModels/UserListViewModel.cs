using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UsersManager.Models;
using UsersManager.ViewModels;

namespace UsersManager.ViewModels
{
    public partial class UserListViewModel: ViewModelBase
    {
        [ObservableProperty] private string? _fileText;
        [ObservableProperty] private string? _filePath;
        
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
            Console.WriteLine(FilePath);
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
        
    }
}
