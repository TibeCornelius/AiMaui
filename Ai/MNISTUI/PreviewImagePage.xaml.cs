using Ai.MNIST.NeuralNetworks;
using MauiImage = Microsoft.Maui.Controls.Image;
using MnistImage = Ai.MNIST.NeuralNetworks.Image;
namespace Ai.MNIST.UI
{
    public partial class PreviewImagePage : ContentPage
    {
        public delegate MnistImage  GetImage();
        private GetImage myGetTrainingImage;
        private GetImage myGetTestingImage;
        public delegate TrainingDataOutput RunImageThroughNetwork( MnistImage image );
        private RunImageThroughNetwork myImageProccesor;
        private MnistImage myCurrentImage;
        public PreviewImagePage( GetImage getTestingimage, GetImage getTrainingImage, RunImageThroughNetwork imageProccesing )
        {
            InitializeComponent();
            this.myGetTestingImage = getTestingimage;
            this.myGetTrainingImage = getTrainingImage;
            MnistImage image = getTrainingImage();
            this.myCurrentImage = image;
            DisplayImage( image );
            this.myImageProccesor = imageProccesing;
        }
        private void NewTestingImage( object sender, EventArgs e )
        {
            myCurrentImage = myGetTestingImage();
            DisplayImage( myCurrentImage );
            
        }
        private void ProccesAndDisplayImageResults( object sender, EventArgs e )
        {
            ClearCurrentResults();
            DisplayCurrentImageResults( myImageProccesor( myCurrentImage ) );
        }
        private void NewTrainingImage( object sender, EventArgs e )
        {
            myCurrentImage = myGetTrainingImage();
            DisplayImage( myCurrentImage );
        }
        private void DisplayImage( MnistImage image )
        {
            myGraphicsView.Drawable = new MnistDrawable( image.ImageData );
        }
        private void ClearCurrentResults()
        {
            ImageResultsContainer.Children.Clear();            
        }
        private void DisplayCurrentImageResults( TrainingDataOutput results )
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


    public class MnistDrawable : IDrawable
    {
        private readonly byte[,] _data;

        public MnistDrawable(byte[,] data)
        {
            _data = data;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float cellSize = dirtyRect.Width / 28f;

            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    byte value = _data[j, i];
                    float grayScale = value / 255f;
                    canvas.FillColor = new Color(grayScale, grayScale, grayScale);
                    canvas.FillRectangle(i * cellSize, j * cellSize, cellSize, cellSize);
                }
            }
        }
    }
}