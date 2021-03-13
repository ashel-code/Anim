using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using System.Net.Http;
using PCLStorage;
using System.IO;

namespace Anim
{
    public partial class MainPage : ContentPage
    {
        private readonly Dictionary<long, SKPath> temporaryPaths = new Dictionary<long, SKPath>();
        private readonly List<SKPath> paths = new List<SKPath>();
		bool clearBool = false;
		bool saveFrameBool = false;
		bool openFrameBool = false;

		// tmp:
		string extPath = "/outfile.jpg";

		public string[] Frames { get; set; }
		public List<Image> FrameImages { get; set; }

		static readonly HttpClient client = new HttpClient();

		static async Task<string> apiRequest(int id) // it must return some value but it doesn't
		{
			// Call asynchronous network methods in a try/catch block to handle exceptions.
			try
			{
				string responseBody = await client.GetStringAsync("http://192.168.1.48/image/0");
				Console.WriteLine("!!!" + responseBody);
				return responseBody;
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine("\n!!!Exception Caught!");
				Console.WriteLine("crya Message :{0} ", e.Message);
				return null;
			}
		}


	//  http://185.145.127.69/ai-quotes/0
	//  http://192.168.1.48/ai-quotes/0

		public MainPage()
		{
			InitializeComponent();
			Frames = new string[] { "1", "2", "3" };
			this.BindingContext = this;
			// Console.WriteLine("#######");
			// string smth = apiRequest(0).ToString();
			// Console.WriteLine("!!!!!!!" + smth);
			// Console.WriteLine("#######");

		}

		void doMagic(object sender, EventArgs e)
        {
			
        }

		private void clearButtonClicked(object sender, EventArgs e)
        {
			clearBool = true;
			canvasView.InvalidateSurface();
		}

		private void convertToString(SkiaSharp.SKSurface surface)
        {
			
			
		}

		private void saveButtonClicked(object sender, EventArgs e)
        {
			saveFrameBool = true;
		}

		private void saveFrame(SkiaSharp.SKSurface surface, string extPath)
        {
			SKData skData = surface.Snapshot().Encode();

			IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
			string path = folder.Path;
			string fileout = path + extPath;

			Console.WriteLine("2");
			// Plan A)
			using (Stream stream1 = File.OpenWrite(fileout))
			{
				Console.WriteLine("1");
				skData.SaveTo(stream1);
				Console.WriteLine("0");
			}
		}

		private void openButtonClicked(object sender, EventArgs e)
		{
			openFrameBool = true;
		}

		private void openFrame(string extPath)
        {
			IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
			string path = folder.Path;
			string filePath = path + extPath;


		}

		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			var haight = DeviceDisplay.MainDisplayInfo.Height;
			var wight = DeviceDisplay.MainDisplayInfo.Width;
			canvasView.HeightRequest = haight;

			var surface = e.Surface;
			var canvas = surface.Canvas;

			canvas.Clear(SKColors.White);
			if (clearBool == true)
			{
				clearBool = false;
				temporaryPaths.Clear();
				paths.Clear();
				return;
			}
			if (saveFrameBool == true)
			{
				Console.WriteLine("smth");
				saveFrameBool = false;
				saveFrame(surface, extPath);
				return;
            }
			if (openFrameBool == true)
            {
				Console.WriteLine("ну тоже робит вроде");
				openFrameBool = false;
				//openFrame(extPath);
				IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
				string path = folder.Path;
				string fileout = path + extPath;

				//temporaryPaths.Clear();
				//paths.Clear();

				var bitmap = SKBitmap.Decode(fileout);

				Console.WriteLine(fileout);
				canvas.DrawBitmap(bitmap, Convert.ToInt32(haight), Convert.ToInt32(wight));

				canvas.Restore();
			}
			var touchPathStroke = new SKPaint
			{
				IsAntialias = true,
				Style = SKPaintStyle.Stroke,
				Color = SKColors.Purple,
				StrokeWidth = 5
			};

			// draw the paths
			foreach (var touchPath in temporaryPaths)
			{
				canvas.DrawPath(touchPath.Value, touchPathStroke);
			}
			foreach (var touchPath in paths)
			{
				canvas.DrawPath(touchPath, touchPathStroke);
			}

		
		}

		private void OnTouch(object sender, SKTouchEventArgs e)
		{
			switch (e.ActionType)
			{
				case SKTouchAction.Pressed:
					// start of a stroke
					var p = new SKPath();
					p.MoveTo(e.Location);
					temporaryPaths[e.Id] = p;
					break;
				case SKTouchAction.Moved:
					// the stroke, while pressed
					if (e.InContact && temporaryPaths.TryGetValue(e.Id, out var moving))
						moving.LineTo(e.Location);
					break;
				case SKTouchAction.Released:
					// end of a stroke
					if (temporaryPaths.TryGetValue(e.Id, out var releasing))
						paths.Add(releasing);
					temporaryPaths.Remove(e.Id);
					break;
				case SKTouchAction.Cancelled:
					// we don't want that stroke
					temporaryPaths.Remove(e.Id);
					break;
			}
			
			// update the UI
			if (e.InContact)
				((SKCanvasView)sender).InvalidateSurface();

			// we have handled these events
			e.Handled = true;	
		}

		private void eraserButtonClicked(object sender, EventArgs e)
        {

        }

		private void pencilButtonClicked(object sender, EventArgs e)
		{

		}
	}
}
