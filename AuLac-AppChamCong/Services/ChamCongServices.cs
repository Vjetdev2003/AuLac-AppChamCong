using AuLac_AppChamCong.Connect;
using AuLac_AppChamCong.Models;
using AuLac_AppChamCong.Models.ToaDo;
using AuLac_AppChamCong.Models.ViewModels;
using Newtonsoft.Json;
using Syncfusion.Blazor.FileManager.Internal;
using Syncfusion.Blazor.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Services
{
    public class ChamCongServices
    {
        private readonly HttpClient _httpClient;

        public ChamCongServices(HttpClient httpClient) {
            _httpClient = httpClient;
        }
        ApiContext newapi = new();

        public async Task<string> CheckInAsync(ChamCongLineModel chamCongLineModel, ChamCongHeaderModel chamCongHeaderModel)
        {
            try
            {
                if (chamCongLineModel == null || chamCongHeaderModel == null)
                {
                    return "Dữ liệu đầu vào không hợp lệ!";
                }
                TimeSpan gioBatDau = chamCongLineModel.GioBatDau;
                chamCongLineModel.Buoi = gioBatDau.Hours < 12 ? 1 : 2;
                chamCongLineModel.ChamCong = 1;
                chamCongHeaderModel.MngChamCongPrkID ??= 0m;
                chamCongLineModel.MngChamCongPrkID ??= 0m;

                // Đảm bảo Status mặc định là 0 (chưa tính Đủ giờ/Thiếu giờ)
                chamCongLineModel.Status = 0;

                var checkInData = new
                {
                    chamCongHeader = chamCongHeaderModel,
                    chamCongLines = chamCongLineModel

                };
                var jsonContent = JsonConvert.SerializeObject(checkInData);
                Console.WriteLine($"[DEBUG] JSON gửi lên API: {jsonContent}");
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{newapi.apiaulac}/ChamCong/CheckIn", content);
                Console.WriteLine($"[DEBUG] API Response Status Code: {response.StatusCode}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] API Response Content: {responseContent}");
                if (!response.IsSuccessStatusCode)
                {
                    string message = "Lỗi không xác định!";
                    try
                    {
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        message = errorResponse?.message?.ToString() ?? message;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Không thể parse JSON lỗi: {ex.Message}");
                    }
                    return $"Lỗi từ API: {message} (Status: {response.StatusCode})";
                }
                return "Check-In thành công!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi khi gọi API Check-In: {ex.Message}");
                return $"Lỗi hệ thống: {ex.Message}";
            }
        }
        public async Task<ResponseModel> CheckLocationAsync(ToaDo model)
        {
            try
            {
                // Log incoming data
                Console.WriteLine($"[DEBUG] Dữ liệu đầu vào model: {JsonConvert.SerializeObject(model)}");

                // Validate model
                if (model == null)
                {
                    Console.WriteLine("[ERROR] Dữ liệu đầu vào không hợp lệ!");
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Dữ liệu đầu vào không hợp lệ!",
                        Data = new ToaDo { Latitude = 0, Longitude = 0 }
                    };
                }

                // Serialize the model into JSON
                var jsonContent = JsonConvert.SerializeObject(model);
                Console.WriteLine($"[DEBUG] JSON gửi lên API: {jsonContent}");

                // Create the HTTP request content
                var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send the POST request to the external API
                var response = await _httpClient.PostAsync($"{newapi}/ChamCong/CheckLocation", requestContent);

                // Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] API Response: Status={response.StatusCode}, Content={responseContent}");
                // Handle the response based on status code
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[ERROR] Lỗi từ API: {responseContent}");
                    return new ResponseModel
                    {
                        Success = false,
                        Message = $"Lỗi từ API: {responseContent}",
                        Data = new ToaDo { Latitude = 0, Longitude = 0 }
                    };
                }

                // Deserialize the response to get the Coordinate
                var result = JsonConvert.DeserializeObject<ToaDo>(responseContent);

                if (result == null)
                {
                    Console.WriteLine("[ERROR] Lỗi trong quá trình deserialization!");
                    return new ResponseModel
                    {
                        Success = false,
                        Message = "Lỗi trong quá trình deserialization!",
                        Data = new ToaDo { Latitude = 0, Longitude = 0 }
                    };
                }

                return new ResponseModel
                {
                    Success = true,
                    Message = "Kiểm tra vị trí thành công.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"[ERROR] Lỗi khi gọi API CheckLocation: {ex.Message}");
                return new ResponseModel
                {
                    Success = false,
                    Message = $"Lỗi khi gọi API CheckLocation: {ex.Message}",
                    Data = new ToaDo { Latitude = 0, Longitude = 0 }
                };
            }
        }

        public async Task<string> CheckOutAsync(ChamCongLineModel model)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Dữ liệu đầu vào model: {JsonConvert.SerializeObject(model)}");

                if (model == null || !model.UserWritePrkID.HasValue || model.UserWritePrkID <= 0 ||
                    !model.Buoi.HasValue || model.Buoi <= 0 || model.NgayCham == default ||
                    model.GioKetThuc == TimeSpan.Zero)
                {
                    Console.WriteLine("[ERROR] Dữ liệu đầu vào không hợp lệ!");
                    return "Dữ liệu đầu vào không hợp lệ!";
                }

                var jsonContent = JsonConvert.SerializeObject(model); // Gửi nguyên model
                Console.WriteLine($"[DEBUG] JSON gửi lên API: {jsonContent}");
                var requestContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{newapi.apiaulac}/ChamCong/CheckOut", requestContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] API Response: Status={response.StatusCode}, Content={responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    return responseContent;
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi khi gọi API Check-Out: {ex.Message}");
                return $"Lỗi hệ thống: {ex.Message}";
            }
        }
        public async Task<string> ChamCongTuXa(ChamCongHeaderModel chamCongHeaderModel, ChamCongLineModel chamCongLineModel)
        {
            chamCongHeaderModel.MngChamCongPrkID ??= 0m;
            chamCongLineModel.MngChamCongPrkID ??= 0m;
            chamCongLineModel.NgayChinhSua = DateTime.Now;
            chamCongLineModel.StatusDescription = "Chấm công từ xa";
            var checkRequest = new 
            {
                chamCongHeader = chamCongHeaderModel,
                chamCongLines = new
                {
                    chamCongLineModel.Buoi,
                    chamCongLineModel.ChamCong,
                    chamCongLineModel.ComputerIP,
                    chamCongLineModel.ComputerName,
                    GioBatDau = chamCongLineModel.GioBatDau.ToString(@"hh\:mm\:ss\.fffffff"), // Định dạng đầy đủ
                    GioKetThuc = chamCongLineModel.GioKetThuc.ToString(@"hh\:mm\:ss\.fffffff"),
                    chamCongLineModel.NgayCham,
                    chamCongLineModel.NgayChinhSua,
                    chamCongLineModel.Status,
                    chamCongLineModel.StatusDescription,
                    chamCongLineModel.UserWritePrkID
                }
            };
            var jsonContent = JsonConvert.SerializeObject(checkRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{newapi.apiaulac}/ChamCong/ChamCongTuXa",content);
            Console.WriteLine($"[DEBUG] API Response Status Code: {response.StatusCode}");
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] API Response Content: {responseContent}");
            if (!response.IsSuccessStatusCode)
            {
                return responseContent;
            }
            return responseContent;

        }
        public async Task<List<HistoryResponse>> GetChamCongHistoryAsync(decimal psnPrkID, int? month = null, int? year = null)
        {
            try
            {
                // Tạo query string từ các tham số
                var queryParams = new List<string> { $"psnPrkID={psnPrkID}" };
                if (month.HasValue) queryParams.Add($"month={month.Value}");
                if (year.HasValue) queryParams.Add($"year={year.Value}");
                var queryString = string.Join("&", queryParams);

                // Tạo URL gọi API
                var url = $"{newapi.apiaulac}/ChamCong/GetChamCongHistory?{queryString}";
                Console.WriteLine($"[DEBUG] API URL: {url}");

                // Gọi API
                var response = await _httpClient.GetAsync(url);
                Console.WriteLine($"[DEBUG] API Response Status Code: {response.StatusCode}");

                // Đọc nội dung phản hồi
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] API Response Content: {responseContent}");

                // Kiểm tra trạng thái phản hồi
                if (!response.IsSuccessStatusCode)
                {
                    string message = "Lỗi không xác định!";
                    try
                    {
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        message = errorResponse?.message?.ToString() ?? message;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Không thể parse JSON lỗi: {ex.Message}");
                    }
                    Console.WriteLine($"[ERROR] Lỗi từ API: {message}");
                    return new List<HistoryResponse>();
                }

                // Deserialize dữ liệu trả về
                var history = JsonConvert.DeserializeObject<List<HistoryResponse>>(responseContent) ?? new List<HistoryResponse>();
                return history;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi khi gọi API GetChamCongHistory: {ex.Message}");
                return new List<HistoryResponse>();
            }
        }
    }
}
