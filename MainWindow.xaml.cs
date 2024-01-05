using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace primecounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        public PrimeCounter PrimeCounter { get; set; } = new PrimeCounter();

        public MainWindow()
        {
            InitializeComponent();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateNumberBox();
        }

        private void UpdateNumberBox()
        {
            NumberTextBox.Text = String.Format("Last Prime = {0}\nTotal Primes = {1}\nPrime Ratio = {2:F3}", PrimeCounter.LastPrime, PrimeCounter.Primes.Count(), ((double)PrimeCounter.Primes.Count() / (double)PrimeCounter.LastPrime * 100.0));
        }

        private async void CalculatePrimes_Click(object sender, RoutedEventArgs e)
        {
            if (!_dispatcherTimer.IsEnabled)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                _dispatcherTimer.Start();
                MessageTextBox.Text = "Calculating...";
                await PrimeCounter.CalculatePrimesAsync(_cancellationTokenSource.Token);
                MessageTextBox.Text = "Stopped";

            }
            else
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }
                _dispatcherTimer.Stop();
            }
        }
    }
}
