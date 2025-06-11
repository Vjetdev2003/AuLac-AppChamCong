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
    public class NotificationServices
    {
        private readonly HttpClient _httpClient;
        public NotificationServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        ApiContext newapi = new();

        public async Task<bool> CreateNotificationAsync(NotificationDb notification)
        {
            try
            {
                // Tạo request POST tới API
                var request = new HttpRequestMessage(HttpMethod.Post, newapi.apiaulac + "/Notification/CreateNotification");

                // Serialize đối tượng notification thành JSON và thêm vào body của request
                var jsonContent = JsonConvert.SerializeObject(notification);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi request tới API
                var response = await _httpClient.SendAsync(request);

                // Kiểm tra trạng thái của response
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorMessage}");
                    throw new Exception($"API Error {response.StatusCode}: {errorMessage}");
                }

                // Đọc phản hồi từ API
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON: " + responseString);

                // Kiểm tra nếu phản hồi rỗng hoặc không hợp lệ
                if (string.IsNullOrWhiteSpace(responseString))
                {
                    Console.WriteLine("API returned empty response.");
                    return false;
                }

                // Giả sử API trả về JSON với một thuộc tính "success" để xác định kết quả
                // Nếu API của bạn có cấu trúc khác, bạn cần điều chỉnh logic này
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);
                bool success = responseObject?.success ?? false;

                if (!success)
                {
                    Console.WriteLine("Failed to create notification: API returned success = false.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateNotificationAsync: {ex.Message}");
                return false; // Trả về false nếu có lỗi xảy ra
            }
        }
        public async Task<List<NotificationDb>> GetAllNotificationsAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, newapi.apiaulac + "/Notification/GetAllNotifications");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode} - {errorMessage}");
                throw new Exception($"API Error {response.StatusCode}: {errorMessage}");
            }
            var responseString = await response.Content.ReadAsStringAsync();

            var notifications = JsonConvert.DeserializeObject<List<NotificationDb>>(responseString);

            return notifications;
        }
        public async Task<bool> MarkAsReadAsync(int notificationID)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, newapi.apiaulac + $"/Notification/MarkAsReadAsync/{notificationID}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode} - {errorMessage}");
                throw new Exception($"API Error {response.StatusCode}: {errorMessage}");
            }
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response JSON: " + responseString);
            return true;
        }
        public async Task<List<NotificationDb>> GetNotificationByPsnPrkID(int psnPrkId)
        {
            try
            {
                // Tạo yêu cầu GET mà không cần body
                var request = new HttpRequestMessage(HttpMethod.Get, newapi.apiaulac + $"/Notification/GetNotificationByPsnPrkID/{psnPrkId}");

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

                // Chuyển chuỗi JSON thành danh sách đối tượng
                var notifications = JsonConvert.DeserializeObject<List<NotificationDb>>(responseString);

                return notifications;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    }
}
