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
using System.Threading;

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
		bool updateFrameBool = false;
		bool cutForCarouselview = false;

		// tmp:
		static string extPath = "/outfile.jpg";

		// net
		static readonly HttpClient client = new HttpClient();

		// for frames
		IFolder folder;
		string path;
		string filePath;
		List<string> images;
		int currentFrame;
		double screenHeight;
		double screenWight;

		double carouselToScreenKoff;
		double carouselFrameHeight;
		double carouselFrameWight;

		public MainPage()
		{
			InitializeComponent();

			screenHeight = DeviceDisplay.MainDisplayInfo.Height;
			screenWight = DeviceDisplay.MainDisplayInfo.Width;

			carouselFrameHeight = MainCarouselView.Height;
			carouselToScreenKoff = screenHeight / carouselFrameHeight;
			carouselFrameWight = carouselFrameWight * carouselToScreenKoff;

			this.BindingContext = this;

			currentFrame = this.MainCarouselView.Position;

			folder = PCLStorage.FileSystem.Current.LocalStorage;
			path = folder.Path;
			filePath = path + extPath;
			
			//var names = new List<string>
			//{
			//	"1"
			//};

			images = new List<string>
			{
				filePath
			};
			this.MainCarouselView.ItemsSource = images;

			updateFrameBool = true;
			// updateFrameImage();
			//Thread thread = new Thread(updateFrameImage);


		}


		async private void updateFrameImage()
        {
			new Thread(() =>
			{
			    Thread.CurrentThread.IsBackground = true;
			    while (updateFrameBool)
			    {
				    Thread.Sleep(1000);
					//saveFrameWithPath("carouselview.jpg", true);
				    MainCarouselView.ItemsSource = images.ToArray();
			    }
		    }).Start();
        }


		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {

            double height = DeviceDisplay.MainDisplayInfo.Height;
            double wight = DeviceDisplay.MainDisplayInfo.Width;

            canvasView.HeightRequest = height;

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;



            if (clearBool == true)
            {
                canvas.Clear(SKColors.White);
                clearBool = false;
                temporaryPaths.Clear();
                paths.Clear();
                return;
            }

            if (saveFrameBool == true)
            {
                saveFrameBool = false;
                if (cutForCarouselview)
                {
                    saveFrameForCarouselView(surface, extPath);
                }
                else
                {
                    saveFrame(surface, extPath);
                }
                //images.ForEach(i => Console.Write("{0}\t", i));
                this.BindingContext = this;
                return;
            }

            if (openFrameBool == true)
            {
                openFrameBool = false;
                //openFrame(extPath);
                IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
                string path = folder.Path;
                string fileout = path + extPath;
                //temporaryPaths.Clear();
                //paths.Clear();

                SKBitmap bitmap = SKBitmap.Decode(fileout);

                //canvas.DrawBitmap(bitmap, Convert.ToInt32(height), Convert.ToInt32(wight));
                canvas.DrawBitmap(bitmap, 0, 0);
                canvas.Restore();
                canvasView.InvalidateSurface();
            }
            SKPaint touchPathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Purple,
                StrokeWidth = 5
            };

            // draw the paths
            foreach (KeyValuePair<long, SKPath> touchPath in temporaryPaths)
            {
                canvas.DrawPath(touchPath.Value, touchPathStroke);
            }
            NewMwethod(canvas, touchPathStroke);

        }

        private void NewMethod(SKCanvas canvas, SKPaint touchPathStroke)
        {
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
	}
}