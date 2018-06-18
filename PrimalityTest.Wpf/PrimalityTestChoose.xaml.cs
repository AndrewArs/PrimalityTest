using PrimalityTest.Core.Enums;
using PrimalityTest.Core.PrimalitTests.DeterministicTests;
using PrimalityTest.Core.PrimalitTests.ProbabilisticTests;
using PrimalityTest.Wpf.Enums;
using PrimalityTest.Wpf.Models;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PrimalityTest.Wpf
{
    /// <summary>
    /// Логика взаимодействия для PrimalityTestChoose.xaml
    /// </summary>
    public partial class PrimalityTestChoose
    {
        private Options _options;
        private readonly BigInteger _number;
        private readonly CancellationTokenSource _token;

        public PrimalityTestChoose(NumberState numberState, BigInteger number)
        {
            InitializeComponent();

            _number = number;
            _token = new CancellationTokenSource();
            _options = new Options();
            Init(numberState);

            DataContext = _options;

            TestButton.Click += TestButton_Click;
        }

        private async void TestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Computations();
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _token.Cancel();
        }

        private async Task Computations()
        {
            Busy.Visibility = Visibility.Visible;

            PrimeNumberState result;

            var token = _token.Token;

            switch (_options.Type)
            {
                case PrimalityTestType.Aks:
                    result = await Task.Run(() => AksTest.IsPrime(_number, token), token);
                    break;
                case PrimalityTestType.Fermat:
                    {
                        var k = GetNumberOfIterations();
                        if (k == -1)
                        {

                            Busy.Visibility = Visibility.Hidden;
                            return;
                        }

                        result = await FermatTest.IsPrime(_number, k, token);
                    }
                    break;
                case PrimalityTestType.LucasLehmer:
                    result = await LucasLehmerTest.IsPrime(_number, token);
                    break;
                case PrimalityTestType.Pepin:
                    result = await Task.Run(() => PepinTest.IsPrime(_number), token);
                    break;
                case PrimalityTestType.RabinMiller:
                    {
                        var k = GetNumberOfIterations();
                        if (k == -1)
                        {

                            Busy.Visibility = Visibility.Hidden;
                            return;
                        }

                        result = await RabinMiller.IsPrime(_number, k, token);
                    }
                    break;
                case PrimalityTestType.SolovayStrassen:
                    {
                        if (!long.TryParse(_number.ToString(), out var num))
                        {
                            MessageBox.Show("This number too big to use this method.\nPlease select another primality test.",
                                "Result", MessageBoxButton.OK);


                            Busy.Visibility = Visibility.Hidden;
                            return;
                        }

                        var k = GetNumberOfIterations();
                        if (k == -1)
                        {

                            Busy.Visibility = Visibility.Hidden;
                            return;
                        }

                        result = await SolovayStrassen.IsPrime(num, k, token);
                        break;
                    }
                default:
                    {
                        Busy.Visibility = Visibility.Hidden;
                        return;
                    }
            }

            MessageBox.Show($"Your number is {Enum.GetName(typeof(PrimeNumberState), result)}", "Result");

            Busy.Visibility = Visibility.Hidden;
        }

        private void Init(NumberState numberState)
        {
            switch (numberState)
            {
                case NumberState.Fermat:
                    {
                        PepinRadio.IsEnabled = true;
                        LucasLehmerRadio.IsEnabled = false;
                        PepinText.Text += "(recommend)";
                        _options.Type = PrimalityTestType.Pepin;
                    }
                    break;
                case NumberState.Mersenne:
                    {
                        PepinRadio.IsEnabled = false;
                        LucasLehmerRadio.IsEnabled = true;
                        LucasLehmerText.Text += "(recommend)";
                        _options.Type = PrimalityTestType.LucasLehmer;
                    }
                    break;
                case NumberState.Proth:
                    {
                        PepinRadio.IsEnabled = false;
                        LucasLehmerRadio.IsEnabled = false;

                        MessageBox.Show("This number is a Proth number, " +
                                        "so best best primality test for you is Proth test.",
                                        "Result", MessageBoxButton.OK);
                    }
                    break;
                case NumberState.None:
                    PepinRadio.IsEnabled = false;
                    LucasLehmerRadio.IsEnabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(numberState), numberState, null);
            }
        }

        private int GetNumberOfIterations()
        {
            if (string.IsNullOrEmpty(IterationsTextBox.Text))
            {
                MessageBox.Show("You must set iterations count for this test");
                return -1;
            }

            if (!int.TryParse(IterationsTextBox.Text, out var k))
            {
                MessageBox.Show("Incorrect iterations count input");
                return -1;
            }

            return k;
        }
    }
}
