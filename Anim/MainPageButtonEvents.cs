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
			saveFrameBool = true;
			canvasView.InvalidateSurface();
		}


		private void openButtonClicked(object sender, EventArgs e)
		{
			Console.WriteLine("что");
			openFrameBool = true;
			canvasView.InvalidateSurface();
		}

		private void clearButtonClicked(object sender, EventArgs e)
		{
			Console.WriteLine("clear pressed");
			clearBool = true;
			canvasView.InvalidateSurface();
		}

		private void eraserButtonClicked(object sender, EventArgs e)
		{
			Console.WriteLine("are");
			saveFrameBool = true;
			canvasView.InvalidateSurface();
			MainCarouselView.ItemsSource = images.ToArray();
		}

		private void pencilButtonClicked(object sender, EventArgs e)
		{

		}

		void doMagic(object sender, EventArgs e)
		{
			
		}
	}
}