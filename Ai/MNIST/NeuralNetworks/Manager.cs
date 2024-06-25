using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using Ai.MNIST.NeuralNetworks.TrainingResults;


namespace Ai.MNIST.NeuralNetworks
{
    public struct NetworkValues
    {
        public int LayerCount;
        public int[] NeuronCount;
        
        public NetworkValues()
        {
            this.LayerCount = 0;
            this.NeuronCount = new int[0];
        }
        public void SetDefault()
        {
            this.LayerCount = 3;
            this.NeuronCount = new int[3]{ 400, 150, 10 };
        }
        public void SetCustom( int LayerCount, int[] NeuronCount )
        {
            this.LayerCount = LayerCount;
            this.NeuronCount = NeuronCount;
        }
    }
    public readonly struct Image
    {
        public readonly byte[,] ImageData;
        public readonly string Label;
        public Image( byte[,] ImageData, string Label )
        {
            this.ImageData = ImageData;
            this.Label = Label;
        }

    }
    public class ImportImages
    {
        public int Ammount;
        public int Itterations;
    }
    public class Manager
    {
        public Network? network;
        private List< byte[,] > bTrainingList;
        private List< string > sTrainingList;
        private List< byte[,] > bTestingList;
        private List< string > sTestingList;
        MNIST.Data.Image DataSet;
        public delegate void DisplayResults( ImageData image );
        public Manager()
        {
            this.DataSet = new MNIST.Data.Image();
            this.bTrainingList = new List< byte[,] >();
            this.sTrainingList = new List<string>();
            this.bTestingList = new List< byte[,] >();
            this.sTestingList = new List< string >();
        }
        public Image GetSingTrainingleImage()
        {
            Random random = new Random();
            int index = random.Next( bTrainingList.Count );
            foreach( MNIST.Data.Image image in MNIST.Data.MNIST.ReadTrainingData() )
            {
                bTrainingList.Add( image.Data );
                sTrainingList.Add( Convert.ToString( image.Label ) );
            }
            return new Image( bTrainingList[ index ], sTrainingList[ index ] );
        }
        public Image GetSingTestingingleImage()
        {
            Random random = new Random();
            foreach( MNIST.Data.Image image in MNIST.Data.MNIST.ReadTestData() )
            {
                bTestingList.Add( image.Data );
                sTestingList.Add( Convert.ToString( image.Label ) );
            }
            int index = random.Next( bTestingList.Count );
            return new Image( bTestingList[ index ], sTestingList[ index ] );
        }

        public void StartNewNetwork( NetworkValues networkValues )
        {
            List<int> Neurons = new List<int>();
            foreach( int NeuronCount in networkValues.NeuronCount )
            {
                Neurons.Add( NeuronCount );
            }
            network = new Network( Neurons );
        }

        public void LoadInNetworkFromJson( NetworkJsonFormat JsonSettings )
        {
            network = new Network( JsonSettings );
        }

        public void SerializeWheightAndBiasesToJson( string output )
        {
            if( network == null )
            {
                return;            
            }

            network.CreateJson( output ); 
        }

        public void SerializeCurrentHistory( string OutputLocation )
        {
            if( network is null )
            {
                return;
            }

            network.SerializeStatsToJson( OutputLocation );
        }


        public List<TrainingBatch> ImportSetOfTestingImages( ImportImages trainingImages, bool DisplayResults = false )
        {
            if( network == null )
            {
                return new List<TrainingBatch>();
            }
            foreach( MNIST.Data.Image image in MNIST.Data.MNIST.ReadTestData() )
            {
                bTestingList.Add( image.Data );
                sTestingList.Add( Convert.ToString( image.Label ) );
            }
            
            List<TrainingBatch> trainingResults = new List<TrainingBatch>();
            Random random = new Random();
            int AmmountImages;
            AmmountImages = trainingImages.Ammount;
            int Itterations = trainingImages.Itterations;
            for( int Itteration = 0 ; Itteration < Itterations ; Itteration++ )
            {
                List< byte[,] > listToTrain = new List< byte[,] >();
                List< string > sListToTrain = new List<string>();
                
                for( int index = 0 ; index < AmmountImages ; index++ )
                {
                    int randomnumber = random.Next( bTrainingList.Count );
                    listToTrain.Add( bTestingList[ randomnumber ] );
                    sListToTrain.Add( sTestingList[ randomnumber ] );
                }
                trainingResults.Add( network.Test( listToTrain, sListToTrain, Itteration + 1 ) );
            }

            bTestingList.Clear();
            sTestingList.Clear();
            return trainingResults;
        }

        public List<TrainingBatch> ImportSetOfTrainingImages( ImportImages trainingImages, bool DisplayResults = false )
        {
            if( network == null )
            {
                Console.WriteLine("Neuralnetworkdoes not yet exist create or import one first");
                return new List<TrainingBatch>();
            }
            foreach( MNIST.Data.Image image in MNIST.Data.MNIST.ReadTestData() )
            {
                bTrainingList.Add( image.Data );
                sTrainingList.Add( Convert.ToString( image.Label ) );
            }

            List<TrainingBatch> trainingResults = new List<TrainingBatch>();
            Random random = new Random();
            int AmmountImages;
            AmmountImages = trainingImages.Ammount;
            int Itterations = trainingImages.Itterations;
            for( int Itteration = 0 ; Itteration < Itterations ; Itteration++ )
            {
                List< byte[,] > listToTrain = new List< byte[,] >();
                List< string > sListToTrain = new List<string>();
                
                for( int index = 0 ; index < AmmountImages ; index++ )
                {
                    int randomnumber = random.Next( bTrainingList.Count );
                    listToTrain.Add( bTrainingList[ randomnumber ] );
                    sListToTrain.Add( sTrainingList[ randomnumber ] );
                }
                if( DisplayResults )
                {
                    trainingResults.Add( network.Train( listToTrain, sListToTrain, Itteration + 1, true ));
                }
                else
                {
                    trainingResults.Add( network.Train( listToTrain, sListToTrain, Itteration + 1 ) );
                }
                
            }
            bTrainingList.Clear();
            sTrainingList.Clear();
            return trainingResults;
        }

    }
}


