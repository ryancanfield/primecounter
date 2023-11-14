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

namespace primecounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List <ulong> Primes { get; set; } = new List <ulong> ();
        public ulong Number { get; set; } = 2;
        CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();


        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            NumberTextBox.Text = String.Format("Last Prime = {0}\nTotal Primes = {1}\nPrime Ratio = {2:F3}", Number, Primes.Count(), ((double)Primes.Count()/ (double)Number * 100.0));
        }

        private void CalculatePrimes(CancellationToken cancellationToken)
        { 
            Primes.Reverse();
            
            while (!cancellationToken.IsCancellationRequested)
            {
                bool flag = false;
                if (Number == 2)
                {
                    flag = true;
                }
                else
                {
                    foreach (ulong prime in Primes)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;
                        if (prime > Number / 2)
                        {
                            flag = true;
                            break;
                        }
                        if (Number % prime == 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                    Primes.Add(Number);
                Number++;
            }
            Primes.Reverse();
        }

        private void CalculatePrimes_Click(object sender, RoutedEventArgs e)
        {
            if (!dispatcherTimer.IsEnabled)
            {
                CancellationTokenSource = new CancellationTokenSource();
                dispatcherTimer.Start();
                Task task = Task.Run(() => CalculatePrimes(CancellationTokenSource.Token));
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
