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
        private bool AddNoise;

        public ImportImagesPage( Manager manager, RunImagesThroughNetwork runImagesThroughNetwork, Mode mode )
        {
            this.ourImportedImageSettings = new ImportSettings();
            this.myNetworkManager = manager;
            this.myMode = mode;
            this.delRunImagesThroughNetwork = runImagesThroughNetwork;
            this.myTrainingResult = new List<TrainingSet>();
            manager.ChangeNetworkDisplayImageResults( LiveDisplayImageResults );
            manager.ChangeNetworkDisplaySetResults( LiveDisplaySetResults );
            InitializeComponent();
        }
        private void LiveDisplaySetResults( TrainingSet trainingSet )
        {

        }
        private void LiveDisplayImageResults(ImageData imageData)
        {

            LiveResultsContainer.Children.Add(new Label { Text = $"Image Number: {imageData.ImageNumber}" });


            LiveResultsContainer.Children.Add(new Label { Text = $"Cost: {imageData.Cost}" });

            LiveResultsContainer.Children.Add(new Label { Text = $"Number Guessed: {imageData.NumberGuessed}" });

            LiveResultsContainer.Children.Add(new Label { Text = $"Was Guess Correct: {imageData.wasGuesCorrect}" });
        }

        private void AddNoiseChange( object sender, EventArgs e )
        {
            if( pNoise.SelectedIndex == 1 )
            {
                AddNoise = false;
            }
            else if( pNoise.SelectedIndex == 0 )
            {
                AddNoise = true;
            }
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
            delRunImagesThroughNetwork( ourImportedImageSettings, myMode, true , AddNoise );
            DisplayTrainingResults();
        }
        private void RemoveOldResults()
        {
            TrainingResultsContainer.Children.Clear();
            myTrainingResult.Clear();
        }
        
        private void DisplayTrainingResults()
        {

            myTrainingResult= myNetworkManager.GetLatestTrainingSet();
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