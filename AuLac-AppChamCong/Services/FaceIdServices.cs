using AuLac_AppChamCong.Connect;
using AuLac_AppChamCong.Models.FaceId;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuLac_AppChamCong.Services
{
    public class FaceIdServices
    {
        private readonly HttpClient _httpClient;
        private readonly ApiContext _apiContext;

        public FaceIdServices(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiContext = new ApiContext();
        }

        public async Task<string> RegisterFace(decimal psnPrkID, List<string> encodedImages)
        {
            try
            {
                if (psnPrkID <= 0 || encodedImages == null || !encodedImages.Any())
                {
                    throw new ArgumentException("Dữ liệu đầu vào không hợp lệ: Kiểm tra psnPrkID hoặc danh sách hình ảnh.");
                }

                // Log dữ liệu đầu vào
                Console.WriteLine($"[DEBUG] Gửi RegisterFace với PsnPrkID: {psnPrkID}, Số lượng hình ảnh: {encodedImages.Count}");
                foreach (var img in encodedImages)
                {
                    Console.WriteLine($"[DEBUG] Hình ảnh base64 length: {(img?.Length ?? 0)}");
                }

                // Kiểm tra định dạng base64
                foreach (var img in encodedImages)
                {
                    if (string.IsNullOrEmpty(img))
                    {
                        throw new ArgumentException("Danh sách hình ảnh chứa phần tử rỗng!");
                    }

                    try
                    {
                        Convert.FromBase64String(img);
                    }
                    catch (FormatException)
                    {
                        throw new ArgumentException("Danh sách hình ảnh chứa chuỗi base64 không hợp lệ!");
                    }
                }

                // Đổi UserId thành PsnPrkID để khớp với RegisterFaceRequest
                var requestData = new
                {
                    PsnPrkID = psnPrkID, // Đổi từ UserId thành PsnPrkID
                    EncodedImage = encodedImages
                };

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, _apiContext.apiaulac + "/FaceId/RegisterFace")
                {
                    Content = content
                };

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] Phản hồi lỗi từ API RegisterFace: {errorMessage}");
                    try
                    {
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                        string message = errorResponse?.Message?.ToString() ?? $"Lỗi không xác định từ API RegisterFace (Status: {response.StatusCode})";
                        throw new Exception(message);
                    }
                    catch (JsonException)
                    {
                        throw new Exception($"Lỗi từ API RegisterFace (Status: {response.StatusCode}): {errorMessage}");
                    }
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Phản hồi từ API RegisterFace: {responseContent}");

                var responseObject = JObject.Parse(responseContent);
                var messageToken = responseObject["Message"] ?? responseObject["message"];
                if (messageToken == null)
                {
                    var errorToken = responseObject["Error"] ?? responseObject["error"];
                    if (errorToken != null)
                    {
                        throw new Exception($"Lỗi từ API: {errorToken.ToString()}");
                    }
                    var resultToken = responseObject["Result"] ?? responseObject["result"];
                    if (resultToken != null)
                    {
                        return resultToken.ToString();
                    }
                    throw new Exception("Phản hồi từ API RegisterFace không chứa thuộc tính 'Message', 'message', 'Error', 'error', hoặc 'Result', 'result'!");
                }

                return messageToken.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi khi đăng ký khuôn mặt: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DetectFace(byte[] faceImage)
        {
            try
            {
                if (faceImage == null || faceImage.Length == 0)
                {
                    Console.WriteLine("[ERROR] Dữ liệu hình ảnh rỗng hoặc null!");
                    return false;
                }

                using var formData = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(faceImage);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                formData.Add(imageContent, "imageFile", "image.jpg");

                var request = new HttpRequestMessage(HttpMethod.Post, _apiContext.apiaulac + "/FaceId/DetectFace")
                {
                    Content = formData
                };

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] Phản hồi lỗi từ API DetectFace: {errorMessage}");
                    throw new Exception($"Lỗi từ API DetectFace (Status: {response.StatusCode}): {errorMessage}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Phản hồi từ API DetectFace: {responseContent}");

                var responseObject = JObject.Parse(responseContent);
                var hasFaceToken = responseObject["hasFace"] ?? responseObject["hasface"];
                if (hasFaceToken == null)
                {
                    Console.WriteLine("[ERROR] Phản hồi từ API không chứa thuộc tính 'HasFace' hoặc các biến thể!");
                    return false;
                }

                string hasFaceString = hasFaceToken.ToString().ToLower();
                if (hasFaceString != "true" && hasFaceString != "false")
                {
                    Console.WriteLine($"[ERROR] Giá trị HasFace không hợp lệ: {hasFaceString}");
                    return false;
                }

                return hasFaceString == "true";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi khi phát hiện khuôn mặt: {ex.Message}");
                return false;
            }
        }

        public async Task<List<FaceId>> GetAllFaceId()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _apiContext.apiaulac + "/FaceId/GetAllFaceId");
                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] Phản hồi lỗi từ API GetAllFaceId: {errorMessage}");
                    try
                    {
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                        string message = errorResponse?.Message?.ToString() ?? $"Lỗi không xác định từ API GetAllFaceId (Status: {response.StatusCode})";
                        throw new Exception(message);
                    }
                    catch (JsonException)
                    {
                        throw new Exception($"Lỗi từ API GetAllFaceId (Status: {response.StatusCode}): {errorMessage}");
                    }
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Phản hồi từ API GetAllFaceId: {responseContent}");
                var responseObject = JsonConvert.DeserializeObject<List<FaceId>>(responseContent);
                return responseObject ?? new List<FaceId>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi khi lấy danh sách FaceId: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> VerifyFace(decimal psnPrkID, byte[] faceImage)
        {
            try
            {
                if (psnPrkID <= 0)
                {
                    Console.WriteLine("[ERROR] ID người dùng không hợp lệ!");
                    return false;
                }

                if (faceImage == null || faceImage.Length == 0)
                {
                    Console.WriteLine("[ERROR] Dữ liệu hình ảnh rỗng hoặc null!");
                    return false;
                }

                Console.WriteLine($"[DEBUG] Gửi VerifyFace với ID: {psnPrkID}, Image length: {faceImage.Length}");

                using var formData = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(faceImage);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                formData.Add(imageContent, "ImageFile", "image.jpg"); // Đổi "imageFile" thành "ImageFile" để khớp với VerifyFaceRequest
                formData.Add(new StringContent(psnPrkID.ToString()), "PsnPrkID"); // Đổi "psnPrkID" thành "PsnPrkID" để khớp với VerifyFaceRequest

                var request = new HttpRequestMessage(HttpMethod.Post, _apiContext.apiaulac + "/FaceId/VerifyFace")
                {
                    Content = formData
                };

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] Phản hồi lỗi từ API VerifyFace: {errorMessage}");
                    try
                    {
                        var errorResponse = JsonConvert.DeserializeObject<dynamic>(errorMessage);
                        string message = errorResponse?.Message?.ToString() ?? $"Lỗi không xác định từ API VerifyFace (Status: {response.StatusCode})";
                        throw new Exception(message);
                    }
                    catch (JsonException)
                    {
                        throw new Exception($"Lỗi từ API VerifyFace (Status: {response.StatusCode}): {errorMessage}");
                    }
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Phản hồi từ API VerifyFace: {responseContent}");

                var responseObject = JObject.Parse(responseContent);
                var isMatchToken = responseObject["isMatch"];
                if (isMatchToken == null)
                {
                    Console.WriteLine("[ERROR] Phản hồi từ API VerifyFace không chứa thuộc tính 'isMatch'!");
                    return false;
                }

                string isMatchString = isMatchToken.ToString().ToLower();
                if (isMatchString != "true" && isMatchString != "false")
                {
                    Console.WriteLine($"[ERROR] Giá trị IsMatch không hợp lệ: {isMatchString}");
                    return false;
                }

                return isMatchString == "true";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Lỗi khi xác minh khuôn mặt: {ex.Message}");
                throw;
            }
        }
    }
}