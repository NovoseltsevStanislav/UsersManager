﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Rendering.Composition;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace UsersManager.Models
{
    // Модель данных пользователя.
    public class User 
    {
        // Уникальный идентификатор пользователя.
        public int Id { get; set; }
        // Имя пользователя.
        public string FirstName { get; set; } = string.Empty;
        // Фамилия пользователя.
        public string LastName { get; set; } = string.Empty;
        // Логин пользователя.
        public string Login { get; set; } = string.Empty;
        // Пароль пользователя.
        public string Password { get; set; } = string.Empty;
        // Электронная почта пользователя.
        public string Email { get; set; } = string.Empty;
        // Уровень доступа пользователя.
        public string AccessLevel { get; set; } = "Guest"; // Значение по умолчанию
        // Заметки о пользователе.
        public string Notes { get; set; } = string.Empty;
    }
}
