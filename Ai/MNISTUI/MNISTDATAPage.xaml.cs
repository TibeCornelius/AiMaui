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
        
        public async void StartNewNetwork( object sender, EventArgs e )
        {
            var createNewNetworkPopup = new CreateNetworkPopUp();
            var result = await this.ShowPopupAsync( createNewNetworkPopup );
            if( createNewNetworkPopup.IhaveNotCanceld )
            {
                NetworkValues networkValues = new NetworkValues();
                if( createNewNetworkPopup.isStandardNetwork )
                {
                    networkValues.SetDefault();
                }
                else
                {
                    ChooseCustomNetworkValues();
                    //networkValues.SetCustom();
                }
                NetworkManager.StartNewNetwork( networkValues );
            }
        }

        public async void ChooseCustomNetworkValues()
        {
            await Navigation.PushAsync( new ChooseCustomNetworkParametersPage() );
        }
        public void LoadOldNetwork( object sender, EventArgs e )
        {
            NetworkManager.LoadInNetworkFromJson();
        }

        public void SerializeNetwork( object sender, EventArgs e )
        {
            NetworkManager.SerializeWheightAndBiasesToJson();       
        }

    }

}