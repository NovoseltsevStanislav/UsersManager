using Avalonia.Controls;
using Microsoft.Win32;
using System.Windows;

namespace UsersManager.Models;

    // Настройки для работы с базой данных.
    public class DatabaseSettings
    {
        // Путь к файлу базы данных.
        public string DatabasePath { get; set; } = "users.db";
        
    }

