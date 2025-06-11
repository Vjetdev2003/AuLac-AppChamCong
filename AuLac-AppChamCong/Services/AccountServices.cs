using AuLac_AppChamCong.Connect;
using AuLac_AppChamCong.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Services
{
    public class AccountServices
    {
        private readonly HttpClient _httpClient;
        ApiContext newapi = new();
        public AccountServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserTraVe> CheckLogin(string userID, string userPassword)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, newapi.apiaulac + "/Login");

            var formData = new MultipartFormDataContent
        {
        { new StringContent(userID), "UserID" },
        { new StringContent(userPassword), "UserPassword" }
        };

            request.Content = formData;

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = errorResponse?.message ?? "Lỗi không xác định!";
                throw new Exception(message);
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response JSON: " + responseString); // Kiểm tra JSON trả về

            var ketqua = JsonConvert.DeserializeObject<UserTraVe>(responseString);
            return ketqua;
        }
    }
}
