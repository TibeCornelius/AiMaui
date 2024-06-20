using MNIST.NeuralNetworks;

namespace Ai.MNIST.UI
{
    public partial class CurrentNetworkStatsPage : ContentPage
    {
        List<TrainingDataOutput> myStats;
        public CurrentNetworkStatsPage( List<TrainingDataOutput> Stats )
        {
            InitializeComponent();
            DisplayCurrentStats( Stats );
            this.myStats = Stats;
        }
        private void DisplayCurrentStats( List<TrainingDataOutput> Stats )
        {
            int TotalCount = 0;
            Dictionary<int,int> AllGuesses = new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
                { 4, 0 },
                { 5, 0 },
                { 6, 0 },
                { 7, 0 },
                { 8, 0 },
                { 9, 0 },
            };
            foreach( TrainingDataOutput stat in Stats )
            {
                TotalCount += stat.ImageData.Count;
                foreach( ImageData imageData in stat.ImageData )
                {
                    int Number = imageData.ImageNumber;
                    switch ( Number )
                    {
                        case 0:
                            AllGuesses[ 0 ] += 1;
                            l0.Text = "'Correct Guesses 0 -> " + AllGuesses[ 0 ];
                            break;
                        case 1:
                            AllGuesses[ 1 ] += 1;
                            l1.Text = "'Correct Guesses 1 -> " + AllGuesses[ 1 ];
                            break;
                        case 2:
                            AllGuesses[ 2 ] += 1;
                            l2.Text = "'Correct Guesses 2 -> " + AllGuesses[ 2 ];
                            break;
                        case 3:
                            AllGuesses[ 3 ] += 1;
                            l3.Text = "'Correct Guesses 3 -> " + AllGuesses[ 3 ];
                            break;
                        case 4:
                            AllGuesses[ 4 ] += 1;
                            l4.Text = "'Correct Guesses 4 -> " + AllGuesses[ 4 ];
                            break;
                        case 5:
                            AllGuesses[ 5 ] += 1;
                            l5.Text = "'Correct Guesses 5 -> " + AllGuesses[ 5 ];
                            break;
                        case 6:
                            AllGuesses[ 6 ] += 1;
                            l6.Text = "'Correct Guesses 6 -> " + AllGuesses[ 6 ];
                            break;
                        case 7:
                            AllGuesses[ 7 ] += 1;
                            l7.Text = "'Correct Guesses 7 -> " + AllGuesses[ 7 ];
                            break;
                        case 8:
                            AllGuesses[ 8 ] += 1;
                            l8.Text = "'Correct Guesses 8 -> " + AllGuesses[ 8 ];
                            break;
                        case 9:
                            AllGuesses[ 9 ] += 1;
                            l9.Text = "'Correct Guesses 9 -> " + AllGuesses[ 9 ];
                            break;
                    }
                }
            }
        }
    }
}