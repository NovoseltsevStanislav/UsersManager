using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using UsersManager.ViewModels;
using File = System.IO.File;

namespace UsersManager.Models
{
    class DataService
    {
        
        public IList<User> GetUserList()
        {
            IList<User> userlist = new List<User>();
            using (var db = new LiteDatabase(OpenedFile.DbPath))
            {
                var userdb = db.GetCollection<User>("users");
                var results = userdb.FindAll();
                foreach (User u in results)
                {
                    u.Password = "********";
                    userlist.Add(u);
                }
            }
            return userlist;
        }

        public bool InsertUser(User user)
        {
            using (var db = new LiteDatabase(OpenedFile.DbPath))
            {
                var userdb = db.GetCollection<User>("users");
                try
                {
                    userdb.Insert(user);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
        }

        public bool UpdateUser(User user)
        {
            using (var db = new LiteDatabase(OpenedFile.DbPath))
            {
                int id = user.Id;
                var userdb = db.GetCollection<User>("users");
                var result = userdb.Find(x => x.Id == id).First();
                result = user;
                userdb.Update(result);
                try
                {
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
        }

        public bool DeleteUser(string id2)
        {
            int id = int.Parse(id2);
            using (var db = new LiteDatabase(OpenedFile.DbPath))
            {
                var userdb = db.GetCollection<User>("users");
                try
                {
                    var result = userdb.Find(x => x.Id == id).First();
                    User user = result;
                    userdb.Delete(id);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        public static string GetMD5Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
    }
}
