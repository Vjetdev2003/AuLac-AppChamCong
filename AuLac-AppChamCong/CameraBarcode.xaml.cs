using ZXing.Net.Maui;

namespace AuLac_AppChamCong;

public partial class CameraBarcode 
{
	public CameraBarcode()
	{
		InitializeComponent();
	}
	private void scanner_BarcodesDetected( object sender, BarcodeDetectionEventArgs e)
	{
		scanner.IsDetecting = false;
		Close(e.Results[0].Value);
	} 
}