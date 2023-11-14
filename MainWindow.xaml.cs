using System;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace primecounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List <ulong> Primes { get; set; } = new List <ulong> ();
        public ulong Number { get; set; } = 2;
        CancellationTokenSource CancellationTokenSource;


        public MainWindow()
        {
            InitializeComponent();
            PrimesList.ItemsSource = Primes;
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
                    Primes.Add (Number);
                Number++;
            }
            Primes.Reverse();
        }

        private void CalculatePrimes_Click(object sender, RoutedEventArgs e)
        {
            PrimesList.IsEnabled = false;
            CancellationTokenSource = new CancellationTokenSource();
            Task myTask = Task.Run(() => CalculatePrimes(CancellationTokenSource.Token));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource.Cancel();
            PrimesList.Items.Refresh();
            PrimesList.IsEnabled = true;
        }
    }
}
