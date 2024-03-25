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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.EngineerExperience Levels { get; set; } = BO.EngineerExperience.All;

        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        /// <summary>
        /// Loading all engineers
        /// </summary>
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }

        /// <summary>
        /// Filter engineers by level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxEngineerLevels(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Levels == BO.EngineerExperience.All) ?
                 s_bl?.Engineer.ReadAll() : s_bl?.Engineer.GetEngineersAppropriateLevel(Levels)!;

        }

        /// <summary>
        /// add engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddUpdateEngineer(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }


        private void DoubleClickButton(object sender, MouseButtonEventArgs e)
        {
            // Get the selected item in the list
            BO.Engineer? selectedEngineer = (sender as ListView)?.SelectedItem as BO.Engineer;

           
            if (selectedEngineer != null)
            {
                //Open a single item view window in edit mode and send the details of the selected item
                new EngineerWindow(selectedEngineer.Id).ShowDialog();
            }
        }
    }
}
