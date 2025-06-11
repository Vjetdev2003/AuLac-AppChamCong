using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui;
using AuLac_AppChamCong.Services;
namespace AuLac_AppChamCong
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler<CameraBarcodeReaderView, CameraBarcodeReaderViewHandler>();
                });

            builder.Services.AddHttpClient();
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddMudServices();
            // Đăng ký dịch vụ
            builder.Services.AddScoped<AuLac_AppChamCong.Services.DonCongTacServices>();
            builder.Services.AddScoped<AuLac_AppChamCong.Services.AccountServices>();
            builder.Services.AddScoped<AuLac_AppChamCong.Services.ChamCongServices>();
            builder.Services.AddScoped<AuLac_AppChamCong.Services.LichNghiServices>();
            builder.Services.AddScoped<AuLac_AppChamCong.Services.NotificationServices>();
            builder.Services.AddScoped<AuLac_AppChamCong.Services.FaceIdServices>();
            return builder.Build();
        }
    }
}
