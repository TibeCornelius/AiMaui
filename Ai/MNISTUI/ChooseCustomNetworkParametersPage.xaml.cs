
using MNIST.NeuralNetworks;

namespace Ai.MNIST.UI
{
    public partial class ChooseCustomNetworkParametersPage : ContentPage
    {
        string LayerCountText;
        int LayerCount;
        Dictionary<int, string> DictNeuronCountEntrys;
        NetworkValues networkValues;
        public ChooseCustomNetworkParametersPage()
        {
            this.LayerCount = 0;
            this.LayerCountText = string.Empty;
            this.DictNeuronCountEntrys = []; 
            InitializeComponent();
            networkValues = new NetworkValues();
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
                for( int index = 0 ; index < LayerCount - 1 ; index++ )
                {
                    Entry entry = new Entry
                    {
                        Placeholder = $"Enter neuron count of layer { index + 1 }",
                        ClassId=$"{index}",
                    };
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
            int[] NeuronCount = new int[ LayerCount ];
            int index = 0;
            foreach( string myNeuronCount in DictNeuronCountEntrys.Values )
            {
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
                    NeuronCount[ index ] = count;
                    index++;
                }
                else
                {
                    await DisplayAlert("Invalid LayerCount",$"Invalid neuron count in entry { index } detected","OK");
                    return;
                }
            }
            networkValues.SetCustom( LayerCount, NeuronCount );
        }
    }
}