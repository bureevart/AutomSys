using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Threading;
using AutomSys.Items;

namespace AutomSys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer _timer;

        public bool SimulationStarted = false;

        public List<Automate> Automates;

        public MainWindow()
        {
            InitializeComponent();
            SetTime();
            _timer = new DispatcherTimer();

            Automates = new List<Automate>
            {
                new Automate((Image)this.FindName("Automate1"), Label1, AutomateStatuses.Green),
                new Automate((Image)this.FindName("Automate2"), Label2, AutomateStatuses.Green),
                new Automate((Image)this.FindName("Automate3"), Label3, AutomateStatuses.Green),
                new Automate((Image)this.FindName("Automate4"), Label4, AutomateStatuses.Green)
            };

            this.Grid.ItemsSource = Automates;


        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Tick += new EventHandler(DispatcherTimer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            SimulationStarted = true;

            Task.Run(() =>
            {
                SimulateController.Simulate(Grid);
            });
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;

            var automate = Automate.FindImageBySource(Grid, image);

            automate.FixAutomate();

        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;

            var automate = Automate.FindImageBySource(Grid, image);

            automate.FixAutomate();
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;

            var automate = Automate.FindImageBySource(Grid, image);

            automate.FixAutomate();
        }

        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;

            var automate = Automate.FindImageBySource(Grid, image);

            automate.FixAutomate();
        }

        private async void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            SetTime();

            if (SimulationStarted)
            {
                /*foreach (var item in Grid.ItemsSource)
                {
                    if (((Automate)item).Status == AutomateStatuses.Green)
                        ((Automate)item).DestroyAutomate();
                }*/

                Grid.Items.Refresh();
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(Grid);

            
        }

        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGridCellTarget = (DataGridCell)sender;
            if(dataGridCellTarget?.Content is TextBlock textbox && textbox.Text == AutomateStatuses.Red)
            {
                (dataGridCellTarget.DataContext as Automate)?.FixAutomate();
            }
        }

        public void SetTime()
        {
            TimeText.Content = "Текущее время: " + DateTime.Now.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FillAutomate(Automates[0]);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FillAutomate(Automates[1]);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FillAutomate(Automates[2]);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            FillAutomate(Automates[3]);
        }

        public void FillAutomate(Automate automate)
        {
            if (automate.ResourcesCount > 0) return;
            automate.FillItems(); 
            Grid.Items.Refresh();
        }
    }
}
