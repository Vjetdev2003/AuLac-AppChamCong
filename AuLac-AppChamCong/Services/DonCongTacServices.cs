using AuLac_AppChamCong.Connect;
using AuLac_AppChamCong.Models;
using AuLac_AppChamCong.Models.DonCongTac;
using AuLac_AppChamCong.Models.ToaDo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Services
{
    public class DonCongTacServices
    {
        private readonly HttpClient _httpClient;
        ApiContext newapi = new();
        public DonCongTacServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TaoDonCongTacAsync(DonCongTac donCongTac)
        {
            // Xây dựng HTTP request với nội dung dạng JSON
            var request = new HttpRequestMessage(HttpMethod.Post, newapi.apiaulac + "/DonCongTac/CreateDonCongTac");

            // Chuyển đổi đối tượng DonCongTac thành JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(donCongTac), Encoding.UTF8, "application/json");

            // Gán JSON vào request content
            request.Content = jsonContent;

            // Gửi request và nhận phản hồi
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                string message = errorResponse?.message ?? "Lỗi không xác định!";
                throw new Exception(message);
            }   

            // Lấy kết quả trả về từ API
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response JSON: " + responseString); // Kiểm tra JSON trả về

            return responseString;
        }
        public async Task<List<HospitalDb>> GetAllHospitalAsync()
        {
                var url = $"{newapi.apiaulac}/DonCongTac/Hospital/GetAllHospitals";
                var response = await _httpClient.GetAsync(url);
                if(!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: Status {response.StatusCode}, Message: {errorMessage}");
                    return new List<HospitalDb>();
                }
                var responseString = await response.Content.ReadAsStringAsync();

                var hospitalDbs = JsonConvert.DeserializeObject<List<HospitalDb>>(responseString);

                return hospitalDbs;
        }
        public async Task<ToaDo> GetCoordinatesForNoiCongTac(string noiCongTac)
        {
            var url = $"{newapi.apiaulac}/DonCongTac/Hospital/GetCoordinatesForNoiCongTac/{noiCongTac}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: Status {response.StatusCode}, Message: {errorMessage}");
                return new ToaDo();
            }
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response JSON: " + responseString); // Kiểm tra JSON trả về
            var coordinates = JsonConvert.DeserializeObject<ToaDo>(responseString);
            if (coordinates != null)
            {
                coordinates.CalculateDistance();
            }
            return coordinates;
        }
        public async Task<List<DonCongTac>> GetHistoryFormAsync(int psnPrkID)
        {
            try
            {
                var url = $"{newapi.apiaulac}/DonCongTac/GetHistoryForm?psnPrkID={psnPrkID}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: Status {response.StatusCode}, Message: {errorMessage}");
                    return new List<DonCongTac>();
                }

                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + responseString);

                var donCongTacResponse = JsonConvert.DeserializeObject<DonCongTacResponse>(responseString);
                return donCongTacResponse?.Success == true ? donCongTacResponse.Data : new List<DonCongTac>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching history: {ex.Message}");
                return new List<DonCongTac>();
            }
        }

    }
}
