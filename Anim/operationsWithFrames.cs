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
		private void AddFrame(/*selectedFrame can be there when the frames will be sent from server*/)
        {
			amountOfFrames++;

			// renaming
			
			int selectedFrame = MainCarouselView.Position;

			for (int i = (selectedFrame + 1); i < (amountOfFrames - 1); i++)
            {
				string oldFileName = frameFileName + i.ToString() + fileExtention;
				string newFileName = frameFileName + (1 + i).ToString() + fileExtention;
				File.Move(oldFileName, newFileName);
			}

			indexOfImages.Add(amountOfFrames);


			saveFrameWithIndex(selectedFrame + 1);
			saveFrameForCarouselView(selectedFrame + 1);

			updateCarouselView();
        }
	}
}