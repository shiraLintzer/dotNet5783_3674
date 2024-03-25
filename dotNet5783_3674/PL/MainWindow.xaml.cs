using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// function that transfers to the page of the engineers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEngineerListWindow(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        /// <summary>
        /// A function that calls Reset and Initialize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonInitialization(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to perform the initialization?", "Initialization Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                BlApi.Factory.Get().ResetDB();
                BlApi.Factory.Get().InitializeDB();
                PerformInitialization();
            }
        }

        /// <summary>
        ///Initialize success message
        /// </summary>
        private void PerformInitialization()
        {
            // Perform the initialization here
            MessageBox.Show("Initialization completed successfully!", "System Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    
    }
}
