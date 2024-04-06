using PL.Engineer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for TaskForList.xaml
    /// </summary>
    public partial class TaskForList : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Status Status { get; set; } = BO.Status.All;

        public IEnumerable<BO.TaskForList> TaskList
        {
            get { return (IEnumerable<BO.TaskForList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskForList>), typeof(TaskForList), new PropertyMetadata(null));


        public TaskForList()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.GetAllTasksForList()!;
        }


        /// <summary>
        /// add Task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddUpdateEngineer(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
        }



        /// <summary>
        /// Filter Tasks by status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxTaskStatus(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Status == BO.Status.All) ?
                 s_bl?.Task.GetAllTasksForList() : s_bl?.Task.GetTasksAppropriateStatus(Status)!;

        }


        /// <summary>
        /// function to choose task after doubleClicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoubleClickButton(object sender, MouseButtonEventArgs e)
        {
            // Get the selected item in the list
            BO.TaskForList? selectedTask = (sender as ListView)?.SelectedItem as BO.TaskForList;


            if (selectedTask != null)
            {
                //Open a single item view window in edit mode and send the details of the selected item
                new TaskWindow(selectedTask.Id).ShowDialog();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(sender is TextBox textBox)
            {
                string filterText = textBox.Text.ToLower();
                ICollectionView view = CollectionViewSource.GetDefaultView(TaskList);
                if(view != null)
                {
                    view.Filter = (item) =>
                    {
                        if (item is BO.TaskForList task)
                        {
                            string taskDescription = task.Description!.ToLower();
                            return taskDescription.StartsWith(filterText);
                        }
                        return false;
                    };
                         
                }
            }
        }
    }
}
