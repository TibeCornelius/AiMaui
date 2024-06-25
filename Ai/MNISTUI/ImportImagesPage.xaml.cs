using Ai.MNIST.NeuralNetworks;
using Ai.MNIST.NeuralNetworks.TrainingResults;

namespace Ai.MNIST.UI
{
    public partial class ImportImagesPage : ContentPage
    {
        public ImportImages ourImportedImageSettings;
        private Network myNetwork;
        public delegate List<TrainingBatch> RunImagesThroughNetwork( ImportImages trainingImages, bool DisplayResults = false );
        public RunImagesThroughNetwork delRunImagesThroughNetwork;

        private List<TrainingBatch> myTrainingResult;

        public ImportImagesPage( Network network, RunImagesThroughNetwork runImagesThroughNetwork )
        {
            this.ourImportedImageSettings = new ImportImages();
            this.myNetwork = network;
            this.delRunImagesThroughNetwork = runImagesThroughNetwork;
            this.myTrainingResult = new List<TrainingBatch>();
            InitializeComponent();
        }
        private void TextChangesImageCount( object sender, TextChangedEventArgs e )
        {
            int newAmmount;
            try
            {
                newAmmount = Convert.ToInt16( e.NewTextValue );
            }
            catch
            {
                return;
            }
            ourImportedImageSettings.Ammount = newAmmount;
        }
        private void PassImagesThroughNetwork( object sender, EventArgs e )
        {
            RemoveOldResults();
            myTrainingResult = delRunImagesThroughNetwork( ourImportedImageSettings, false );
            DisplayTrainingResults();
        }
        private void RemoveOldResults()
        {
            TrainingResultsContainer.Children.Clear();
            myTrainingResult.Clear();
        }
        
        private void DisplayTrainingResults()
        {
            if( myTrainingResult.Count == 0 )
            {
                return;
            }

            foreach( TrainingBatch results in myTrainingResult )
            {
                Label lCorrectGuesses = new Label
                {
                    Text = $"Ammount of correct guesses {results.CorrectGuesses}"
                };
                Label lAverageCost = new Label
                {
                    Text = $"Average cost {results.TotalAverageCost }"
                };
                TrainingResultsContainer.Children.Add( lCorrectGuesses );
                TrainingResultsContainer.Children.Add( lAverageCost );
            }
        }
        private void TextChangedItterationsCount( object sender, TextChangedEventArgs e )
        {
            int newAmmount;
            try
            {
                newAmmount = Convert.ToInt16( e.NewTextValue );
            }
            catch
            {
                return;
            }
            ourImportedImageSettings.Itterations = newAmmount;
        }
    }
}