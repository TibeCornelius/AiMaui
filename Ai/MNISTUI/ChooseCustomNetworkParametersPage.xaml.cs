
using MNIST.NeuralNetworks;
namespace Ai.MNIST.UI
{
    public partial class ChooseCustomNetworkParametersPage : ContentPage
    {
        string LayerCountText;
        int LayerCount;
        Dictionary<int, string> DictNeuronCountEntrys;
        NetworkValues myNetworkValeus;
        MNISTDATAPage myParentPage;
        ActivationFunctionOptions? activationFunctionOptions;        
        public ChooseCustomNetworkParametersPage( MNISTDATAPage ParentPage )
        {
            this.myParentPage = ParentPage;
            this.LayerCount = 0;
            this.LayerCountText = string.Empty;
            this.DictNeuronCountEntrys = []; 
            InitializeComponent();
            myNetworkValeus = new NetworkValues();
            this.activationFunctionOptions = null;
        }
        private void OnActivationFunctionChanged( object sender, EventArgs e )
        {
            if( pActivationFunction.SelectedIndex != -1 )
            {
                activationFunctionOptions = (ActivationFunctionOptions)pActivationFunction.SelectedIndex;
            }
        }
        private void LayerCountEntry( object sender, TextChangedEventArgs e )
        {
            LayerCountText = e.NewTextValue;
        }
        private async void StartDefineNeuronCount( object sender, EventArgs e )
        {
            Button? button = sender as Button;
            try
            {
                if (string.IsNullOrEmpty( LayerCountText ))
                {
                    return;
                }
                LayerCount = Convert.ToInt16( LayerCountText );
            }
            catch
            {
                await DisplayAlert("Invalid entry", "Invalid input detected, numbers only", "OK");
                return;
            }
            if( LayerCount <= 10 && LayerCount > 0 )
            {
                for( int index = 0 ; index < LayerCount ; index++ )
                {
                    Entry entry = new();
                    if( LayerCount - 1 == index )
                    {
                        entry.Placeholder = "10 ( output layer neurons cannot be changed)";
                        entry.ClassId = $"{index}";
                        entry.IsEnabled = false;
                    }
                    else
                    {
                        entry.Placeholder = $"Enter neuron count of layer { index + 1 }";
                        entry.ClassId = $"{index}";
                    }
              
                    entry.TextChanged += NeuronCountEntry;
                    NeuronCountContainer.Children.Add( entry );
                    DictNeuronCountEntrys.Add( index, string.Empty );
                }
                Button bNext = new Button
                {
                    Text="Next",
                };
                bNext.Clicked +=  ParametersEnterd;
                SemanticProperties.SetHint( bNext, "Initialize network with current inputs" );
                NeuronCountContainer.Children.Add( bNext );
                Button bCancel = new Button
                {
                    Text="Go Back"
                };
                SemanticProperties.SetHint( bCancel, "Change the layer count");
                NeuronCountContainer.Children.Add( bCancel );
                if( button is not null )
                {
                    button.IsEnabled = false;
                    bCancel.Clicked += ( sender, e ) => GoBackToDefiningLayerCount( sender, e, button, NeuronCountContainer );
                }
                else
                {
                    throw new Exception("Button was null when defining Neuron count");
                }
            }
            else
            {
                await DisplayAlert("Invalid LayerCount","Please enter a number between 1 and 10","OK");
                return;
            }
        }
        private void GoBackToDefiningLayerCount( object? sender, EventArgs e, Button LayerCountEnteredButton, StackLayout myStackLayout )
        {
            myStackLayout.Children.Clear();
            DictNeuronCountEntrys.Clear();
            LayerCountEnteredButton.IsEnabled = true;
        }
        private void NeuronCountEntry( object? sender, TextChangedEventArgs e )
        {
            Entry? entry = sender as Entry;
            if( entry is not null )
            {
                DictNeuronCountEntrys[ Convert.ToInt16( entry.ClassId)] = e.NewTextValue;
            }
        }

        private async void ParametersEnterd( object? sender, EventArgs e )
        {
            if( activationFunctionOptions.HasValue)
            {
                int[] NeuronCount = new int[ LayerCount ];
                int index = 1;
                foreach( string myNeuronCount in DictNeuronCountEntrys.Values )
                {
                    if( index == DictNeuronCountEntrys.Count )
                    {
                        continue;
                    }
                    int count;
                    try
                    {
                        count = Convert.ToInt16( myNeuronCount );
                    }
                    catch
                    {
                        await DisplayAlert("Invalid LayerCount",$"Invalid neuron count in entry { index } detected","OK");
                        return;
                    }
                    if( count > 0 && count < 2000 )
                    {
                        NeuronCount[ index - 1 ] = count;
                        index++;
                    }
                    else
                    {
                        await DisplayAlert("Invalid LayerCount",$"Invalid neuron count in entry { index } detected","OK");
                        return;
                    }
                }
                NeuronCount[ LayerCount - 1 ] = 10;
                myNetworkValeus.SetCustom( LayerCount, NeuronCount, activationFunctionOptions.Value, true );
                myParentPage.myInternalNetworkValues = myNetworkValeus;
                myParentPage.StartNewNetwork();
                myParentPage.ChageCurrentDisplayOfNetwork();
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Invalid activation function", "Please choose a activation function from the provided options", "OK");
            }
        }
    }
}