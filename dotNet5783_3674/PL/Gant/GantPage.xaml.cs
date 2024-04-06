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


namespace PL.Gant
{
    /// <summary>
    /// Interaction logic for GantPage.xaml
    /// </summary>
    public partial class GantPage : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public List<BO.TaskScheduleDays> listTasksScheduale
        {
            get { return (List<BO.TaskScheduleDays>)GetValue(listTasksSchedualeProperty); }
            set { SetValue(listTasksSchedualeProperty, value); }
        }

        public static readonly DependencyProperty listTasksSchedualeProperty =
            DependencyProperty.Register("listTasksScheduale", typeof(List<BO.TaskScheduleDays>), typeof(GantPage), new PropertyMetadata(null));

        public GantPage()
        {
            InitializeComponent();
            listTasksScheduale = s_bl?.Task.GetAllScheduleTasks()!;

        }
    }
}
