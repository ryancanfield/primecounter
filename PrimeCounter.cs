using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace primecounter
{
    class PrimeCounter : INotifyPropertyChanged
    {
        public List<ulong> Primes { get; private set; } = new List<ulong>();
        public ulong LastPrime { get; set; } = 2;
        public event PropertyChangedEventHandler? PropertyChanged;
        public bool Running = false;

        public PrimeCounter() { }


        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CalculatePrimes(CancellationToken cancellationToken)
        {
            Running = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                bool flag = false;
                if (LastPrime == 2)
                {
                    flag = true;
                }
                else
                {
                    foreach (ulong prime in Primes)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;
                        if (prime > LastPrime / 2)
                        {
                            flag = true;
                            break;
                        }
                        if (LastPrime % prime == 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    Primes.Add(LastPrime);
                    NotifyPropertyChanged("LastPrime");
                }

                LastPrime++;
            }
            Running = false;
        }
    }
}
