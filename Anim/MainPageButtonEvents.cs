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
			saveFrame(surface, tmpPath);
		}


		private void openButtonClicked(object sender, EventArgs e)
		{
			openFrame(tmpPath);
		}

		private void clearButtonClicked(object sender, EventArgs e)
		{
			clear();
		}

		private void eraserButtonClicked(object sender, EventArgs e)
		{

		}

		private void pencilButtonClicked(object sender, EventArgs e)
		{

		}

		void doMagic(object sender, EventArgs e)
		{

		}
	}
}