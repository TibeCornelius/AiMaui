using MNIST.NeuralNetworks;
using MNIST.NeuralNetworks.TrainingResults;
using MNIST.Data;

namespace Ai.MNIST.UI
{
    public partial class ImportImagesPage : ContentPage
    {
        public ImportSettings ourImportedImageSettings;
        private Manager myNetworkManager;
        public delegate bool RunImagesThroughNetwork( ImportSettings trainingImages, Mode mode,  bool DisplayResults, bool AddNoise = false );
        public RunImagesThroughNetwork delRunImagesThroughNetwork;

        private List<TrainingSet> myTrainingResult;
        private Mode myMode;

        public ImportImagesPage( Manager manager, RunImagesThroughNetwork runImagesThroughNetwork, Mode mode )
        {
            this.ourImportedImageSettings = new ImportSettings();
            this.myNetworkManager = manager;
            this.myMode = mode;
            this.delRunImagesThroughNetwork = runImagesThroughNetwork;
            this.myTrainingResult = new List<TrainingSet>();
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
            delRunImagesThroughNetwork( ourImportedImageSettings, myMode, false , false );
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

            foreach( TrainingSet results in myTrainingResult )
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