using Ai.MNIST.NeuralNetworks;


namespace Ai.MNIST.UI
{
    public partial class CurrentNetworkPage : ContentPage
    {
        Network myNetwork;
        public Manager myManager;
        public CurrentNetworkPage( Network network, Manager manager )
        {
            this.myNetwork = network;
            this.myManager = manager;
            InitializeComponent();
        }

        private async void ImporteSetOfTrainingImages( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new ImportImagesPage( myNetwork, myManager.ImportSetOfTrainingImages ) );
        }
        private async void PreviewAndImportImage( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new PreviewImagePage( myManager.GetSingTestingingleImage, myManager.GetSingTrainingleImage, myNetwork.ImportSingleImage ) );
        }

        private async void ImporteSetOfTestingImages( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new ImportImagesPage( myNetwork, myManager.ImportSetOfTestingImages ) );
        }
        private async void CurrentNetworkStats( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new CurrentNetworkStatsPage( myNetwork.OurResultsContainer.OurTrainingResults ) );
        }
    }
}