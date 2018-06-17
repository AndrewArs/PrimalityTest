using PrimalityTest.Core.Enums;
using PrimalityTest.Core.PrimalitTests.DeterministicTests;
using PrimalityTest.Core.PrimalitTests.ProbabilisticTests;
using PrimalityTest.Wpf.Enums;
using PrimalityTest.Wpf.Models;
using System;
using System.Numerics;
using System.Windows;

namespace PrimalityTest.Wpf
{
    /// <summary>
    /// Логика взаимодействия для PrimalityTestChoose.xaml
    /// </summary>
    public partial class PrimalityTestChoose : Window
    {
        private Options options;
        private BigInteger _number;

        public PrimalityTestChoose(NumberState numberState, BigInteger number)
        {
            InitializeComponent();

            _number = number;
            options = new Options();
            Init(numberState);

            DataContext = options;

            TestButton.Click += TestButton_Click;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            PrimeNumberState result;

            switch (options.Type)
            {
                case PrimalityTestType.Aks:
                    result = AKSTest.IsPrime(_number);
                    break;
                case PrimalityTestType.Fermat:
                    {
                        var k = GetNumberOfIterations();
                        if (k == -1)
                        {
                            return;
                        }

                        result = FermatTest.IsPrime(_number, k);
                    }
                    break;
                case PrimalityTestType.LucasLehmer:
                    result = LucasLehmerTest.IsPrime(_number);
                    break;
                case PrimalityTestType.Pepin:
                    result = PepinTest.IsPrime(_number);
                    break;
                case PrimalityTestType.RabinMiller:
                    {
                        var k = GetNumberOfIterations();
                        if (k == -1)
                        {
                            return;
                        }

                        result = RabinMiller.IsPrime(_number, k);
                    }
                    break;
                case PrimalityTestType.SolovayStrassen:
                    {
                        if (!long.TryParse(_number.ToString(), out long num))
                        {
                            MessageBox.Show("This number too big to use this method./nPlease select another primality test.",
                                "Result", MessageBoxButton.OK);

                            return;
                        }

                        var k = GetNumberOfIterations();
                        if (k == -1)
                        {
                            return;
                        }

                        result = SolovayStrassen.IsPrime(num, k);
                        break;
                    }
                default: return;
            }

            MessageBox.Show($"Your number is {Enum.GetName(typeof(PrimeNumberState), result)}", "Result");
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
                        options.Type = PrimalityTestType.Pepin;
                    }
                    break;
                case NumberState.Mersenne:
                    {
                        PepinRadio.IsEnabled = false;
                        LucasLehmerRadio.IsEnabled = true;
                        LucasLehmerText.Text += "(recommend)";
                        options.Type = PrimalityTestType.LucasLehmer;
                    }
                    break;
                case NumberState.Proth:
                    {
                        PepinRadio.IsEnabled = false;
                        LucasLehmerRadio.IsEnabled = false;

                        var result = MessageBox.Show("This number is a Proth number, " +
                            "so best best primality test for you is Proth test.",
                            "Result", MessageBoxButton.OK);
                    }
                    break;
                case NumberState.None:
                    PepinRadio.IsEnabled = false;
                    LucasLehmerRadio.IsEnabled = false;
                    break;
            }
        }

        private int GetNumberOfIterations()
        {
            if (string.IsNullOrEmpty(IterationsTextBox.Text))
            {
                MessageBox.Show("You must set iterations count for this test");
                return -1;
            }

            if (!int.TryParse(IterationsTextBox.Text, out int k))
            {
                MessageBox.Show("Incorrect iterations count input");
                return -1;
            }

            return k;
        }
    }
}
