using CommunityToolkit.Maui.Views;
using Ai.MNIST.NeuralNetworks;

using System.Text.Json;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Storage;

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
        private async Task ChooseJsonFile()
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
                        string OutPutPath = result.FullPath;
                        string JsonString = File.ReadAllText( OutPutPath );
                        try
                        {
                            NetworkJsonFormat? settings = JsonSerializer.Deserialize<NetworkJsonFormat>( JsonString );
                            if( settings is null )
                            {
                                throw new NullReferenceException();
                            }
                            myNetworkManager.LoadInNetworkFromJson( settings );
                            
                            ChageCurrentDisplayOfNetwork();
                        }
                        catch
                        {

                        }
                        
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public async void LoadOldNetwork( object sender, EventArgs e )
        {
            await ChooseJsonFile();
            //myNetworkManager.LoadInNetworkFromJson();
        }
        private async Task SerializeNetworkToFolder( CancellationToken cancellationToken )
        {
            FolderPickerResult result = await FolderPicker.Default.PickAsync( cancellationToken );
            if( result.IsSuccessful )
            {
                string? Result = await DisplayPromptAsync("FileName", "What should the output file be called");
                if( Result is not null && Result != string.Empty )
                {
                    myNetworkManager.SerializeWheightAndBiasesToJson( Result );
                }
            }
            else
            {
                return;
            }
        }

        public async void SerializeNetwork( object sender, EventArgs e )
        {
            await SerializeNetworkToFolder( new CancellationToken() );       
        }

    }

}