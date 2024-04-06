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
    /// Interaction logic for CurrentTask.xaml
    /// </summary>
    public partial class CurrentTask : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty TaskProperty =
           DependencyProperty.Register("CurrentTaskEng", typeof(BO.Task), typeof(CurrentTask), new PropertyMetadata(null));

        public BO.Task CurrentTaskEng
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        int myEng;


        public CurrentTask(int taskId = 0, int engId = 0)
        {
            myEng = engId;
            InitializeComponent();
            //if the engineer does not have a task or he finshined his task
            if(taskId == 0 || s_bl.Task.Read(taskId)?.Status == (BO.Status)3)
            {
                new ChooseTask(engId).ShowDialog();
            }
            else
            {
                //if he has a task
                try
                {
                    CurrentTaskEng = s_bl.Task.Read(taskId)!;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }



        /// <summary>
        /// save/update engineer function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave(object sender, RoutedEventArgs e)
        {
            ///if there is no task
            if(CurrentTaskEng == null)
            {
                Close();
            }
            else
            {
                //save the date of complete
                CurrentTaskEng!.CompleteDate = s_bl.Clock;
                CurrentTaskEng.Status = (BO.Status)3;
                try
                {
                    s_bl.Task.Update(CurrentTaskEng);
                    MessageBox.Show("Task successfully updated");
                    new ChooseTask(myEng).ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Close();
            }
          
        }


        /// <summary>
        /// function to change the startDate o the engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cahngeStartDate(object sender, RoutedEventArgs e)
        {
            if (CurrentTaskEng == null)
            {
                Close();
            }
            else
            {
                try
                {
                    s_bl.Task.Update(CurrentTaskEng);
                    MessageBox.Show("Task successfully updated");
                    new MainWindow().ShowDialog();  
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
