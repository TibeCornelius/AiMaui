using CommunityToolkit.Maui.Views;

namespace Ai.MNIST.UI
{

    public partial class CreateNetworkPopUp : Popup
    {
        public bool IhaveNotCanceld { get; private set; }
        public bool isStandardNetwork { get; private set; }
        public CreateNetworkPopUp()
        {
            IhaveNotCanceld = true;
            InitializeComponent();
        }
        private void StandardNetwork(object sender, EventArgs e)
        {
            isStandardNetwork = true;
            Close();
        }

        private void CustomNetwork(object sender, EventArgs e)
        {
            isStandardNetwork = false;
            Close();
        }
        private void Close( object sender, EventArgs e )
        {
            IhaveNotCanceld = false;
            Close();
        }
    }
}