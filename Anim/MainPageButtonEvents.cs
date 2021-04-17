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
		private void saveButtonClicked(object sender, EventArgs e)
		{
			// calling saving function with index of frame
			saveFrameWithIndex(currentFrame);
		}

		private void AddFrame(object sender, EventArgs e)
        {
			AddFrame();
        }

		private void DeleteCurrentFrame(object sender, EventArgs e)
        {

        }

		private void openButtonClicked(object sender, EventArgs e)
		{
			// turning the opening bool on
			openFrameBool = true;
			// updating canvasview
			//canvasView.InvalidateSurface();
		}

		private void clearButtonClicked(object sender, EventArgs e)
		{
			// turning the clearing bool on
			clearBool = true;
			// updating canvasview
			canvasView.InvalidateSurface();
		}

		private void eraserButtonClicked(object sender, EventArgs e)
		{
			// calling saving function with index of frame for preview
			saveFrameForCarouselView(currentFrame);
			// incrementing the index of current frame
			currentFrame++;
		}

		private void pencilButtonClicked(object sender, EventArgs e)
		{

		}

		void doMagic(object sender, EventArgs e)
		{
			// test output for magix
			Console.WriteLine("magic!! Pfff");
		}
	}
}