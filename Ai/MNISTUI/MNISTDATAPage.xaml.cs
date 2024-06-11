using CommunityToolkit.Maui.Views;
using MNIST.NeuralNetworks;

namespace Ai.MNIST.UI
{
    public partial class MNISTDATAPage : ContentPage
    {
        Manager NetworkManager;
        public MNISTDATAPage()
        {
            InitializeComponent();
            this.NetworkManager = new Manager();
        }
        
        public void StartNewNetwork( object sender, EventArgs e )
        {
            CreateNetworkPopUp CreateNewNetwork = new();
            this.ShowPopup( CreateNewNetwork );
        }

        public void LoadOldNetwork( object sender, EventArgs e )
        {

        }

        public void SerializeNetwork( object sender, EventArgs e )
        {
            
        }

    }

}