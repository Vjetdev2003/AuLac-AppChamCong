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
    public class LichLamViecService
    {
        private readonly HttpClient _httpClient;

        public LichLamViecService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        ApiContext newapi = new();

        public async Task<string> CreateLichLamViecAsync(LichLamViecDb lichLamViec)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, newapi.apiaulac + "/LichLamViec/CreateLichLamViec");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(lichLamViec), Encoding.UTF8, "application/json");
            Console.WriteLine("Request JSON: " + JsonConvert.SerializeObject(lichLamViec));
            request.Content = jsonContent;

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode} - {errorMessage}");
                throw new Exception($"API Error {response.StatusCode}: {errorMessage}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response JSON: " + responseString);
            return responseString;
        }
    }
}
