using Microsoft.Maui.Graphics.Skia;

namespace MNIST.Data
{
    public class MNISTExtra
    {   
        private string imagesPath ;
        private string labelsPath ;

        private int ImagesAmmount;
        private int ImageSize;
        private string PngOutput;
        private byte[] image;
        private byte[] Labels;

        public MNISTExtra()
        {
            this.imagesPath = @"C:\Users\corne\Desktop\Everything\C#\HelloWorld\Ai\AiNumbers\DataSetMNIST\train-images.idx3-ubyte";
            this.labelsPath = @"C:\Users\corne\Desktop\Everything\C#\HelloWorld\Ai\AiNumbers\DataSetMNIST\train-labels.idx1-ubyte";
            this.PngOutput = @"C:\Users\corne\Desktop\Everything\C#\HelloWorld\Ai\AiNumbers\DataBase\PngFiles";
            this.ImagesAmmount = 60000;
            this.ImageSize = 28 * 28;
            this.image = new byte[ ImagesAmmount * ImageSize];
            this.Labels = new byte[ ImagesAmmount ];
            using (FileStream fileStream = new FileStream( imagesPath, FileMode.Open ))
            {
                fileStream.Read( image, 0,  ImagesAmmount * ImageSize);
            }

            using (FileStream fileStream = new FileStream( labelsPath, FileMode.Open ))
            {
                fileStream.Read( Labels, 0, ImagesAmmount );
            }
        }
        public void DisplayEachLabel()
        {
            for( int index = 30000 ; index < 30020 ; index++  )
            {
                Console.WriteLine("index = " + index );
                Console.WriteLine("Label: " + Labels[0]);
            }
            
        }
        public void ConvertDatasetToPng()
        {
            // Create the output directory if it doesn't exist
            Directory.CreateDirectory(PngOutput);

            // Convert each image in the dataset to PNG
            for (int index = 0; index < ImagesAmmount; index++)
            {
                
            }
        }
  
        public void DisplayImage( int index )
        {
            for ( int i = 0 ; i < 28 ; i++ )
            {
                for( int j = 0 ; j < 28 ; j ++ )
                {
                    if ( image[index * 28 * 28 + i * 28 + j ] > 128 )
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine("Label: " + Labels[0]);
        }

        public void DisplayImageEnhanced( int index)
        {
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; j < 28; j++)
                {
                    int pixelValue = image[index * 28 * 28 + i * 28 + j ];
                    char asciiChar = GetAsciiCharacter(pixelValue);
                    Console.Write(asciiChar); 
                }
                Console.WriteLine(); 
            }
        }

        private char GetAsciiCharacter(int pixelValue)
        {
            // Define a range of ASCII characters representing different intensity levels
            char[] asciiChars = { ' ', '.', ',', '-', '~', ':', ';', '=', '!', '*', '#', '$', '@' };
            
            // Map pixel value to ASCII character
            int index = pixelValue * (asciiChars.Length - 1) / 255;
            
            // Ensure index is within bounds
            index = Math.Max(0, Math.Min(index, asciiChars.Length - 1));
            
            return asciiChars[index];
        }

        public void ConvertToPng( int index, string mqlskdjf )
        {
            // Create a directory if it doesn't exist
            Directory.CreateDirectory( PngOutput );

            // Generate a unique file name based on the current timestamp
            string fileName = $"image_{index}_{DateTime.Now:yyyyMMddHHmmssfff}.png";
            string outputPath = Path.Combine( PngOutput, fileName);

            // Create a new bitmap with the dimensions of an MNIST image (28x28)
            using ( SkiaBitmapExportContext bitmap = new( 28, 28, 1f ))
            {
                ICanvas canvas = bitmap.Canvas;
                canvas.FillColor = Colors.White;
                canvas.FillRectangle(0, 0, 28, 28);
                // Set each pixel in the bitmap based on the corresponding value in the MNIST image data
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < 28; j++)
                    {
                        int pixelValue = image[index * 28 * 28 + i * 28 + j ];
                        Color color = new( pixelValue / 255.0f, pixelValue / 255.0f, pixelValue / 255.0f);
                        canvas.FillColor = color;
                        canvas.FillRectangle(j, i, 1, 1);
                    }
                }

                // Save the bitmap as a PNG image
                bitmap.WriteToFile( outputPath );
            }
        }
        public static void ConvertToPng( byte[,] image )
        {
            string PngOutput = @"C:\Users\corne\Desktop\Everything\C#\HelloWorld\Ai\AiNumbers\DataBase\PngFiles";
            int index = 0 ;
            // Create a directory if it doesn't exist
            Directory.CreateDirectory( PngOutput );

            // Generate a unique file name based on the current timestamp
            string fileName = $"image_{index}_{DateTime.Now:yyyyMMddHHmmssfff}.png";
            string outputPath = Path.Combine( PngOutput, fileName);

            // Create a new bitmap with the dimensions of an MNIST image (28x28)
#pragma warning disable CA1416 // Validate platform compatibility
            using (SkiaBitmapExportContext bitmap = new (28, 28, 1f))
            {
                // Set each pixel in the bitmap based on the corresponding value in the MNIST image data
                ICanvas canvas = bitmap.Canvas;
                canvas.FillColor = Colors.White;
                canvas.FillRectangle(0, 0, 28, 28);
                for (int i = 0; i < 28; i++)
                {
                    for (int j = 0; j < 28; j++)
                    {
                        int pixelValue = image[index + i , j ];
                        Color color = new( pixelValue / 255.0f, pixelValue / 255.0f, pixelValue / 255.0f);
                        canvas.FillColor = color;
                        canvas.FillRectangle(j, i, 1, 1);
                    }
                }

                // Save the bitmap as a PNG image
                bitmap.WriteToFile( outputPath );
            }
        }
    }
}