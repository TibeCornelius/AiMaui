using MNIST.NeuralNetworks;
using MNIST.NeuralNetworks.TrainingResults;
using CommunityToolkit.Maui.Views;
using MauiImage = Microsoft.Maui.Controls.Image;
using MnistImage = MNIST.NeuralNetworks.Image;
using MNIST.Data;
using Microsoft.Maui.Controls.Handlers;
using CommunityToolkit.Maui.Core;
namespace Ai.MNIST.UI
{
    public partial class PreviewImagePage : ContentPage
    {
        public delegate MnistImage  GetImage( bool AddNoise );
        private GetImage myGetTrainingImage;
        private GetImage myGetTestingImage;
        public delegate TrainingSet RunImageThroughNetwork( MnistImage image );
        private RunImageThroughNetwork myImageProccesor;
        private MnistImage myCurrentImage;
        private double myNoiseFactor;
        private double myZoomFactor;
        private double myRotationAngle;
        private double Variance;

        MauiImage mauiImage;
        public PreviewImagePage( GetImage getTestingimage, GetImage getTrainingImage, RunImageThroughNetwork imageProccesing )
        {
            InitializeComponent();
            this.mauiImage = new MauiImage();
            this.myGetTestingImage = getTestingimage;
            this.myGetTrainingImage = getTrainingImage;
            MnistImage image = getTrainingImage( false );
            this.myCurrentImage = image;
            this.BindingContext = this;

            DisplayImage( image );
            this.myImageProccesor = imageProccesing;
        }
        private void ZoomFactorChanged( object sender, ValueChangedEventArgs e )
        {
            myZoomFactor = e.NewValue;
        }
        private void NoiseFactorChanged( object sender, ValueChangedEventArgs e )
        {
            myNoiseFactor = e.NewValue;
        }
        private void RotationAngleChanged( object sender, ValueChangedEventArgs e )
        {
            myRotationAngle = e.NewValue;
        }
        private void ChangeVariance( object sender, TextChangedEventArgs e )
        {
            try
            {
                Variance = Convert.ToDouble(e.NewTextValue);
            }
            catch
            {

            }
        }
        
        private void NewTestingImage( object sender, EventArgs e )
        {
            myCurrentImage = myGetTestingImage( false );
            DisplayImage( myCurrentImage );
            
        }
        private void ProccesAndDisplayImageResults( object sender, EventArgs e )
        {
            ClearCurrentResults();
            DisplayCurrentImageResults( myImageProccesor( myCurrentImage ) );
        }
        private void NewTrainingImage( object sender, EventArgs e )
        {
            myCurrentImage = myGetTrainingImage( false );
            DisplayImage( myCurrentImage );
        }
        private void DisplayImage( MnistImage image )
        {
            myGraphicsView.Drawable = new myDrawable( image.ImageData );
        }
        private void ClearCurrentResults()
        {
            ImageResultsContainer.Children.Clear();            
        }
        private void AddNoiseToImage( object sender, EventArgs e )
        {
            byte[,] imageData = myCurrentImage.ImageData;
            Random random = new Random();
           
            imageData = Data.AddNoiseToImage( imageData, 0.1 );
            DisplayImage( myCurrentImage );
        }
        private void AddZoomIn( object sender, EventArgs e )
        {   
            byte[,] imageData = myCurrentImage.ImageData;
            Random random = new Random();
            double ZoomFactor = myZoomFactor;
            int focalX = 14;
            int focalY = 14;
            myCurrentImage = new MnistImage( Data.ZoomImageByMatrix( imageData, (float)ZoomFactor, focalX, focalY ), myCurrentImage.Label );
            DisplayImage( myCurrentImage );
        }
        private async void loadSelfDrawnImage( object sender, EventArgs e )
        {
            int originalBitmap = await DrawableGraphicsView.RenderToBitmapAsync();

            // Resize the image to 28x28
            var resizedBitmap = originalBitmap.Resize(28, 28, ScalingMode.AspectFit);

            // Convert the resized image to a byte array
            var pixelArray = new byte[28 * 28];

            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    var color = resizedBitmap.GetPixel(x, y);
                    // Assuming black is 0 and white is 1 for simplicity
                    // You may adjust this threshold based on your needs
                    pixelArray[y * 28 + x] = (byte)(color.ToGray() > 0.5 ? 255 : 0);
                }
            }
        } 

        private void RotateImage( object sender, EventArgs e )
        {
            byte[,] imageData = myCurrentImage.ImageData;
            double RotationAngle = myRotationAngle;
            myCurrentImage = new MnistImage( Data.RotateImage( imageData, RotationAngle ), myCurrentImage.Label );
            DisplayImage( myCurrentImage );
        }
        private void AddRandomVariance( object sender, EventArgs e )
        {
            byte[,] imageData = myCurrentImage.ImageData;
            myCurrentImage = new MnistImage( Data.AddRandomVariance( imageData, Variance ), myCurrentImage.Label );
            DisplayImage( myCurrentImage );
        }


        private void InvertColors( object sender, EventArgs e )
        {
            byte[,] imageData = myCurrentImage.ImageData;
            int rowLength = imageData.GetLength( 0 );
            int columnLength = imageData.GetLength( 1 );
            byte[,] newImage = Data.InvertColors( imageData );
            myCurrentImage = new MnistImage( newImage, myCurrentImage.Label );
            DisplayImage( myCurrentImage );
        }
        private void DisplayCurrentImageResults( TrainingSet results )
        {
            ClearCurrentResults();
            ImageData realResults = results.ImageData[0];
            Label iGuessed = new Label
            {
                Text= "Network output --> " + realResults.NumberGuessed ,
            };
            Label Cost = new Label
            {
                Text = "The total cost was --> " + realResults.Cost,
            };
            ImageResultsContainer.Children.Add( iGuessed );
            ImageResultsContainer.Children.Add( Cost );
            for( int numberIndex = 0 ; numberIndex < 10 ; numberIndex++ )
            {
                Label ResultProbability = new Label
                {
                    Text = numberIndex + " neuron output --> " + results.ImageData[0].neuronResults[ numberIndex ].output
                };
                ImageResultsContainer.Children.Add( ResultProbability );
            }
            
        }

    }
    
    


    public class myDrawable : IDrawable
    {
        private byte[,]? _mnistData;
        private List<(float x, float y, Color color)> _userDrawings;

        public myDrawable( byte[,]? mnistData = null )
        {
            this._mnistData = mnistData;
            _userDrawings = new List<(float x, float y, Color color)>();
        }

        public void SetMnistData( byte[,] data )
        {
            _mnistData = data;
        }

        public void AddUserDrawing(float x, float y, Color color)
        {
            _userDrawings.Add((x, y, color));
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Draw MNIST Image
            if (_mnistData != null)
            {
                float cellSize = dirtyRect.Width / 28f;

                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < 28; j++)
                    {
                        byte value = _mnistData[j, i];
                        float grayScale = value / 255f;
                        canvas.FillColor = new Color(grayScale, grayScale, grayScale);
                        canvas.FillRectangle(i * cellSize, j * cellSize, cellSize, cellSize);
                    }
                }
            }
        }
    }

}