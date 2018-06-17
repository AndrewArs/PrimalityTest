using PrimalityTest.Core.NumberAnalysers;
using System.Numerics;
using System.Windows;

namespace PrimalityTest.Wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AnalysButton.Click += AnalysButton_Click;
        }

        private void AnalysButton_Click(object sender, RoutedEventArgs e)
        {
            if(!BigInteger.TryParse(NumberInput.Text, out BigInteger number))
            {
                var result = MessageBox.Show("Incorrect input!", "Error", MessageBoxButton.OK);

                if(result == MessageBoxResult.OK)
                {
                    NumberInput.Clear();
                }

                return;
            }

            var primeResult = NumbersAnalys.AnalysNumber(number);

            var window = new PrimalityTestChoose(primeResult, number);

            window.ShowDialog();
        }
    }
}
