using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using ColorPicker.Effects;



namespace Anim
{
    public partial class MainPage : ContentPage
    {
        public int Red = 0;
        public int Green = 0;
        public int Blue = 0;

        public int LineWidth = 5;
        public MainPage()
        {
            // initializing component
            InitializeComponent();

            amountOfFrames = 1;

            // get screen resolusion
            screenHeight = DeviceDisplay.MainDisplayInfo.Height;
            screenWight = DeviceDisplay.MainDisplayInfo.Width;

            // get sizes of carouselview to format image in future
            carouselFrameHeight = (screenHeight / 6.25) * 1.25 / 2;
            carouselFrameWight = (screenWight / 6.25) * 1.25 / 2;

            this.BindingContext = this;

            // getting current active frame of carouselview
            currentFrame = MainCarouselView.Position;

            // getting path to deffault image:
            // getting current folder
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //string filePath = Path.Combine(path, fileName);
            //filePath = path + fileName;

            // setting deffault path in pathes to images for carouselview 
            images = new List<string>
            {

            };


            updateFrameBool = true;

            indexOfImages = new List<int>
            {

            };




            saveFrameWithIndex(0);
            saveFrameForCarouselView(0);

            //update carouselview
            updateCarouselView();

            Color SelectedColorVariable = ColorPickerEffects.SelectedColor();

            Red = 0;
            Green = 0;
            Blue = 0;

        }

        

        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {

            Console.WriteLine("±±±±±±±±±±±1");
            // getting screen resolution but for canvasview
            double height = DeviceDisplay.MainDisplayInfo.Height;
            double wight = DeviceDisplay.MainDisplayInfo.Width;

            // setting height of canvasview
            canvasView.HeightRequest = height;
            //	e	{SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs}	SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs
            // intilizing surface
            SKSurface surface = e.Surface;
            // intilizing canvas
            SKCanvas canvas = surface.Canvas;


            // clearing canvas:
            // checking bool
            if (clearBool == true)
            {
                // clearing and filling canvas white
                canvas.Clear(SKColors.White);
                // tunting the bool off
                clearBool = false;
                // clearing temp paths
                temporaryPaths.Clear();
                // clearing main paths
                paths.Clear();
                return;
            }

            // saving image from canvas:
            // checking bool
            if (saveFrameBool == true)
            {
                // turning the bool off
                saveFrameBool = false;
                // checking is saving for preview in carouselview
                if (cutForCarouselview)
                {
                    // calling function for saving for carousellview with current surface and file name
                    saveFrameForCarouselView(surface, fileName);
                }
                // if saving isn't for preview:
                else
                {
                    // calling function for usual saving with current surface and file name
                    saveFrame(surface, fileName);
                }
                //images.ForEach(i => Console.Write("{0}\t", i));

                BindingContext = this;
                return;
            }

            // opening image
            if (openFrameBool == true)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string fileout = Path.Combine(path, fileName);


                // creating bitmap from image file
                SKBitmap bitmap = SKBitmap.Decode(fileout);

                // placing bitmap oon the canvas
                canvas.DrawBitmap(bitmap, 0, 0);
                // updating canvas
                canvas.Restore();
                // updating canvasview
                canvasView.InvalidateSurface();
                Console.WriteLine("±±±±±±±±±±±1");
            }


            SKPaint touchPathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                //Color = SKColors.Purple,
                Color = Color.FromRgb(Red, Green, Blue).ToSKColor(),
                StrokeWidth = LineWidth
            };

            // draw the paths
            foreach (KeyValuePair<long, SKPath> touchPath in temporaryPaths)
            {
                canvas.DrawPath(touchPath.Value, touchPathStroke);
            }
            foreach (SKPath touchPath in paths)
            {
                canvas.DrawPath(touchPath, touchPathStroke);
            }

        }

        public void eraserButtonClicked(object sender, EventArgs e)
        {
            // calling saving function with index of frame for preview
            //saveFrameForCarouselView(currentFrame);
            // incrementing the index of current frame
            //currentFrame++;
            Red = 255;
            Green = 255;
            Blue = 255;
            LineWidth = 20;
        }

        private void OnTouch(object sender, SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    // start of a stroke
                    SKPath p = new SKPath();
                    p.MoveTo(e.Location);
                    temporaryPaths[e.Id] = p;
                    break;
                case SKTouchAction.Moved:
                    // the stroke, while pressed
                    if (e.InContact && temporaryPaths.TryGetValue(e.Id, out SKPath moving))
                        moving.LineTo(e.Location);
                    break;
                case SKTouchAction.Released:
                    // end of a stroke
                    if (temporaryPaths.TryGetValue(e.Id, out SKPath releasing))
                        paths.Add(releasing);
                    temporaryPaths.Remove(e.Id);
                    break;
                case SKTouchAction.Cancelled:
                    // we don't want that stroke
                    temporaryPaths.Remove(e.Id);
                    break;
            }

            // update the UI
            if (e.InContact)
                ((SKCanvasView)sender).InvalidateSurface();

            // we have handled these events
            e.Handled = true;
            /*if (Red == null)
            {
                Red = 0;
                Green = 255;
                Blue = 255;
            }*/
        }
    }
}