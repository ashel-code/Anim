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
			saveFrameWithPath(currentFrame);
		}


		private void openButtonClicked(object sender, EventArgs e)
		{
			openFrameBool = true;
			canvasView.InvalidateSurface();
		}

		private void clearButtonClicked(object sender, EventArgs e)
		{
			clearBool = true;
			canvasView.InvalidateSurface();
		}

		private void eraserButtonClicked(object sender, EventArgs e)
		{
			saveFrameForCarouselView(currentFrame);
			currentFrame++;
		}

		private void pencilButtonClicked(object sender, EventArgs e)
		{

		}

		void doMagic(object sender, EventArgs e)
		{
			
		}
	}
}