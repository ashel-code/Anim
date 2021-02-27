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

namespace Anim
{
    public partial class MainPage : ContentPage
    {
        private readonly Dictionary<long, SKPath> temporaryPaths = new Dictionary<long, SKPath>();
        private readonly List<SKPath> paths = new List<SKPath>();

		bool clear = false;
		public MainPage()
        {
             
            InitializeComponent();
            
        }

		//public void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
		//{
		//canvasView.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;

		//SKSurface surface = e.Surface;
		//SKCanvas canvas = surface.Canvas;

		//canvas.Clear(SKColors.DarkBlue);



		//}
		private void clearButtonClicked(object sender, EventArgs e)
        {
			clear = true;
			canvasView.InvalidateSurface();
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
	}
}
