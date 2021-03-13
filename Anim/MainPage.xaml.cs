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

namespace Anim
{
    public partial class MainPage : ContentPage
    {
        private readonly Dictionary<long, SKPath> temporaryPaths = new Dictionary<long, SKPath>();
        private readonly List<SKPath> paths = new List<SKPath>();
		bool clear = false;

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
			clear = true;
			canvasView.InvalidateSurface();
		}

		private void convertToString(SkiaSharp.SKCanvas canvas)
        {
			
        }

		private void saveButtonClicked(object sender, EventArgs e)
        {

		}

		private void openButtonClicked(object sender, EventArgs e)
		{

		}

		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			canvasView.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;

			var surface = e.Surface;
			var canvas = surface.Canvas;

			canvas.Clear(SKColors.White);
			if (clear == true)
			{
				clear = false;
				temporaryPaths.Clear();
				paths.Clear();
				return;
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
