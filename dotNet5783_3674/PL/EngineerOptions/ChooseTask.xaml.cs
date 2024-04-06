using BO;
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

namespace PL.EngineerOptions
{
    /// <summary>
    /// Interaction logic for ChooseTask.xaml
    /// </summary>
    public partial class ChooseTask : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        BO.Engineer currEng;

        public IEnumerable<BO.TaskInEngineer> TaskListForEng
        {
            get { return (IEnumerable<BO.TaskInEngineer>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskListForEng", typeof(IEnumerable<BO.TaskInEngineer>), typeof(ChooseTask), new PropertyMetadata(null));

        public ChooseTask(int Id)
        {
            InitializeComponent();
            currEng = s_bl.Engineer.Read(Id)!;
            TaskListForEng = s_bl?.Task.GetAvailableTasksForEngineer(Id)!;
        }


        //in case the engineer DoubleClicked the task he chose
        private void DoubleClickButton(object sender, MouseButtonEventArgs e)
        {
            // Get the selected item in the list
            BO.TaskInEngineer? selectedTask = (sender as ListView)?.SelectedItem as BO.TaskInEngineer;
            

            if (selectedTask != null)
            {
                //Open a single item view window in edit mode and send the details of the selected item
                currEng.Task = selectedTask;
                s_bl.Engineer.Update(currEng);
                Close();
                new MainWindow().Show();
                
            }
        }
    }
}
