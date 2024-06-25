using Ai.MNIST.NeuralNetworks;
using Ai.MNIST.NeuralNetworks.TrainingResults;

namespace Ai.MNIST.UI
{
    public partial class CurrentNetworkStatsPage : ContentPage
    {
        List<TrainingBatch> myStats;
        private Label[] AllTimeTotalGuesses;
        private Label[] AllTimeCorrectGuesses;
        private Label[] RecentTimeCorrectGuesses;
        private Label[] RecentTimeTotalGuesses;
        public CurrentNetworkStatsPage( List<TrainingBatch> Stats )
        {
            InitializeComponent();
            this.AllTimeTotalGuesses =
            [
                lTotal0, lTotal1, lTotal2, lTotal3,
                lTotal4, lTotal5, lTotal6, lTotal7, lTotal8, lTotal9
                ,lTotalGuesses
            ];
            this.AllTimeCorrectGuesses =
            [
                lCorrect0, lCorrect1, lCorrect2, lCorrect3,
                lCorrect4, lCorrect5, lCorrect6, lCorrect7, lCorrect8, lCorrect9
                ,lCorrectGuesses,
            ];
            this.RecentTimeCorrectGuesses =
            [
                lRecentTotal0, lRecentTotal1, lRecentTotal2, lRecentTotal3,
                lRecentTotal4, lRecentTotal5, lRecentTotal6, lRecentTotal7, lRecentTotal8, lRecentTotal9
                ,lRecentTotalGuesses
            ];
            this.RecentTimeTotalGuesses =
            [
                lRecentCorrect0, lRecentCorrect1, lRecentCorrect2, lRecentCorrect3,
                lRecentCorrect4, lRecentCorrect5, lRecentCorrect6, lRecentCorrect7, lRecentCorrect8, lRecentCorrect9
                ,lRecentCorrectGuesses,
            ];
            DisplayCurrentStats( Stats );
            this.myStats = Stats;
        }
        private void TimeDurationStatsEntry( object sender, TextChangedEventArgs e )
        {
            try
            {
                int Historyindex = Convert.ToInt16( e.NewTextValue );
                DisplayRecentStats( Historyindex );
            }
            catch
            {

            }
        }
        private void DisplayRecentStats( int Historyindex )
        {
            int TotalCount = 0;
            int TotalCorrectGuesses = 0;
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
            Dictionary<int,int> CorrectGuesses = new Dictionary<int, int>
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
            int index = 0;
            
            for( int StatIndex = myStats.Count - 1 ; StatIndex >= 0 ; StatIndex-- )
            {
                List<ImageData> imageList = myStats[ StatIndex ].ImageData;
                for( int ImageIndex = myStats[ StatIndex ].ImageData.Count - 1 ; ImageIndex >= 0 ; ImageIndex-- )
                {
                    TotalCount += myStats[ StatIndex ].ImageData.Count;
                    int Number = imageList[ ImageIndex ].ImageNumber;
                    Label currentCorrectLabel = RecentTimeCorrectGuesses[ Number ];
                    Label currentTotalLabel = RecentTimeTotalGuesses[ Number ];
                    if( imageList[ ImageIndex ].wasGuesCorrect )
                    {
                        TotalCorrectGuesses += 1;
                        CorrectGuesses[ Number ] += 1;
                    }
                    AllGuesses[ Number ] += 1;
                    currentCorrectLabel.Text = "Correct Guesses -> " + CorrectGuesses[ Number ] + " ";
                    currentTotalLabel.Text = "Total Guesses -> " + AllGuesses[ Number ] + " ";
                    index++;
                    if( index == Historyindex ) 
                    {
                        break;
                    }
                }
                if( index == Historyindex ) 
                {
                    break;
                }
                RecentTimeTotalGuesses[ 10 ].Text = "Total Guesses -> " + TotalCount + " ";
                RecentTimeCorrectGuesses[ 10 ].Text = "Correct Guesses -> " + TotalCorrectGuesses + " ";
            } 
        }
        private void DisplayCurrentStats( List<TrainingBatch> Stats )
        {
            int TotalCount = 0;
            int TotalCorrectGuesses = 0;
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
            Dictionary<int,int> CorrectGuesses = new Dictionary<int, int>
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
            foreach( TrainingBatch stat in Stats )
            {
                foreach( ImageData imageData in stat.ImageData )
                {
                    int Number = imageData.ImageNumber;
                    Label currentCorrectLabel = AllTimeCorrectGuesses[ Number ];
                    Label currentTotalLabel = AllTimeTotalGuesses[ Number ];
                    if( imageData.wasGuesCorrect )
                    {
                        TotalCorrectGuesses += 1;
                        CorrectGuesses[ Number ] += 1;
                    }
                    AllGuesses[ Number ] += 1;
                    currentCorrectLabel.Text = "Correct Guesses -> " + CorrectGuesses[ Number ] + " ";
                    currentTotalLabel.Text = "Total Guesses -> " + AllGuesses[ Number ] + " ";
                }
                TotalCount += stat.ImageData.Count;
                AllTimeTotalGuesses[ 10 ].Text = "Total Guesses -> " + TotalCount + " ";
                AllTimeCorrectGuesses[ 10 ].Text = "Correct Guesses -> " + TotalCorrectGuesses + " ";
            }
        }
    }
}