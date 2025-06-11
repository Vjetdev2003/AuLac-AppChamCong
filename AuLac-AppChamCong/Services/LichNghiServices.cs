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
    public class LichNghiServices
    {
        private readonly HttpClient _httpClient;

        public LichNghiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        ApiContext newapi = new();
        public async Task<string> CreateLichNghiAsync(LichNghiDb lichNghiDb)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, newapi.apiaulac + "/LichNghi/CreateDonXinNghi");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(lichNghiDb), Encoding.UTF8, "application/json");
            Console.WriteLine("Request JSON: " + JsonConvert.SerializeObject(lichNghiDb));
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
        public async Task<List<LichNghiDb>> GetLichNghiByPsnPrkID(int psnPrkId)
        {
            try
            {
                // Tạo yêu cầu GET mà không cần body
                var request = new HttpRequestMessage(HttpMethod.Get, newapi.apiaulac + $"/LichNghi/GetLichNghiByPsnPrkID/{psnPrkId}");

                // Gửi yêu cầu
                var response = await _httpClient.SendAsync(request);

                // Kiểm tra trạng thái phản hồi
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorMessage}");
                    throw new Exception($"API Error {response.StatusCode}: {errorMessage}");
                }

                // Lấy chuỗi JSON từ phản hồi
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + responseString);

                // Deserialize JSON thành List<LichNghiDb>
                var lichNghiList = JsonConvert.DeserializeObject<List<LichNghiDb>>(responseString);

                return lichNghiList ?? new List<LichNghiDb>(); // Trả về danh sách rỗng nếu null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching LichNghi: {ex.Message}");
                return new List<LichNghiDb>(); // Trả về danh sách rỗng nếu có lỗi
            }
        }
        public async Task<List<LichNghiDb>> GetAllLichNghi()
        {
            try
            {
                // Tạo yêu cầu GET mà không cần body
                var request = new HttpRequestMessage(HttpMethod.Get, newapi.apiaulac + "/LichNghi/GetAllLichNghi");

                // Gửi yêu cầu
                var response = await _httpClient.SendAsync(request);

                // Kiểm tra trạng thái phản hồi
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorMessage}");
                    throw new Exception($"API Error {response.StatusCode}: {errorMessage}");
                }

                // Lấy chuỗi JSON từ phản hồi
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + responseString);

                // Deserialize JSON thành List<LichNghiDb>
                var lichNghiList = JsonConvert.DeserializeObject<List<LichNghiDb>>(responseString);

                return lichNghiList ?? new List<LichNghiDb>(); // Trả về danh sách rỗng nếu null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching LichNghi: {ex.Message}");
                return new List<LichNghiDb>(); // Trả về danh sách rỗng nếu có lỗi
            }

        }

        public async Task<List<NghiLeDb>> GetHolidaysAsync(int month, int year)
        {
            try
            {
                // Tạo yêu cầu GET với query parameters
                var request = new HttpRequestMessage(HttpMethod.Get, newapi.apiaulac + $"/LichNghi/GetHolidaysAsync?month={month}&year={year}");

                // Gửi yêu cầu
                var response = await _httpClient.SendAsync(request);

                // Kiểm tra trạng thái phản hồi
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorMessage}");
                    throw new Exception($"API Error {response.StatusCode}: {errorMessage}");
                }

                // Lấy chuỗi JSON từ phản hồi
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + responseString);

                // Deserialize JSON thành List<NghiLeDb>
                var holidays = JsonConvert.DeserializeObject<List<NghiLeDb>>(responseString);

                return holidays ?? new List<NghiLeDb>(); // Trả về danh sách rỗng nếu null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching holidays: {ex.Message}");
                return new List<NghiLeDb>(); // Trả về danh sách rỗng nếu có lỗi
            }
        }

    }
}
