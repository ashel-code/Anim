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
		public MainPage()
		{
			// initializing component
			InitializeComponent();

			amountOfFrames = 1;

			// get screen resolusion
			screenHeight = DeviceDisplay.MainDisplayInfo.Height;
			screenWight = DeviceDisplay.MainDisplayInfo.Width;

			// get sizes of carouselview to format image in future
			carouselFrameHeight = (screenHeight / 6.25) * 1.25 / 2; 
			carouselFrameWight = (screenWight / 6.25) * 1.25 / 2;
			
			this.BindingContext = this;

			// getting current active frame of carouselview
			currentFrame = MainCarouselView.Position;

			// getting path to deffault image:
			// getting current folder
			//string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//string filePath = Path.Combine(path, fileName);
			//filePath = path + fileName;
			
			// setting deffault path in pathes to images for carouselview 
			images = new List<string>
			{
				
			};
			

			updateFrameBool = true;

			indexOfImages = new List<int>
			{
				
			};

			


			saveFrameWithIndex(0);
			saveFrameForCarouselView(0);

			//update carouselview
			updateCarouselView();
		}



		private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
			// getting screen resolution but for canvasview
            double height = DeviceDisplay.MainDisplayInfo.Height;
            double wight = DeviceDisplay.MainDisplayInfo.Width;

			// setting height of canvasview
            canvasView.HeightRequest = height;

			// intilizing surface
			SKSurface surface = e.Surface;
			// intilizing canvas
			SKCanvas canvas = surface.Canvas;


			// clearing canvas:
			// checking bool
            if (clearBool == true)
            {
				// clearing and filling canvas white
                canvas.Clear(SKColors.White);
				// tunting the bool off
                clearBool = false;
				// clearing temp paths
                temporaryPaths.Clear();
				// clearing main paths
                paths.Clear();
                return;
            }

			// saving image from canvas:
			// checking bool
            if (saveFrameBool == true)
            {
				// turning the bool off
                saveFrameBool = false;
				// checking is saving for preview in carouselview
                if (cutForCarouselview)
                {
					// calling function for saving for carousellview with current surface and file name
                    saveFrameForCarouselView(surface, fileName);
                }
				// if saving isn't for preview:
                else
                {
					// calling function for usual saving with current surface and file name
                    saveFrame(surface, fileName);
                }
                //images.ForEach(i => Console.Write("{0}\t", i));

                BindingContext = this;
                return;
            }

			// opening image
            if (openFrameBool == true)
            {
				string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string fileout = Path.Combine(path, fileName);
				
                
				// creating bitmap from image file
                SKBitmap bitmap = SKBitmap.Decode(fileout);

                // placing bitmap oon the canvas
                canvas.DrawBitmap(bitmap, 0, 0);
				// updating canvas
                canvas.Restore();
				// updating canvasview
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
			foreach (SKPath touchPath in paths)
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
                    SKPath p = new SKPath();
					p.MoveTo(e.Location);
					temporaryPaths[e.Id] = p;
					break;
				case SKTouchAction.Moved:
					// the stroke, while pressed
					if (e.InContact && temporaryPaths.TryGetValue(e.Id, out SKPath moving))
						moving.LineTo(e.Location);
					break;
				case SKTouchAction.Released:
					// end of a stroke
					if (temporaryPaths.TryGetValue(e.Id, out SKPath releasing))
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