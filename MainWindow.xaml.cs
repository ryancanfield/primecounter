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
        private CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private PrimeCounter primeCounter = new PrimeCounter();

        public MainWindow()
        {
            InitializeComponent();
            //dispatcherTimer.Tick += dispatcherTimer_Tick;
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
        }

        //private void dispatcherTimer_Tick(object sender, EventArgs e)
        //{
        //    UpdateNumberBox();
        //}

        private void UpdateNumberBox()
        {
            NumberTextBox.Text = String.Format("Last Prime = {0}\nTotal Primes = {1}\nPrime Ratio = {2:F3}", primeCounter.LastPrime, primeCounter.Primes.Count(), ((double)primeCounter.Primes.Count() / (double)primeCounter.LastPrime * 100.0));
        }

        private Task RunCalculatePrimesTask(CancellationToken cancellationToken)
        {
            Task task = Task.Run(() => primeCounter.CalculatePrimes(cancellationToken));
            return task;

        }

        private async void CalculatePrimes_Click(object sender, RoutedEventArgs e)
        {
            if (!dispatcherTimer.IsEnabled)
            {
                CancellationTokenSource = new CancellationTokenSource();
                dispatcherTimer.Start();
                MessageTextBox.Text = "Calculating...";
                await RunCalculatePrimesTask(CancellationTokenSource.Token);
                MessageTextBox.Text = "Stopped";

            }
            else
            {
                if (CancellationTokenSource != null)
                {
                    CancellationTokenSource.Cancel();
                }
                dispatcherTimer.Stop();
            }
        }
    }
}
