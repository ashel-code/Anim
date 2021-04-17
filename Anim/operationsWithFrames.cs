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

/*
 
string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
string filename = Path.Combine(path, "myfile.txt");

 */



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
				string pathSaving = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

				Console.WriteLine(pathSaving);

				string oldFileName = Path.Combine(pathSaving, frameFileName + formatNumber(i) + fileExtention);
				string newFileName = Path.Combine(pathSaving, frameFileName + formatNumber(i + 1) + fileExtention);
				Console.WriteLine(oldFileName);
				Console.WriteLine(newFileName);
				File.Move(oldFileName, newFileName);
			}

			indexOfImages.Add(amountOfFrames - 1);


			saveFrameWithIndex(selectedFrame + 1);
			saveFrameForCarouselView(selectedFrame + 1);

			updateCarouselView();
        }
	}
}