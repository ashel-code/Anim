﻿using System;
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
		private string formatNumber(int number)
        {
			if (number < 1000000)
            {
				int amountOfSimbols = number.ToString().Length;
				string result = "";

				for (int i = 0; i < (6 - amountOfSimbols); i++)
                {
					result += "0";
                }
				result += number.ToString();
				return result;
            }
            else
            {
				return null;
            }
        }


		private void updateCarouselView()
        {
			images.Sort();
			MainCarouselView.ItemsSource = images.ToArray();
		}

		// function fur usual saving image from canvas.
		private void saveFrame(SkiaSharp.SKSurface surface, string extPathSaving)
		{
			string pathSaving = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string fileoutSaving = Path.Combine(pathSaving, extPathSaving);


			// making "screenshot" of survace
			SKData skData = surface.Snapshot().Encode();

			// opening a stream and setting path for writing
			using (Stream stream1 = File.OpenWrite(fileoutSaving))
			{
				// saving "screenshot" we got in steam we opened
				skData.SaveTo(stream1);
			}
		}

		// function for saving image for preview in carouselview
		private void saveFrameForCarouselView(SkiaSharp.SKSurface surface, string extPathSaving)
		{
			// turning the bool for saving as preview off
			cutForCarouselview = false;

			// turning the bool for saving of
			saveFrameBool = false;
			// calling function for usual saving
			saveFrame(surface, fileName);

			this.BindingContext = this;
			// turning the saving bool off
			openFrameBool = false;

			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string fileout = Path.Combine(path, fileName);

			// getting bitmap from just saved image file
			SKBitmap bitmap = SKBitmap.Decode(fileout);

			// creating info about image and set a less size
			SKImageInfo imageInfo = new SKImageInfo(Convert.ToInt32(carouselFrameWight), Convert.ToInt32(carouselFrameHeight));

			//Console.WriteLine("!!!!!!!!!!!!!!!!!!");
			//Console.WriteLine(Convert.ToInt32(carouselFrameWight));
			//Console.WriteLine(Convert.ToInt32(carouselFrameHeight));

			// creating resized bitmap from old one, resizing it with info about image we created
			SKBitmap resized = bitmap.Resize(imageInfo, SKBitmapResizeMethod.Lanczos3);
			// creating image from bitmap
            SKImage image = SKImage.FromBitmap(resized);


			string pathSaving = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string fileoutSaving = Path.Combine(path, extPathSaving);



			//encoding image before saving
			SKData skData = image.Encode();


			// opening a stream and setting path for writing
			using (Stream stream1 = File.OpenWrite(fileoutSaving))
			{
				// saving "screenshot" we got in steam we opened
				skData.SaveTo(stream1);
			}

			// adding just added image to list of images for frames preview in carouselview
			images.Add(fileoutSaving);
			// updating shown images in carouselview
			updateCarouselView();
		}


		// saving frame using only index of it in video
		private void saveFrameWithIndex(int frameIndex)
        {
			// setting file path we need
			fileName = frameFileName + formatNumber(frameIndex) + fileExtention;
			// turning bool for saving on
			saveFrameBool = true;
			// updating canvasview

			Console.WriteLine("±±±±±±±±±±±±start1");

			// canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
			// 	e	{SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs}	SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs
			// canvasView_PaintSurface(this, )
			canvasView.PaintSurface += canvasView_PaintSurface;
			canvasView.InvalidateSurface();

			Console.WriteLine("±±±±±±±±±±±±end1");

		}

		// saving frame using only index of it in video but for carouselview preview
		private void saveFrameForCarouselView(int frameIndex)
		{
			// turning bool for saving for preview on
			cutForCarouselview = true;

			// setting file path we need
			fileName = carouselFileName + formatNumber(frameIndex) + fileExtention;
			// turning bool for saving on
			saveFrameBool = true;
			// updating canvasview
			canvasView.InvalidateSurface();


		}


		// function for calling server
		static async Task<string> apiRequest(int id) // it must return some value but it doesn't
		{
			// Call asynchronous network methods in a try/catch block to handle exceptions.
			try
			{
				// calling server and tries to get response
				string responseBody = await client.GetStringAsync("http://192.168.1.48/image/0");
				return responseBody;
			}
			// catching the error
			catch (HttpRequestException e)
			{
				return null;
			}


			//  adresses we use
			//  http://185.145.127.69/ai-quotes/0
			//  http://192.168.1.48/ai-quotes/0
		}
	}
}