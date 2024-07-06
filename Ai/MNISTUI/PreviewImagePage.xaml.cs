using MNIST.NeuralNetworks;
using MNIST.NeuralNetworks.TrainingResults;
using Microsoft.Maui.Graphics;
using MauiImage = Microsoft.Maui.Controls.Image;
using MnistImage = MNIST.NeuralNetworks.Image;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Handlers;
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
        public PreviewImagePage( GetImage getTestingimage, GetImage getTrainingImage, RunImageThroughNetwork imageProccesing )
        {
            InitializeComponent();
            this.myGetTestingImage = getTestingimage;
            this.myGetTrainingImage = getTrainingImage;
            MnistImage image = getTrainingImage( false );
            this.myCurrentImage = image;
            DisplayImage( image );
            this.myImageProccesor = imageProccesing;
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
           
            for (int row = 0; row < imageData.GetLength( 0 ); row++)
            {
                for (int column = 0; column < imageData.GetLength( 1 ); column++)
                {
                    if ( imageData[ row, column ] < 200)
                    {
                        if ( random.NextDouble() < 0.1)
                        {
                            int noise = random.Next( 0, ( 256 - imageData[row, column] ) / 2 );
                            imageData[ row, column ] = ( byte )Math.Min( 255, imageData[ row, column ] + noise );
                        }
                    }
                }
            }
            DisplayImage( myCurrentImage );
        }


        private void RotateImage90Degrees( object sender, EventArgs e )
        {
            byte[,] imageData = myCurrentImage.ImageData;
            int rowLength = imageData.GetLength( 0 );
            int columnLength = imageData.GetLength( 1 );
            byte[,] newImage = new byte[ rowLength, columnLength ];
            for( int rowIndex = 0 ; rowIndex < rowLength ; rowIndex++ )
            {
                for( int columnIndex = 0 ; columnIndex < columnLength ; columnIndex++ )
                {
                    newImage[ rowIndex, columnIndex ] = imageData[ columnIndex, rowIndex ];
                }
            }
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

            // Draw User Drawings
            foreach ( var drawing in _userDrawings )
            {
                canvas.FillColor = drawing.color;
                canvas.FillCircle(drawing.x, drawing.y, 5); // Example size
            }
        }
    }

}