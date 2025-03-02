using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace UsersManager.Models;

public class ApiSevice
{
    public static async Task<User?> JsonParse()
    {
        using var httpClient = new HttpClient();

        try
        {
            //Запрос данных
            var json = await httpClient.GetAsync("https://randomuser.me/api/");
            json.EnsureSuccessStatusCode();

            //Чтение JSON
            var jsonString = await json.Content.ReadAsStringAsync();
            //Десериализация
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<RandomUser>(jsonString);
            
            User randomUser = new User
            {
                FirstName = result.results[0].name.first,
                LastName = result.results[0].name.last,
                Login= result.results[0].login.username,
                Email = result.results[0].email, 
                Password = DataService.GetMD5Hash(result.results[0].login.password),
                Notes = result.results[0].location.country+"-"+result.results[0].location.city,
            };
            return randomUser;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
}