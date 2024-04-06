using BO;
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
using System.Windows.Shapes;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {

        int pageStatus = 0;
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        
        public BO.EngineerExperience Levels { get; set; } = BO.EngineerExperience.All;
        public BO.Status StatusOptions { get; set; } = BO.Status.Unscheduled;
        public IEnumerable<BO.EngineerInTask?> allAvilableTasks = s_bl.Engineer.GetAvailableEngineer();
        public IEnumerable<BO.TaskInList?> allDependencyOption = s_bl.Task.GetAllDependenciesOptions();

        public static readonly DependencyProperty TaskProperty =
           DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }



        public TaskWindow(int Id = 0)
        {
            InitializeComponent();
            pageStatus = Id;
            if (Id == 0)
                CurrentTask = new BO.Task();
            else
                try { CurrentTask = s_bl.Task.Read(Id)!; }
                catch { }
            Closed += TaskWindow_Closed!;
        }



        /// <summary>
        /// function to refresh the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            // An instance of the main window EngineerListWindow
            var mainWindow = Application.Current.Windows
                                            .OfType<TaskForList>()
                                            .FirstOrDefault();
            if (mainWindow != null)
            {
                // Updating the list of engineers in the main window by calling the BL
                // function that returns the list of engineers
                mainWindow.TaskList = s_bl.Task.GetAllTasksForList()!;
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
                    s_bl.Task.Create(CurrentTask);
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
                    s_bl.Task.Update(CurrentTask);
                    MessageBox.Show("Task successfully updated");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Close();
            }
        }


        /// <summary>
        /// function to choose many dependencies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                var dependencies = listBox.SelectedItems.OfType<TaskInList>();

                foreach (var selectedItem in dependencies)
                {

                    if (CurrentTask.Dependencies == null || CurrentTask.Dependencies.Count() ==0)
                    {
                        CurrentTask.Dependencies = new List<TaskInList>();
                    }
                    List<TaskInList> currentDep = CurrentTask.Dependencies.ToList();
                    currentDep.Add(selectedItem);
                    CurrentTask.Dependencies = currentDep;

                }
            }
        }

    }



}
