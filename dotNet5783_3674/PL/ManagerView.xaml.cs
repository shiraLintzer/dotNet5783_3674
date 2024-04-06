using PL.Engineer;
using PL.Gant;
using PL.Task;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerView.xaml
    /// </summary>
    public partial class ManagerView : Window
    {
        public ManagerView()
        {
            InitializeComponent();
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
        /// A function that calls Reset 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonReset(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to perform the reset?", "Initialization Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                BlApi.Factory.Get().ResetDB();
                //BlApi.Factory.Get().InitializeDB();
                PerformReset();
            }
        }



        /// <summary>
        /// function that transfers to the page of the tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTasks(object sender, RoutedEventArgs e)
        {
            new TaskForList().Show();
        }



        /// <summary>
        /// function that call gant chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGanttChart(object sender, RoutedEventArgs e)
        {
            try
            {
                new GantPage().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        /// <summary>
        /// function that creat a schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSchedule(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the project start date (MM/dd/yyyy):", "Date Enter");

                if (DateTime.TryParse(input, out DateTime startDate))
                {
                    try
                    {
                        BlApi.Factory.Get().CreateProject(startDate);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid date format. Please enter the date in MM/dd/yyyy format.");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        //

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
        ///Initialize success message
        /// </summary>
        private void PerformInitialization()
        {
            // Perform the initialization here
            MessageBox.Show("Initialization completed successfully!", "System Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        /// <summary>
        ///Reset success message
        /// </summary>
        private void PerformReset()
        {
            // Perform the initialization here
            MessageBox.Show("Reset completed successfully!", "System Message", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
