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

		// graphics and drawing

		double height = DeviceDisplay.MainDisplayInfo.Height;
		double wight = DeviceDisplay.MainDisplayInfo.Width;

		SKSurface surface;
		SKCanvas canvas;

		// for carusel
		public string[] Frames { get; set; }
		public List<Image> FrameImages { get; set; }

		// for api
		static readonly HttpClient client = new HttpClient();


		// tmp:
		string tmpPath = "/outfile.jpg";

		public MainPage()
		{
			InitializeComponent();
			canvasView.HeightRequest = height;
			Frames = new string[] { "1", "2", "3" };
			this.BindingContext = this;
			// Console.WriteLine("#######");
			// string smth = apiRequest(0).ToString();
			// Console.WriteLine("!!!!!!!" + smth);
			// Console.WriteLine("#######");

		}

		private void clear()
		{

			Console.WriteLine("cleared");

			//canvas.Clear(SKColors.White);

			Console.WriteLine("cleared2");
			temporaryPaths.Clear();

			Console.WriteLine("cleared1");
			paths.Clear();

			Console.WriteLine("cleared3");
			canvasView.
		}

		private void saveFrame(SkiaSharp.SKSurface surface, string extPath)
		{
			SKData skData = surface.Snapshot().Encode();

			IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
			string path = folder.Path;
			string fileout = path + extPath;

			Console.WriteLine(fileout);
			// Plan A)
			using (Stream stream1 = File.OpenWrite(fileout))
			{
				Console.WriteLine("1");
				skData.SaveTo(stream1);
				Console.WriteLine("0");
			}
			canvasView.InvalidateSurface();
		}

		private void openFrame(string extPath)
        {
			Console.WriteLine("ну тоже робит вроде");
			IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
			string path = folder.Path;
			string fileout = path + extPath;
			Console.WriteLine("!!!    " + fileout);
			var bitmap = SKBitmap.Decode(fileout);

			canvas.DrawBitmap(bitmap, 0, 0);
			canvas.Restore();
			canvasView.InvalidateSurface();
		}

		private void convertFrameToString(SkiaSharp.SKSurface surface)
		{

		}


		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			Console.WriteLine("iiiiii");
			surface = e.Surface;
			canvas = surface.Canvas;


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