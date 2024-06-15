using MNIST.NeuralNetworks;


namespace Ai.MNIST.UI
{
    public partial class CurrentNetworkPage : ContentPage
    {
        Network myNetwork;
        public CurrentNetworkPage( Network network )
        {
            this.myNetwork = network;
            InitializeComponent();
        }

        private async void ImporteSetOfTrainingImages( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new ImportImagesPage() );
        }

        private async void ImporteSetOfTestingImages( object sender, EventArgs e )
        {
            await Navigation.PushAsync( new ImportImagesPage() );
        }
    }
}