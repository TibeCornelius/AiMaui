using MNIST.NeuralNetworks;

namespace Ai.MNIST.UI
{
    public partial class CurrentNetworkStatsPage : ContentPage
    {
        List<TrainingDataOutput> myStats;
        private Label[] AllTimeTotalGuesses;
        private Label[] AllTimeCorrectGuesses;
        public CurrentNetworkStatsPage( List<TrainingDataOutput> Stats )
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
            DisplayCurrentStats( Stats );
            this.myStats = Stats;
        }
        private void DisplayCurrentStats( List<TrainingDataOutput> Stats )
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
            foreach( TrainingDataOutput stat in Stats )
            {
                foreach( ImageData imageData in stat.ImageData )
                {
                    TotalCount += stat.ImageData.Count;
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
                AllTimeTotalGuesses[ 10 ].Text = "Total Guesses -> " + TotalCount + " ";
                AllTimeCorrectGuesses[ 10 ].Text = "Correct Guesses -> " + TotalCorrectGuesses + " ";
            }
        }
    }
}