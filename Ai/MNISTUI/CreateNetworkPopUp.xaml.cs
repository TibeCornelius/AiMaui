using CommunityToolkit.Maui.Views;

namespace Ai.MNIST.UI
{
    public partial class CreateNetworkPopUp : Popup
    {
        public CreateNetworkPopUp()
        {
            InitializeComponent();
        }
        private void OnButton1Clicked(object sender, EventArgs e)
        {
            // Handle Button 1 click
            Close();
        }

        private void OnButton2Clicked(object sender, EventArgs e)
        {
            // Handle Button 2 click
            Close();
        }
    }
}