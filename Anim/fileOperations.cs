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
		private void openFrame(string extPath)
		{
			IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
			string path = folder.Path;
			string filePath = path + extPath;
		}


		private void saveFrame(SkiaSharp.SKSurface surface, string extPathSaving)
		{
			SKData skData = surface.Snapshot().Encode();

			IFolder folderSaving = PCLStorage.FileSystem.Current.LocalStorage;
			string pathSaving = folderSaving.Path;
			string fileoutSaving = pathSaving + extPathSaving;

			// Plan A
			using (Stream stream1 = File.OpenWrite(fileoutSaving))
			{
				skData.SaveTo(stream1);
			}

			//images.Add(fileoutSaving);

			canvasView.InvalidateSurface();
		}

        [Obsolete]
        private void saveFrameForCarouselView(SkiaSharp.SKSurface surface, string extPathSaving)
		{
			cutForCarouselview = false;

			saveFrameBool = false;
			saveFrame(surface, extPath);
			//images.ForEach(i => Console.Write("{0}\t", i));
			this.BindingContext = this;

			openFrameBool = false;
			//openFrame(extPath);
			IFolder folder = PCLStorage.FileSystem.Current.LocalStorage;
			string path = folder.Path;
			string fileout = path + extPath;
			//temporaryPaths.Clear();
			//paths.Clear();
			
			var bitmap = SKBitmap.Decode(fileout);

			//SKBitmap resized = bitmap.Resize(new SKImageInfo(Convert.ToInt32(carouselFrameWight), Convert.ToInt32(carouselFrameHeight)), SKBitmapResizeMethod.Lanczos3);
			//var image = SKImage.FromBitmap(resized);
			var resized = bitmap.Resize(new SKImageInfo(400, 400), SKBitmapResizeMethod.Lanczos3);
			var image = SKImage.FromBitmap(resized);
			
			SKData skData = image.Encode();
			IFolder folderSaving = PCLStorage.FileSystem.Current.LocalStorage;
			string pathSaving = folderSaving.Path;
			string fileoutSaving = pathSaving + extPathSaving;

			// Plan A
			using (Stream stream1 = File.OpenWrite(fileoutSaving))
			{
				skData.SaveTo(stream1);
			}

			images.Add(fileoutSaving);
			MainCarouselView.ItemsSource = images.ToArray();
		}


		//private void saveFrameWithth(string savingFilePath, bool isForCarousel)
        //{
		//	if (isForCarousel)
        //    {
		//		cutForCarouselview = true;
        //    }
		//	filePath = savingFilePath;
		//	saveFrameBool = true;
		//	canvasView.InvalidateSurface();
		//}


		private void saveFrameForCarouselView(int frameIndex)
        {
			cutForCarouselview = true;

			filePath = "carouselFrame" + frameIndex.ToString() + ".jpg";
			saveFrameBool = true;
			canvasView.InvalidateSurface();
		}



		private void saveFrameWithPath(int frameIndex)
        {
			filePath = "frame" + frameIndex.ToString() + ".jpg";
			saveFrameBool = true;
			canvasView.InvalidateSurface();
		}


		static async Task<string> apiRequest(int id) // it must return some value but it doesn't
		{
			// Call asynchronous network methods in a try/catch block to handle exceptions.
			try
			{
				string responseBody = await client.GetStringAsync("http://192.168.1.48/image/0");
				return responseBody;
			}
			catch (HttpRequestException e)
			{
				return null;
			}



			//  http://185.145.127.69/ai-quotes/0
			//  http://192.168.1.48/ai-quotes/0
		}
	}
}