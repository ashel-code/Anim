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
		private void addFrame(/*selectedFrame can be there when the frames will be sent from server*/)
        {
			amountOfFrames++;

			// renaming
			foreach (int i in indexOfImages)
            {
				string oldFileName = frameFileName + i.ToString() + fileExtention;
				string newFileName = frameFileName + (1 + i).ToString() + fileExtention;
				File.Move(oldFileName, newFileName);
			}

			int selectedFrame = MainCarouselView.Position;

			for (int i = selectedFrame; i < amountOfFrames; i++)
            {
				string oldFileName = frameFileName + i.ToString() + fileExtention;
				string newFileName = frameFileName + (1 + i).ToString() + fileExtention;
				File.Move(oldFileName, newFileName);
			}

			indexOfImages.Add(amountOfFrames);


			saveFrameWithIndex(selectedFrame + 1);
			
        }
	}
}