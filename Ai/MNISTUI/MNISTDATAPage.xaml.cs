using CommunityToolkit.Maui.Views;
using Ai.MNIST.NeuralNetworks;
using Microsoft.Maui.Storage;

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
                PickOptions options = new PickOptions();
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
        private async void ChooseJsonFile()
        {
            try
            {
                PickOptions options = new()
                {
                    PickerTitle = "Please select a .json file",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.json" } }, // UTType for iOS
                        { DevicePlatform.MacCatalyst, new[] { "public.json" } }, // UTType for MacCatalyst
                        { DevicePlatform.Android, new[] { "application/json" } }, // MIME type for Android
                        { DevicePlatform.WinUI, new[] { ".json" } } // Extension for Windows
                    })
                };
                FileResult? result = await FilePicker.Default.PickAsync( options );

                if( result is not null )
                {
                    if( result.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase ) )
                    {
                        using IDisposable Stream = await result.OpenReadAsync();
                        
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public async void LoadOldNetwork( object sender, EventArgs e )
        {
            ChooseJsonFile();
            //myNetworkManager.LoadInNetworkFromJson();
        }

        public void SerializeNetwork( object sender, EventArgs e )
        {
            myNetworkManager.SerializeWheightAndBiasesToJson();       
        }

    }

}