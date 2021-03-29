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
		// paths for canvas
		private readonly Dictionary<long, SKPath> temporaryPaths = new Dictionary<long, SKPath>();
		private readonly List<SKPath> paths = new List<SKPath>();

		// bools for buttons
		bool clearBool = false;
		bool saveFrameBool = false;
		bool openFrameBool = false;

		// tmp:
		static string extPath = "/outfile.jpg";

		// net
		static readonly HttpClient client = new HttpClient();

		// for frames
		IFolder folder;
		string path;
		string filePath;
		List<string> images;
		
		public MainPage()
		{
			InitializeComponent();
			this.BindingContext = this;

			// updateFrameImage();


			folder = PCLStorage.FileSystem.Current.LocalStorage;
			path = folder.Path;
			filePath = path + extPath;
			
			var names = new List<string>
			{
				"1"
			};

			images = new List<string>
			{
				filePath
			};
			this.MainCarouselView.ItemsSource = images;
		}

		async void updateFrameImage()
        {

        }

		private void convertToString(SkiaSharp.SKSurface surface)
		{

			
		}


		private void saveFrame(SkiaSharp.SKSurface surface, string extPathSaving)
		{
			SKData skData = surface.Snapshot().Encode();

			IFolder folderSaving = PCLStorage.FileSystem.Current.LocalStorage;
			string pathSaving = folder.Path;
			string fileoutSaving = pathSaving + extPathSaving;

			Console.WriteLine(fileoutSaving);
			// Plan A)
			using (Stream stream1 = File.OpenWrite(fileoutSaving))
			{
				Console.WriteLine("1");
				skData.SaveTo(stream1);
				Console.WriteLine("0");
			}
			canvasView.InvalidateSurface();
		}


		private void openFrame(string extPath)
		{
			IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
			string path = folder.Path;
			string filePath = path + extPath;


		}

		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{

			var height = DeviceDisplay.MainDisplayInfo.Height;
			var wight = DeviceDisplay.MainDisplayInfo.Width;

			canvasView.HeightRequest = height;

			var surface = e.Surface;
			var canvas = surface.Canvas;



			if (clearBool == true)
			{
				Console.WriteLine("cleared");
				canvas.Clear(SKColors.White);
				clearBool = false;
				temporaryPaths.Clear();
				paths.Clear();
				return;
			}

			if (saveFrameBool == true)
			{
				saveFrameBool = false;
				Console.WriteLine("smth");
				saveFrame(surface, extPath);

				this.MainCarouselView.ItemsSource = images;
				return;
			}

			if (openFrameBool == true)
			{
				openFrameBool = false;
				Console.WriteLine("ну тоже робит вроде");
				//openFrame(extPath);
				IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
				string path = folder.Path;
				string fileout = path + extPath;
				Console.WriteLine("!!!    " + fileout);
				//temporaryPaths.Clear();
				//paths.Clear();

				var bitmap = SKBitmap.Decode(fileout);

				//canvas.DrawBitmap(bitmap, Convert.ToInt32(height), Convert.ToInt32(wight));
				canvas.DrawBitmap(bitmap, 0, 0);
				canvas.Restore();
				canvasView.InvalidateSurface();
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
	}
}