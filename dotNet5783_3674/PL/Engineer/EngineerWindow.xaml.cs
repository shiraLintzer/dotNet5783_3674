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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        int pageStatus = 0;
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.TaskInEngineer?> allAvilableTasks = s_bl.Task.GetAvailableTask();

        public BO.EngineerExperience Levels { get; set; } = BO.EngineerExperience.All;

        public static readonly DependencyProperty EngineerProperty =
           DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        //public bool IsEditEnabled { get; private set; }



        public EngineerWindow(int Id = 0)
        {

            InitializeComponent();
            pageStatus = Id;
            if (Id == 0)
                CurrentEngineer = new BO.Engineer();
            else
                try { CurrentEngineer = s_bl.Engineer.Read(Id)!; }
                catch { }
            Closed += EngineerWindow_Closed!;
        }

        /// <summary>
        /// function to refresh the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EngineerWindow_Closed(object sender, EventArgs e)
        {
            // An instance of the main window EngineerListWindow
            var mainWindow = Application.Current.Windows
                                            .OfType<EngineerListWindow>()
                                            .FirstOrDefault();
            if (mainWindow != null)
            {
                // Updating the list of engineers in the main window by calling the BL
                // function that returns the list of engineers
                mainWindow.EngineerList = s_bl.Engineer.ReadAll()!;
            }
        }



        /// <summary>
        /// save/update engineer function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave(object sender, RoutedEventArgs e)
        {
            if (pageStatus == 0)
            {
                try
                {
                    s_bl.Engineer.Create(CurrentEngineer);
                    MessageBox.Show("User successfully added");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                // Display a confirmation message box
                Close();

            }
            else
            {

                try
                {
                    s_bl.Engineer.Update(CurrentEngineer);
                    MessageBox.Show("User successfully updated");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Close();
            }
        }

    }
}
