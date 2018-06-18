using PrimalityTest.Core.NumberAnalysers;
using System.Numerics;
using System.Windows;

namespace PrimalityTest.Wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            AnalysButton.Click += AnalysButton_Click;
        }

        private void AnalysButton_Click(object sender, RoutedEventArgs e)
        {
            if(!BigInteger.TryParse(NumberInput.Text, out var number))
            {
                var result = MessageBox.Show("Incorrect input!", "Error", MessageBoxButton.OK);

                if(result == MessageBoxResult.OK)
                {
                    NumberInput.Clear();
                }

                return;
            }

            Busy.Visibility = Visibility.Visible;
            var primeResult = NumbersAnalys.AnalysNumber(number);

            var window = new PrimalityTestChoose(primeResult, number);
            Busy.Visibility = Visibility.Hidden;

            window.ShowDialog();
        }
    }
}
