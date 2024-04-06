using PL.Engineer;
using PL.EngineerOptions;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));


        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// function that transfers to the page of Login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEngineerListWindow(object sender, RoutedEventArgs e)
        {
            int id;
            string input = Microsoft.VisualBasic.Interaction.InputBox("please enter your ID:", "Enginner Enter");


            if (int.TryParse(input, out id))
            {
                try
                {
                    ////s_bl.Engineer.Read(id);
                    //BO.Engineer newUser = s_bl.Engineer.Read(id);
                    //new CurrentTask(newUser.Task.Id, newUser.Id).ShowDialog();
                    ////  new EngeneerTaskWindow(id).ShowDialog();



                    BO.Engineer newUser = s_bl.Engineer.Read(id)!;
                    int takId = newUser.Task != null ? newUser.Task.Id : 0;
                    new CurrentTask(takId, newUser.Id).ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        /// <summary>
        /// function that transfers to the page of the engineers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonManager(object sender, RoutedEventArgs e)
        {
            new ManagerView().Show();
        }



        private void AdvanceYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByYear(1);
            CurrentTime = CurrentTime.AddYears(1);
        }

        private void AdvanceMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByMonth(1);
            CurrentTime = CurrentTime.AddMonths(1);
        }

        private void AdvanceDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByDay(1);
            CurrentTime = CurrentTime.AddDays(1);
        }

        private void AdvanceHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByHour(1);
            CurrentTime = CurrentTime.AddHours(1);
        }

        private void InitializeTime_Click(object sender, RoutedEventArgs e)
        {
            s_bl.InitializeTime();
            CurrentTime = s_bl.Clock;
        }


    }
}
