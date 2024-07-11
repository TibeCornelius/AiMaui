using MNIST.NeuralNetworks;
using MNIST.Data;


namespace Ai.MNIST.UI
{
    public partial class CurrentNetworkPage : ContentPage
    {
        public Manager myManager;
        public CurrentNetworkPage( Manager manager )
        {
            this.myManager = manager;
            InitializeComponent();
        }

        private async void ImporteSetOfTrainingImages( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new ImportImagesPage( myManager, myManager.ImportSetOfImages, Mode.Training ) );
        }
        private async void PreviewAndImportImage( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new PreviewImagePage( myManager.GetSingleTestingImage, myManager.GetSingTrainingleImage, myManager.ImportSingleImage ) );
        }

        private async void ImporteSetOfTestingImages( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new ImportImagesPage( myManager, myManager.ImportSetOfImages, Mode.Testing ) );
        }
        private async void CurrentNetworkStats( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new CurrentNetworkStatsPage( myManager.GetLatestTrainingSet() ) );
        }
    }
}