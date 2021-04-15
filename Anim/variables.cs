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
using System.Threading;

namespace Anim
{
	public partial class MainPage : ContentPage
	{
		// paths for canvas
		private readonly Dictionary<long, SKPath> temporaryPaths = new Dictionary<long, SKPath>();
		private readonly List<SKPath> paths = new List<SKPath>();

		// deffault file names
		string frameFileName = "frame";
		string carouselFileName = "carouselFrame";
		string fileExtention = ".jpg";

		// bools for buttons:
		// used for clearing
		bool clearBool = false;
		// for saving frame
		bool saveFrameBool = false;
		// for opening frame on canvas
		bool openFrameBool = false;
		// for updating the frame
		bool updateFrameBool = false;
		// for checking is saving for frame preview in carouselview
		bool cutForCarouselview = false;

		// tmp:
		static string fileName = "/outfile.jpg";

		// variables for connection to sever
		static readonly HttpClient client = new HttpClient();

		// for frames:
		// used for current folder
		IFolder folder;
		// for path to current folder
		string path;
		// for path to file we need
		string filePath;
		// for list of images in carouselview
		List<string> images;
		List<int> indexOfImages;
		int amountOfFrames;
		// for index of current frame
		int currentFrame;
		// for height of user's device screen
		double screenHeight;
		// for widht of user's device screen
		double screenWight;

		// carousel preview:
		// coefficient that shows how does the carouselview image smaller than the whole screen
		double carouselToScreenKoff;
		// height of image in carouselview
		double carouselFrameHeight;
		// wight of image in carouselview
		double carouselFrameWight;
	}
}