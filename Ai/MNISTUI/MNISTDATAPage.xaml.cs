using CommunityToolkit.Maui.Views;
using Ai.MNIST.NeuralNetworks;

namespace Ai.MNIST.UI
{
    public partial class MNISTDATAPage : ContentPage
    {
        Manager myNetworkManager;
        internal NetworkValues myInternalNetworkValues;
        public MNISTDATAPage()
        {
            InitializeComponent();
            this.myInternalNetworkValues = new NetworkValues();
            this.myNetworkManager = new Manager();
            ChageCurrentDisplayOfNetwork();
        }
        internal void StartNewNetwork()
        {
            myNetworkManager.StartNewNetwork( myInternalNetworkValues );
        }
        
        public async void StartNewNetwork( object sender, EventArgs e )
        {
            var createNewNetworkPopup = new CreateNetworkPopUp();
            var result = await this.ShowPopupAsync( createNewNetworkPopup );
            if( createNewNetworkPopup.IhaveNotCanceld )
            {
                if( createNewNetworkPopup.isStandardNetwork )
                {
                    myInternalNetworkValues.SetDefault();
                    StartNewNetwork();
                    ChageCurrentDisplayOfNetwork();
                }
                else
                {
                    ChooseCustomNetworkValues();
                }
            }
        }
        internal void ChageCurrentDisplayOfNetwork()
        {
            if( myNetworkManager.network is not null )
            {
                lLayerCount.Text = "Layerer Count -> " + Convert.ToString( myNetworkManager.network.LiNetwork.Count );
                foreach( int NeuronCount in myNetworkManager.network.LiNetwork )
                {
                    Label neuronCount = new Label
                    {
                        Text = "NeuronCount -> " + Convert.ToString( NeuronCount ),
                        HorizontalOptions = LayoutOptions.Center,
                    };
                    LayerCountStack.Children.Add( neuronCount );
                }
            }
        }

        private async void GoToCurrentNetworkPage( object sender, EventArgs e )
        {
            if( myNetworkManager.network is not null )
            {
                await Navigation.PushAsync( new CurrentNetworkPage( myNetworkManager.network, myNetworkManager ) );
            }
            else
            {
                await DisplayAlert("No network loaded",$"You need to load in a network before you can acces the CurrentNetwork page","OK");
            }
        }

        public async void ChooseCustomNetworkValues()
        {
            await Navigation.PushAsync( new ChooseCustomNetworkParametersPage( this ) );
        }
        public async void LoadOldNetwork( object sender, EventArgs e )
        {
            #if WINDOWS
            // Windows specific file picker using FileOpenPicker
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.FileTypeFilter.Add(".json");

            // Show file picker and get the selected file
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Process the selected file (e.g., read content)
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string jsonContent = await reader.ReadToEndAsync();
                        // Handle the jsonContent as needed
                    }
                }
            }
            #endif
            //myNetworkManager.LoadInNetworkFromJson();
        }

        public void SerializeNetwork( object sender, EventArgs e )
        {
            myNetworkManager.SerializeWheightAndBiasesToJson();       
        }

    }

}