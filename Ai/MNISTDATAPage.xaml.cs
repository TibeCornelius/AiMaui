using MNISTDATA.NeuralNetworks;
namespace Ai;

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

    }

    public void LoadOldNetwork( object sender, EventArgs e )
    {

    }

    public void SerializeNetwork( object sender, EventArgs e )
    {
        
    }

}